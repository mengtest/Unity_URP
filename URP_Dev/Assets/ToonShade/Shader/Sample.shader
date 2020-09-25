Shader "Universal Render Pipeline/Sample"
{
	Properties
	{
		// Editor Extend Queue & Stencil
		[HideInInspector] _simpleUI("SimpleUI", Int) = 0
		[HideInInspector] _AutoRenderQueue("Automatic Render Queue ", int) = 1
		[Enum(OFF,0, StencilOut,1, StencilMask,2)] _StencilMode("StencilMode",int) = 0
		_StencilComp("Stencil Comparison", Float) = 8
		_StencilNo("Stencil Ref Main", Range(0, 255)) = 0
		_StencilOpPass("Stencil OpPass", Float) = 0
		_StencilOpFail("Stencil OpFail", Float) = 0

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
		_ClippingLevel("Clipping Level", Range(0, 1)) = 0
		_Tweak_transparency("Tweak Transparency", Range(-1, 1)) = 0
		[Toggle(_)] _Inverse_Clipping("Clipping Inverse", Float) = 0
		[HideInInspector] _IsBaseMapAlphaAsClippingMask("AlphaAsClippingMask", Float) = 0

		_MainTex("BaseMap", 2D) = "white" {}
		_MainColor("MainColor", Color) = (1,1,1,1)

		_1st_ShadeMap("1st_ShadeMap", 2D) = "white" {}
		_1st_ShadeColor("1st_ShadeColor", Color) = (1,1,1,1)
		[Toggle(_)] _Use_BaseAs1st("Use BaseMap as 1st_ShadeMap", Float) = 0

		_2nd_ShadeMap("2nd_ShadeMap", 2D) = "white" {}
		_2nd_ShadeColor("2nd_ShadeColor", Color) = (1,1,1,1)
		[Toggle(_)] _Use_1stAs2nd("Use 1st_ShadeMap as 2nd_ShadeMap", Float) = 0

		_NormalMap("NormalMap", 2D) = "bump" {}
		_NormalMapStrength("Normal Scale", Range(0, 1)) = 1
		_Tweak_SystemShadowsLevel("Tweak_SystemShadowsLevel", Range(-0.5, 0.5)) = 0
		[Toggle(_)] _Is_NormalMapToBase("Is_NormalMapToBase", Float) = 0
		[Toggle(_)] _Set_SystemShadowsToBase("Set_SystemShadowsToBase", Float) = 1

		_BaseColor_Step("BaseColor_Step", Range(0, 1)) = 0.5
		_BaseShade_Feather("Shade Feather", Range(0.0001, 1)) = 0.0001
		_ShadeColor_Step("ShadeColor Step", Range(0, 1)) = 0
		_1st2nd_Shades_Feather("Shades Feather", Range(0.0001, 1)) = 0.0001
		_Set_1st_ShadePosition("1st ShadePosition Tex", 2D) = "white" {}
		_Set_2nd_ShadePosition("2nd ShadePosition Tex", 2D) = "white" {}
		_ShadingGradeMap("ShadingGradeMap", 2D) = "white" {}
		_Tweak_ShadingGradeMapLevel("Tweak SGM Level", Range(-0.5, 0.5)) = 0
		_BlurLevelSGM("Blur Level", Range(0, 10)) = 0

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
		[HideInInspector] _1st_ShadeColor_Step("1st_ShadeColor_Step", Range(0, 1)) = 0.5
		[HideInInspector] _1st_ShadeColor_Feather("1st_ShadeColor_Feather", Range(0.0001, 1)) = 0.0001
		[HideInInspector] _2nd_ShadeColor_Step("2nd_ShadeColor_Step", Range(0, 1)) = 0
		[HideInInspector] _2nd_ShadeColor_Feather("2nd_ShadeColor_Feather", Range(0.0001, 1)) = 0.0001

		_StepOffset("Step ShadeOffset", Range(-0.5, 0.5)) = 0
		[Toggle(_)] _Is_Filter_HiCutPointLightColor("PointLights HiCut Filter", Float) = 1
		_GI_Intensity("GI_Intensity", Range(0, 1)) = 0
		_Unlit_Intensity("Unlit_Intensity", Range(0.001, 4)) = 1


		_HighColor("HighColor", Color) = (0,0,0,1)
		_HighColor_Tex("HighColor_Tex", 2D) = "white" {}
		_HighColor_Power("HighColor_Power", Range(0, 1)) = 0
		_TweakHighColorOnShadow("TweakHighColorOnShadow", Range(0, 1)) = 0
		_Set_HighColorMask("Set_HighColorMask", 2D) = "white" {}
		_Tweak_HighColorMaskLevel("Tweak_HighColorMaskLevel", Range(-1, 1)) = 0
		[Toggle(_)] _Is_NormalMapToHighColor("_Is_NormalMapToHighColor", Float) = 0
		[Toggle(_)] _Is_SpecularToHighColor("_Is_SpecularToHighColor", Float) = 0
		[Toggle(_)] _Is_BlendAddToHiColor("_Is_BlendAddToHiColor", Float) = 0
		[Toggle(_)] _Is_UseTweakHighColorOnShadow("_Is_UseTweakHighColorOnShadow", Float) = 0

		[Toggle(_)] _RimLight("RimLight", Float) = 0
		_RimLightColor("RimLightColor", Color) = (1,1,1,1)
		_RimLight_Power("RimLight_Power", Range(0, 1)) = 0.1
		_RimLight_InsideMask("RimLight_InsideMask", Range(0.0001, 1)) = 0.0001
		_Tweak_LightDirection_MaskLevel("Tweak_LightDirection_MaskLevel", Range(0, 0.5)) = 0
		_Ap_RimLightColor("Ap_RimLightColor", Color) = (1,1,1,1)
		_Ap_RimLight_Power("Ap_RimLight_Power", Range(0, 1)) = 0.1
		_Set_RimLightMask("Set_RimLightMask", 2D) = "white" {}
		_Tweak_RimLightMaskLevel("Tweak_RimLightMaskLevel", Range(-1, 1)) = 0
		[Toggle(_)] _Is_NormalMapToRimLight("Is_NormalMapToRimLight", Float) = 0
		[Toggle(_)] _Is_RimLight_FeatherOff("Is_RimLight_FeatherOff", Float) = 0
		[Toggle(_)] _Is_LightDirection_MaskOn("Is_LightDirection_MaskOn", Float) = 0
		[Toggle(_)] _Is_Antipodean_RimLight("Is_Add_Antipodean_RimLight", Float) = 0
		[Toggle(_)] _Is_ApRimLight_FeatherOff("Is_Ap_RimLight_FeatherOff", Float) = 0

		[Toggle(_)] _MatCap("MatCap", Float) = 0
		_MatCap_Sampler("MatCap_Sampler", 2D) = "black" {}
		_BlurLevelMatcap("Blur Level of MatCap_Sampler", Range(0, 10)) = 0
		_MatCapColor("MatCapColor", Color) = (1,1,1,1)
		_Tweak_MatCapUV("Tweak_MatCapUV", Range(-0.5, 0.5)) = 0
		_Rotate_MatCapUV("Rotate_MatCapUV", Range(-1, 1)) = 0
		_NormalMapForMatCap("NormalMapForMatCap", 2D) = "bump" {}
		_BumpScaleMatcap("Scale for NormalMapforMatCap", Range(0, 1)) = 1
		_Rotate_NormalMapForMatCapUV("Rotate_NormalMapForMatCapUV", Range(-1, 1)) = 0
		_TweakMatCapOnShadow("TweakMatCapOnShadow", Range(0, 1)) = 0
		_Set_MatcapMask("Set_MatcapMask", 2D) = "white" {}
		_Tweak_MatcapMaskLevel("Tweak_MatcapMaskLevel", Range(-1, 1)) = 0
		[Toggle(_)] _Is_BlendAddToMatCap("Is_BlendAddToMatCap", Float) = 1
		[Toggle(_)] _Is_CameraRolling("CameraRolling ", Float) = 0
		[Toggle(_)] _Is_NormalMapForMatCap("Is_NormalMapForMatCap", Float) = 0
		[Toggle(_)] _Is_UseTweakMatCapOnShadow("Is_UseTweakMatCapOnShadow", Float) = 0
		[Toggle(_)] _Is_InverseMatcapMask("Inverse_MatcapMask", Float) = 0
		[Toggle(_)] _Is_Ortho("Orthographic Projection for MatCap", Float) = 0

		[Toggle(_)] _AngelRing("AngelRing", Float) = 0
		_AngelRing_Sampler("AngelRing_Sampler", 2D) = "black" {}
		_AngelRing_Color("AngelRing_Color", Color) = (1,1,1,1)
		_AR_OffsetU("AR_OffsetU", Range(0, 0.5)) = 0
		_AR_OffsetV("AR_OffsetV", Range(0, 1)) = 0.3
		[Toggle(_)] _Is_AngelRingAlphaOn("AngelRing AlphaOn", Float) = 0

		[Toggle(_)] _Emissive("Emissive", Float) = 0
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
		_Outline_Width("Outline Width", Float) = 0
		_Outline_Sampler("Outline Sampler", 2D) = "white" {}
		_Outline_Color("Outline Color", Color) = (0.5,0.5,0.5,1)
		_OutlineTex("Outline Texture", 2D) = "white" {}
		_Offset_Z("Camera Forward_Offset", Float) = 0
		_BakedNormal("Baked Normal Outline", 2D) = "white" {}
		[Toggle(_)] _Is_BlendBaseColor("Is_BlendBaseColor", Float) = 0
		[Toggle(_)] _Is_OutlineTex("Is_OutlineTex", Float) = 0
		[Toggle(_)] _Is_BakedNormal("Is_BakedNormal", Float) = 0
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }


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
			#include "Common.hlsl"
			#include "Function.hlsl"

			CBUFFER_START(UnityPerMaterial)
			float4 _MainColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;

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
			CBUFFER_END

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 texcoord0 : TEXCOORD0;
#ifdef _ANGELRING_ON
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			VertexOutput vert(VertexInput v)
			{
				VertexOutput o = (VertexOutput)0;

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

				VertexPositionInputs vertex = GetVertexPositionInputs(v.vertex.xyz);
				o.pos = vertex.positionCS;

				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
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

			float4 frag(VertexOutput i, half facing : VFACE) : SV_TARGET
			{
				return tex2D(_MainTex, i.uv0);
			}

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
