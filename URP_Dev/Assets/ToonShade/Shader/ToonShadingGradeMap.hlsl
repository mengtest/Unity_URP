#ifndef SHADING_GRADEMAP
#define SHADING_GRADEMAP


#if (SHADER_LIBRARY_VERSION_MAJOR ==7 && SHADER_LIBRARY_VERSION_MINOR >= 3) || (SHADER_LIBRARY_VERSION_MAJOR >= 8)

	#ifdef _ADDITIONAL_LIGHTS
		#ifndef  REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
			#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
		#endif
	#endif
#else
	#ifdef _MAIN_LIGHT_SHADOWS
		#ifndef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
		#endif
	#endif

	#ifdef _ADDITIONAL_LIGHTS
		#ifndef REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
			#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
		#endif
	#endif
#endif


sampler2D _MainTex;
float4 _MainTex_ST;
#if TOON_URP
#else
float4 _BaseColor;
#endif

float4 _Color;
fixed _Use_BaseAs1st;
fixed _Use_1stAs2nd;
fixed _Is_LightColor_Base;

sampler2D _1st_ShadeMap; float4 _1st_ShadeMap_ST;
float4 _1st_ShadeColor;
fixed _Is_LightColor_1st_Shade;
sampler2D _2nd_ShadeMap; float4 _2nd_ShadeMap_ST;
float4 _2nd_ShadeColor;
fixed _Is_LightColor_2nd_Shade;
sampler2D _NormalMap; float4 _NormalMap_ST;
fixed _Is_NormalMapToBase;
fixed _Set_SystemShadowsToBase;
float _Tweak_SystemShadowsLevel;
sampler2D _ShadingGradeMap; float4 _ShadingGradeMap_ST;
float _Tweak_ShadingGradeMapLevel;
fixed _BlurLevelSGM;
float _1st_ShadeColor_Step;
float _1st_ShadeColor_Feather;
float _2nd_ShadeColor_Step;
float _2nd_ShadeColor_Feather;


float4 _HighColor;
sampler2D _HighColor_Tex; float4 _HighColor_Tex_ST;
fixed _Is_LightColor_HighColor;
fixed _Is_NormalMapToHighColor;
float _HighColor_Power;
fixed _Is_SpecularToHighColor;
fixed _Is_BlendAddToHiColor;
fixed _Is_UseTweakHighColorOnShadow;
float _TweakHighColorOnShadow;
sampler2D _Set_HighColorMask; float4 _Set_HighColorMask_ST;
float _Tweak_HighColorMaskLevel;

fixed _RimLight;
float4 _RimLightColor;
fixed _Is_LightColor_RimLight;
fixed _Is_NormalMapToRimLight;
float _RimLight_Power;
float _RimLight_InsideMask;
fixed _RimLight_FeatherOff;
fixed _LightDirection_MaskOn;
float _Tweak_LightDirection_MaskLevel;
fixed _Add_Antipodean_RimLight;
float4 _Ap_RimLightColor;
fixed _Is_LightColor_Ap_RimLight;
float _Ap_RimLight_Power;
fixed _Ap_RimLight_FeatherOff;
sampler2D _Set_RimLightMask; float4 _Set_RimLightMask_ST;
float _Tweak_RimLightMaskLevel;

fixed _MatCap;
sampler2D _MatCap_Sampler; float4 _MatCap_Sampler_ST;
float4 _MatCapColor;
fixed _Is_LightColor_MatCap;
fixed _Is_BlendAddToMatCap;
float _Tweak_MatCapUV;
float _Rotate_MatCapUV;
fixed _Is_NormalMapForMatCap;
sampler2D _NormalMapForMatCap; float4 _NormalMapForMatCap_ST;
float _Rotate_NormalMapForMatCapUV;
fixed _Is_UseTweakMatCapOnShadow;
float _TweakMatCapOnShadow;
sampler2D _Set_MatcapMask; float4 _Set_MatcapMask_ST;
float _Tweak_MatcapMaskLevel;
fixed _Is_Ortho;
float _CameraRolling_Stabilizer;
fixed _BlurLevelMatcap;
fixed _Inverse_MatcapMask;
#if TOON_URP
#else
float _BumpScale;
#endif
float _BumpScaleMatcap;

sampler2D _Emissive_Tex; float4 _Emissive_Tex_ST;
float4 _Emissive_Color;
fixed _Is_ViewCoord_Scroll;
float _Rotate_EmissiveUV;
float _Base_Speed;
float _Scroll_EmissiveU;
float _Scroll_EmissiveV;
fixed _Is_PingPong_Base;
float4 _ColorShift;
float4 _ViewShift;
float _ColorShift_Speed;
fixed _Is_ColorShift;
fixed _Is_ViewShift;
float3 emissive;

