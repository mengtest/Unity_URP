#ifndef COMMON
#define COMMON


#define fixed  half
#define fixed3 half3
#define fixed4 half4



#ifndef DIRECTIONAL
	#define DIRECTIONAL
#endif

#if defined(UNITY_PASS_PREPASSBASE) || defined(UNITY_PASS_DEFERRED) || defined(UNITY_PASS_SHADOWCASTER)
#undef FOG_LINEAR
#undef FOG_EXP
#undef FOG_EXP2
#endif


#define UnityObjectToClipPos UnityObjectToClipPosInstanced

#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
	#define UNITY_FOG_COORDS(idx) UNITY_FOG_COORDS_PACKED(idx, float1)

	#if (SHADER_TARGET < 30) || defined(SHADER_API_MOBILE)
		// mobile or SM2.0: calculate fog factor per-vertex
		#define UNITY_TRANSFER_FOG(o,outpos) UNITY_CALC_FOG_FACTOR((outpos).z); o.fogCoord.x = unityFogFactor
	#else
		// SM3.0 and PC/console: calculate fog distance per-vertex, and fog factor per-pixel
		#define UNITY_TRANSFER_FOG(o,outpos) o.fogCoord.x = (outpos).z
	#endif
#else
	#define UNITY_FOG_COORDS(idx)
	#define UNITY_TRANSFER_FOG(o,outpos)
#endif

#define UNITY_FOG_LERP_COLOR(col,fogCol,fogFac) col.rgb = lerp((fogCol).rgb, (col).rgb, saturate(fogFac))


#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
	#if (SHADER_TARGET < 30) || defined(SHADER_API_MOBILE)
		// mobile or SM2.0: fog factor was already calculated per-vertex, so just lerp the color
		#define UNITY_APPLY_FOG_COLOR(coord, col, fogCol) UNITY_FOG_LERP_COLOR(col,fogCol,(coord).x)
	#else
		// SM3.0 and PC/console: calculate fog factor and lerp fog color
		#define UNITY_APPLY_FOG_COLOR(coord, col, fogCol) UNITY_CALC_FOG_FACTOR((coord).x); UNITY_FOG_LERP_COLOR(col,fogCol,unityFogFactor)
	#endif
#else
	#define UNITY_APPLY_FOG_COLOR(coord,col,fogCol)
#endif

#ifdef UNITY_PASS_FORWARDADD
	#define UNITY_APPLY_FOG(coord,col) UNITY_APPLY_FOG_COLOR(coord, col, half4(0,0,0,0))
#else
	#define UNITY_APPLY_FOG(coord,col) UNITY_APPLY_FOG_COLOR(coord, col, unity_FogColor)
#endif

#ifdef DIRECTIONAL
#define LIGHTING_COORDS(idx1,idx2) SHADOW_COORDS(idx1)
#define TRANSFER_VERTEX_TO_FRAGMENT(a) TRANSFER_SHADOW(a)
#define LIGHT_ATTENUATION(a)    SHADOW_ATTENUATION(a)
#endif

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

#define UNITY_PROJ_COORD(a) a
#define UNITY_SAMPLE_SCREEN_SHADOW(tex, uv) tex2Dproj( tex, UNITY_PROJ_COORD(uv) ).r
#define TEXTURE2D_SAMPLER2D(textureName, samplerName) Texture2D textureName; SamplerState samplerName 
		TEXTURE2D_SAMPLER2D(_RaytracedHardShadow, sampler_RaytracedHardShadow);

// #define PI 3.141592654
//float4 _RaytracedHardShadow_TexelSize;

inline bool IsGammaSpace()
{
#ifdef UNITY_COLORSPACE_GAMMA
    return true;
#else
	return false;
#endif
}

inline float GammaToLinearSpaceExact(float value)
{
	if (value <= 0.04045f)
	{
		return value / 12.92f;
	}
	else if (value < 1.0f)
	{
		return pow((value + 0.055f) / 1.055f, 2.4f);
	}
	else
	{
		return pow(value, 2.2f);
	}
}

inline half3 GammaToLinearSpace(half3 sRGB)
{
    // Approximate version from http://chilliant.blogspot.com.au/2012/08/srgb-approximations-for-hlsl.html?m=1
	return sRGB * (sRGB * (sRGB * 0.305306011h + 0.682171111h) + 0.012522878h);
}

inline float LinearToGammaSpaceExact(float value)
{
	if (value <= 0.0f)
	{
		return 0.0f;
	}
	else if (value <= 0.0031308f)
	{
		return 12.92f * value;
	}
	else if (value < 1.0f)
	{
		return 1.055f * pow(value, 0.4166667f) - 0.055f;
	}
	else
	{
		return pow(value, 0.45454545f);
	}
}

inline half3 LinearToGammaSpace(half3 linRGB)
{
    // An almost-perfect approximation from http://chilliant.blogspot.com.au/2012/08/srgb-approximations-for-hlsl.html?m=1
	linRGB = max(linRGB, half3(0.h, 0.h, 0.h));
	return max(1.055h * pow(linRGB, 0.416666667h) - 0.055h, 0.h);
}

inline float4 UnityObjectToClipPosInstanced(in float3 pos)
{
	return mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, float4(pos, 1.0)));
}

inline float4 UnityObjectToClipPosInstanced(float4 pos)
{
	return UnityObjectToClipPosInstanced(pos.xyz);
}

inline float3 UnityObjectToWorldNormal(in float3 norm)
{
#ifdef UNITY_ASSUME_UNIFORM_SCALING
    return UnityObjectToWorldDir(norm);
#else
	return normalize(mul(norm, (float3x3) unity_WorldToObject));
#endif
}

half3 SHEvalLinearL0L1(half4 normal)
{
	half3 x;
	x.r = dot(unity_SHAr, normal);
	x.g = dot(unity_SHAg, normal);
	x.b = dot(unity_SHAb, normal);
	return x;
}

half3 SHEvalLinearL2(half4 normal)
{
	half3 x1, x2;
	half4 vB = normal.xyzz * normal.yzzx;
	x1.r = dot(unity_SHBr, vB);
	x1.g = dot(unity_SHBg, vB);
	x1.b = dot(unity_SHBb, vB);

	half vC = normal.x * normal.x - normal.y * normal.y;
	x2 = unity_SHC.rgb * vC;
	return x1 + x2;
}

half3 ShadeSH9(half4 normal)
{
	half3 res = SHEvalLinearL0L1(normal);
	res += SHEvalLinearL2(normal);

#ifdef UNITY_COLORSPACE_GAMMA
    res = LinearToGammaSpace(res);
#endif
	return res;
}

half3 DecodeLightProbe(half3 N)
{
	return ShadeSH9(float4(N, 1));
}


#endif // COMMON
