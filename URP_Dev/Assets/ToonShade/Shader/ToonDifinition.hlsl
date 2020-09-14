#ifndef TOON_DIFINITION
#define TOON_DIFINITION

#define TOON_URP 1

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

#define UCTS_TEXTURE2D(tex,name)  SAMPLE_TEXTURE2D(tex, sampler##tex, TRANSFORM_TEX(name, tex));

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
		#define UNITY_APPLY_FOG_COLOR(coord,col,fogCol) UNITY_FOG_LERP_COLOR(col,fogCol,(coord).x)
	#else
		// SM3.0 and PC/console: calculate fog factor and lerp fog color
		#define UNITY_APPLY_FOG_COLOR(coord,col,fogCol) UNITY_CALC_FOG_FACTOR((coord).x); UNITY_FOG_LERP_COLOR(col,fogCol,unityFogFactor)
	#endif
#else
	#define UNITY_APPLY_FOG_COLOR(coord,col,fogCol)
#endif

#ifdef UNITY_PASS_FORWARDADD
	#define UNITY_APPLY_FOG(coord,col) UNITY_APPLY_FOG_COLOR(coord,col,fixed4(0,0,0,0))
#else
	#define UNITY_APPLY_FOG(coord,col) UNITY_APPLY_FOG_COLOR(coord,col,unity_FogColor)
#endif

#ifdef DIRECTIONAL
#define LIGHTING_COORDS(idx1,idx2) SHADOW_COORDS(idx1)
#define TRANSFER_VERTEX_TO_FRAGMENT(a) TRANSFER_SHADOW(a)
#define LIGHT_ATTENUATION(a)    SHADOW_ATTENUATION(a)
#endif



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
    // 4 of the quadratic (L2) polynomials
	half4 vB = normal.xyzz * normal.yzzx;
	x1.r = dot(unity_SHBr, vB);
	x1.g = dot(unity_SHBg, vB);
	x1.b = dot(unity_SHBb, vB);

    // Final (5th) quadratic (L2) polynomial
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
#endif