float _Unlit_Intensity;
fixed _Is_Filter_HiCutPointLightColor;
fixed _Is_Filter_LightColor;
float _StepOffset;
fixed _Is_BLD;
float _Offset_X_Axis_BLD;
float _Offset_Y_Axis_BLD;
fixed _Inverse_Z_Axis_BLD;

#ifdef _IS_TRANSCLIPPING_OFF
#elif _IS_TRANSCLIPPING_ON
	sampler2D _ClippingMask; float4 _ClippingMask_ST;
	fixed _IsBaseMapAlphaAsClippingMask;
	float _Clipping_Level;
	fixed _Inverse_Clipping;
	float _Tweak_transparency;
#endif

#define UNITY_PROJ_COORD(a) a
#define UNITY_SAMPLE_SCREEN_SHADOW(tex, uv) tex2Dproj( tex, UNITY_PROJ_COORD(uv) ).r
#define TEXTURE2D_SAMPLER2D(textureName, samplerName) Texture2D textureName; SamplerState samplerName 
		TEXTURE2D_SAMPLER2D(_RaytracedHardShadow, sampler_RaytracedHardShadow);

//float4 _RaytracedHardShadow_TexelSize;


float2 RotateUV(float2 _uv, float _radian, float2 _piv, float _time)
{
	float RotateUV_ang = _radian;
	float RotateUV_cos = cos(_time * RotateUV_ang);
	float RotateUV_sin = sin(_time * RotateUV_ang);
	return (mul(_uv - _piv, float2x2(RotateUV_cos, -RotateUV_sin, RotateUV_sin, RotateUV_cos)) + _piv);
}

fixed3 DecodeLightProbe(fixed3 N)
{
	return ShadeSH9(float4(N, 1));
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

float _GI_Intensity;
#ifdef _IS_ANGELRING_ON
	fixed _AngelRing;
	sampler2D _AngelRing_Sampler; float4 _AngelRing_Sampler_ST;
	float4 _AngelRing_Color;
	fixed _Is_LightColor_AR;
	float _AR_OffsetU;
	float _AR_OffsetV;
	fixed _ARSampler_AlphaOn;
#endif

struct VertexInput
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
	float2 texcoord0 : TEXCOORD0;
#ifdef _IS_ANGELRING_ON
	float2 texcoord1 : TEXCOORD1;
	float2 lightmapUV : TEXCOORD2;	
#else
	float2 lightmapUV : TEXCOORD1;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct VertexOutput
{
	// half4 fogFactorAndVertexLight : TEXCOORD7; 
	// x: fogFactor, yzw: vertex light
	float4 pos : SV_POSITION;
	float2 uv0 : TEXCOORD0;
#ifdef _IS_ANGELRING_OFF
	float4 posWorld : TEXCOORD1;
	float3 normalDir : TEXCOORD2;
	float3 tangentDir : TEXCOORD3;
	float3 bitangentDir : TEXCOORD4;
	float mirrorFlag : TEXCOORD5;
	DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 6);
	half4 fogFactorAndVertexLight : TEXCOORD7;
	#ifndef _MAIN_LIGHT_SHADOWS
		float4 positionCS : TEXCOORD8;
		int mainLightID : TEXCOORD9;
	#else
		float4 shadowCoord : TEXCOORD8;
		float4 positionCS : TEXCOORD9;
		int mainLightID : TEXCOORD10;
	#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO	
	
#elif _IS_ANGELRING_ON
	float2 uv1 : TEXCOORD1;
	float4 posWorld : TEXCOORD2;
	float3 normalDir : TEXCOORD3;
	float3 tangentDir : TEXCOORD4;
	float3 bitangentDir : TEXCOORD5;
	float mirrorFlag : TEXCOORD6;
	DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 7);
	half4 fogFactorAndVertexLight : TEXCOORD8;
	#ifndef _MAIN_LIGHT_SHADOWS
		float4 positionCS : TEXCOORD9;
		int mainLightID : TEXCOORD10;
	#else
		float4 shadowCoord : TEXCOORD9;
		float4 positionCS : TEXCOORD10;
		int mainLightID : TEXCOORD11;
	#endif	
	
#else
	LIGHTING_COORDS(7,8)
	UNITY_FOG_COORDS(9)
