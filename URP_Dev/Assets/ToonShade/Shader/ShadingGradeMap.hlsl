#ifndef SHADING_GRADEMAP
#define SHADING_GRADEMAP


// Using pow often result to a warning like this
// "pow(f, e) will not work for negative f, use abs(f) or conditionally handle negative values if you expect them"
// PositivePow remove this warning when you know the value is positive and avoid inf/NAN.
#include "ToonLib.hlsl"

#define fixed  half

float GetShadowMask(float toonShade, float step, float feather)
{
	return saturate((1.0 + ((toonShade - (step - feather)) * (0.0 - 1.0)) / (step - (step - feather))));
}

float GetEnvLightIntensity(float3 envLightColor)
{
	return (0.299 * envLightColor.r + 0.587 * envLightColor.g + 0.114 * envLightColor.b < 1) ? 
		(0.299 * envLightColor.r + 0.587 * envLightColor.g + 0.114 * envLightColor.b) : 1;
}

// Forward Delta
inline void AddtionalPointLight(VertexOutput i, InputData input, float4 baseColor, float3 viewDir, float3 normalDir, out float3 pointLightColor)
{
	int pixelLightCount = GetAdditionalLightsCount();

	for (int iLight = -1; iLight < pixelLightCount; ++iLight)
	{
		if (iLight != i.mainLightID)
		{
			float notDirectional = 1.0f;
			ToonLight additionalLight = GetMainToonLight(0, 0);
			if (iLight != -1)
			{
				additionalLight = GetAdditionalToonLight(iLight, input.positionWS, i.positionCS);
			}
			
			half3 additionalLightColor = GetLightColor(additionalLight);
			float3 lightDirection = additionalLight.direction;
			
			float3 addPassLightColor = (0.5 * dot(lerp(i.normalDir, normalDir, _Is_NormalMapToBase), lightDirection) + 0.5);
			addPassLightColor *= additionalLightColor.rgb;	
			float pureIntencity = max(0.001, (0.299 * additionalLightColor.r + 0.587 * additionalLightColor.g + 0.114 * additionalLightColor.b));
			
			float3 addPassLightMin = min(addPassLightColor, addPassLightColor / pureIntencity);
			float3 lightColor = max(0, lerp(addPassLightColor, lerp(0, addPassLightMin, notDirectional), 1 /*_Is_Filter_LightColor*/));
			float3 halfDirection = normalize(viewDir + lightDirection);

			_1st_ShadeColor_Step = saturate(_1st_ShadeColor_Step + _StepOffset);
			_2nd_ShadeColor_Step = saturate(_2nd_ShadeColor_Step + _StepOffset);
			
			float lightMaxIntensity = (0.299 * additionalLightColor.r + 0.587 * additionalLightColor.g + 0.114 * additionalLightColor.b);
			float lightIntensity = lerp(0, lightMaxIntensity, notDirectional);
			
			float3 lightMin = min(lightColor, additionalLightColor.rgb * _1st_ShadeColor_Step);
			float3 baseLightColor = lerp(lightColor, lerp(lightColor, lightMin, notDirectional), _Is_Filter_HiCutPointLightColor);
			
			float3 diffuseLight = (_Color.rgb * baseColor.rgb * lightIntensity);
			float3 diffuseBaseLight = ((_Color.rgb * baseColor.rgb) * baseLightColor);
			float3 diffuseColor = lerp(diffuseLight, diffuseBaseLight, _Is_LightColor_Base);	
			
			float4 toonShade1st = lerp(tex2D(_1st_ShadeMap, i.uv0), baseColor, _Use_BaseAs1st);
			float4 toonShade2nd = lerp(tex2D(_2nd_ShadeMap, i.uv0), toonShade1st, _Use_1stAs2nd);
			
			float3 shade1stColorA = (_1st_ShadeColor.rgb * toonShade1st.rgb * lightIntensity);
			float3 shade1stColorB = ((_1st_ShadeColor.rgb * toonShade1st.rgb) * baseLightColor);
			float3 Set_1st_ShadeColor = lerp(shade1stColorA, shade1stColorB, _Is_LightColor_1st_Shade);
			
			float3 shade2ndColorA = (_2nd_ShadeColor.rgb * toonShade2nd.rgb * lightIntensity);
			float3 shade2ndColorB = ((_2nd_ShadeColor.rgb * toonShade2nd.rgb) * baseLightColor);
			float3 Set_2nd_ShadeColor = lerp(shade2ndColorA, shade2ndColorB, _Is_LightColor_2nd_Shade);
			
			float halfLambert = 0.5 * dot(lerp(i.normalDir, normalDir, _Is_NormalMapToBase), lightDirection) + 0.5;
			float4 shadingGradeMap = tex2Dlod(_ShadingGradeMap, float4(TRANSFORM_TEX(i.uv0, _ShadingGradeMap), 0.0, _BlurLevelSGM));
			float toonShadeLevel = shadingGradeMap.r < 0.95 ? shadingGradeMap.r + _Tweak_ShadingGradeMapLevel : 1;
			
			float halfLambertClamp = (halfLambert * saturate(1.0 + _Tweak_SystemShadowsLevel));
			float toonShade = saturate(toonShadeLevel) * lerp(halfLambert, halfLambertClamp, _Set_SystemShadowsToBase);
			
			float finalShadowMask = GetShadowMask(toonShade, _1st_ShadeColor_Step, _1st_ShadeColor_Feather);
			float shadeShadowMask = GetShadowMask(toonShade, _2nd_ShadeColor_Step, _2nd_ShadeColor_Feather); 

			float3 finalColor = lerp(diffuseColor, lerp(Set_1st_ShadeColor, Set_2nd_ShadeColor, shadeShadowMask), finalShadowMask);
			float4 highColorMaskTex = tex2D(_Set_HighColorMask, i.uv0);
			float specular = 0.5 * dot(halfDirection, lerp(i.normalDir, normalDir, _Is_NormalMapToHighColor)) + 0.5;
			
			float tweakHighColorMask = (saturate((highColorMaskTex.g + _Tweak_HighColorMaskLevel)) *
				lerp((1.0 - step(specular, (1.0 - pow(_HighColor_Power, 5)))),
				PositivePow(specular, exp2(lerp(11, 1, _HighColor_Power))),
				_Is_SpecularToHighColor));
			
			float4 highColorTex = tex2D(_HighColor_Tex, i.uv0);
			
			float3 highColorA = (highColorTex.rgb * _HighColor.rgb);
			float3 highColorB = ((highColorTex.rgb * _HighColor.rgb) * baseLightColor);
			float3 highColor = (lerp(highColorA, highColorB, _Is_LightColor_HighColor) * tweakHighColorMask);
			
			float3 addLightColor = lerp(
				lerp(highColor, (highColor * ((1.0 - finalShadowMask) + (finalShadowMask * _TweakHighColorOnShadow))),
				_Is_UseTweakHighColorOnShadow), float3(0, 0, 0), _Is_Filter_HiCutPointLightColor);
			
			finalColor += addLightColor;
			finalColor = saturate(finalColor);
			
			pointLightColor += finalColor;
		}
	}
}

