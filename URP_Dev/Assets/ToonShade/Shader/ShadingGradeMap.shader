
Shader "ToonShade/ShadingGradeMap"
{
	Properties
	{
		// Editor Extend Queue & Stencil
		[HideInInspector] _simpleUI("SimpleUI", Int) = 0
		[HideInInspector] _AutoRenderQueue("Automatic Render Queue ", int) = 1
		[Enum(OFF,0, StencilOut,1, StencilMask,2)] _StencilMode("StencilMode",int) = 0
		_StencilComp("Stencil Comparison", Float) = 8
		_StencilNo("Stencil Ref Main", Range(0, 255)) = 0
		_StencilOpPass("Stencil Operation", Float) = 0
		_StencilOpFail("Stencil Operation", Float) = 0

		// 0 : _ClippingMode_Off 1 : _ClippingMode_ON
		[Enum(OFF,0, ON,1)] _TransparentEnabled("Transparent Mode", int) = 0

		// 0 : _IS_TRANSCLIPPING_OFF 1 : _IS_TRANSCLIPPING_ON
		[Enum(OFF,0, ON,1)] _ClippingMode("CliippingMode",int) = 0

		[Enum(OFF,0, FRONT,1, BACK,2)] _CullMode("Cull Mode", int) = 2 
		[Enum(OFF,0, ON,1)] _ZWriteMode("ZWrite Mode", int) = 1
		[Enum(OFF,0, ON,1)] _ZOverDrawMode("ZOver Draw Mode", Float) = 0
		_SPRDefaultUnlitColorMask("SPRDefaultUnlit Path Color Mask", int) = 15

		[Enum(OFF,0, FRONT,1, BACK,2)] _SRPDefaultUnlitColMode("SPRDefaultUnlit  Cull Mode", int) = 1
		
		// if _ClippingMode_ON selected show property
		_ClippingMask("ClippingMask", 2D) = "white" {}
		[Toggle(_)] _Inverse_Clipping("Clipping Inverse", Float) = 0
		_ClippingLevel("Clipping Level", Range(0, 1)) = 0
		_Tweak_transparency("Tweak Transparency", Range(-1, 1)) = 0
		[HideInInspector] _IsBaseMapAlphaAsClippingMask("AlphaAsClippingMask", Float) = 0

		_MainTex("BaseMap", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (1,1,1,1)

		_1st_ShadeMap("1st_ShadeMap", 2D) = "white" {}
		[Toggle(_)] _Use_BaseAs1st("Use BaseMap as 1st_ShadeMap", Float) = 0
		_1st_ShadeColor("1st_ShadeColor", Color) = (1,1,1,1)

		_2nd_ShadeMap("2nd_ShadeMap", 2D) = "white" {}
		[Toggle(_)] _Use_1stAs2nd("Use 1st_ShadeMap as 2nd_ShadeMap", Float) = 0
		_2nd_ShadeColor("2nd_ShadeColor", Color) = (1,1,1,1)

		_NormalMap("NormalMap", 2D) = "bump" {}
		_BumpScale("Normal Scale", Range(0, 1)) = 1
		[Toggle(_)] _Is_NormalMapToBase("Is_NormalMapToBase", Float) = 0
		[Toggle(_)] _Set_SystemShadowsToBase("Set_SystemShadowsToBase", Float) = 1
		_Tweak_SystemShadowsLevel("Tweak_SystemShadowsLevel", Range(-0.5, 0.5)) = 0
		_BaseColor_Step("BaseColor_Step", Range(0, 1)) = 0.5
		_BaseShade_Feather("Base/Shade_Feather", Range(0.0001, 1)) = 0.0001
		_ShadeColor_Step("ShadeColor_Step", Range(0, 1)) = 0
		_1st2nd_Shades_Feather("1st/2nd_Shades_Feather", Range(0.0001, 1)) = 0.0001

		[Header(Realtime LightColor Contribution to each colors)]
		[Toggle(_)] _Is_LightColor_Base("Is_LightColor_Base", Float) = 1
		[Toggle(_)] _Is_LightColor_1st_Shade("Is_LightColor_1st_Shade", Float) = 1
		[Toggle(_)] _Is_LightColor_2nd_Shade("Is_LightColor_2nd_Shade", Float) = 1
		[Toggle(_)] _Is_LightColor_HighColor("Is_LightColor_HighColor", Float) = 1
		[Toggle(_)] _Is_LightColor_RimLight("Is_LightColor_RimLight", Float) = 1
		[Toggle(_)] _Is_LightColor_Ap_RimLight("Is_LightColor_Ap_RimLight", Float) = 1
		[Toggle(_)] _Is_LightColor_MatCap("Is_LightColor_MatCap", Float) = 1
		[Toggle(_)] _Is_LightColor_AR("Is_LightColor_AR", Float) = 1
		[Toggle(_)] _Is_LightColor_Outline("Is_LightColor_Outline", Float) = 1

		[HideInInspector] _BaseMap("BaseMap", 2D) = "white" {}
		[HideInInspector] _Color("Color", Color) = (1,1,1,1)
		[HideInInspector] _1st_ShadeColor_Step("1st_ShadeColor_Step", Range(0, 1)) = 0.5
		[HideInInspector] _1st_ShadeColor_Feather("1st_ShadeColor_Feather", Range(0.0001, 1)) = 0.0001
		[HideInInspector] _2nd_ShadeColor_Step("2nd_ShadeColor_Step", Range(0, 1)) = 0
		[HideInInspector] _2nd_ShadeColor_Feather("2nd_ShadeColor_Feather", Range(0.0001, 1)) = 0.0001

		_StepOffset("Step_Offset (ForwardAdd Only)", Range(-0.5, 0.5)) = 0
		[Toggle(_)] _Is_Filter_HiCutPointLightColor("PointLights HiCut_Filter (ForwardAdd Only)", Float) = 1

		_Set_1st_ShadePosition("Set_1st_ShadePosition", 2D) = "white" {}
		_Set_2nd_ShadePosition("Set_2nd_ShadePosition", 2D) = "white" {}
		_ShadingGradeMap("ShadingGradeMap", 2D) = "white" {}
		_Tweak_ShadingGradeMapLevel("Tweak_ShadingGradeMapLevel", Range(-0.5, 0.5)) = 0
		_BlurLevelSGM("Blur Level of ShadingGradeMap", Range(0, 10)) = 0

		_HighColor("HighColor", Color) = (0,0,0,1)
		_HighColor_Tex("HighColor_Tex", 2D) = "white" {}
		[Toggle(_)] _Is_NormalMapToHighColor("Is_NormalMapToHighColor", Float) = 0
		_HighColor_Power("HighColor_Power", Range(0, 1)) = 0
		[Toggle(_)] _Is_SpecularToHighColor("Is_SpecularToHighColor", Float) = 0
		[Toggle(_)] _Is_BlendAddToHiColor("Is_BlendAddToHiColor", Float) = 0
		[Toggle(_)] _Is_UseTweakHighColorOnShadow("Is_UseTweakHighColorOnShadow", Float) = 0
		_TweakHighColorOnShadow("TweakHighColorOnShadow", Range(0, 1)) = 0
		_Set_HighColorMask("Set_HighColorMask", 2D) = "white" {}
		_Tweak_HighColorMaskLevel("Tweak_HighColorMaskLevel", Range(-1, 1)) = 0

		[Toggle(_)] _RimLight("RimLight", Float) = 0
		_RimLightColor("RimLightColor", Color) = (1,1,1,1)
		[Toggle(_)] _Is_NormalMapToRimLight("Is_NormalMapToRimLight", Float) = 0
		_RimLight_Power("RimLight_Power", Range(0, 1)) = 0.1
		_RimLight_InsideMask("RimLight_InsideMask", Range(0.0001, 1)) = 0.0001
		[Toggle(_)] _RimLight_FeatherOff("RimLight_FeatherOff", Float) = 0
		[Toggle(_)] _LightDirection_MaskOn("LightDirection_MaskOn", Float) = 0
		_Tweak_LightDirection_MaskLevel("Tweak_LightDirection_MaskLevel", Range(0, 0.5)) = 0
		[Toggle(_)] _Add_Antipodean_RimLight("Add_Antipodean_RimLight", Float) = 0
		_Ap_RimLightColor("Ap_RimLightColor", Color) = (1,1,1,1)

		_Ap_RimLight_Power("Ap_RimLight_Power", Range(0, 1)) = 0.1
		[Toggle(_)] _Ap_RimLight_FeatherOff("Ap_RimLight_FeatherOff", Float) = 0
		_Set_RimLightMask("Set_RimLightMask", 2D) = "white" {}
		_Tweak_RimLightMaskLevel("Tweak_RimLightMaskLevel", Range(-1, 1)) = 0

		[Toggle(_)] _MatCap("MatCap", Float) = 0
		_MatCap_Sampler("MatCap_Sampler", 2D) = "black" {}
		_BlurLevelMatcap("Blur Level of MatCap_Sampler", Range(0, 10)) = 0
		_MatCapColor("MatCapColor", Color) = (1,1,1,1)
		[Toggle(_)] _Is_BlendAddToMatCap("Is_BlendAddToMatCap", Float) = 1
		_Tweak_MatCapUV("Tweak_MatCapUV", Range(-0.5, 0.5)) = 0
		_Rotate_MatCapUV("Rotate_MatCapUV", Range(-1, 1)) = 0
		[Toggle(_)] _CameraRolling_Stabilizer("Activate CameraRolling_Stabilizer", Float) = 0
		[Toggle(_)] _Is_NormalMapForMatCap("Is_NormalMapForMatCap", Float) = 0
		_NormalMapForMatCap("NormalMapForMatCap", 2D) = "bump" {}
		_BumpScaleMatcap("Scale for NormalMapforMatCap", Range(0, 1)) = 1
		_Rotate_NormalMapForMatCapUV("Rotate_NormalMapForMatCapUV", Range(-1, 1)) = 0
		[Toggle(_)] _Is_UseTweakMatCapOnShadow("Is_UseTweakMatCapOnShadow", Float) = 0
		_TweakMatCapOnShadow("TweakMatCapOnShadow", Range(0, 1)) = 0
		_Set_MatcapMask("Set_MatcapMask", 2D) = "white" {}
		_Tweak_MatcapMaskLevel("Tweak_MatcapMaskLevel", Range(-1, 1)) = 0
		[Toggle(_)] _Inverse_MatcapMask("Inverse_MatcapMask", Float) = 0
		[Toggle(_)] _Is_Ortho("Orthographic Projection for MatCap", Float) = 0

		[Toggle(_)] _AngelRing("AngelRing", Float) = 0
		_AngelRing_Sampler("AngelRing_Sampler", 2D) = "black" {}
		_AngelRing_Color("AngelRing_Color", Color) = (1,1,1,1)
		_AR_OffsetU("AR_OffsetU", Range(0, 0.5)) = 0
		_AR_OffsetV("AR_OffsetV", Range(0, 1)) = 0.3
		[Toggle(_)] _ARSampler_AlphaOn("ARSampler_AlphaOn", Float) = 0

		[Enum(OFF, 0, ON, 1)] _EMISSIVE("EMISSIVE MODE", Float) = 0
		_Emissive_Tex("Emissive_Tex", 2D) = "white" {}
		[HDR]_Emissive_Color("Emissive_Color", Color) = (0,0,0,1)
		_Base_Speed("Base_Speed", Float) = 0
		_Scroll_EmissiveU("Scroll_EmissiveU", Range(-1, 1)) = 0
		_Scroll_EmissiveV("Scroll_EmissiveV", Range(-1, 1)) = 0
		_Rotate_EmissiveUV("Rotate_EmissiveUV", Float) = 0
		[Toggle(_)] _Is_PingPong_Base("Is_PingPong_Base", Float) = 0
		[Toggle(_)] _Is_ColorShift("Activate ColorShift", Float) = 0
		[HDR]_ColorShift("ColorSift", Color) = (0,0,0,1)
		_ColorShift_Speed("ColorShift_Speed", Float) = 0
		[Toggle(_)] _Is_ViewShift("Activate ViewShift", Float) = 0
		[HDR]_ViewShift("ViewSift", Color) = (0,0,0,1)
		[Toggle(_)] _Is_ViewCoord_Scroll("Is_ViewCoord_Scroll", Float) = 0

		[KeywordEnum(NML, POS)] _OUTLINE("OUTLINE MODE", Float) = 0
		_Outline_Width("Outline_Width", Float) = 0
		_Outline_Sampler("Outline_Sampler", 2D) = "white" {}
		_Outline_Color("Outline_Color", Color) = (0.5,0.5,0.5,1)
		[Toggle(_)] _Is_BlendBaseColor("Is_BlendBaseColor", Float) = 0
		[Toggle(_)] _Is_OutlineTex("Is_OutlineTex", Float) = 0
		_OutlineTex("OutlineTex", 2D) = "white" {}
		_Offset_Z("Offset_Camera_Z", Float) = 0
		[Toggle(_)] _Is_BakedNormal("Is_BakedNormal", Float) = 0
		_BakedNormal("Baked Normal for Outline", 2D) = "white" {}

		_GI_Intensity("GI_Intensity", Range(0, 1)) = 0
		_Unlit_Intensity("Unlit_Intensity", Range(0.001, 4)) = 1
		//[Toggle(_)] _Is_Filter_LightColor("VRChat : SceneLights HiCut_Filter", Float) = 0
		//[Toggle(_)] _Is_BLD("Advanced : Activate Built-in Light Direction", Float) = 0
		//_Offset_X_Axis_BLD("Offset X-Axis (Built-in Light Direction)", Range(-1, 1)) = -0.05
		//_Offset_Y_Axis_BLD("Offset Y-Axis (Built-in Light Direction)", Range(-1, 1)) = 0.09
		//[Toggle(_)] _Inverse_Z_Axis_BLD(" Inverse Z-Axis (Built-in Light Direction)", Float) = 1
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

		Pass
		{
			Name "OUTLINE"
			Tags { "LightMode" = "SRPDefaultUnlit" }

			Cull[_SRPDefaultUnlitColMode]
			ColorMask[_SPRDefaultUnlitColorMask]
			Blend SrcAlpha OneMinusSrcAlpha
			Stencil
			{
				Ref[_StencilNo]
				Comp[_StencilComp]
				Pass[_StencilOpPass]
				Fail[_StencilOpFail]

			}

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _IS_OUTLINE_CLIPPING_NO _IS_OUTLINE_CLIPPING_YES
			#pragma multi_compile _OUTLINE_NML _OUTLINE_POS
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "ToonOutline.hlsl"
			ENDHLSL
		}

		Pass
		{
			Name "FORWARD"
			Tags { "LightMode" = "LightweightForward" }
			ZWrite[_ZWriteMode]
			Cull[_CullMode]
			Blend SrcAlpha OneMinusSrcAlpha

			Stencil 
			{
				Ref[_StencilNo]
				Comp[_StencilComp]
				Pass[_StencilOpPass]
				Fail[_StencilOpFail]
			}

			HLSLPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _METALLICSPECGLOSSMAP
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature _OCCLUSIONMAP

			#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature _ENVIRONMENTREFLECTIONS_OFF
			#pragma shader_feature _SPECULAR_SETUP
			#pragma shader_feature _RECEIVE_SHADOWS_OFF

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog

			#pragma shader_feature _ _RAYTRACINGSHADOW_ON
			#pragma shader_feature _IS_TRANSCLIPPING_OFF _IS_TRANSCLIPPING_ON
			#pragma shader_feature _ANGELRING_OFF _ANGELRING_ON
			#pragma shader_feature _EMISSIVE_OFF _EMISSIVE_ON
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"
			#include "ToonShadingGradeMap.hlsl"
			ENDHLSL
		}

		Pass
		{
			Name "SHADOW_CASTER"
			Tags { "LightMode" = "ShadowCaster" }
			ZWrite On
			ZTest LEqual
			Cull[_CullMode]

			HLSLPROGRAM
			#pragma target 3.0
			#pragma shader_feature _ALPHATEST_ON
			#pragma multi_compile_instancing
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment
			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
			ENDHLSL
		}

		Pass
		{
			Name "DEPTH_ONLY"
			Tags{ "LightMode" = "DepthOnly" }

			ZWrite On
			ColorMask 0
			Cull[_CullMode]

			HLSLPROGRAM
			#pragma target 3.0
			#pragma vertex DepthOnlyVertex
			#pragma fragment DepthOnlyFragment
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma multi_compile_instancing
			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
			ENDHLSL
		}

	}
	FallBack Off
	CustomEditor "ToonShade.ToonShadeInspector"
}