#endif
};

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
#if defined(USE_RAYTRACING_SHADOW)
	float4 screenPos =  ComputeScreenPos(positionCS/ positionCS.w);
	return SAMPLE_TEXTURE2D(_RaytracedHardShadow, sampler_RaytracedHardShadow, screenPos);
#endif 
	return SampleShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData, shadowParams, false);
}

half AdditionalLightRealtimeShadowToonShade(int lightIndex, float3 positionWS, float4 positionCS)
{
#if defined(USE_RAYTRACING_SHADOW)
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
	return SampleShadowmap(TEXTURE2D_ARGS(_AdditionalLightsShadowmapTexture, sampler_AdditionalLightsShadowmapTexture), shadowCoord, shadowSamplingData, shadowParams, true);
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
	half attenuation = DistanceAttenuation(distanceSqr, distanceAndSpotAttenuation.xy) * AngleAttenuation(spotDirection.xyz, lightDirection, distanceAndSpotAttenuation.zw);

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

int DetermineToonShade_MainLightIndex(float3 posW, float4 shadowCoord, float4 positionCS)
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

// VSInput
VertexOutput vert(VertexInput v)
{
	VertexOutput o = (VertexOutput) 0;
	
	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
	
	o.uv0 = v.texcoord0;
#ifdef _IS_ANGELRING_ON
	o.uv1 = v.texcoord1;
#endif
	o.normalDir = UnityObjectToWorldNormal(v.normal);
	o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
	o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
	
	//float3 lightColor = _LightColor0.rgb;
	o.pos = UnityObjectToClipPos(v.vertex);
	float3 crossFwd = cross(UNITY_MATRIX_V[0], UNITY_MATRIX_V[1]);
	o.mirrorFlag = dot(crossFwd, UNITY_MATRIX_V[2]) < 0 ? 1 : -1;
	
	float3 positionWS = TransformObjectToWorld(v.vertex);
	float4 positionCS = TransformWorldToHClip(positionWS);
	half3 vertexLight = VertexLighting(o.posWorld, o.normalDir);
	half fogFactor = ComputeFogFactor(positionCS.z);
	OUTPUT_LIGHTMAP_UV(v.lightmapUV, unity_LightmapST, o.lightmapUV);
	OUTPUT_SH(o.normalDir.xyz, o.vertexSH);
	o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
	o.positionCS = positionCS;
	
#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
	#if SHADOWS_SCREEN
		o.shadowCoord = ComputeScreenPos(positionCS);
	#else
		o.shadowCoord = TransformWorldToShadowCoord(o.posWorld);
	#endif
	o.mainLightID = DetermineToonShade_MainLightIndex(o.posWorld, o.shadowCoord, positionCS);
	
#else
	o.mainLightID = DetermineToonShade_MainLightIndex(o.posWorld, 0, positionCS);
#endif
	return o;
}


// PSInput
float4 frag(VertexOutput i, fixed facing : VFACE) : SV_TARGET
{
	i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
	float2 Set_UV0 = i.uv0;
	
	float3 _NormalMap_var = UnpackNormalScale(tex2D(_NormalMap, TRANSFORM_TEX(Set_UV0, _NormalMap)), _BumpScale);
	float3 normalLocal = _NormalMap_var.rgb;
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

	ToonLight mainLight = GetMainToonShadeLightByID(i.mainLightID, i.posWorld.xyz, inputData.shadowCoord, i.positionCS);
	half3 mainLightColor = GetLightColor(mainLight);

	float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(Set_UV0, _MainTex));
	
#ifdef _IS_TRANSCLIPPING_OFF
#elif _IS_TRANSCLIPPING_ON
	float4 _ClippingMask_var = tex2D(_ClippingMask,TRANSFORM_TEX(Set_UV0, _ClippingMask));
	float Set_MainTexAlpha = _MainTex_var.a;
	float _IsBaseMapAlphaAsClippingMask_var = lerp( _ClippingMask_var.r, Set_MainTexAlpha, _IsBaseMapAlphaAsClippingMask );
	float _Inverse_Clipping_var = lerp( _IsBaseMapAlphaAsClippingMask_var, (1.0 - _IsBaseMapAlphaAsClippingMask_var), _Inverse_Clipping );
	float Set_Clipping = saturate((_Inverse_Clipping_var+_Clipping_Level));
	clip(Set_Clipping - 0.5);
#endif

	half shadowAttenuation = 1.0;

#ifdef _MAIN_LIGHT_SHADOWS
	shadowAttenuation = mainLight.shadowAttenuation;
#endif


	float3 defaultLightDirection = normalize(UNITY_MATRIX_V[2].xyz + UNITY_MATRIX_V[1].xyz);
	float3 defaultLightColor = saturate(max(half3(0.05, 0.05, 0.05) * _Unlit_Intensity, max(ShadeSH9(half4(0.0, 0.0, 0.0, 1.0)), ShadeSH9(half4(0.0, -1.0, 0.0, 1.0)).rgb) * _Unlit_Intensity));
	float3 customLightDirection = normalize(mul(unity_ObjectToWorld, float4(((float3(1.0, 0.0, 0.0) * _Offset_X_Axis_BLD * 10) + (float3(0.0, 1.0, 0.0) * _Offset_Y_Axis_BLD * 10) + (float3(0.0, 0.0, -1.0) * lerp(-1.0, 1.0, _Inverse_Z_Axis_BLD))), 0)).xyz);
	float3 lightDirection = normalize(lerp(defaultLightDirection, mainLight.direction.xyz, any(mainLight.direction.xyz)));
	lightDirection = lerp(lightDirection, customLightDirection, _Is_BLD);

	half3 originalLightColor = mainLightColor.rgb;
	float3 lightColor = lerp(max(defaultLightColor, originalLightColor), max(defaultLightColor, saturate(originalLightColor)), _Is_Filter_LightColor);



	float3 halfDirection = normalize(viewDirection + lightDirection);
	_Color = _BaseColor;

	float3 Set_LightColor = lightColor.rgb;
	float3 Set_BaseColor = lerp((_MainTex_var.rgb * _BaseColor.rgb), ((_MainTex_var.rgb * _BaseColor.rgb) * Set_LightColor), _Is_LightColor_Base);
	float4 _1st_ShadeMap_var = lerp(tex2D(_1st_ShadeMap, TRANSFORM_TEX(Set_UV0, _1st_ShadeMap)), _MainTex_var, _Use_BaseAs1st);
	float3 _Is_LightColor_1st_Shade_var = lerp((_1st_ShadeMap_var.rgb * _1st_ShadeColor.rgb), ((_1st_ShadeMap_var.rgb * _1st_ShadeColor.rgb) * Set_LightColor), _Is_LightColor_1st_Shade);
	float _HalfLambert_var = 0.5 * dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToBase), lightDirection) + 0.5; // Half Lambert
	float4 _ShadingGradeMap_var = tex2Dlod(_ShadingGradeMap, float4(TRANSFORM_TEX(Set_UV0, _ShadingGradeMap), 0.0, _BlurLevelSGM));
	