// High Color
float3 SetHighColor(VertexOutput i, float3 normalDir, float3 halfDir, float3 lightColor, float3 finalBaseColor, float finalShadowMask)
{
	float4 highColorMaskTex = tex2D(_Set_HighColorMask, i.uv0);
	float specular = 0.5 * dot(halfDir, lerp(i.normalDir, normalDir, _Is_NormalMapToHighColor)) + 0.5;
	
	float clampPow = PositivePow(specular, exp2(lerp(11, 1, _HighColor_Power)));
	float tweakHighColorMask = (saturate((highColorMaskTex.g + _Tweak_HighColorMaskLevel)) *
		lerp((1.0 - step(specular, (1.0 - pow(_HighColor_Power, 5)))), clampPow, _Is_SpecularToHighColor));
	
	
	float4 highColorTex = tex2D(_HighColor_Tex, i.uv0);
	
	float3 highColorA = (highColorTex.rgb * _HighColor.rgb);
	float3 highColorB = ((highColorTex.rgb * _HighColor.rgb) * lightColor);
	float3 highColor = (lerp(highColorA, highColorB, _Is_LightColor_HighColor) * tweakHighColorMask);
	
	float blendHighColor = lerp(_Is_BlendAddToHiColor, 1.0, _Is_SpecularToHighColor);
	float3 resultHighColor = lerp(highColor, (highColor * ((1.0 - finalShadowMask) + (finalShadowMask * _TweakHighColorOnShadow))), _Is_UseTweakHighColorOnShadow);
	float3 baseHighColor = (lerp(saturate((finalBaseColor - tweakHighColorMask)), finalBaseColor, blendHighColor) + resultHighColor);
	return baseHighColor;
}

