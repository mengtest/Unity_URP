#ifndef SHADING_GRADEMAP
#define SHADING_GRADEMAP
#include "ToonProperty.hlsl"


float4 frag(VertexOutput i, fixed facing : VFACE) : SV_TARGET
{
	i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
	float2 Set_UV0 = i.uv0;
	float3 _NormalMap_var = UnpackScaleNormal(tex2D(_NormalMap, TRANSFORM_TEX(Set_UV0, _NormalMap)), _BumpScale);
	float3 normalLocal = _NormalMap_var.rgb;
	float3 normalDirection = normalize(mul(normalLocal, tangentTransform));
	float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(Set_UV0, _MainTex));
	
#ifdef _IS_TRANSCLIPPING_ON
	float4 _ClippingMask_var = tex2D(_ClippingMask,TRANSFORM_TEX(Set_UV0, _ClippingMask));
	float Set_MainTexAlpha = _MainTex_var.a;
	float _IsBaseMapAlphaAsClippingMask_var = lerp( _ClippingMask_var.r, Set_MainTexAlpha, _IsBaseMapAlphaAsClippingMask );
	float _Inverse_Clipping_var = lerp( _IsBaseMapAlphaAsClippingMask_var, (1.0 - _IsBaseMapAlphaAsClippingMask_var), _Inverse_Clipping );
	float Set_Clipping = saturate((_Inverse_Clipping_var+_Clipping_Level));
	clip(Set_Clipping - 0.5);
