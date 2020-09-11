#ifndef SHADING_GRADEMAP_DELTA
#define SHADING_GRADEMAP_DELTA
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
	
	float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
	float3 addPassLightColor = (0.5*dot(lerp( i.normalDir, normalDirection, _Is_NormalMapToBase ), lightDirection)+0.5) * _LightColor0.rgb * attenuation;
	float pureIntencity = max(0.001,(0.299*_LightColor0.r + 0.587*_LightColor0.g + 0.114*_LightColor0.b));
	float3 lightColor = max(0, lerp(addPassLightColor, lerp(0,min(addPassLightColor,addPassLightColor/pureIntencity),_WorldSpaceLightPos0.w),_Is_Filter_LightColor));
	float3 halfDirection = normalize(viewDirection + lightDirection);
	_Color = _BaseColor;


	_1st_ShadeColor_Step = saturate(_1st_ShadeColor_Step + _StepOffset);
	_2nd_ShadeColor_Step = saturate(_2nd_ShadeColor_Step + _StepOffset);
	float _LightIntensity = lerp(0,(0.299*_LightColor0.r + 0.587*_LightColor0.g + 0.114*_LightColor0.b)*attenuation,_WorldSpaceLightPos0.w) ;
	float3 Set_LightColor = lerp(lightColor,lerp(lightColor,min(lightColor,_LightColor0.rgb*attenuation*_1st_ShadeColor_Step),_WorldSpaceLightPos0.w),_Is_Filter_HiCutPointLightColor);
	float3 Set_BaseColor = lerp( (_MainTex_var.rgb*_BaseColor.rgb*_LightIntensity), ((_MainTex_var.rgb*_BaseColor.rgb)*Set_LightColor), _Is_LightColor_Base );
	float4 _1st_ShadeMap_var = lerp(tex2D(_1st_ShadeMap,TRANSFORM_TEX(Set_UV0, _1st_ShadeMap)),_MainTex_var,_Use_BaseAs1st);
	float3 _Is_LightColor_1st_Shade_var = lerp( (_1st_ShadeMap_var.rgb*_1st_ShadeColor.rgb*_LightIntensity), ((_1st_ShadeMap_var.rgb*_1st_ShadeColor.rgb)*Set_LightColor), _Is_LightColor_1st_Shade );
	float _HalfLambert_var = 0.5*dot(lerp( i.normalDir, normalDirection, _Is_NormalMapToBase ),lightDirection)+0.5; // Half Lambert
	float4 _ShadingGradeMap_var = tex2Dlod(_ShadingGradeMap,float4(TRANSFORM_TEX(Set_UV0, _ShadingGradeMap),0.0,_BlurLevelSGM));
	float _ShadingGradeMapLevel_var = _ShadingGradeMap_var.r < 0.95 ? _ShadingGradeMap_var.r+_Tweak_ShadingGradeMapLevel : 1;
	float Set_ShadingGrade = saturate(_ShadingGradeMapLevel_var)*lerp( _HalfLambert_var, (_HalfLambert_var*saturate(1.0+_Tweak_SystemShadowsLevel)), _Set_SystemShadowsToBase );
	float Set_FinalShadowMask = saturate((1.0 + ( (Set_ShadingGrade - (_1st_ShadeColor_Step-_1st_ShadeColor_Feather)) * (0.0 - 1.0) ) / (_1st_ShadeColor_Step - (_1st_ShadeColor_Step-_1st_ShadeColor_Feather)))); // Base and 1st Shade Mask
	float3 _BaseColor_var = lerp(Set_BaseColor,_Is_LightColor_1st_Shade_var,Set_FinalShadowMask);
	float4 _2nd_ShadeMap_var = lerp(tex2D(_2nd_ShadeMap,TRANSFORM_TEX(Set_UV0, _2nd_ShadeMap)),_1st_ShadeMap_var,_Use_1stAs2nd);
	float Set_ShadeShadowMask = saturate((1.0 + ( (Set_ShadingGrade - (_2nd_ShadeColor_Step-_2nd_ShadeColor_Feather)) * (0.0 - 1.0) ) / (_2nd_ShadeColor_Step - (_2nd_ShadeColor_Step-_2nd_ShadeColor_Feather)))); // 1st and 2nd Shades Mask
	float3 finalColor = lerp(_BaseColor_var,lerp(_Is_LightColor_1st_Shade_var,lerp( (_2nd_ShadeMap_var.rgb*_2nd_ShadeColor.rgb*_LightIntensity), ((_2nd_ShadeMap_var.rgb*_2nd_ShadeColor.rgb)*Set_LightColor), _Is_LightColor_2nd_Shade ),Set_ShadeShadowMask),Set_FinalShadowMask);
	
	float4 _Set_HighColorMask_var = tex2D(_Set_HighColorMask,TRANSFORM_TEX(Set_UV0, _Set_HighColorMask));
	float _Specular_var = 0.5*dot(halfDirection,lerp( i.normalDir, normalDirection, _Is_NormalMapToHighColor ))+0.5; // Specular
	float _TweakHighColorMask_var = (saturate((_Set_HighColorMask_var.g+_Tweak_HighColorMaskLevel))*lerp( (1.0 - step(_Specular_var,(1.0 - pow(_HighColor_Power,5)))), pow(_Specular_var,exp2(lerp(11,1,_HighColor_Power))), _Is_SpecularToHighColor ));
	float4 _HighColor_Tex_var = tex2D(_HighColor_Tex,TRANSFORM_TEX(Set_UV0, _HighColor_Tex));
	float3 _HighColor_var = (lerp( (_HighColor_Tex_var.rgb*_HighColor.rgb), ((_HighColor_Tex_var.rgb*_HighColor.rgb)*Set_LightColor), _Is_LightColor_HighColor )*_TweakHighColorMask_var);
	finalColor = finalColor + lerp(lerp( _HighColor_var, (_HighColor_var*((1.0 - Set_FinalShadowMask)+(Set_FinalShadowMask*_TweakHighColorOnShadow))), _Is_UseTweakHighColorOnShadow ),float3(0,0,0),_Is_Filter_HiCutPointLightColor);
	finalColor = saturate(finalColor);


#ifdef _IS_TRANSCLIPPING_OFF
	fixed4 finalRGBA = fixed4(finalColor,0);
	
#elif _IS_TRANSCLIPPING_ON
	float Set_Opacity = saturate((_Inverse_Clipping_var+_Tweak_transparency));
	fixed4 finalRGBA = fixed4(finalColor * Set_Opacity,0);	
#endif
	UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
	return finalRGBA;
}


#endif
