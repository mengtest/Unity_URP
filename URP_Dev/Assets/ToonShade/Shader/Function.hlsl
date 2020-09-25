#ifndef FUNCTION
#define FUNCTION


float2 RotateUV(float2 _uv, float _radian, float2 _piv, float _time)
{
	float RotateUV_ang = _radian;
	float RotateUV_cos = cos(_time * RotateUV_ang);
	float RotateUV_sin = sin(_time * RotateUV_ang);
	return (mul(_uv - _piv, float2x2(RotateUV_cos, -RotateUV_sin, RotateUV_sin, RotateUV_cos)) + _piv);
}

inline void InitializeStandardLitSurfaceDataToonShade(float2 uv, out SurfaceData outSurfaceData)
{
	half4 albedoAlpha = half4(1.0, 1.0, 1.0, 1.0);
	outSurfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);
	half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	outSurfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;
#if _SPECULAR_SETUP
	outSurfaceData.metallic = 1.0h;
	outSurfaceData.specular = specGloss.rgb;
#else
	outSurfaceData.metallic = specGloss.r;
	outSurfaceData.specular = half3(0.0h, 0.0h, 0.0h);
#endif
	outSurfaceData.smoothness = specGloss.a;
	outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	outSurfaceData.occlusion = SampleOcclusion(uv);
	outSurfaceData.emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
}

half3 GlobalIlluminationToonShade(BRDFData brdfData, half3 bakedGI, half occlusion, half3 normalWS, half3 viewDirectionWS)
{
	half3 reflectVector = reflect(-viewDirectionWS, normalWS);
	half fresnelTerm = Pow4(1.0 - saturate(dot(normalWS, viewDirectionWS)));
	half3 indirectDiffuse = bakedGI * occlusion;
	half3 indirectSpecular = GlossyEnvironmentReflection(reflectVector, brdfData.perceptualRoughness, occlusion);
	return EnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);
}

struct ToonLight
{
	float3 direction;
	float3 color;
	float distanceAttenuation;
	float shadowAttenuation;
	int type;
};

#define INIT_TOONLIGHT(toonlight) \
	toonlight.direction = 0; \
	toonlight.color = 0; \
	toonlight.distanceAttenuation = 0; \
	toonlight.shadowAttenuation = 0; \
	toonlight.type = 0

half MainLightRealtimeShadowToonShade(float4 shadowCoord, float4 positionCS)
{
#if !defined(MAIN_LIGHT_CALCULATE_SHADOWS)
	return 1.0h;
#endif
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	half4 shadowParams = GetMainLightShadowParams();
#if defined(_RAYTRACINGSHADOW_ON)
	float4 screenPos =  ComputeScreenPos(positionCS/ positionCS.w);
	return SAMPLE_TEXTURE2D(_RaytracedHardShadow, sampler_RaytracedHardShadow, screenPos);
#endif 
	return SampleShadowmap(
		TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), 
		shadowCoord, 
		shadowSamplingData, 
		shadowParams, 
		false);
}

half AdditionalLightRealtimeShadowToonShade(int lightIndex, float3 positionWS, float4 positionCS)
{
#if defined(_RAYTRACINGSHADOW_ON)
	float4 screenPos = ComputeScreenPos(positionCS / positionCS.w);
	return SAMPLE_TEXTURE2D(_RaytracedHardShadow, sampler_RaytracedHardShadow, screenPos);
#endif

#if !defined(ADDITIONAL_LIGHT_CALCULATE_SHADOWS)
	return 1.0h;
#endif
	ShadowSamplingData shadowSamplingData = GetAdditionalLightShadowSamplingData();

#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
	lightIndex = _AdditionalShadowsIndices[lightIndex];

	UNITY_BRANCH
	if (lightIndex < 0)
		return 1.0;

	float4 shadowCoord = mul(_AdditionalShadowsBuffer[lightIndex].worldToShadowMatrix, float4(positionWS, 1.0));
#else
	float4 shadowCoord = mul(_AdditionalLightsWorldToShadow[lightIndex], float4(positionWS, 1.0));
#endif

	half4 shadowParams = GetAdditionalLightShadowParams(lightIndex);
	return SampleShadowmap(
		TEXTURE2D_ARGS(_AdditionalLightsShadowmapTexture, sampler_AdditionalLightsShadowmapTexture), 
		shadowCoord, 
		shadowSamplingData, 
		shadowParams, 
		true);
}

ToonLight GetMainToonLight()
{
	ToonLight light;
	light.direction = _MainLightPosition.xyz;
	light.distanceAttenuation = unity_LightData.z;
#if defined(LIGHTMAP_ON) || defined(_MIXED_LIGHTING_SUBTRACTIVE)
	light.distanceAttenuation *= unity_ProbesOcclusion.x;
#endif
	light.shadowAttenuation = 1.0;
	light.color = _MainLightColor.rgb;
	light.type = _MainLightPosition.w;
	return light;
}

ToonLight GetMainToonLight(float4 shadowCoord, float4 positionCS)
{
	ToonLight light = GetMainToonLight();
	light.shadowAttenuation = MainLightRealtimeShadowToonShade(shadowCoord, positionCS);
	return light;
}

