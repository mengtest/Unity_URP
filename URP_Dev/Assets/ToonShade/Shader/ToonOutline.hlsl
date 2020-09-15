#ifndef OUTLINE
#define OUTLINE

#include "ToonDifinition.hlsl"

float4 _LightColor0;
float4 _BaseColor;
float _Unlit_Intensity;
float4 _Color;

float _Outline_Width;
float _Offset_Z;
float4 _Outline_Color;

sampler2D _MainTex; float4 _MainTex_ST;
sampler2D _Outline_Sampler; float4 _Outline_Sampler_ST;
sampler2D _OutlineTex; float4 _OutlineTex_ST;
sampler2D _BakedNormal; float4 _BakedNormal_ST;

fixed _Is_OutlineTex;
fixed _Is_BlendBaseColor;
fixed _Is_Filter_LightColor;
fixed _Is_LightColor_Outline;
fixed _Is_BakedNormal;

#ifdef _IS_OUTLINE_CLIPPING_YES
	sampler2D _ClippingMask; uniform float4 _ClippingMask_ST;
	float _ClippingLevel;
	fixed _Inverse_Clipping;
	fixed _IsBaseMapAlphaAsClippingMask;
#endif

struct VertexInput
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
};

struct VertexOutput
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normalDir : TEXCOORD1;
	float3 tangentDir : TEXCOORD2;
	float3 bitangentDir : TEXCOORD3;
};


VertexOutput vert(VertexInput v)
{
	VertexOutput o = (VertexOutput) 0;
	o.uv = v.uv;
	float4 objPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
	float4 samplerTex = tex2Dlod(_Outline_Sampler, float4(TRANSFORM_TEX(o.uv, _Outline_Sampler), 0.0, 0));
	
	o.normalDir = UnityObjectToWorldNormal(v.normal);
	o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
	o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
	float3x3 tangentTransform = float3x3(o.tangentDir, o.bitangentDir, o.normalDir);
	
	float4 bakedNormal = (tex2Dlod(_BakedNormal, float4(TRANSFORM_TEX(o.uv, _BakedNormal), 0.0, 0)) * 2 - 1);
	float3 bakedNormalDir = normalize(mul(bakedNormal.rgb, tangentTransform));
	
	half near = _ProjectionParams.y;
	half far = _ProjectionParams.z;
	float outlineStep = smoothstep(far, near, distance(objPos.rgb, _WorldSpaceCameraPos));
	
	float baseWidth = _Outline_Width * 0.001;
	float outlineWidth = (baseWidth * outlineStep * samplerTex.rgb).r;
	
#if defined(UNITY_REVERSED_Z)
	_Offset_Z = _Offset_Z * -0.01;
#else
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	float3 eyeDir = UnityWorldSpaceViewDir(worldPos);
	half diff = clamp(max(0, dot(normalize(v.normal), eyeDir)), 0, 1);
	_Offset_Z = _Offset_Z * 0.01;
#endif
	
#ifdef _OUTLINE_NML
	float4 a = float4(v.vertex.xyz + v.normal * outlineWidth, 1);
	float4 b = float4(v.vertex.xyz + bakedNormalDir * outlineWidth, 1);
	o.pos = UnityObjectToClipPos(lerp(a, b, _Is_BakedNormal));
	
#elif _OUTLINE_POS
	outlineWidth = outlineWidth * 2;
	float signVar = (dot(normalize(v.vertex),normalize(v.normal)) < 0) ? -1 : 1;
	o.pos = UnityObjectToClipPos(float4(v.vertex.xyz + signVar * normalize(v.vertex) * outlineWidth, 1));
#endif
	
	float4 viewDirectionVP = mul(UNITY_MATRIX_VP, float4(_WorldSpaceCameraPos.xyz, 1));
	o.pos.z = o.pos.z + _Offset_Z * viewDirectionVP.z;
	return o;
}


float4 frag(VertexOutput i) : SV_Target
{
	_Color = _BaseColor;
	float4 objPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
	half3 ambientSkyColor = (unity_AmbientSky.rgb > 0.05) ? unity_AmbientSky.rgb * _Unlit_Intensity : half3(0.05, 0.05, 0.05) * _Unlit_Intensity;
	
	float3 lightColor = (_LightColor0.rgb > 0.05) ? _LightColor0.rgb : ambientSkyColor.rgb;
	float lightColorIntensity = (0.299 * lightColor.r + 0.587 * lightColor.g + 0.114 * lightColor.b);
	lightColor = (lightColorIntensity < 1) ? lightColor : lightColor / lightColorIntensity;
	lightColor = lerp(half3(1.0, 1.0, 1.0), lightColor, _Is_LightColor_Outline);

	
	float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv, _MainTex));
	float3 _BaseColorMap_var  = _BaseColor.rgb * _MainTex_var.rgb;
	
	float3 _BlendA = (_Outline_Color.rgb * lightColor);
	float3 _BlendB = (_Outline_Color.rgb * _BaseColorMap_var * _BaseColorMap_var * lightColor);
	float3 _Is_BlendBaseColor_var = lerp(_BlendA, _BlendB, _Is_BlendBaseColor);
	float3 _OutlineTex_var = tex2D(_OutlineTex, TRANSFORM_TEX(i.uv, _OutlineTex));
		
#ifdef _IS_OUTLINE_CLIPPING_YES
	
	float4 _ClippingMask_var = tex2D(_ClippingMask, TRANSFORM_TEX(i.uv, _ClippingMask));
	float Set_MainTexAlpha = _MainTex_var.a;
	float _IsBaseMapAlphaAsClippingMask_var = lerp(_ClippingMask_var.r, Set_MainTexAlpha, _IsBaseMapAlphaAsClippingMask);
	float _Inverse_Clipping_var = lerp(_IsBaseMapAlphaAsClippingMask_var, (1.0 - _IsBaseMapAlphaAsClippingMask_var), _Inverse_Clipping);
	float Set_Clipping = saturate((_Inverse_Clipping_var + _ClippingLevel));
	clip(Set_Clipping - 0.5);
	
	float4 _ClipA = float4(_Is_BlendBaseColor_var, Set_Clipping);
	float4 _ClipB = float4((_OutlineTex_var.rgb * _Outline_Color.rgb * lightColor), Set_Clipping);
	float4 Set_Outline_Color = lerp(_ClipA, _ClipB, _Is_OutlineTex);
	return Set_Outline_Color;
	
#else
	// _IS_OUTLINE_CLIPPING_NO
	float3 _ClipOffA = (_Is_BlendBaseColor_var);
	float3 _ClipOffB = (_OutlineTex_var.rgb * _Outline_Color.rgb * lightColor);
	float3 Set_Outline_Color = lerp(_ClipOffA, _ClipOffB, _Is_OutlineTex);	
	return float4(Set_Outline_Color, 1.0);
	
#endif
}

#endif