// Rim Light
float3 SetRimLight(VertexOutput i, float3 normalDir, float3 viewDir, float3 lightDir, float3 baseLightColor)
{
	float4 rimLightMask = tex2D(_Set_RimLightMask, i.uv0);
	float3 lightColorRim = lerp(_RimLightColor.rgb, (_RimLightColor.rgb * baseLightColor), _Is_LightColor_RimLight);
	float rimArea = (1.0 - dot(lerp(i.normalDir, normalDir, _Is_NormalMapToRimLight), viewDir));
	float rimLightPower = PositivePow(rimArea, exp2(lerp(3, 0, _RimLight_Power)));
	
	float rimLightInsideMask = saturate(
		lerp((0.0 + ((rimLightPower - _RimLight_InsideMask) * (1.0 - 0.0)) / (1.0 - _RimLight_InsideMask)),
		step(_RimLight_InsideMask, rimLightPower), _Is_RimLight_FeatherOff));
	
	float vertHalfLambert = 0.5 * dot(i.normalDir, lightDir) + 0.5;
	
	float3 lightDirectionMask = lerp(
		(lightColorRim * rimLightInsideMask),
		(lightColorRim * saturate((rimLightInsideMask - ((1.0 - vertHalfLambert) + _Tweak_LightDirection_MaskLevel)))),
		_Is_LightDirection_MaskOn);
	
	float powRimLight = PositivePow(rimArea, exp2(lerp(3, 0, _Ap_RimLight_Power)));
	
	float rimLightClamp = saturate((lerp((0.0 + ((powRimLight - _RimLight_InsideMask) * (1.0 - 0.0)) / (1.0 - _RimLight_InsideMask)),
			step(_RimLight_InsideMask, powRimLight), _Is_ApRimLight_FeatherOff) - (saturate(vertHalfLambert) + _Tweak_LightDirection_MaskLevel)));
	
	float3 rimLightDirectionAdd = lerp(_Ap_RimLightColor.rgb, (_Ap_RimLightColor.rgb * baseLightColor), _Is_LightColor_Ap_RimLight) * rimLightClamp;
	float3 baseRimLight = (saturate((rimLightMask.g + _Tweak_RimLightMaskLevel)) *
		lerp(lightDirectionMask, (lightDirectionMask + (rimLightDirectionAdd * rimLightClamp)), _Is_Antipodean_RimLight));
	return baseRimLight;
}

