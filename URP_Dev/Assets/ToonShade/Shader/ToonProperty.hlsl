#ifndef SHADING_GRADEMAP_PROP
#define SHADING_GRADEMAP_PROP

sampler2D _MainTex; float4 _MainTex_ST;
float4 _BaseColor;
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
float _BumpScale;
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
#endif
};

struct VertexOutput
{
	float4 pos : SV_POSITION;
	float2 uv0 : TEXCOORD0;
#ifdef _IS_ANGELRING_ON
	float2 uv1 : TEXCOORD1;
	float4 posWorld : TEXCOORD2;
	float3 normalDir : TEXCOORD3;
	float3 tangentDir : TEXCOORD4;
	float3 bitangentDir : TEXCOORD5;
	float mirrorFlag : TEXCOORD6;
	LIGHTING_COORDS(7,8)
	UNITY_FOG_COORDS(9)
#else
	
	float4 posWorld : TEXCOORD1;
	float3 normalDir : TEXCOORD2;
	float3 tangentDir : TEXCOORD3;
	float3 bitangentDir : TEXCOORD4;
	float mirrorFlag : TEXCOORD5;
	LIGHTING_COORDS(6,7)
	UNITY_FOG_COORDS(8)
#endif
};

VertexOutput vert(VertexInput v)
{
	VertexOutput o = (VertexOutput) 0;
	o.uv0 = v.texcoord0;
#ifdef _IS_ANGELRING_ON
	o.uv1 = v.texcoord1;
#endif
	o.normalDir = UnityObjectToWorldNormal(v.normal);
	o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
	o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
	float3 lightColor = _LightColor0.rgb;
	o.pos = UnityObjectToClipPos(v.vertex);
	float3 crossFwd = cross(UNITY_MATRIX_V[0], UNITY_MATRIX_V[1]);
	o.mirrorFlag = dot(crossFwd, UNITY_MATRIX_V[2]) < 0 ? 1 : -1;
	UNITY_TRANSFER_FOG(o, o.pos);
	TRANSFER_VERTEX_TO_FRAGMENT(o)
	return o;
}


#endif