ToonLight GetAdditionalPerObjectToonLight(int perObjectLightIndex, float3 positionWS, float4 positionCS)
{
#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
	float4 lightPositionWS = _AdditionalLightsBuffer[perObjectLightIndex].position;
	half3 color = _AdditionalLightsBuffer[perObjectLightIndex].color.rgb;
	half4 distanceAndSpotAttenuation = _AdditionalLightsBuffer[perObjectLightIndex].attenuation;
	half4 spotDirection = _AdditionalLightsBuffer[perObjectLightIndex].spotDirection;
	half4 lightOcclusionProbeInfo = _AdditionalLightsBuffer[perObjectLightIndex].occlusionProbeChannels;
#else
	float4 lightPositionWS = _AdditionalLightsPosition[perObjectLightIndex];
	half3 color = _AdditionalLightsColor[perObjectLightIndex].rgb;
	half4 distanceAndSpotAttenuation = _AdditionalLightsAttenuation[perObjectLightIndex];
	half4 spotDirection = _AdditionalLightsSpotDir[perObjectLightIndex];
	half4 lightOcclusionProbeInfo = _AdditionalLightsOcclusionProbes[perObjectLightIndex];
#endif
	
	float3 lightVector = lightPositionWS.xyz - positionWS * lightPositionWS.w;
	float distanceSqr = max(dot(lightVector, lightVector), HALF_MIN);
	half3 lightDirection = half3(lightVector * rsqrt(distanceSqr));
	half attenuation = DistanceAttenuation(distanceSqr, distanceAndSpotAttenuation.xy) * 
		AngleAttenuation(spotDirection.xyz, lightDirection, distanceAndSpotAttenuation.zw);

	ToonLight light;
	light.direction = lightDirection;
	light.distanceAttenuation = attenuation;
	light.shadowAttenuation = AdditionalLightRealtimeShadowToonShade(perObjectLightIndex, positionWS, positionCS);
	light.color = color;
	light.type = lightPositionWS.w;

#if defined(LIGHTMAP_ON) || defined(_MIXED_LIGHTING_SUBTRACTIVE)
	int probeChannel = lightOcclusionProbeInfo.x;
	half lightProbeContribution = lightOcclusionProbeInfo.y;
	half probeOcclusionValue = unity_ProbesOcclusion[probeChannel];
	light.distanceAttenuation *= max(probeOcclusionValue, lightProbeContribution);
#endif
	return light;
}

ToonLight GetAdditionalToonLight(uint i, float3 positionWS, float4 positionCS)
{
	int perObjectLightIndex = GetPerObjectLightIndex(i);
	return GetAdditionalPerObjectToonLight(perObjectLightIndex, positionWS, positionCS);
}

half3 GetLightColor(ToonLight light)
{
	return light.color * light.distanceAttenuation;
}

int DetermineToonShadeMainLightIndex(float3 posW, float4 shadowCoord, float4 positionCS)
{
	ToonLight mainLight;
	INIT_TOONLIGHT(mainLight);

	int mainLightIndex = -2;
	ToonLight nextLight = GetMainToonLight(shadowCoord, positionCS);
	if (nextLight.distanceAttenuation > mainLight.distanceAttenuation && nextLight.type == 0)
	{
		mainLight = nextLight;
		mainLightIndex = -1;
	}
	int lightCount = GetAdditionalLightsCount();
	for (int ii = 0; ii < lightCount; ++ii)
	{
		nextLight = GetAdditionalToonLight(ii, posW, positionCS);
		if (nextLight.distanceAttenuation > mainLight.distanceAttenuation && nextLight.type == 0)
		{
			mainLight = nextLight;
			mainLightIndex = ii;
		}
	}
	return mainLightIndex;
}

ToonLight GetMainToonShadeLightByID(int index, float3 posW, float4 shadowCoord, float4 positionCS)
{
	ToonLight mainLight;
	INIT_TOONLIGHT(mainLight);
	if (index == -2)
	{
		return mainLight;
	}
	if (index == -1)
	{
		return GetMainToonLight(shadowCoord, positionCS);
	}
	return GetAdditionalToonLight(index, posW, positionCS);
}

float3 GetLightDirection(float3 direction)
{
	float3 defaultLightDirection = normalize(UNITY_MATRIX_V[2].xyz + UNITY_MATRIX_V[1].xyz);

#if false	
	float3 customLightDirection = normalize(
		mul(unity_ObjectToWorld,
			float4((
				(float3(1.0, 0.0, 0.0) * _Offset_X_Axis_BLD * 10) +
				(float3(0.0, 1.0, 0.0) * _Offset_Y_Axis_BLD * 10) +
				(float3(0.0, 0.0, -1.0) * lerp(-1.0, 1.0, _Inverse_Z_Axis_BLD))
			), 0)).xyz);
	
	float3 lightDirection = normalize(lerp(defaultLightDirection, direction.xyz, any(direction.xyz)));
	lightDirection = lerp(lightDirection, customLightDirection, _Is_BLD);
#endif
	
	float3 lightDirection = normalize(lerp(defaultLightDirection, direction.xyz, any(direction.xyz)));
	return lightDirection;
}


#endif // FUNCTION