// MatCap UV
float2 SetMatCapUV(VertexOutput i, float3x3 tangentTransform, float3 viewDir, float matcapUVAngle, fixed signMirror)
{
	float2 matcapRotateUV = RotateUV(i.uv0, (_Rotate_NormalMapForMatCapUV * PI), float2(0.5, 0.5), 1.0);
	float3 normalMap4Matcap = UnpackNormalScale(tex2D(_NormalMapForMatCap, TRANSFORM_TEX(matcapRotateUV, _NormalMapForMatCap)), _BumpScaleMatcap);
	float3 matcapMultiply = mul(normalMap4Matcap.rgb, tangentTransform).rgb;
	float3 viewNormal = (mul(UNITY_MATRIX_V, float4(lerp(i.normalDir, matcapMultiply, _Is_NormalMapForMatCap), 0))).rgb;

	float3 normalBlendMatcapUVDetail = viewNormal.rgb * float3(-1, -1, 1);
	float3 normalBlendMatcapUVBase = (mul(UNITY_MATRIX_V, float4(viewDir, 0)).rgb * float3(-1, -1, 1)) + float3(0, 0, 1);
	float viewNormalDot = dot(normalBlendMatcapUVBase, normalBlendMatcapUVDetail);
	float3 noSknewViewNormal = normalBlendMatcapUVBase * viewNormalDot / (normalBlendMatcapUVBase.b - normalBlendMatcapUVDetail);
	
	float2 viewNormalMatCapUV = (lerp(noSknewViewNormal, viewNormal, _Is_Ortho).rg * 0.5) + 0.5;
	float2 tweakMatCapUV = (0.0 + ((viewNormalMatCapUV - (0.0 + _Tweak_MatCapUV)) * (1.0 - 0.0)) / ((1.0 - _Tweak_MatCapUV) - (0.0 + _Tweak_MatCapUV)));
	float2 rotateMatCapUV = RotateUV(tweakMatCapUV, matcapUVAngle, float2(0.5, 0.5), 1.0);
	
	if (signMirror < 0)
	{
		rotateMatCapUV.x = 1 - rotateMatCapUV.x;
	}
	else
	{
		rotateMatCapUV = rotateMatCapUV;
	}
	return rotateMatCapUV;

}

// MatCap Color
float3 SetMatCap(VertexOutput i, float2 inUV, float3 lightColor, float finalShadowMask, float3 inHighColor, float3 inRimLight, float3 rimLight)
{
	float4 matcapSampler = tex2Dlod(_MatCap_Sampler, float4(TRANSFORM_TEX(inUV, _MatCap_Sampler), 0.0, _BlurLevelMatcap));
	float4 matcapMask = tex2D(_Set_MatcapMask, i.uv0);
	float tweakMatCapMaskLevel = saturate(lerp(matcapMask.g, (1.0 - matcapMask.g), _Is_InverseMatcapMask) + _Tweak_MatcapMaskLevel);
	
	float3 lightColorMatCap = lerp((matcapSampler.rgb * _MatCapColor.rgb),
		((matcapSampler.rgb * _MatCapColor.rgb) * lightColor), _Is_LightColor_MatCap);
	
	float3 finalMatCapAdd = lerp(inHighColor * finalShadowMask * (1.0 - _TweakMatCapOnShadow), float3(0.0, 0.0, 0.0), _Is_BlendAddToMatCap);
	
	float3 baseMatCap = lerp(lightColorMatCap,
		(lightColorMatCap * ((1.0 - finalShadowMask) + (finalShadowMask * _TweakMatCapOnShadow)) + finalMatCapAdd), _Is_UseTweakMatCapOnShadow);
	
	float3 matCapColorOnAddMode = rimLight + baseMatCap * tweakMatCapMaskLevel;
	float matcapMaskLevelMultiplyMode = tweakMatCapMaskLevel *
		lerp(1, (1 - (finalShadowMask) * (1 - _TweakMatCapOnShadow)), _Is_UseTweakMatCapOnShadow);
	
	float3 resultRim = lerp(float3(0, 0, 0), inRimLight, _RimLight);	
	
	float3 matCapColorOnMultiplyMode = inHighColor * (1 - matcapMaskLevelMultiplyMode) + inHighColor * baseMatCap * matcapMaskLevelMultiplyMode + resultRim;
	return lerp(matCapColorOnMultiplyMode, matCapColorOnAddMode, _Is_BlendAddToMatCap);
}

