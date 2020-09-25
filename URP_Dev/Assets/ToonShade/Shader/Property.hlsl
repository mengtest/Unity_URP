#ifndef PROPERTY
#define PROPERTY

//CBUFFER_START(UnityPerMaterial)
float4 _MainColor;
sampler2D _MainTex;
float4 _MainTex_ST;
//CBUFFER_END

float4 _1st_ShadeColor;
float4 _2nd_ShadeColor;
sampler2D _1st_ShadeMap;
float4 _1st_ShadeMap_ST;
sampler2D _2nd_ShadeMap;
float4 _2nd_ShadeMap_ST;
sampler2D _ShadingGradeMap;
float4 _ShadingGradeMap_ST;

float _Tweak_SystemShadowsLevel;
float _Tweak_ShadingGradeMapLevel;
float _1st_ShadeColor_Step;
float _1st_ShadeColor_Feather;
float _2nd_ShadeColor_Step;
float _2nd_ShadeColor_Feather;
float _BlurLevelSGM;

half _Is_NormalMapToBase;
half _Set_SystemShadowsToBase;


float4 _HighColor;
sampler2D _HighColor_Tex;
float4 _HighColor_Tex_ST;
float _HighColor_Power;
float _TweakHighColorOnShadow;
sampler2D _Set_HighColorMask;
float4 _Set_HighColorMask_ST;
float _Tweak_HighColorMaskLevel;
half _Is_NormalMapToHighColor;
half _Is_SpecularToHighColor;
half _Is_BlendAddToHiColor;
half _Is_UseTweakHighColorOnShadow;

half _RimLight;
float4 _RimLightColor;
float _RimLight_Power;
float _RimLight_InsideMask;
float _Tweak_LightDirection_MaskLevel;
float4 _Ap_RimLightColor;
float _Ap_RimLight_Power;
sampler2D _Set_RimLightMask;
float4 _Set_RimLightMask_ST;
float _Tweak_RimLightMaskLevel;
half _Is_NormalMapToRimLight;
half _Is_RimLight_FeatherOff;
half _Is_LightDirection_MaskOn;
half _Is_Antipodean_RimLight;
half _Is_ApRimLight_FeatherOff;

half _MatCap;
sampler2D _MatCap_Sampler;
float4 _MatCap_Sampler_ST;
float _BlurLevelMatcap;
float4 _MatCapColor;
float _Tweak_MatCapUV;
float _Rotate_MatCapUV;
sampler2D _NormalMapForMatCap;
float4 _NormalMapForMatCap_ST;
sampler2D _NormalMap;
float4 _NormalMap_ST;
float _NormalMapStrength;
float _BumpScaleMatcap;
float _Rotate_NormalMapForMatCapUV;
float _TweakMatCapOnShadow;
sampler2D _Set_MatcapMask;
float4 _Set_MatcapMask_ST;
float _Tweak_MatcapMaskLevel;
half _Is_BlendAddToMatCap;
half _Is_NormalMapForMatCap;
half _Is_UseTweakMatCapOnShadow;
half _Is_Ortho;
half _Is_InverseMatcapMask;
half _Is_CameraRolling;

half _Emissive;
sampler2D _Emissive_Tex;
float4 _Emissive_Tex_ST;
float4 _Emissive_Color;
float _Rotate_EmissiveUV;
float _Base_Speed;
float _Scroll_EmissiveU;
float _Scroll_EmissiveV;
float4 _ColorShift;
float4 _ViewShift;
float _ColorShift_Speed;
half _Is_ViewCoord_Scroll;
half _Is_PingPong_Base;
half _Is_ColorShift;
half _Is_ViewShift;

float _GI_Intensity;
float _Unlit_Intensity;
float _StepOffset;
half _Is_Filter_HiCutPointLightColor;

sampler2D _ClippingMask;
float4 _ClippingMask_ST;
float _ClippingLevel;
float _Tweak_transparency;
half _IsBaseMapAlphaAsClippingMask;
half _Inverse_Clipping;


half _AngelRing;
sampler2D _AngelRing_Sampler;
float4 _AngelRing_Sampler_ST;
float4 _AngelRing_Color;
float _AR_OffsetU;
float _AR_OffsetV;
half _Is_AngelRingAlphaOn;

half _Use_BaseAs1st;
half _Use_1stAs2nd;
half _Is_LightColor_Base;
half _Is_LightColor_1st_Shade;
half _Is_LightColor_2nd_Shade;
half _Is_LightColor_HighColor;
half _Is_LightColor_RimLight;
half _Is_LightColor_Ap_RimLight;
half _Is_LightColor_MatCap;
half _Is_LightColor_AR;

float4 _LightColor0;
float _Outline_Width;
float _Offset_Z;
float4 _Outline_Color;

sampler2D _Outline_Sampler;
float4 _Outline_Sampler_ST;
sampler2D _OutlineTex;
float4 _OutlineTex_ST;
sampler2D _BakedNormal;
float4 _BakedNormal_ST;

half _Is_OutlineTex;
half _Is_BlendBaseColor;
half _Is_Filter_LightColor;
half _Is_LightColor_Outline;
half _Is_BakedNormal;


#endif