#if !defined (USE_RAYTRACING_SHADOW)
	shadowAttenuation *= 2.0f;
	shadowAttenuation = saturate(shadowAttenuation);
#endif

	float _SystemShadowsLevel_var = (shadowAttenuation * 0.5) + 0.5 + _Tweak_SystemShadowsLevel > 0.001 ? (shadowAttenuation * 0.5) + 0.5 + _Tweak_SystemShadowsLevel : 0.0001;
	float _ShadingGradeMapLevel_var = _ShadingGradeMap_var.r < 0.95 ? _ShadingGradeMap_var.r + _Tweak_ShadingGradeMapLevel : 1;
	float Set_ShadingGrade = saturate(_ShadingGradeMapLevel_var) * lerp(_HalfLambert_var, (_HalfLambert_var * saturate(_SystemShadowsLevel_var)), _Set_SystemShadowsToBase);
	float Set_FinalShadowMask = saturate((1.0 + ((Set_ShadingGrade - (_1st_ShadeColor_Step - _1st_ShadeColor_Feather)) * (0.0 - 1.0)) / (_1st_ShadeColor_Step - (_1st_ShadeColor_Step - _1st_ShadeColor_Feather)))); // Base and 1st Shade Mask
	float3 _BaseColor_var = lerp(Set_BaseColor, _Is_LightColor_1st_Shade_var, Set_FinalShadowMask);
	float4 _2nd_ShadeMap_var = lerp(tex2D(_2nd_ShadeMap, TRANSFORM_TEX(Set_UV0, _2nd_ShadeMap)), _1st_ShadeMap_var, _Use_1stAs2nd);
	float Set_ShadeShadowMask = saturate((1.0 + ((Set_ShadingGrade - (_2nd_ShadeColor_Step - _2nd_ShadeColor_Feather)) * (0.0 - 1.0)) / (_2nd_ShadeColor_Step - (_2nd_ShadeColor_Step - _2nd_ShadeColor_Feather)))); // 1st and 2nd Shades Mask
	float3 Set_FinalBaseColor = lerp(_BaseColor_var, lerp(_Is_LightColor_1st_Shade_var, lerp((_2nd_ShadeMap_var.rgb * _2nd_ShadeColor.rgb), ((_2nd_ShadeMap_var.rgb * _2nd_ShadeColor.rgb) * Set_LightColor), _Is_LightColor_2nd_Shade), Set_ShadeShadowMask), Set_FinalShadowMask);
	float4 _Set_HighColorMask_var = tex2D(_Set_HighColorMask, TRANSFORM_TEX(Set_UV0, _Set_HighColorMask));
	float _Specular_var = 0.5 * dot(halfDirection, lerp(i.normalDir, normalDirection, _Is_NormalMapToHighColor)) + 0.5; // Specular
	float _TweakHighColorMask_var = (saturate((_Set_HighColorMask_var.g + _Tweak_HighColorMaskLevel)) * lerp((1.0 - step(_Specular_var, (1.0 - pow(_HighColor_Power, 5)))), pow(_Specular_var, exp2(lerp(11, 1, _HighColor_Power))), _Is_SpecularToHighColor));
	float4 _HighColor_Tex_var = tex2D(_HighColor_Tex, TRANSFORM_TEX(Set_UV0, _HighColor_Tex));
	float3 _HighColor_var = (lerp((_HighColor_Tex_var.rgb * _HighColor.rgb), ((_HighColor_Tex_var.rgb * _HighColor.rgb) * Set_LightColor), _Is_LightColor_HighColor) * _TweakHighColorMask_var);
	float3 Set_HighColor = (lerp(saturate((Set_FinalBaseColor - _TweakHighColorMask_var)), Set_FinalBaseColor, lerp(_Is_BlendAddToHiColor, 1.0, _Is_SpecularToHighColor)) + lerp(_HighColor_var, (_HighColor_var * ((1.0 - Set_FinalShadowMask) + (Set_FinalShadowMask * _TweakHighColorOnShadow))), _Is_UseTweakHighColorOnShadow));

	float4 _Set_RimLightMask_var = tex2D(_Set_RimLightMask, TRANSFORM_TEX(Set_UV0, _Set_RimLightMask));
	float3 _Is_LightColor_RimLight_var = lerp(_RimLightColor.rgb, (_RimLightColor.rgb * Set_LightColor), _Is_LightColor_RimLight);
	float _RimArea_var = (1.0 - dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToRimLight), viewDirection));
	float _RimLightPower_var = pow(_RimArea_var, exp2(lerp(3, 0, _RimLight_Power)));
	float _Rimlight_InsideMask_var = saturate(lerp((0.0 + ((_RimLightPower_var - _RimLight_InsideMask) * (1.0 - 0.0)) / (1.0 - _RimLight_InsideMask)), step(_RimLight_InsideMask, _RimLightPower_var), _RimLight_FeatherOff));
	float _VertHalfLambert_var = 0.5 * dot(i.normalDir, lightDirection) + 0.5;
	float3 _LightDirection_MaskOn_var = lerp((_Is_LightColor_RimLight_var * _Rimlight_InsideMask_var), (_Is_LightColor_RimLight_var * saturate((_Rimlight_InsideMask_var - ((1.0 - _VertHalfLambert_var) + _Tweak_LightDirection_MaskLevel)))), _LightDirection_MaskOn);
	float _ApRimLightPower_var = pow(_RimArea_var, exp2(lerp(3, 0, _Ap_RimLight_Power)));
	float3 Set_RimLight = (saturate((_Set_RimLightMask_var.g + _Tweak_RimLightMaskLevel)) * lerp(_LightDirection_MaskOn_var, (_LightDirection_MaskOn_var + (lerp(_Ap_RimLightColor.rgb, (_Ap_RimLightColor.rgb * Set_LightColor), _Is_LightColor_Ap_RimLight) * saturate((lerp((0.0 + ((_ApRimLightPower_var - _RimLight_InsideMask) * (1.0 - 0.0)) / (1.0 - _RimLight_InsideMask)), step(_RimLight_InsideMask, _ApRimLightPower_var), _Ap_RimLight_FeatherOff) - (saturate(_VertHalfLambert_var) + _Tweak_LightDirection_MaskLevel))))), _Add_Antipodean_RimLight));
	float3 _RimLight_var = lerp(Set_HighColor, (Set_HighColor + Set_RimLight), _RimLight);
	fixed _sign_Mirror = i.mirrorFlag;
	float3 _Camera_Right = UNITY_MATRIX_V[0].xyz;
	float3 _Camera_Front = UNITY_MATRIX_V[2].xyz;
	float3 _Up_Unit = float3(0, 1, 0);
	float3 _Right_Axis = cross(_Camera_Front, _Up_Unit);
	
	if (_sign_Mirror < 0)
	{
		_Right_Axis = -1 * _Right_Axis;
		_Rotate_MatCapUV = -1 * _Rotate_MatCapUV;
	}
	else
	{
		_Right_Axis = _Right_Axis;
	}
	float _Camera_Right_Magnitude = sqrt(_Camera_Right.x * _Camera_Right.x + _Camera_Right.y * _Camera_Right.y + _Camera_Right.z * _Camera_Right.z);
	float _Right_Axis_Magnitude = sqrt(_Right_Axis.x * _Right_Axis.x + _Right_Axis.y * _Right_Axis.y + _Right_Axis.z * _Right_Axis.z);
	float _Camera_Roll_Cos = dot(_Right_Axis, _Camera_Right) / (_Right_Axis_Magnitude * _Camera_Right_Magnitude);
	float _Camera_Roll = acos(clamp(_Camera_Roll_Cos, -1, 1));
	fixed _Camera_Dir = _Camera_Right.y < 0 ? -1 : 1;
	float _Rot_MatCapUV_var_ang = (_Rotate_MatCapUV * 3.141592654) - _Camera_Dir * _Camera_Roll * _CameraRolling_Stabilizer;
	float2 _Rot_MatCapNmUV_var = RotateUV(Set_UV0, (_Rotate_NormalMapForMatCapUV * 3.141592654), float2(0.5, 0.5), 1.0);
	float3 _NormalMapForMatCap_var = UnpackNormalScale(tex2D(_NormalMapForMatCap, TRANSFORM_TEX(_Rot_MatCapNmUV_var, _NormalMapForMatCap)), _BumpScaleMatcap);
	
	float3 viewNormal = (mul(UNITY_MATRIX_V, float4(lerp(i.normalDir, mul(_NormalMapForMatCap_var.rgb, tangentTransform).rgb, _Is_NormalMapForMatCap), 0))).rgb;
	float3 NormalBlend_MatcapUV_Detail = viewNormal.rgb * float3(-1, -1, 1);
	float3 NormalBlend_MatcapUV_Base = (mul(UNITY_MATRIX_V, float4(viewDirection, 0)).rgb * float3(-1, -1, 1)) + float3(0, 0, 1);
	float3 noSknewViewNormal = NormalBlend_MatcapUV_Base * dot(NormalBlend_MatcapUV_Base, NormalBlend_MatcapUV_Detail) / NormalBlend_MatcapUV_Base.b - NormalBlend_MatcapUV_Detail;
	float2 _ViewNormalAsMatCapUV = (lerp(noSknewViewNormal, viewNormal, _Is_Ortho).rg * 0.5) + 0.5;
	float2 _Rot_MatCapUV_var = RotateUV((0.0 + ((_ViewNormalAsMatCapUV - (0.0 + _Tweak_MatCapUV)) * (1.0 - 0.0)) / ((1.0 - _Tweak_MatCapUV) - (0.0 + _Tweak_MatCapUV))), _Rot_MatCapUV_var_ang, float2(0.5, 0.5), 1.0);
	if (_sign_Mirror < 0)
	{
		_Rot_MatCapUV_var.x = 1 - _Rot_MatCapUV_var.x;
	}
	else
	{
		_Rot_MatCapUV_var = _Rot_MatCapUV_var;
	}

	float4 _MatCap_Sampler_var = tex2Dlod(_MatCap_Sampler, float4(TRANSFORM_TEX(_Rot_MatCapUV_var, _MatCap_Sampler), 0.0, _BlurLevelMatcap));
	float4 _Set_MatcapMask_var = tex2D(_Set_MatcapMask, TRANSFORM_TEX(Set_UV0, _Set_MatcapMask));
	float _Tweak_MatcapMaskLevel_var = saturate(lerp(_Set_MatcapMask_var.g, (1.0 - _Set_MatcapMask_var.g), _Inverse_MatcapMask) + _Tweak_MatcapMaskLevel);
	float3 _Is_LightColor_MatCap_var = lerp((_MatCap_Sampler_var.rgb * _MatCapColor.rgb), ((_MatCap_Sampler_var.rgb * _MatCapColor.rgb) * Set_LightColor), _Is_LightColor_MatCap);
	float3 Set_MatCap = lerp(
		_Is_LightColor_MatCap_var,
		(_Is_LightColor_MatCap_var * ((1.0 - Set_FinalShadowMask) + (Set_FinalShadowMask * _TweakMatCapOnShadow)) +
			lerp(Set_HighColor * Set_FinalShadowMask * (1.0 - _TweakMatCapOnShadow),
			float3(0.0, 0.0, 0.0), _Is_BlendAddToMatCap)
		), _Is_UseTweakMatCapOnShadow);
	
	float3 matCapColorOnAddMode = _RimLight_var + Set_MatCap * _Tweak_MatcapMaskLevel_var;
	float _Tweak_MatcapMaskLevel_var_MultiplyMode = _Tweak_MatcapMaskLevel_var * lerp(1, (1 - (Set_FinalShadowMask) * (1 - _TweakMatCapOnShadow)), _Is_UseTweakMatCapOnShadow);
	float3 matCapColorOnMultiplyMode = Set_HighColor * (1 - _Tweak_MatcapMaskLevel_var_MultiplyMode) + Set_HighColor * Set_MatCap * _Tweak_MatcapMaskLevel_var_MultiplyMode + lerp(float3(0, 0, 0), Set_RimLight, _RimLight);
	float3 matCapColorFinal = lerp(matCapColorOnMultiplyMode, matCapColorOnAddMode, _Is_BlendAddToMatCap);
	
	