// Angel Ring
#ifdef _ANGELRING_OFF
#else
float3 AdditionalHairSpecular(VertexOutput i, fixed dir, float roll, float3 baseLightColor, float3 finalColor)
{
	float3 hairSpecularOffsetU = lerp(mul(UNITY_MATRIX_V, float4(i.normalDir, 0)).xyz, float3(0, 0, 1), _AR_OffsetU);
	float2 hairSpecularViewNormal = hairSpecularOffsetU.xy * 0.5 + float2(0.5, 0.5);
	float2 hairSpecularViewNormalRotate = RotateUV(hairSpecularViewNormal, -(dir * roll), float2(0.5, 0.5), 1.0);
	float2 hairSpecularOffsetV = float2(hairSpecularViewNormalRotate.x, lerp(i.uv1.y, hairSpecularViewNormalRotate.y, _AR_OffsetV));
	
	float4 hairSpecularSampler = tex2D(_AngelRing_Sampler, hairSpecularOffsetV);
	
	float3 hairSpecular = lerp((hairSpecularSampler.rgb * _AngelRing_Color.rgb),
		((hairSpecularSampler.rgb * _AngelRing_Color.rgb) * baseLightColor), _Is_LightColor_AR);
	
	float3 Set_AngelRing = hairSpecular;
	Set_AngelRing += finalColor;
	float Set_ARtexAlpha = hairSpecularSampler.a;
	float3 Set_AngelRingWithAlpha = (hairSpecular * hairSpecularSampler.a);
	float3 alphaResult = ((finalColor * (1.0 - Set_ARtexAlpha)) + Set_AngelRingWithAlpha);
	return lerp(finalColor, lerp(Set_AngelRing, alphaResult, _Is_AngelRingAlphaOn), _AngelRing);
}
#endif

// Emissive
#ifdef _EMISSIVE_OFF
#else
float3 SetEmissive(VertexOutput i, float3 viewDir, float3 normalDir, fixed signMirror, fixed dir, float roll)
{
	float3 viewNormalEmissive = (mul(UNITY_MATRIX_V, float4(i.normalDir, 0))).xyz;
	float3 normalBlendEmissiveDetail = viewNormalEmissive * float3(-1, -1, 1);
	float3 blendEmissiveBase = (mul(UNITY_MATRIX_V, float4(viewDir, 0)).xyz * float3(-1, -1, 1)) + float3(0, 0, 1);
	half baseDotDetail = dot(blendEmissiveBase, normalBlendEmissiveDetail);
	float3 viewNormalEmissiveDiff = blendEmissiveBase * baseDotDetail / blendEmissiveBase.z - normalBlendEmissiveDetail;
	float2 emissiveUV = viewNormalEmissiveDiff.xy * 0.5 + 0.5;
	float2 viewUV = RotateUV(emissiveUV, -(dir * roll), float2(0.5, 0.5), 1.0);
	if (signMirror < 0)
	{
		viewUV.x = 1 - viewUV.x;
	}
	else
	{
		viewUV = viewUV;
	}
	float time = _Time.g;
	float2 emissive_uv = lerp(i.uv0, viewUV, _Is_ViewCoord_Scroll);
	float baseSpeed = (time * _Base_Speed);
	float pingpongSpeed = lerp(baseSpeed, sin(baseSpeed), _Is_PingPong_Base);
	float rotateVelocity = _Rotate_EmissiveUV * PI;
	float2 scrolledUV = emissive_uv + float2(_Scroll_EmissiveU, _Scroll_EmissiveV) * pingpongSpeed;
	float2 rotateEmissiveUV = RotateUV(scrolledUV, rotateVelocity, float2(0.5, 0.5), pingpongSpeed);
	float4 emissiveTex = tex2D(_Emissive_Tex, i.uv0);
	float emissiveMask = emissiveTex.a;
	emissiveTex = tex2D(_Emissive_Tex, rotateEmissiveUV);
	float colorShiftSpeed = 1.0 - cos(time * _ColorShift_Speed);
	float viewShift = smoothstep(0.0, 1.0, max(0, dot(normalDir, viewDir)));
	float4 colorShiftColor = lerp(_Emissive_Color, lerp(_Emissive_Color, _ColorShift, colorShiftSpeed), _Is_ColorShift);
	float4 viewShiftColor = lerp(_ViewShift, colorShiftColor, viewShift);
	float4 emissiveColor = lerp(colorShiftColor, viewShiftColor, _Is_ViewShift);
	return emissiveColor.rgb * emissiveTex.rgb * emissiveMask;
}
#endif