#endif

	UNITY_LIGHT_ATTENUATION(attenuation, i, i.posWorld.xyz);
	
	float3 defaultLightDirection = normalize(UNITY_MATRIX_V[2].xyz + UNITY_MATRIX_V[1].xyz);
	float3 defaultLightColor = saturate(max(half3(0.05,0.05,0.05)*_Unlit_Intensity,max(ShadeSH9(half4(0.0, 0.0, 0.0, 1.0)),ShadeSH9(half4(0.0, -1.0, 0.0, 1.0)).rgb)*_Unlit_Intensity));
	float3 customLightDirection = normalize(mul( unity_ObjectToWorld, float4(((float3(1.0,0.0,0.0)*_Offset_X_Axis_BLD*10)+(float3(0.0,1.0,0.0)*_Offset_Y_Axis_BLD*10)+(float3(0.0,0.0,-1.0)*lerp(-1.0,1.0,_Inverse_Z_Axis_BLD))),0)).xyz);
	float3 lightDirection = normalize(lerp(defaultLightDirection,_WorldSpaceLightPos0.xyz,any(_WorldSpaceLightPos0.xyz)));
	lightDirection = lerp(lightDirection, customLightDirection, _Is_BLD);
	float3 lightColor = lerp(max(defaultLightColor,_LightColor0.rgb),max(defaultLightColor,saturate(_LightColor0.rgb)),_Is_Filter_LightColor);

	float3 halfDirection = normalize(viewDirection + lightDirection);
	_Color = _BaseColor;

	float3 Set_LightColor = lightColor.rgb;
	float3 Set_BaseColor = lerp( (_MainTex_var.rgb*_BaseColor.rgb), ((_MainTex_var.rgb*_BaseColor.rgb)*Set_LightColor), _Is_LightColor_Base );
	float4 _1st_ShadeMap_var = lerp(tex2D(_1st_ShadeMap, TRANSFORM_TEX(Set_UV0, _1st_ShadeMap)),_MainTex_var,_Use_BaseAs1st);
	float3 _Is_LightColor_1st_Shade_var = lerp( (_1st_ShadeMap_var.rgb*_1st_ShadeColor.rgb), ((_1st_ShadeMap_var.rgb*_1st_ShadeColor.rgb)*Set_LightColor), _Is_LightColor_1st_Shade );
	float _HalfLambert_var = 0.5*dot(lerp( i.normalDir, normalDirection, _Is_NormalMapToBase ),lightDirection)+0.5; // Half Lambert
	float4 _ShadingGradeMap_var = tex2Dlod(_ShadingGradeMap,float4(TRANSFORM_TEX(Set_UV0, _ShadingGradeMap),0.0,_BlurLevelSGM));
	float _SystemShadowsLevel_var = (attenuation*0.5)+0.5+_Tweak_SystemShadowsLevel > 0.001 ? (attenuation*0.5)+0.5+_Tweak_SystemShadowsLevel : 0.0001;
	float _ShadingGradeMapLevel_var = _ShadingGradeMap_var.r < 0.95 ? _ShadingGradeMap_var.r+_Tweak_ShadingGradeMapLevel : 1;
	float Set_ShadingGrade = saturate(_ShadingGradeMapLevel_var)*lerp( _HalfLambert_var, (_HalfLambert_var*saturate(_SystemShadowsLevel_var)), _Set_SystemShadowsToBase );
	float Set_FinalShadowMask = saturate((1.0 + ( (Set_ShadingGrade - (_1st_ShadeColor_Step-_1st_ShadeColor_Feather)) * (0.0 - 1.0) ) / (_1st_ShadeColor_Step - (_1st_ShadeColor_Step-_1st_ShadeColor_Feather)))); // Base and 1st Shade Mask
	float3 _BaseColor_var = lerp(Set_BaseColor,_Is_LightColor_1st_Shade_var,Set_FinalShadowMask);
	float4 _2nd_ShadeMap_var = lerp(tex2D(_2nd_ShadeMap,TRANSFORM_TEX(Set_UV0, _2nd_ShadeMap)),_1st_ShadeMap_var,_Use_1stAs2nd);
	float Set_ShadeShadowMask = saturate((1.0 + ( (Set_ShadingGrade - (_2nd_ShadeColor_Step-_2nd_ShadeColor_Feather)) * (0.0 - 1.0) ) / (_2nd_ShadeColor_Step - (_2nd_ShadeColor_Step-_2nd_ShadeColor_Feather)))); // 1st and 2nd Shades Mask
	
	float3 Set_FinalBaseColor = lerp(_BaseColor_var,lerp(_Is_LightColor_1st_Shade_var,lerp( (_2nd_ShadeMap_var.rgb*_2nd_ShadeColor.rgb), ((_2nd_ShadeMap_var.rgb*_2nd_ShadeColor.rgb)*Set_LightColor), _Is_LightColor_2nd_Shade ),Set_ShadeShadowMask),Set_FinalShadowMask);
	float4 _Set_HighColorMask_var = tex2D(_Set_HighColorMask, TRANSFORM_TEX(Set_UV0, _Set_HighColorMask));
	float _Specular_var = 0.5*dot(halfDirection,lerp( i.normalDir, normalDirection, _Is_NormalMapToHighColor ))+0.5; // Specular
	float _TweakHighColorMask_var = (saturate((_Set_HighColorMask_var.g+_Tweak_HighColorMaskLevel))*lerp( (1.0 - step(_Specular_var,(1.0 - pow(_HighColor_Power,5)))), pow(_Specular_var,exp2(lerp(11,1,_HighColor_Power))), _Is_SpecularToHighColor ));
	float4 _HighColor_Tex_var = tex2D(_HighColor_Tex, TRANSFORM_TEX(Set_UV0, _HighColor_Tex));
	float3 _HighColor_var = (lerp( (_HighColor_Tex_var.rgb*_HighColor.rgb), ((_HighColor_Tex_var.rgb*_HighColor.rgb)*Set_LightColor), _Is_LightColor_HighColor )*_TweakHighColorMask_var);
	float3 Set_HighColor = (lerp( saturate((Set_FinalBaseColor-_TweakHighColorMask_var)), Set_FinalBaseColor, lerp(_Is_BlendAddToHiColor,1.0,_Is_SpecularToHighColor) )+lerp( _HighColor_var, (_HighColor_var*((1.0 - Set_FinalShadowMask)+(Set_FinalShadowMask*_TweakHighColorOnShadow))), _Is_UseTweakHighColorOnShadow ));
	float4 _Set_RimLightMask_var = tex2D(_Set_RimLightMask, TRANSFORM_TEX(Set_UV0, _Set_RimLightMask));
	float3 _Is_LightColor_RimLight_var = lerp( _RimLightColor.rgb, (_RimLightColor.rgb*Set_LightColor), _Is_LightColor_RimLight );
	float _RimArea_var = (1.0 - dot(lerp( i.normalDir, normalDirection, _Is_NormalMapToRimLight ),viewDirection));
	float _RimLightPower_var = pow(_RimArea_var,exp2(lerp(3,0,_RimLight_Power)));
	float _Rimlight_InsideMask_var = saturate(lerp( (0.0 + ( (_RimLightPower_var - _RimLight_InsideMask) * (1.0 - 0.0) ) / (1.0 - _RimLight_InsideMask)), step(_RimLight_InsideMask,_RimLightPower_var), _RimLight_FeatherOff ));
	float _VertHalfLambert_var = 0.5*dot(i.normalDir,lightDirection) + 0.5;
	float3 _LightDirection_MaskOn_var = lerp( (_Is_LightColor_RimLight_var*_Rimlight_InsideMask_var), (_Is_LightColor_RimLight_var*saturate((_Rimlight_InsideMask_var-((1.0 - _VertHalfLambert_var)+_Tweak_LightDirection_MaskLevel)))), _LightDirection_MaskOn );
	float _ApRimLightPower_var = pow(_RimArea_var,exp2(lerp(3,0,_Ap_RimLight_Power)));
	float3 Set_RimLight = (saturate((_Set_RimLightMask_var.g+_Tweak_RimLightMaskLevel))*lerp( _LightDirection_MaskOn_var, (_LightDirection_MaskOn_var+(lerp( _Ap_RimLightColor.rgb, (_Ap_RimLightColor.rgb*Set_LightColor), _Is_LightColor_Ap_RimLight )*saturate((lerp( (0.0 + ( (_ApRimLightPower_var - _RimLight_InsideMask) * (1.0 - 0.0) ) / (1.0 - _RimLight_InsideMask)), step(_RimLight_InsideMask,_ApRimLightPower_var), _Ap_RimLight_FeatherOff )-(saturate(_VertHalfLambert_var)+_Tweak_LightDirection_MaskLevel))))), _Add_Antipodean_RimLight ));

	float3 _RimLight_var = lerp( Set_HighColor, (Set_HighColor+Set_RimLight), _RimLight );
	fixed _sign_Mirror = i.mirrorFlag;
	float3 _Camera_Right = UNITY_MATRIX_V[0].xyz;
	float3 _Camera_Front = UNITY_MATRIX_V[2].xyz;
	float3 _Up_Unit = float3(0, 1, 0);
	float3 _Right_Axis = cross(_Camera_Front, _Up_Unit);
	if(_sign_Mirror < 0)
	{
		_Right_Axis = -1 * _Right_Axis;
		_Rotate_MatCapUV = -1 * _Rotate_MatCapUV;
	}
	else
	{
		_Right_Axis = _Right_Axis;
	}
	float _Camera_Right_Magnitude = sqrt(_Camera_Right.x*_Camera_Right.x + _Camera_Right.y*_Camera_Right.y + _Camera_Right.z*_Camera_Right.z);
	float _Right_Axis_Magnitude = sqrt(_Right_Axis.x*_Right_Axis.x + _Right_Axis.y*_Right_Axis.y + _Right_Axis.z*_Right_Axis.z);
	float _Camera_Roll_Cos = dot(_Right_Axis, _Camera_Right) / (_Right_Axis_Magnitude * _Camera_Right_Magnitude);
	float _Camera_Roll = acos(clamp(_Camera_Roll_Cos, -1, 1));
	fixed _Camera_Dir = _Camera_Right.y < 0 ? -1 : 1;
	float _Rot_MatCapUV_var_ang = (_Rotate_MatCapUV*3.141592654) - _Camera_Dir*_Camera_Roll*_CameraRolling_Stabilizer;

	float2 _Rot_MatCapNmUV_var = RotateUV(Set_UV0, (_Rotate_NormalMapForMatCapUV*3.141592654), float2(0.5, 0.5), 1.0);
	float3 _NormalMapForMatCap_var = UnpackScaleNormal(tex2D(_NormalMapForMatCap,TRANSFORM_TEX(_Rot_MatCapNmUV_var, _NormalMapForMatCap)),_BumpScaleMatcap);
	float3 viewNormal = (mul(UNITY_MATRIX_V, float4(lerp( i.normalDir, mul( _NormalMapForMatCap_var.rgb, tangentTransform ).rgb, _Is_NormalMapForMatCap ),0))).rgb;
	float3 NormalBlend_MatcapUV_Detail = viewNormal.rgb * float3(-1,-1,1);
	float3 NormalBlend_MatcapUV_Base = (mul( UNITY_MATRIX_V, float4(viewDirection,0) ).rgb*float3(-1,-1,1)) + float3(0,0,1);
	float3 noSknewViewNormal = NormalBlend_MatcapUV_Base*dot(NormalBlend_MatcapUV_Base, NormalBlend_MatcapUV_Detail)/NormalBlend_MatcapUV_Base.b - NormalBlend_MatcapUV_Detail;                
	float2 _ViewNormalAsMatCapUV = (lerp(noSknewViewNormal,viewNormal,_Is_Ortho).rg*0.5)+0.5;

	float2 _Rot_MatCapUV_var = RotateUV((0.0 + ((_ViewNormalAsMatCapUV - (0.0+_Tweak_MatCapUV)) * (1.0 - 0.0) ) / ((1.0-_Tweak_MatCapUV) - (0.0+_Tweak_MatCapUV))), _Rot_MatCapUV_var_ang, float2(0.5, 0.5), 1.0);
	if(_sign_Mirror < 0)
	{
		_Rot_MatCapUV_var.x = 1-_Rot_MatCapUV_var.x;
	}
	else
	{
		_Rot_MatCapUV_var = _Rot_MatCapUV_var;
	}
	
	float4 _MatCap_Sampler_var = tex2Dlod(_MatCap_Sampler,float4(TRANSFORM_TEX(_Rot_MatCapUV_var, _MatCap_Sampler),0.0,_BlurLevelMatcap));
	float4 _Set_MatcapMask_var = tex2D(_Set_MatcapMask,TRANSFORM_TEX(Set_UV0, _Set_MatcapMask));
	float _Tweak_MatcapMaskLevel_var = saturate(lerp(_Set_MatcapMask_var.g, (1.0 - _Set_MatcapMask_var.g), _Inverse_MatcapMask) + _Tweak_MatcapMaskLevel);
	float3 _Is_LightColor_MatCap_var = lerp( (_MatCap_Sampler_var.rgb*_MatCapColor.rgb), ((_MatCap_Sampler_var.rgb*_MatCapColor.rgb)*Set_LightColor), _Is_LightColor_MatCap );
	float3 Set_MatCap = lerp( _Is_LightColor_MatCap_var, (_Is_LightColor_MatCap_var*((1.0 - Set_FinalShadowMask)+(Set_FinalShadowMask*_TweakMatCapOnShadow)) + lerp(Set_HighColor*Set_FinalShadowMask*(1.0-_TweakMatCapOnShadow), float3(0.0, 0.0, 0.0), _Is_BlendAddToMatCap)), _Is_UseTweakMatCapOnShadow );
	float3 matCapColorOnAddMode = _RimLight_var+Set_MatCap*_Tweak_MatcapMaskLevel_var;
	float _Tweak_MatcapMaskLevel_var_MultiplyMode = _Tweak_MatcapMaskLevel_var * lerp (1, (1 - (Set_FinalShadowMask)*(1 - _TweakMatCapOnShadow)), _Is_UseTweakMatCapOnShadow);
	float3 matCapColorOnMultiplyMode = Set_HighColor*(1-_Tweak_MatcapMaskLevel_var_MultiplyMode) + Set_HighColor*Set_MatCap*_Tweak_MatcapMaskLevel_var_MultiplyMode + lerp(float3(0,0,0),Set_RimLight,_RimLight);
	float3 matCapColorFinal = lerp(matCapColorOnMultiplyMode, matCapColorOnAddMode, _Is_BlendAddToMatCap);
	