#ifdef _IS_ANGELRING_ON
	float3 finalColor = lerp(_RimLight_var, matCapColorFinal, _MatCap);
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
	float3 noSknewViewNormal_Emissive = NormalBlend_Emissive_Base*dot(NormalBlend_Emissive_Base, NormalBlend_Emissive_Detail) / 
		NormalBlend_Emissive_Base.z - NormalBlend_Emissive_Detail;
	float2 _ViewNormalAsEmissiveUV = noSknewViewNormal_Emissive.xy * 0.5 + 0.5;
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
	float3 envLightColor = envColor.rgb;
	float envLightIntensity = 0.299 * envLightColor.r + 0.587 * envLightColor.g + 0.114 * envLightColor.b < 1 ? (0.299 * envLightColor.r + 0.587 * envLightColor.g + 0.114 * envLightColor.b) : 1;
	float3 pointLightColor = 0;
	
//#ifdef _ADDITIONAL_LIGHTS
	int pixelLightCount = GetAdditionalLightsCount();

	for (int iLight = -1; iLight < pixelLightCount; ++iLight)
	{
		if (iLight != i.mainLightID)
		{
			float notDirectional = 1.0f;
			ToonLight additionalLight = GetMainToonLight(0, 0);
			if (iLight != -1)
			{
				additionalLight = GetAdditionalToonLight(iLight, inputData.positionWS, i.positionCS);
			}
			half3 additionalLightColor = GetLightColor(additionalLight);
			float3 lightDirection = additionalLight.direction;
			float3 addPassLightColor = (0.5 * dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToBase), lightDirection) + 0.5) * additionalLightColor.rgb;
			float pureIntencity = max(0.001, (0.299 * additionalLightColor.r + 0.587 * additionalLightColor.g + 0.114 * additionalLightColor.b));
			float3 lightColor = max(0, lerp(addPassLightColor, lerp(0, min(addPassLightColor, addPassLightColor / pureIntencity), notDirectional), _Is_Filter_LightColor));
			float3 halfDirection = normalize(viewDirection + lightDirection);

			_1st_ShadeColor_Step = saturate(_1st_ShadeColor_Step + _StepOffset);
			_2nd_ShadeColor_Step = saturate(_2nd_ShadeColor_Step + _StepOffset);
			
			float _LightIntensity = lerp(0, (0.299 * additionalLightColor.r + 0.587 * additionalLightColor.g + 0.114 * additionalLightColor.b), notDirectional);
			float3 Set_LightColor = lerp(lightColor, lerp(lightColor, min(lightColor, additionalLightColor.rgb * _1st_ShadeColor_Step), notDirectional), _Is_Filter_HiCutPointLightColor);
			float3 Set_BaseColor = lerp((_BaseColor.rgb * _MainTex_var.rgb * _LightIntensity), ((_BaseColor.rgb * _MainTex_var.rgb) * Set_LightColor), _Is_LightColor_Base);
			float4 _1st_ShadeMap_var = lerp(tex2D(_1st_ShadeMap, TRANSFORM_TEX(Set_UV0, _1st_ShadeMap)), _MainTex_var, _Use_BaseAs1st);
			float3 Set_1st_ShadeColor = lerp((_1st_ShadeColor.rgb * _1st_ShadeMap_var.rgb * _LightIntensity), ((_1st_ShadeColor.rgb * _1st_ShadeMap_var.rgb) * Set_LightColor), _Is_LightColor_1st_Shade);
			float4 _2nd_ShadeMap_var = lerp(tex2D(_2nd_ShadeMap, TRANSFORM_TEX(Set_UV0, _2nd_ShadeMap)), _1st_ShadeMap_var, _Use_1stAs2nd);
			float3 Set_2nd_ShadeColor = lerp((_2nd_ShadeColor.rgb * _2nd_ShadeMap_var.rgb * _LightIntensity), ((_2nd_ShadeColor.rgb * _2nd_ShadeMap_var.rgb) * Set_LightColor), _Is_LightColor_2nd_Shade);
			float _HalfLambert_var = 0.5 * dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToBase), lightDirection) + 0.5;

			float4 _ShadingGradeMap_var = tex2Dlod(_ShadingGradeMap, float4(TRANSFORM_TEX(Set_UV0, _ShadingGradeMap), 0.0, _BlurLevelSGM));
			float _ShadingGradeMapLevel_var = _ShadingGradeMap_var.r < 0.95 ? _ShadingGradeMap_var.r + _Tweak_ShadingGradeMapLevel : 1;
			float Set_ShadingGrade = saturate(_ShadingGradeMapLevel_var) * lerp(_HalfLambert_var, (_HalfLambert_var * saturate(1.0 + _Tweak_SystemShadowsLevel)), _Set_SystemShadowsToBase);
			float Set_FinalShadowMask = saturate((1.0 + ((Set_ShadingGrade - (_1st_ShadeColor_Step - _1st_ShadeColor_Feather)) * (0.0 - 1.0)) / (_1st_ShadeColor_Step - (_1st_ShadeColor_Step - _1st_ShadeColor_Feather))));
			float Set_ShadeShadowMask = saturate((1.0 + ((Set_ShadingGrade - (_2nd_ShadeColor_Step - _2nd_ShadeColor_Feather)) * (0.0 - 1.0)) / (_2nd_ShadeColor_Step - (_2nd_ShadeColor_Step - _2nd_ShadeColor_Feather))));

			float3 finalColor = lerp(Set_BaseColor, lerp(Set_1st_ShadeColor, Set_2nd_ShadeColor, Set_ShadeShadowMask), Set_FinalShadowMask);
			float4 _Set_HighColorMask_var = tex2D(_Set_HighColorMask, TRANSFORM_TEX(Set_UV0, _Set_HighColorMask));
			float _Specular_var = 0.5 * dot(halfDirection, lerp(i.normalDir, normalDirection, _Is_NormalMapToHighColor)) + 0.5;
			float _TweakHighColorMask_var = (saturate((_Set_HighColorMask_var.g + _Tweak_HighColorMaskLevel)) * lerp((1.0 - step(_Specular_var, (1.0 - pow(_HighColor_Power, 5)))), pow(_Specular_var, exp2(lerp(11, 1, _HighColor_Power))), _Is_SpecularToHighColor));
			float4 _HighColor_Tex_var = tex2D(_HighColor_Tex, TRANSFORM_TEX(Set_UV0, _HighColor_Tex));
			float3 _HighColor_var = (lerp((_HighColor_Tex_var.rgb * _HighColor.rgb), ((_HighColor_Tex_var.rgb * _HighColor.rgb) * Set_LightColor), _Is_LightColor_HighColor) * _TweakHighColorMask_var);
			finalColor = finalColor + lerp(lerp(_HighColor_var, (_HighColor_var * ((1.0 - Set_FinalShadowMask) + (Set_FinalShadowMask * _TweakHighColorOnShadow))), _Is_UseTweakHighColorOnShadow), float3(0, 0, 0), _Is_Filter_HiCutPointLightColor);
			finalColor = saturate(finalColor);
			pointLightColor += finalColor;
		}
	}
// _ADDITIONAL_LIGHTS
//#endif
	
	finalColor = saturate(finalColor) + (envLightColor * envLightIntensity * _GI_Intensity * smoothstep(1, 0, envLightIntensity / 2)) + emissive;
	finalColor += pointLightColor;


#ifdef _IS_TRANSCLIPPING_OFF
	fixed4 finalRGBA = fixed4(finalColor,1);
#elif _IS_TRANSCLIPPING_ON
	float Set_Opacity = saturate((_Inverse_Clipping_var+_Tweak_transparency));
	fixed4 finalRGBA = fixed4(finalColor,Set_Opacity);
#endif
	return finalRGBA;
}


#endif