VertexOutput vert(VertexInput v)
{
	VertexOutput o = (VertexOutput) 0;
	
	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
	
	o.uv0 = v.texcoord0;
#ifdef _ANGELRING_ON
	o.uv1 = v.texcoord1;
#endif
	o.normalDir = UnityObjectToWorldNormal(v.normal);
	o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
	o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
	
	//float3 lightColor = _LightColor0.rgb;
	o.pos = UnityObjectToClipPos(v.vertex);
	
	float3 crossFwd = cross(UNITY_MATRIX_V[0].xyz, UNITY_MATRIX_V[1].xyz);
	o.mirrorFlag = dot(crossFwd, UNITY_MATRIX_V[2].xyz) < 0 ? 1 : -1;
	
	float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
	float4 positionCS = TransformWorldToHClip(positionWS);
	
	half3 vertexLight = VertexLighting(o.posWorld.xyz, o.normalDir);
	half fogFactor = ComputeFogFactor(positionCS.z);
	
	OUTPUT_LIGHTMAP_UV(v.lightmapUV, unity_LightmapST, o.lightmapUV);
	OUTPUT_SH(o.normalDir.xyz, o.vertexSH);
	o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
	o.positionCS = positionCS;
	
#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
	#if SHADOWS_SCREEN
		o.shadowCoord = ComputeScreenPos(positionCS);
	#else
		o.shadowCoord = TransformWorldToShadowCoord(o.posWorld.xyz);
	#endif
	o.mainLightID = DetermineToonShadeMainLightIndex(o.posWorld.xyz, o.shadowCoord, positionCS);
	
#else
	o.mainLightID = DetermineToonShadeMainLightIndex(o.posWorld.xyz, 0, positionCS);
#endif
	return o;
}


float4 frag(VertexOutput i, fixed facing : VFACE) : SV_TARGET
{
	i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
	
	float3 normalMap = UnpackNormalScale(tex2D(_NormalMap, TRANSFORM_TEX(i.uv0, _NormalMap)), _BumpScale);
	float3 normalLocal = normalMap.rgb;
	float3 normalDirection = normalize(mul(normalLocal, tangentTransform));

	SurfaceData surfaceData;
	InitializeStandardLitSurfaceDataToonShade(i.uv0, surfaceData);

	InputData inputData;
	Varyings input;

	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	input.vertexSH = i.vertexSH;
	input.uv = i.uv0;
	input.fogFactorAndVertexLight = i.fogFactorAndVertexLight;
#ifdef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
	input.shadowCoord = i.shadowCoord;
#endif

#ifdef REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
	input.positionWS = i.posWorld.xyz;
#endif
	
#ifdef _NORMALMAP
	// xyz: normal, w: viewDir.x
	input.normalWS = half4(i.normalDir, viewDirection.x);
	// xyz: tangent, w: viewDir.y
	input.tangentWS = half4(i.tangentDir, viewDirection.y);
	// xyz: bitangent, w: viewDir.z
	input.bitangentWS = half4(i.bitangentDir, viewDirection.z);
#else
	input.normalWS = half3(i.normalDir);
	input.viewDirWS = half3(viewDirection);
#endif
	
	InitializeInputData(input, surfaceData.normalTS, inputData);
	
	BRDFData brdfData;
	InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);
	half3 envColor = GlobalIlluminationToonShade(brdfData, inputData.bakedGI, surfaceData.occlusion, inputData.normalWS, inputData.viewDirectionWS);
	envColor *= 1.8f;

	float3 envLightColor = envColor.rgb;
	float envLightIntensity = GetEnvLightIntensity(envLightColor);
	
	ToonLight mainLight = GetMainToonShadeLightByID(i.mainLightID, i.posWorld.xyz, inputData.shadowCoord, i.positionCS);
	half3 mainLightColor = GetLightColor(mainLight);
	float4 mainTex = tex2D(_MainTex, i.uv0);
	