#ifdef _IS_ANGELRING_ON
	float3 finalColor = lerp(_RimLight_var, matCapColorFinal, _MatCap);// Final Composition before AR
	float3 _AR_OffsetU_var = lerp(mul(UNITY_MATRIX_V, float4(i.normalDir,0)).xyz,float3(0,0,1),_AR_OffsetU);
	float2 AR_VN = _AR_OffsetU_var.xy*0.5 + float2(0.5,0.5);
	float2 AR_VN_Rotate = RotateUV(AR_VN, -(_Camera_Dir*_Camera_Roll), float2(0.5,0.5), 1.0);
	float2 _AR_OffsetV_var = float2(AR_VN_Rotate.x, lerp(i.uv1.y, AR_VN_Rotate.y, _AR_OffsetV));
	float4 _AngelRing_Sampler_var = tex2D(_AngelRing_Sampler,TRANSFORM_TEX(_AR_OffsetV_var, _AngelRing_Sampler));
	float3 _Is_LightColor_AR_var = lerp( (_AngelRing_Sampler_var.rgb*_AngelRing_Color.rgb), ((_AngelRing_Sampler_var.rgb*_AngelRing_Color.rgb)*Set_LightColor), _Is_LightColor_AR );
	float3 Set_AngelRing = _Is_LightColor_AR_var;
	float Set_ARtexAlpha = _AngelRing_Sampler_var.a;
	float3 Set_AngelRingWithAlpha = (_Is_LightColor_AR_var*_AngelRing_Sampler_var.a);
	finalColor = lerp(finalColor, lerp((finalColor + Set_AngelRing), ((finalColor*(1.0 - Set_ARtexAlpha))+Set_AngelRingWithAlpha), _ARSampler_AlphaOn ), _AngelRing );// Final Composition before Emissive