#ifdef _IS_TRANSCLIPPING_OFF
#elif _IS_TRANSCLIPPING_ON
	float4 clippingMask = tex2D(_ClippingMask, i.uv0);
	float clippingAlpha = mainTex.a;
	float alphaAsClippingMask = lerp(clippingMask.r, clippingAlpha, _IsBaseMapAlphaAsClippingMask);
	float inverseClipping = lerp(alphaAsClippingMask, (1.0 - alphaAsClippingMask), _Inverse_Clipping);
	float clipping = saturate((inverseClipping + _ClippingLevel));
	clip(clipping - 0.5);
#endif

	half shadowAttenuation = 1.0;

#ifdef _MAIN_LIGHT_SHADOWS
	shadowAttenuation = mainLight.shadowAttenuation;
#endif

	float3 shadeLight = max(ShadeSH9(half4(0.0, 0.0, 0.0, 1.0)), ShadeSH9(half4(0.0, -1.0, 0.0, 1.0)).rgb);
	float3 defaultLightColor = saturate(max(half3(0.05, 0.05, 0.05) * _Unlit_Intensity, shadeLight * _Unlit_Intensity));

	float3 lightDirection = GetLightDirection(mainLight.direction);
	half3 originalLightColor = mainLightColor.rgb;
	float3 lightColor = lerp(max(defaultLightColor, originalLightColor),  max(defaultLightColor, saturate(originalLightColor)), 1 /*_Is_Filter_LightColor*/);
	
	float3 halfDirection = normalize(viewDirection + lightDirection);
	float lambert = dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToBase), lightDirection);
	float halfLambert = 0.5 * lambert + 0.5;
	_Color = _BaseColor;
	
	float3 baseLightColor = lightColor.rgb;
	float3 diffuseColor = lerp((mainTex.rgb * _Color.rgb), ((mainTex.rgb * _Color.rgb) * baseLightColor), _Is_LightColor_Base);
	
	float4 toonShade1st = lerp(tex2D(_1st_ShadeMap, i.uv0), mainTex, _Use_BaseAs1st);
	float4 toonShade2nd = lerp(tex2D(_2nd_ShadeMap, i.uv0), toonShade1st, _Use_1stAs2nd);
	
	float3 lightColor1stShade = lerp((toonShade1st.rgb * _1st_ShadeColor.rgb), ((toonShade1st.rgb * _1st_ShadeColor.rgb) * baseLightColor), _Is_LightColor_1st_Shade);
	
	float4 shadingGradeMap = tex2Dlod(_ShadingGradeMap, float4(TRANSFORM_TEX(i.uv0, _ShadingGradeMap), 0.0, _BlurLevelSGM));
	
#if !defined (_RAYTRACINGSHADOW_ON)
	shadowAttenuation *= 2.0f;
	shadowAttenuation = saturate(shadowAttenuation);