#else
	float3 finalColor = lerp(_RimLight_var, matCapColorFinal, _MatCap);
#endif
	
#ifdef _EMISSIVE_SIMPLE
	float4 _Emissive_Tex_var = tex2D(_Emissive_Tex,TRANSFORM_TEX(Set_UV0, _Emissive_Tex));
	float emissiveMask = _Emissive_Tex_var.a;
	emissive = _Emissive_Tex_var.rgb * _Emissive_Color.rgb * emissiveMask;
	
#elif _EMISSIVE_ANIMATION
	float3 viewNormal_Emissive = (mul(UNITY_MATRIX_V, float4(i.normalDir,0))).xyz;
	float3 NormalBlend_Emissive_Detail = viewNormal_Emissive * float3(-1,-1,1);
	float3 NormalBlend_Emissive_Base = (mul( UNITY_MATRIX_V, float4(viewDirection,0)).xyz*float3(-1,-1,1)) + float3(0,0,1);
	float3 noSknewViewNormal_Emissive = NormalBlend_Emissive_Base*dot(NormalBlend_Emissive_Base, NormalBlend_Emissive_Detail)/NormalBlend_Emissive_Base.z - NormalBlend_Emissive_Detail;
	float2 _ViewNormalAsEmissiveUV = noSknewViewNormal_Emissive.xy*0.5+0.5;
	float2 _ViewCoord_UV = RotateUV(_ViewNormalAsEmissiveUV, -(_Camera_Dir*_Camera_Roll), float2(0.5,0.5), 1.0);
	if(_sign_Mirror < 0)
	{
		_ViewCoord_UV.x = 1-_ViewCoord_UV.x;
	}
	else
	{
		_ViewCoord_UV = _ViewCoord_UV;
	}
	
	float2 emissive_uv = lerp(i.uv0, _ViewCoord_UV, _Is_ViewCoord_Scroll);
	float4 _time_var = _Time;
	float _base_Speed_var = (_time_var.g*_Base_Speed);
	float _Is_PingPong_Base_var = lerp(_base_Speed_var, sin(_base_Speed_var), _Is_PingPong_Base );
	float2 scrolledUV = emissive_uv + float2(_Scroll_EmissiveU, _Scroll_EmissiveV)*_Is_PingPong_Base_var;
	float rotateVelocity = _Rotate_EmissiveUV*3.141592654;
	float2 _rotate_EmissiveUV_var = RotateUV(scrolledUV, rotateVelocity, float2(0.5, 0.5), _Is_PingPong_Base_var);
	float4 _Emissive_Tex_var = tex2D(_Emissive_Tex,TRANSFORM_TEX(Set_UV0, _Emissive_Tex));
	float emissiveMask = _Emissive_Tex_var.a;
	_Emissive_Tex_var = tex2D(_Emissive_Tex,TRANSFORM_TEX(_rotate_EmissiveUV_var, _Emissive_Tex));
	float _colorShift_Speed_var = 1.0 - cos(_time_var.g*_ColorShift_Speed);
	float viewShift_var = smoothstep( 0.0, 1.0, max(0,dot(normalDirection,viewDirection)));
	float4 colorShift_Color = lerp(_Emissive_Color, lerp(_Emissive_Color, _ColorShift, _colorShift_Speed_var), _Is_ColorShift);
	float4 viewShift_Color = lerp(_ViewShift, colorShift_Color, viewShift_var);
	float4 emissive_Color = lerp(colorShift_Color, viewShift_Color, _Is_ViewShift);
	emissive = emissive_Color.rgb * _Emissive_Tex_var.rgb * emissiveMask;
#endif
	
	float3 envLightColor = DecodeLightProbe(normalDirection) < float3(1,1,1) ? DecodeLightProbe(normalDirection) : float3(1,1,1);
	float envLightIntensity = 0.299*envLightColor.r + 0.587*envLightColor.g + 0.114*envLightColor.b <1 ? (0.299*envLightColor.r + 0.587*envLightColor.g + 0.114*envLightColor.b) : 1;
	finalColor =  saturate(finalColor) + (envLightColor*envLightIntensity*_GI_Intensity*smoothstep(1,0,envLightIntensity/2)) + emissive;


#ifdef _IS_TRANSCLIPPING_OFF
	fixed4 finalRGBA = fixed4(finalColor,1);
	
#elif _IS_TRANSCLIPPING_ON
	float Set_Opacity = saturate((_Inverse_Clipping_var+_Tweak_transparency));
	fixed4 finalRGBA = fixed4(finalColor,Set_Opacity);	
#endif
	UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
	return finalRGBA;
}


#endif