#endif

	float systemShadowLevel = ((shadowAttenuation * 0.5) + 0.5 + _Tweak_SystemShadowsLevel > 0.001) ? (shadowAttenuation * 0.5) + 0.5 + _Tweak_SystemShadowsLevel : 0.0001;
	
	// _ShadingGradeMapLevel_var 
	float toonShadeLevel = (shadingGradeMap.r < 0.95) ? shadingGradeMap.r + _Tweak_ShadingGradeMapLevel : 1;
	
	float toonShade = saturate(toonShadeLevel) * lerp(halfLambert, (halfLambert * saturate(systemShadowLevel)), _Set_SystemShadowsToBase);
	float finalShadowMask = GetShadowMask(toonShade, _1st_ShadeColor_Step, _1st_ShadeColor_Feather);
	float shadeShadowMask = GetShadowMask(toonShade, _2nd_ShadeColor_Step, _2nd_ShadeColor_Feather);
	
	float3 base2ndA = (toonShade2nd.rgb * _2nd_ShadeColor.rgb);
	float3 base2ndB = ((toonShade2nd.rgb * _2nd_ShadeColor.rgb) * baseLightColor);
	float3 baseLerpA = lerp(diffuseColor, lightColor1stShade, finalShadowMask);
	float3 baseLerpB = lerp(lightColor1stShade, lerp(base2ndA, base2ndB, _Is_LightColor_2nd_Shade), shadeShadowMask);
	float3 finalBaseColor = lerp(baseLerpA, baseLerpB, finalShadowMask);

	// HighColor Setting	
	float3 baseHighColor = SetHighColor(i, normalDirection, halfDirection, baseLightColor, finalBaseColor, finalShadowMask);

	// RimLight Setting
	float3 baseRimLight = SetRimLight(i, normalDirection, viewDirection, lightDirection, baseLightColor);
	float3 rimLight = lerp(baseHighColor, (baseHighColor + baseRimLight), _RimLight);
	
	// Matcap & Emissive UV Settings
	fixed signMirror = i.mirrorFlag;
	float3 cameraRight = UNITY_MATRIX_V[0].xyz;
	float3 cameraForward = UNITY_MATRIX_V[2].xyz;
	float3 upUnlit = float3(0, 1, 0);
	float3 rightAxis = cross(cameraForward, upUnlit);
	_Rotate_MatCapUV = (signMirror < 0) ? -1 * _Rotate_MatCapUV : _Rotate_MatCapUV;
	rightAxis = (signMirror < 0) ? -1 * rightAxis : rightAxis;
	float cameraRightMagnitude = sqrt(cameraRight.x * cameraRight.x + cameraRight.y * cameraRight.y + cameraRight.z * cameraRight.z);
	float rightAxisMagnitude = sqrt(rightAxis.x * rightAxis.x + rightAxis.y * rightAxis.y + rightAxis.z * rightAxis.z);
	float cameraRollCos = dot(rightAxis, cameraRight) / (rightAxisMagnitude * cameraRightMagnitude);
	float cameraRoll = acos(clamp(cameraRollCos, -1, 1));
	fixed cameraDir = (cameraRight.y < 0) ? -1 : 1;

	// Create MatCap coordinate
	float matcapUVAngle = (_Rotate_MatCapUV * PI) - cameraDir * cameraRoll * _Is_CameraRolling;
	float2 rotateMatCapUV = SetMatCapUV(i, tangentTransform, viewDirection, matcapUVAngle, signMirror);
	
	// Create MatCap color
	float3 matCapColorFinal = SetMatCap(i, rotateMatCapUV, baseLightColor, finalShadowMask, baseHighColor, baseRimLight, rimLight);
	float3 finalColor = lerp(rimLight, matCapColorFinal, _MatCap);
	float3 emissiveColor = 0;
	
#ifdef _ANGELRING_OFF
	//
#else
	// _ANGELRING_ON
	finalColor = AdditionalHairSpecular(i, cameraDir, cameraRoll, baseLightColor, finalColor);
#endif
	
#ifdef _EMISSIVE_OFF
	//	
#else
	// _EMISSIVE_ON
	emissiveColor = SetEmissive(i, viewDirection, normalDirection, signMirror, cameraDir, cameraRoll);
#endif

	
	float3 pointLightColor = 0;
#ifdef _ADDITIONAL_LIGHTS
	AddtionalPointLight(i, inputData, mainTex, viewDirection, normalDirection, pointLightColor);
#endif
	
	finalColor = saturate(finalColor) + (envLightColor * envLightIntensity * _GI_Intensity * smoothstep(1, 0, envLightIntensity / 2)) + emissiveColor;
	finalColor += pointLightColor;

#ifdef _IS_TRANSCLIPPING_ON
	float opacity = saturate((inverseClipping + _Tweak_transparency));
	fixed4 finalRGBA = fixed4(finalColor, opacity);
	return finalRGBA;	
#else
	fixed4 finalRGBA = fixed4(finalColor, 1);
	return finalRGBA;	
#endif
}


#endif
