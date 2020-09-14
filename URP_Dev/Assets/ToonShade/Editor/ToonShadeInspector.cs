using UnityEngine;
using UnityEditor;

namespace ToonShade
{
	public sealed class ToonShadeInspector : ShaderGUI
	{
		const string ShaderDefineSHADINGGRADEMAP = "_SHADINGGRADEMAP";
		const string ShaderDefineANGELRING_ON = "_IS_ANGELRING_ON";
		const string ShaderDefineANGELRING_OFF = "_IS_ANGELRING_OFF";
		const string ShaderDefineUTS_USE_RAYTRACING_SHADOW = "_USE_RAYTRACING_SHADOW";
		const string ShaderPropAngelRing = "_AngelRing";
		const string ShaderPropRTHS = "_RTHS";
		const string ShaderPropMatCap = "_MatCap";
		const string ShaderPropClippingMode = "_ClippingMode";
		const string ShaderPropClippingMask = "_ClippingMask";
		const string ShaderPropSimpleUI = "_simpleUI";
		const string ShaderPropUtsTechniqe = "_utsTechnique";
		const string ShaderPropAutoRenderQueue = "_AutoRenderQueue";
		const string ShaderPropStencilMode = "_StencilMode";
		const string ShaderPropStencilNo = "_StencilNo";
		const string ShaderPropTransparentEnabled = "_TransparentEnabled";
		const string ShaderPropStencilComp = "_StencilComp";
		const string ShaderPropStencilOpPass = "_StencilOpPass";
		const string ShaderPropStencilOpFail = "_StencilOpFail";
		const string ShaderPropStencilWriteMask = "_StencilWriteMask";
		const string ShaderPropStencilReadMask = "_StencilReadMask";
		const string ShaderPropOutline = "_OUTLINE";
		const string ShaderPropNormalMapToHighColor = "_Is_NormalMapToHighColor";
		const string ShaderPropIsNormalMapToRimLight = "_Is_NormalMapToRimLight";
		const string ShaderPropSetSystemShadowsToBase = "_Set_SystemShadowsToBase";
		const string ShaderPropIsFilterHiCutPointLightColor = "_Is_Filter_HiCutPointLightColor";
		const string ShaderPropInverseClipping = "_Inverse_Clipping";
		const string ShaderPropIsBaseMapAlphaAsClippingMask = "_IsBaseMapAlphaAsClippingMask";
		const string ShaderPropIsLightColor_Base = "_Is_LightColor_Base";
		const string ShaderPropCameraRolling_Stabilizer = "_CameraRolling_Stabilizer";
		const string ShaderPropIs_Ortho = "_Is_Ortho";
		const string ShaderPropGI_Intensity = "_GI_Intensity";
		const string ShaderPropUnlit_Intensity = "_Unlit_Intensity";
		const string ShaderPropIs_Filter_LightColor = "_Is_Filter_LightColor";
		const string ShaderPropIs_LightColor_1st_Shade = "_Is_LightColor_1st_Shade";
		const string ShaderPropIs_LightColor_2nd_Shade = "_Is_LightColor_2nd_Shade";
		const string ShaderPropIs_LightColor_HighColor = "_Is_LightColor_HighColor";
		const string ShaderPropIs_LightColor_RimLight = "_Is_LightColor_RimLight";
		const string ShaderPropIs_LightColor_Ap_RimLight = "_Is_LightColor_Ap_RimLight";
		const string ShaderPropIs_LightColor_MatCap = "_Is_LightColor_MatCap";
		const string ShaderPropIs_LightColor_AR = "_Is_LightColor_AR";
		const string ShaderPropIs_LightColor_Outline = "_Is_LightColor_Outline";
		const string ShaderPropInverse_MatcapMask = "_Inverse_MatcapMask";
		const string ShaderPropUse_BaseAs1st = "_Use_BaseAs1st";
		const string ShaderPropUse_1stAs2nd = "_Use_1stAs2nd";
		const string ShaderPropIs_NormalMapToBase = "_Is_NormalMapToBase";
		const string ShaderPropIs_ColorShift = "_Is_ColorShift";
		const string ShaderPropRimLight = "_RimLight";
		const string ShaderPropRimLight_FeatherOff = "_RimLight_FeatherOff";
		const string ShaderPropAp_RimLight_FeatherOff = "_Ap_RimLight_FeatherOff";
		const string ShaderPropIs_BlendAddToMatCap = "_Is_BlendAddToMatCap";
		const string ShaderPropARSampler_AlphaOn = "_ARSampler_AlphaOn";
		const string ShaderPropIs_UseTweakHighColorOnShadow = "_Is_UseTweakHighColorOnShadow";

		const string ShaderPropIs_SpecularToHighColor = "_Is_SpecularToHighColor";
		const string ShaderPropIs_BlendAddToHiColor = "_Is_BlendAddToHiColor";

		const string ShaderPropAdd_Antipodean_RimLight = "_Add_Antipodean_RimLight";
		const string ShaderPropLightDirection_MaskOn = "_LightDirection_MaskOn";

		const string ShaderProp1st_ShadeColor_Step = "_1st_ShadeColor_Step";
		const string ShaderPropBaseColor_Step = "_BaseColor_Step";
		const string ShaderProp1st_ShadeColor_Feather = "_1st_ShadeColor_Feather";
		const string ShaderPropBaseShade_Feather = "_BaseShade_Feather";
		const string ShaderProp2nd_ShadeColor_Step = "_2nd_ShadeColor_Step";
		const string ShaderPropShadeColor_Step = "_ShadeColor_Step";
		const string ShaderProp2nd_ShadeColor_Feather = "_2nd_ShadeColor_Feather";
		const string ShaderProp1st2nd_Shades_Feather = "_1st2nd_Shades_Feather";
		const string ShaderPropIs_NormalMapForMatCap = "_Is_NormalMapForMatCap";
		const string ShaderPropIs_UseTweakMatCapOnShadow = "_Is_UseTweakMatCapOnShadow";
		const string ShaderPropIs_ViewCoord_Scroll = "_Is_ViewCoord_Scroll";
		const string ShaderPropIs_PingPong_Base = "_Is_PingPong_Base";

		const string ShaderPropIs_ViewShift = "_Is_ViewShift";
		const string ShaderPropIs_BlendBaseColor = "_Is_BlendBaseColor";
		const string ShaderPropIs_OutlineTex = "_Is_OutlineTex";
		const string ShaderPropIs_BakedNormal = "_Is_BakedNormal";
		const string ShaderPropIs_BLD = "_Is_BLD";
		const string ShaderPropInverse_Z_Axis_BLD = "_Inverse_Z_Axis_BLD";

		const string ShaderDefineIS_OUTLINE_CLIPPING_NO = "_IS_OUTLINE_CLIPPING_NO";
		const string ShaderDefineIS_OUTLINE_CLIPPING_YES = "_IS_OUTLINE_CLIPPING_YES";

		const string ShaderDefineIS_CLIPPING_OFF = "_IS_CLIPPING_OFF";
		const string ShaderDefineIS_CLIPPING_MODE = "_IS_CLIPPING_MODE";
		const string ShaderDefineIS_CLIPPING_TRANSMODE = "_IS_CLIPPING_TRANSMODE";

		const string ShaderDefineIS_TRANSCLIPPING_OFF = "_IS_TRANSCLIPPING_OFF";
		const string ShaderDefineIS_TRANSCLIPPING_ON = "_IS_TRANSCLIPPING_ON";


		const string STR_ONSTATE = "Active";
		const string STR_OFFSTATE = "Off";

		const string SRPDefaultLightModeName = "SRPDefaultUnlit";
		const string SRPDefaultColorMask = "_SPRDefaultUnlitColorMask";
		const string SRPDefaultCullMode = "_SRPDefaultUnlitColMode";


		public enum TransClippingMode
		{
			Off,
			On,
		}

		public enum Transparent
		{
			Off,
			On,
		}

		public enum ToonStencilMode
		{
			Off,
			StencilOut,
			StencilMask
		}

		public enum StencilType
		{
			Keep,     // Keep the current contents of the buffer.
			Zero,     // Write 0 into the buffer.
			Replace,  // Write the reference value into the buffer.
			IncrSat,  // Increment the current value in the buffer. If the value is 255 already, it stays at 255.
			DecrSat,  // Decrement the current value in the buffer. If the value is 0 already, it stays at 0.
			Invert,   // Negate all the bits.
			IncrWrap, // Increment the current value in the buffer. If the value is 255 already, it becomes 0.
			DecrWrap, // Decrement the current value in the buffer. If the value is 0 already, it becomes 255.
		}

		public enum StencilCompFunction
		{
			Disabled,     // Depth or stencil test is disabled.
			Never,        // Never pass depth or stencil test.
			Less,         // Pass depth or stencil test when new value is less than old one.
			Equal,        // Pass depth or stencil test when values are equal.
			LessEqual,    // Pass depth or stencil test when new value is less or equal than old one.
			Greater,      // Pass depth or stencil test when new value is greater than old one.
			NotEqual,     // Pass depth or stencil test when values are different.
			GreaterEqual, // Pass depth or stencil test when new value is greater or equal than old one.
			Always,       // Always pass depth or stencil test.
		}

		public enum OutlineMode
		{
			NormalDirection,
			PositionScaling
		}

		public enum CullingMode
		{
			CullingOff,
			FrontCulling,
			BackCulling
		}

		public enum EmissiveMode
		{
			SimpleEmissive,
			EmissiveAnimation
		}

		public int _autoRenderQueue = 1;
		public int _renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
		public OutlineMode outlineMode;
		public CullingMode cullingMode;
		public EmissiveMode emissiveMode;


		public GUILayoutOption[] shortButtonStyle = new GUILayoutOption[] { GUILayout.Width(130) };
		public GUILayoutOption[] middleButtonStyle = new GUILayoutOption[] { GUILayout.Width(130) };


		static Transparent _Transparent_Setting;
		static int _StencilNo_Setting;
		static bool _OriginalInspector = false;
		static bool _SimpleUI = false;
		bool _Use_VrcRecommend = false;

		static bool _BasicShaderSettings_Foldout = false;
		static bool _BasicThreeColors_Foldout = true;
		static bool _NormalMap_Foldout = false;
		static bool _ShadowControlMaps_Foldout = false;
		static bool _StepAndFeather_Foldout = true;
		static bool _AdditionalLookdevs_Foldout = false;
		static bool _HighColor_Foldout = true;

		static bool _RimLight_Foldout = true;
		static bool _MatCap_Foldout = true;
		static bool _AngelRing_Foldout = true;
		static bool _Emissive_Foldout = true;
		static bool _Outline_Foldout = true;
		static bool _AdvancedOutline_Foldout = false;
		static bool _Tessellation_Foldout = false;
		static bool _LightColorContribution_Foldout = false;
		static bool _AdditionalLightingSettings_Foldout = false;

		private MaterialProperty transparentMode = null;
		private MaterialProperty clippingMode = null;
		private MaterialProperty clippingMask = null;
		private MaterialProperty clipping_Level = null;
		private MaterialProperty tweak_transparency = null;
		private MaterialProperty stencilMode = null;
		private MaterialProperty mainTex = null;
		private MaterialProperty baseColor = null;
		private MaterialProperty firstShadeMap = null;
		private MaterialProperty firstShadeColor = null;
		private MaterialProperty secondShadeMap = null;
		private MaterialProperty secondShadeColor = null;
		private MaterialProperty normalMap = null;
		private MaterialProperty bumpScale = null;
		private MaterialProperty set_1st_ShadePosition = null;
		private MaterialProperty set_2nd_ShadePosition = null;
		private MaterialProperty shadingGradeMap = null;
		private MaterialProperty tweak_ShadingGradeMapLevel = null;
		private MaterialProperty blurLevelSGM = null;
		private MaterialProperty tweak_SystemShadowsLevel = null;
		private MaterialProperty baseColor_Step = null;
		private MaterialProperty baseShade_Feather = null;
		private MaterialProperty shadeColor_Step = null;
		private MaterialProperty first2nd_Shades_Feather = null;
		private MaterialProperty first_ShadeColor_Step = null;
		private MaterialProperty first_ShadeColor_Feather = null;
		private MaterialProperty second_ShadeColor_Step = null;
		private MaterialProperty second_ShadeColor_Feather = null;
		private MaterialProperty stepOffset = null;
		private MaterialProperty highColor_Tex = null;
		private MaterialProperty highColor = null;
		private MaterialProperty highColor_Power = null;
		private MaterialProperty tweakHighColorOnShadow = null;
		private MaterialProperty set_HighColorMask = null;
		private MaterialProperty tweak_HighColorMaskLevel = null;
		private MaterialProperty rimLightColor = null;
		private MaterialProperty rimLight_Power = null;
		private MaterialProperty rimLight_InsideMask = null;
		private MaterialProperty tweak_LightDirection_MaskLevel = null;
		private MaterialProperty ap_RimLightColor = null;
		private MaterialProperty ap_RimLight_Power = null;
		private MaterialProperty set_RimLightMask = null;
		private MaterialProperty tweak_RimLightMaskLevel = null;
		private MaterialProperty matCap_Sampler = null;
		private MaterialProperty matCapColor = null;
		private MaterialProperty blurLevelMatcap = null;
		private MaterialProperty tweak_MatCapUV = null;
		private MaterialProperty rotate_MatCapUV = null;
		private MaterialProperty normalMapForMatCap = null;
		private MaterialProperty bumpScaleMatcap = null;
		private MaterialProperty rotate_NormalMapForMatCapUV = null;
		private MaterialProperty tweakMatCapOnShadow = null;
		private MaterialProperty set_MatcapMask = null;
		private MaterialProperty tweak_MatcapMaskLevel = null;
		private MaterialProperty angelRing_Sampler = null;
		private MaterialProperty angelRing_Color = null;
		private MaterialProperty ar_OffsetU = null;
		private MaterialProperty ar_OffsetV = null;
		private MaterialProperty emissive_Tex = null;
		private MaterialProperty emissive_Color = null;
		private MaterialProperty base_Speed = null;
		private MaterialProperty scroll_EmissiveU = null;
		private MaterialProperty scroll_EmissiveV = null;
		private MaterialProperty rotate_EmissiveUV = null;
		private MaterialProperty colorShift = null;
		private MaterialProperty colorShift_Speed = null;
		private MaterialProperty viewShift = null;
		private MaterialProperty outline_Width = null;
		private MaterialProperty outline_Color = null;
		private MaterialProperty outline_Sampler = null;
		private MaterialProperty offset_Z = null;
		private MaterialProperty farthest_Distance = null;
		private MaterialProperty nearest_Distance = null;
		private MaterialProperty outlineTex = null;
		private MaterialProperty bakedNormal = null;
		private MaterialProperty tessEdgeLength = null;
		private MaterialProperty tessPhongStrength = null;
		private MaterialProperty tessExtrusionAmount = null;
		private MaterialProperty gi_Intensity = null;
		private MaterialProperty unlit_Intensity = null;
		private MaterialProperty offset_X_Axis_BLD = null;
		private MaterialProperty offset_Y_Axis_BLD = null;

		private MaterialEditor m_MaterialEditor = default;

		private bool IsClippingMaskPropertyAvailable()
		{
			Material material = m_MaterialEditor.target as Material;
			bool bRet = (TransClippingMode)material.GetInt(ShaderPropClippingMode) != TransClippingMode.Off;
			return bRet;
		}

		private bool ClippingModePropertyAvailable
		{
			get
			{
				Material material = m_MaterialEditor.target as Material;
				return material.GetInt(ShaderPropClippingMode) != 0;
			}
		}

		private bool StencilShaderPropertyAvailable => true;


		public static GUIContent transparentModeText = new GUIContent("Transparent Mode", "Transparent  mode that fits you. ");
		public static GUIContent clippingmodeModeText0 = new GUIContent("Clipping Mode", "Select clipping mode that fits you. ");
		public static GUIContent clippingmodeModeText1 = new GUIContent("Trans Clipping", "Select clipping mode that fits you. ");
		public static GUIContent stencilmodeModeText = new GUIContent("Stencil Mode", "Select stencil mode that fits you. ");


		public void FindProperties(MaterialProperty[] props)
		{
			transparentMode = FindProperty(ShaderPropTransparentEnabled, props);
			clippingMask = FindProperty(ShaderPropClippingMask, props);
			clippingMode = FindProperty(ShaderPropClippingMode, props);
			clipping_Level = FindProperty("_Clipping_Level", props, false);
			tweak_transparency = FindProperty("_Tweak_transparency", props, false);
			stencilMode = FindProperty(ShaderPropStencilMode, props);
			mainTex = FindProperty("_MainTex", props);
			baseColor = FindProperty("_BaseColor", props);
			firstShadeMap = FindProperty("_1st_ShadeMap", props);
			firstShadeColor = FindProperty("_1st_ShadeColor", props);
			secondShadeMap = FindProperty("_2nd_ShadeMap", props);
			secondShadeColor = FindProperty("_2nd_ShadeColor", props);
			normalMap = FindProperty("_NormalMap", props);
			bumpScale = FindProperty("_BumpScale", props);
			set_1st_ShadePosition = FindProperty("_Set_1st_ShadePosition", props, false);
			set_2nd_ShadePosition = FindProperty("_Set_2nd_ShadePosition", props, false);
			shadingGradeMap = FindProperty("_ShadingGradeMap", props, false);
			tweak_ShadingGradeMapLevel = FindProperty("_Tweak_ShadingGradeMapLevel", props, false);
			blurLevelSGM = FindProperty("_BlurLevelSGM", props, false);
			tweak_SystemShadowsLevel = FindProperty("_Tweak_SystemShadowsLevel", props);
			baseColor_Step = FindProperty(ShaderPropBaseColor_Step, props);
			baseShade_Feather = FindProperty(ShaderPropBaseShade_Feather, props);
			shadeColor_Step = FindProperty(ShaderPropShadeColor_Step, props);
			first2nd_Shades_Feather = FindProperty(ShaderProp1st2nd_Shades_Feather, props);
			first_ShadeColor_Step = FindProperty(ShaderProp1st_ShadeColor_Step, props);
			first_ShadeColor_Feather = FindProperty(ShaderProp1st_ShadeColor_Feather, props);
			second_ShadeColor_Step = FindProperty(ShaderProp2nd_ShadeColor_Step, props);
			second_ShadeColor_Feather = FindProperty(ShaderProp2nd_ShadeColor_Feather, props);
			stepOffset = FindProperty("_StepOffset", props, false);
			highColor_Tex = FindProperty("_HighColor_Tex", props);
			highColor = FindProperty("_HighColor", props);
			highColor_Power = FindProperty("_HighColor_Power", props);
			tweakHighColorOnShadow = FindProperty("_TweakHighColorOnShadow", props);
			set_HighColorMask = FindProperty("_Set_HighColorMask", props);
			tweak_HighColorMaskLevel = FindProperty("_Tweak_HighColorMaskLevel", props);
			rimLightColor = FindProperty("_RimLightColor", props);
			rimLight_Power = FindProperty("_RimLight_Power", props);
			rimLight_InsideMask = FindProperty("_RimLight_InsideMask", props);
			tweak_LightDirection_MaskLevel = FindProperty("_Tweak_LightDirection_MaskLevel", props);
			ap_RimLightColor = FindProperty("_Ap_RimLightColor", props);
			ap_RimLight_Power = FindProperty("_Ap_RimLight_Power", props);
			set_RimLightMask = FindProperty("_Set_RimLightMask", props);
			tweak_RimLightMaskLevel = FindProperty("_Tweak_RimLightMaskLevel", props);
			matCap_Sampler = FindProperty("_MatCap_Sampler", props);
			matCapColor = FindProperty("_MatCapColor", props);
			blurLevelMatcap = FindProperty("_BlurLevelMatcap", props);
			tweak_MatCapUV = FindProperty("_Tweak_MatCapUV", props);
			rotate_MatCapUV = FindProperty("_Rotate_MatCapUV", props);
			normalMapForMatCap = FindProperty("_NormalMapForMatCap", props);
			bumpScaleMatcap = FindProperty("_BumpScaleMatcap", props);
			rotate_NormalMapForMatCapUV = FindProperty("_Rotate_NormalMapForMatCapUV", props);
			tweakMatCapOnShadow = FindProperty("_TweakMatCapOnShadow", props);
			set_MatcapMask = FindProperty("_Set_MatcapMask", props);
			tweak_MatcapMaskLevel = FindProperty("_Tweak_MatcapMaskLevel", props);
			angelRing_Sampler = FindProperty("_AngelRing_Sampler", props, false);
			angelRing_Color = FindProperty("_AngelRing_Color", props, false);
			ar_OffsetU = FindProperty("_AR_OffsetU", props, false);
			ar_OffsetV = FindProperty("_AR_OffsetV", props, false);
			emissive_Tex = FindProperty("_Emissive_Tex", props);
			emissive_Color = FindProperty("_Emissive_Color", props);
			base_Speed = FindProperty("_Base_Speed", props);
			scroll_EmissiveU = FindProperty("_Scroll_EmissiveU", props);
			scroll_EmissiveV = FindProperty("_Scroll_EmissiveV", props);
			rotate_EmissiveUV = FindProperty("_Rotate_EmissiveUV", props);
			colorShift = FindProperty("_ColorShift", props);
			colorShift_Speed = FindProperty("_ColorShift_Speed", props);
			viewShift = FindProperty("_ViewShift", props);
			outline_Width = FindProperty("_Outline_Width", props, false);
			outline_Color = FindProperty("_Outline_Color", props, false);
			outline_Sampler = FindProperty("_Outline_Sampler", props, false);
			offset_Z = FindProperty("_Offset_Z", props, false);
			farthest_Distance = FindProperty("_Farthest_Distance", props, false);
			nearest_Distance = FindProperty("_Nearest_Distance", props, false);
			outlineTex = FindProperty("_OutlineTex", props, false);
			bakedNormal = FindProperty("_BakedNormal", props, false);
			tessEdgeLength = FindProperty("_TessEdgeLength", props, false);
			tessPhongStrength = FindProperty("_TessPhongStrength", props, false);
			tessExtrusionAmount = FindProperty("_TessExtrusionAmount", props, false);
			gi_Intensity = FindProperty(ShaderPropGI_Intensity, props);
			unlit_Intensity = FindProperty(ShaderPropUnlit_Intensity, props);
			offset_X_Axis_BLD = FindProperty("_Offset_X_Axis_BLD", props);
			offset_Y_Axis_BLD = FindProperty("_Offset_Y_Axis_BLD", props);
		}

		static void Line()
		{
			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
		}

		static bool Foldout(bool display, string title)
		{
			var style = new GUIStyle("ShurikenModuleTitle");
			style.font = new GUIStyle(EditorStyles.boldLabel).font;
			style.border = new RectOffset(15, 7, 4, 4);
			style.fixedHeight = 22;
			style.contentOffset = new Vector2(20f, -2f);

			var rect = GUILayoutUtility.GetRect(16f, 22f, style);
			GUI.Box(rect, title, style);
			var e = Event.current;
			var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
			if (e.type == EventType.Repaint)
			{
				EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
			}
			if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
			{
				display = !display;
				e.Use();
			}
			return display;
		}

		static bool FoldoutSubMenu(bool display, string title)
		{
			var style = new GUIStyle("ShurikenModuleTitle");
			style.font = new GUIStyle(EditorStyles.boldLabel).font;
			style.border = new RectOffset(15, 7, 4, 4);
			style.padding = new RectOffset(5, 7, 4, 4);
			style.fixedHeight = 22;
			style.contentOffset = new Vector2(32f, -2f);

			var rect = GUILayoutUtility.GetRect(16f, 22f, style);
			GUI.Box(rect, title, style);

			var e = Event.current;

			var toggleRect = new Rect(rect.x + 16f, rect.y + 2f, 13f, 13f);
			if (e.type == EventType.Repaint)
			{
				EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
			}
			if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
			{
				display = !display;
				e.Use();
			}

			return display;
		}

		private static class Styles
		{
			public static GUIContent baseColorText = new GUIContent("BaseMap", "Base Color : Texture(sRGB) × Color(RGB) Default:White");
			public static GUIContent firstShadeColorText = new GUIContent("1st ShadeMap", "1st ShadeColor : Texture(sRGB) × Color(RGB) Default:White");
			public static GUIContent secondShadeColorText = new GUIContent("2nd ShadeMap", "2nd ShadeColor : Texture(sRGB) × Color(RGB) Default:White");
			public static GUIContent normalMapText = new GUIContent("NormalMap", "NormalMap : Texture(bump)");
			public static GUIContent highColorText = new GUIContent("HighColor", "High Color : Texture(sRGB) × Color(RGB) Default:Black");
			public static GUIContent highColorMaskText = new GUIContent("HighColor Mask", "HighColor Mask : Texture(linear)");
			public static GUIContent rimLightMaskText = new GUIContent("RimLight Mask", "RimLight Mask : Texture(linear)");
			public static GUIContent matCapSamplerText = new GUIContent("MatCap Sampler", "MatCap Sampler : Texture(sRGB) × Color(RGB) Default:White");
			public static GUIContent matCapMaskText = new GUIContent("MatCap Mask", "MatCap Mask : Texture(linear)");
			public static GUIContent angelRingText = new GUIContent("AngelRing", "AngelRing : Texture(sRGB) × Color(RGB) Default:Black");
			public static GUIContent emissiveTexText = new GUIContent("Emissive", "Emissive : Texture(sRGB)× EmissiveMask(alpha) × Color(HDR) Default:Black");
			public static GUIContent shadingGradeMapText = new GUIContent("Shading Grade Map", "影のかかり方マップ。UV座標で影のかかりやすい場所を指定する。Shading Grade Map : Texture(linear)");
			public static GUIContent firstPositionMapText = new GUIContent("1st Shade Position Map", "1影色領域に落ちる固定影の位置を、UV座標で指定する。1st Position Map : Texture(linear)");
			public static GUIContent secondPositionMapText = new GUIContent("2nd Shade Position Map", "2影色領域に落ちる固定影の位置を、UV座標で指定する。2nd Position Map : Texture(linear)");
			public static GUIContent outlineSamplerText = new GUIContent("Outline Sampler", "Outline Sampler : Texture(linear)");
			public static GUIContent outlineTexText = new GUIContent("Outline tex", "Outline Tex : Texture(sRGB) Default:White");
			public static GUIContent bakedNormalOutlineText = new GUIContent("Baked NormalMap for Outline", "Unpacked Normal Map : Texture(linear) ※通常のノーマルマップではないので注意");
			public static GUIContent clippingMaskText = new GUIContent("Clipping Mask", "Clipping Mask : Texture(linear)");
		}

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
		{
			EditorGUIUtility.fieldWidth = 0;
			FindProperties(props);
			m_MaterialEditor = materialEditor;
			Material material = materialEditor.target as Material;

			_Transparent_Setting = (Transparent)material.GetInt(ShaderPropTransparentEnabled);
			_StencilNo_Setting = material.GetInt(ShaderPropStencilNo);

			EditorGUILayout.BeginHorizontal();

			if (material.HasProperty(ShaderPropSimpleUI))
			{
				var selectedUI = material.GetInt(ShaderPropSimpleUI);
				if (selectedUI == 2)
				{
					_OriginalInspector = true;
				}
				else if (selectedUI == 1)
				{
					_SimpleUI = true;
				}
				if (_OriginalInspector)
				{
					if (GUILayout.Button("Change CustomUI", middleButtonStyle))
					{
						_OriginalInspector = false;
						material.SetInt(ShaderPropSimpleUI, 0);
					}
					EditorGUILayout.EndHorizontal();
					m_MaterialEditor.PropertiesDefaultGUI(props);
					return;
				}
				if (GUILayout.Button("Show All properties", middleButtonStyle))
				{
					_OriginalInspector = true;
					material.SetInt(ShaderPropSimpleUI, 2);
				}
			}

			EditorGUILayout.EndHorizontal();
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.Space();
			_autoRenderQueue = material.GetInt(ShaderPropAutoRenderQueue);
			_renderQueue = material.renderQueue;

			EditorGUILayout.Space();
			_BasicShaderSettings_Foldout = Foldout(_BasicShaderSettings_Foldout, "Basic Shader Settings");
			if (_BasicShaderSettings_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_SetCullingMode(material);
				GUI_SetRenderQueue(material);
				GUI_Tranparent(material);
				if (StencilShaderPropertyAvailable)
				{
					GUI_StencilMode(material);

				}

				//GUILayout.Label("TransClipping Shader", EditorStyles.boldLabel);
				//DoPopup(clippingmodeModeText1, clippingMode, System.Enum.GetNames(typeof(TransClippingMode)));

				EditorGUILayout.Space();
				if (IsClippingMaskPropertyAvailable())
				{
					GUI_SetClippingMask(material);
					GUI_SetTransparencySetting(material);
				}

				//GUI_OptionMenu(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_BasicThreeColors_Foldout = Foldout(_BasicThreeColors_Foldout, "【Basic Three Colors Settings】");
			if (_BasicThreeColors_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_BasicThreeColors(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_StepAndFeather_Foldout = Foldout(_StepAndFeather_Foldout, "【Basic Band Step Settings】");
			if (_StepAndFeather_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_StepAndFeather(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_HighColor_Foldout = Foldout(_HighColor_Foldout, "【HighColor Settings】");
			if (_HighColor_Foldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				GUI_HighColor(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_RimLight_Foldout = Foldout(_RimLight_Foldout, "【RimLight Settings】");
			if (_RimLight_Foldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				GUI_RimLight(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_MatCap_Foldout = Foldout(_MatCap_Foldout, "【MatCap Settings】");
			if (_MatCap_Foldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				GUI_MatCap(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_AngelRing_Foldout = Foldout(_AngelRing_Foldout, "【AngelRing Settings】");
			if (_AngelRing_Foldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				GUI_AngelRing(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_Emissive_Foldout = Foldout(_Emissive_Foldout, "【Emissive Settings】");
			if (_Emissive_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_Emissive(material);
				EditorGUI.indentLevel--;
			}


			EditorGUILayout.Space();
			if (material.HasProperty(ShaderPropOutline) && _Transparent_Setting != Transparent.On)
			{
				SetuOutline(material);
				_Outline_Foldout = Foldout(_Outline_Foldout, "【Outline Settings】");
				if (_Outline_Foldout)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.Space();
					GUI_Outline(material);
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.Space();
			}
			else
			{
				SetupOverDrawTransparentObject(material);
			}


			if (!_SimpleUI)
			{
				_LightColorContribution_Foldout = Foldout(_LightColorContribution_Foldout, "【LightColor Contribution to Materials】");
				if (_LightColorContribution_Foldout)
				{
					EditorGUI.indentLevel++;
					GUI_LightColorContribution(material);
					EditorGUI.indentLevel--;
				}

				EditorGUILayout.Space();

				_AdditionalLightingSettings_Foldout = Foldout(_AdditionalLightingSettings_Foldout, "【Environmental Lighting Contributions Setups】");
				if (_AdditionalLightingSettings_Foldout)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.Space();
					GUI_AdditionalLightingSettings(material);
					EditorGUI.indentLevel--;
				}

				EditorGUILayout.Space();
			}

			ApplyClippingMode(material);
			ApplyStencilMode(material);
			ApplyAngelRing(material);
			ApplyMatCapMode(material);
			ApplyQueueAndRenderType(material);

			if (EditorGUI.EndChangeCheck())
			{
				m_MaterialEditor.PropertiesChanged();
			}
		}

		private void GUI_SetRTHS(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Raytraced Hard Shadow");
			var isRTHSenabled = material.IsKeywordEnabled(ShaderDefineUTS_USE_RAYTRACING_SHADOW);

			if (isRTHSenabled)
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.DisableKeyword(ShaderDefineUTS_USE_RAYTRACING_SHADOW);
				}
			}
			else
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.EnableKeyword(ShaderDefineUTS_USE_RAYTRACING_SHADOW);
				}
			}

			EditorGUILayout.EndHorizontal();
			if (isRTHSenabled)
			{
				EditorGUILayout.LabelField("ShadowRaytracer component must be attached to the camera when this feature is enabled.");
			}
		}

		private void GUI_SetRenderQueue(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Auto Queue");

			if (_autoRenderQueue == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetInt(ShaderPropAutoRenderQueue, _autoRenderQueue = 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetInt(ShaderPropAutoRenderQueue, _autoRenderQueue = 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUI.BeginDisabledGroup(_autoRenderQueue == 1);
			_renderQueue = (int)EditorGUILayout.IntField("Render Queue", _renderQueue);
			EditorGUI.EndDisabledGroup();
		}

		private void GUI_SetCullingMode(Material material)
		{
			const string _CullMode = "_CullMode";
			int _CullMode_Setting = material.GetInt(_CullMode);
			if ((int)CullingMode.CullingOff == _CullMode_Setting)
			{
				cullingMode = CullingMode.CullingOff;
			}
			else if ((int)CullingMode.FrontCulling == _CullMode_Setting)
			{
				cullingMode = CullingMode.FrontCulling;
			}
			else
			{
				cullingMode = CullingMode.BackCulling;
			}
			cullingMode = (CullingMode)EditorGUILayout.EnumPopup("Culling Mode", cullingMode);
			if (_CullMode_Setting != (int)cullingMode)
			{
				switch (cullingMode)
				{
					case CullingMode.CullingOff:
						material.SetInt(_CullMode, 0);
						break;
					case CullingMode.FrontCulling:
						material.SetInt(_CullMode, 1);
						break;
					default:
						material.SetInt(_CullMode, 2);
						break;
				}

			}


		}

		private void GUI_Tranparent(Material material)
		{
			//GUILayout.Label("Transparent Shader", EditorStyles.boldLabel);
			DoPopup(transparentModeText, transparentMode, System.Enum.GetNames(typeof(Transparent)));
			const string _ZWriteMode = "_ZWriteMode";
			const string _ZOverDrawMode = "_ZOverDrawMode";

			if (_Transparent_Setting == Transparent.On)
			{
				material.SetInt(ShaderPropClippingMode, (int)TransClippingMode.On);
				material.SetInt(_ZWriteMode, 0);
				material.SetFloat(_ZOverDrawMode, 1);
			}
			else
			{
				material.SetInt(_ZWriteMode, 1);
				material.SetFloat(_ZOverDrawMode, 0);
			}
		}

		private void GUI_StencilMode(Material material)
		{
			//GUILayout.Label("StencilMask or StencilOut Shader", EditorStyles.boldLabel);
			DoPopup(stencilmodeModeText, stencilMode, System.Enum.GetNames(typeof(ToonStencilMode)));

			int _Current_StencilNo = _StencilNo_Setting;
			_Current_StencilNo = (int)EditorGUILayout.IntField("Stencil No.", _Current_StencilNo);
			if (_StencilNo_Setting != _Current_StencilNo)
			{
				material.SetInt(ShaderPropStencilNo, _Current_StencilNo);
			}

		}

		private void GUI_SetClippingMask(Material material)
		{
			//GUILayout.Label("Options for Clipping or TransClipping features", EditorStyles.boldLabel);
			m_MaterialEditor.TexturePropertySingleLine(Styles.clippingMaskText, clippingMask);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Inverse Clipping Mask");
			if (material.GetFloat(ShaderPropInverseClipping) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropInverseClipping, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropInverseClipping, 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			m_MaterialEditor.RangeProperty(clipping_Level, "Clipping Level");
		}

		private void GUI_SetTransparencySetting(Material material)
		{
			//GUILayout.Label("Options for TransClipping or Transparent features", EditorStyles.boldLabel);
			m_MaterialEditor.RangeProperty(tweak_transparency, "Transparency Level");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("BaseMap As Clipping Mask");
			if (material.GetFloat(ShaderPropIsBaseMapAlphaAsClippingMask) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIsBaseMapAlphaAsClippingMask, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIsBaseMapAlphaAsClippingMask, 0);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// @TODO
		/// </summary>
		/// <param name="material"></param>
		private void GUI_OptionMenu(Material material)
		{
			GUILayout.Label("Option Menu", EditorStyles.boldLabel);
			if (material.HasProperty(ShaderPropSimpleUI))
			{
				_SimpleUI = (material.GetInt(ShaderPropSimpleUI) == 1) ? true : false;
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Current UI Type");
			if (_SimpleUI == false)
			{
				if (GUILayout.Button("Pro / Full Control", middleButtonStyle))
				{
					material.SetInt(ShaderPropSimpleUI, 1);
				}
			}
			else
			{
				if (GUILayout.Button("Biginner", middleButtonStyle))
				{
					material.SetInt(ShaderPropSimpleUI, 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("VRChat Recommendation");

			if (GUILayout.Button("Apply Settings", middleButtonStyle))
			{
				Set_Vrchat_Recommendation(material);
				_Use_VrcRecommend = true;
			}
			EditorGUILayout.EndHorizontal();
			if (_Use_VrcRecommend)
			{
				EditorGUILayout.HelpBox("UTS2 : Applied VRChat Recommended Settings.", MessageType.Info);
			}
		}

		/// <summary>
		/// @TODO
		/// </summary>
		/// <param name="material"></param>
		private void Set_Vrchat_Recommendation(Material material)
		{
			material.SetFloat(ShaderPropIsLightColor_Base, 1);
			material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 1);
			material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 1);
			material.SetFloat(ShaderPropIs_LightColor_HighColor, 1);
			material.SetFloat(ShaderPropIs_LightColor_RimLight, 1);
			material.SetFloat(ShaderPropIs_LightColor_Ap_RimLight, 1);
			material.SetFloat(ShaderPropIs_LightColor_MatCap, 1);
			if (material.HasProperty(ShaderPropAngelRing))
			{
				material.SetFloat(ShaderPropIs_LightColor_AR, 1);
			}
			if (material.HasProperty(ShaderPropOutline))
			{
				material.SetFloat(ShaderPropIs_LightColor_Outline, 1);
			}
			material.SetFloat(ShaderPropSetSystemShadowsToBase, 1);
			material.SetFloat(ShaderPropIsFilterHiCutPointLightColor, 1);
			material.SetFloat(ShaderPropCameraRolling_Stabilizer, 1);
			material.SetFloat(ShaderPropIs_Ortho, 0);
			material.SetFloat(ShaderPropGI_Intensity, 0);
			material.SetFloat(ShaderPropUnlit_Intensity, 1);
			material.SetFloat(ShaderPropIs_Filter_LightColor, 1);
		}

		private void GUI_BasicThreeColors(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			m_MaterialEditor.TexturePropertySingleLine(Styles.baseColorText, mainTex, baseColor);
			material.SetColor("_Color", material.GetColor("_BaseColor"));

			if (material.GetFloat(ShaderPropUse_BaseAs1st) == 0)
			{
				if (GUILayout.Button("No Sharing", middleButtonStyle))
				{
					material.SetFloat(ShaderPropUse_BaseAs1st, 1);
				}
			}
			else
			{
				if (GUILayout.Button("With 1st ShadeMap", middleButtonStyle))
				{
					material.SetFloat(ShaderPropUse_BaseAs1st, 0);
				}
			}
			GUILayout.Space(60);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			m_MaterialEditor.TexturePropertySingleLine(Styles.firstShadeColorText, firstShadeMap, firstShadeColor);
			if (material.GetFloat(ShaderPropUse_1stAs2nd) == 0)
			{
				if (GUILayout.Button("No Sharing", middleButtonStyle))
				{
					material.SetFloat(ShaderPropUse_1stAs2nd, 1);
				}
			}
			else
			{
				if (GUILayout.Button("With 2nd ShadeMap", middleButtonStyle))
				{
					material.SetFloat(ShaderPropUse_1stAs2nd, 0);
				}
			}
			GUILayout.Space(60);
			EditorGUILayout.EndHorizontal();
			m_MaterialEditor.TexturePropertySingleLine(Styles.secondShadeColorText, secondShadeMap, secondShadeColor);
			EditorGUILayout.Space();
			_NormalMap_Foldout = FoldoutSubMenu(_NormalMap_Foldout, "● NormalMap Settings");
			if (_NormalMap_Foldout)
			{
				m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, normalMap, bumpScale);
				m_MaterialEditor.TextureScaleOffsetProperty(normalMap);

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("3 Basic Colors");
				if (material.GetFloat(ShaderPropIs_NormalMapToBase) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_NormalMapToBase, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_NormalMapToBase, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("HighColor");
				if (material.GetFloat(ShaderPropNormalMapToHighColor) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropNormalMapToHighColor, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropNormalMapToHighColor, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("RimLight");
				if (material.GetFloat(ShaderPropIsNormalMapToRimLight) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIsNormalMapToRimLight, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIsNormalMapToRimLight, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space();
			}

			_ShadowControlMaps_Foldout = FoldoutSubMenu(_ShadowControlMaps_Foldout, "● Shadow Control Maps");
			if (_ShadowControlMaps_Foldout)
			{
				GUI_ShadowControlMaps(material);
				EditorGUILayout.Space();
			}
		}

		private void GUI_ShadowControlMaps(Material material)
		{
			//GUILayout.Label("Technipue : Shading Grade Map", EditorStyles.boldLabel);
			m_MaterialEditor.TexturePropertySingleLine(Styles.shadingGradeMapText, shadingGradeMap);
			m_MaterialEditor.RangeProperty(tweak_ShadingGradeMapLevel, "ShadingGradeMap Level");
			m_MaterialEditor.RangeProperty(blurLevelSGM, "Blur Level of ShadingGradeMap");
		}

		private void GUI_StepAndFeather(Material material)
		{
			GUI_BasicLookdevs(material);

			if (!_SimpleUI)
			{
				GUI_SystemShadows(material);

				if (material.HasProperty("_StepOffset"))
				{
					_AdditionalLookdevs_Foldout = FoldoutSubMenu(_AdditionalLookdevs_Foldout, "● Additional Settings");
					if (_AdditionalLookdevs_Foldout)
					{
						GUI_AdditionalLookdevs(material);
					}
				}
			}
		}

		private void GUI_SystemShadows(Material material)
		{
			//GUILayout.Label("System Shadows : Self Shadows Receiving", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Receive System Shadows");
			if (material.GetFloat(ShaderPropSetSystemShadowsToBase) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropSetSystemShadowsToBase, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropSetSystemShadowsToBase, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat(ShaderPropSetSystemShadowsToBase) == 1)
			{
				EditorGUI.indentLevel++;
				m_MaterialEditor.RangeProperty(tweak_SystemShadowsLevel, "System Shadows Level");
				GUI_SetRTHS(material);
				EditorGUI.indentLevel--;
				EditorGUILayout.Space();
			}
			EditorGUILayout.Space();
		}

		private void GUI_BasicLookdevs(Material material)
		{
			//GUILayout.Label("Technipue : Shading Grade Map", EditorStyles.boldLabel);
			m_MaterialEditor.RangeProperty(first_ShadeColor_Step, "1st ShaderColor Step");
			m_MaterialEditor.RangeProperty(first_ShadeColor_Feather, "1st ShadeColor Feather");
			m_MaterialEditor.RangeProperty(second_ShadeColor_Step, "2nd ShadeColor Step");
			m_MaterialEditor.RangeProperty(second_ShadeColor_Feather, "2nd ShadeColor Feather");
			material.SetFloat(ShaderPropBaseColor_Step, material.GetFloat(ShaderProp1st_ShadeColor_Step));
			material.SetFloat(ShaderPropBaseShade_Feather, material.GetFloat(ShaderProp1st_ShadeColor_Feather));
			material.SetFloat(ShaderPropShadeColor_Step, material.GetFloat(ShaderProp2nd_ShadeColor_Step));
			material.SetFloat(ShaderProp1st2nd_Shades_Feather, material.GetFloat(ShaderProp2nd_ShadeColor_Feather));

			EditorGUILayout.Space();
		}

		private void GUI_AdditionalLookdevs(Material material)
		{
			//GUILayout.Label("Settings for PointLights in ForwardAdd Pass");
			//EditorGUI.indentLevel++;
			m_MaterialEditor.RangeProperty(stepOffset, "Step Offset for PointLights");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("PointLights Hi-Cut Filter");
			if (material.GetFloat(ShaderPropIsFilterHiCutPointLightColor) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIsFilterHiCutPointLightColor, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat("_Is_Filter_HiCutPointLightColor", 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			//EditorGUI.indentLevel--;
			EditorGUILayout.Space();
		}

		private void GUI_HighColor(Material material)
		{
			m_MaterialEditor.TexturePropertySingleLine(Styles.highColorText, highColor_Tex, highColor);
			m_MaterialEditor.RangeProperty(highColor_Power, "HighColor Power");

			if (!_SimpleUI)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Specular Mode");
				if (material.GetFloat(ShaderPropIs_SpecularToHighColor) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_SpecularToHighColor, 1);
						material.SetFloat(ShaderPropIs_BlendAddToHiColor, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_SpecularToHighColor, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Color Blend Mode");
				if (material.GetFloat(ShaderPropIs_BlendAddToHiColor) == 0)
				{
					if (GUILayout.Button("Multiply", shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_BlendAddToHiColor, 1);
					}
				}
				else
				{
					if (GUILayout.Button("Additive", shortButtonStyle))
					{
						if (material.GetFloat(ShaderPropIs_SpecularToHighColor) == 1)
						{
							material.SetFloat(ShaderPropIs_BlendAddToHiColor, 1);
						}
						else
						{
							material.SetFloat(ShaderPropIs_BlendAddToHiColor, 0);
						}
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("ShadowMask on HihgColor");
				if (material.GetFloat(ShaderPropIs_UseTweakHighColorOnShadow) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_UseTweakHighColorOnShadow, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_UseTweakHighColorOnShadow, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				if (material.GetFloat(ShaderPropIs_UseTweakHighColorOnShadow) == 1)
				{
					EditorGUI.indentLevel++;
					m_MaterialEditor.RangeProperty(tweakHighColorOnShadow, "HighColor Power on Shadow");
					EditorGUI.indentLevel--;
				}
			}

			EditorGUILayout.Space();

			//GUILayout.Label("HighColor Mask", EditorStyles.boldLabel);
			//EditorGUI.indentLevel++;
			m_MaterialEditor.TexturePropertySingleLine(Styles.highColorMaskText, set_HighColorMask);
			m_MaterialEditor.RangeProperty(tweak_HighColorMaskLevel, "HighColor Mask Level");
			//EditorGUI.indentLevel--;

			EditorGUILayout.Space();
		}

		private void GUI_RimLight(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("RimLight");
			if (material.GetFloat(ShaderPropRimLight) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropRimLight, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropRimLight, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat(ShaderPropRimLight) == 1)
			{
				//EditorGUI.indentLevel++;
				//GUILayout.Label("RimLight Settings", EditorStyles.boldLabel);
				m_MaterialEditor.ColorProperty(rimLightColor, "RimLight Color");
				m_MaterialEditor.RangeProperty(rimLight_Power, "RimLight Power");

				if (!_SimpleUI)
				{
					m_MaterialEditor.RangeProperty(rimLight_InsideMask, "RimLight Inside Mask");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("RimLight FeatherOff");
					if (material.GetFloat(ShaderPropRimLight_FeatherOff) == 0)
					{
						if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropRimLight_FeatherOff, 1);
						}
					}
					else
					{
						if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropRimLight_FeatherOff, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("LightDirection Mask");
					if (material.GetFloat(ShaderPropLightDirection_MaskOn) == 0)
					{
						if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropLightDirection_MaskOn, 1);
						}
					}
					else
					{
						if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropLightDirection_MaskOn, 0);
						}
					}
					EditorGUILayout.EndHorizontal();

					if (material.GetFloat(ShaderPropLightDirection_MaskOn) == 1)
					{
						EditorGUI.indentLevel++;
						m_MaterialEditor.RangeProperty(tweak_LightDirection_MaskLevel, "LightDirection MaskLevel");

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Antipodean(Ap)_RimLight");
						if (material.GetFloat(ShaderPropAdd_Antipodean_RimLight) == 0)
						{
							if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
							{
								material.SetFloat(ShaderPropAdd_Antipodean_RimLight, 1);
							}
						}
						else
						{
							if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
							{
								material.SetFloat(ShaderPropAdd_Antipodean_RimLight, 0);
							}
						}
						EditorGUILayout.EndHorizontal();

						if (material.GetFloat(ShaderPropAdd_Antipodean_RimLight) == 1)
						{
							EditorGUI.indentLevel++;
							GUILayout.Label("Ap_RimLight Settings", EditorStyles.boldLabel);
							m_MaterialEditor.ColorProperty(ap_RimLightColor, "Ap_RimLight Color");
							m_MaterialEditor.RangeProperty(ap_RimLight_Power, "Ap_RimLight Power");

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PrefixLabel("Ap_RimLight FeatherOff");
							if (material.GetFloat(ShaderPropAp_RimLight_FeatherOff) == 0)
							{
								if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
								{
									material.SetFloat(ShaderPropAp_RimLight_FeatherOff, 1);
								}
							}
							else
							{
								if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
								{
									material.SetFloat(ShaderPropAp_RimLight_FeatherOff, 0);
								}
							}
							EditorGUILayout.EndHorizontal();
							EditorGUI.indentLevel--;
						}

						EditorGUI.indentLevel--;

					}
				}

				EditorGUILayout.Space();
				//GUILayout.Label("RimLight Mask", EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.rimLightMaskText, set_RimLightMask);
				m_MaterialEditor.RangeProperty(tweak_RimLightMaskLevel, "RimLight Mask Level");

				//EditorGUI.indentLevel--;
				EditorGUILayout.Space();
			}
		}

		private void GUI_MatCap(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("MatCap");
			if (material.GetFloat(ShaderPropMatCap) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropMatCap, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropMatCap, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat(ShaderPropMatCap) == 1)
			{
				//GUILayout.Label("MatCap Settings", EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.matCapSamplerText, matCap_Sampler, matCapColor);
				m_MaterialEditor.TextureScaleOffsetProperty(matCap_Sampler);

				if (!_SimpleUI)
				{

					m_MaterialEditor.RangeProperty(blurLevelMatcap, "Blur Level of MatCap Sampler");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Color Blend Mode");
					if (material.GetFloat(ShaderPropIs_BlendAddToMatCap) == 0)
					{
						if (GUILayout.Button("Multipy", shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_BlendAddToMatCap, 1);
						}
					}
					else
					{
						if (GUILayout.Button("Additive", shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_BlendAddToMatCap, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					m_MaterialEditor.RangeProperty(tweak_MatCapUV, "Scale MatCapUV");
					m_MaterialEditor.RangeProperty(rotate_MatCapUV, "Rotate MatCapUV");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("CameraRolling Stabilizer");
					if (material.GetFloat(ShaderPropCameraRolling_Stabilizer) == 0)
					{
						if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropCameraRolling_Stabilizer, 1);
						}
					}
					else
					{
						if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropCameraRolling_Stabilizer, 0);
						}
					}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("NormalMap for MatCap");
					if (material.GetFloat(ShaderPropIs_NormalMapForMatCap) == 0)
					{
						if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_NormalMapForMatCap, 1);
						}
					}
					else
					{
						if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_NormalMapForMatCap, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					if (material.GetFloat(ShaderPropIs_NormalMapForMatCap) == 1)
					{
						EditorGUI.indentLevel++;
						m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, normalMapForMatCap, bumpScaleMatcap);
						m_MaterialEditor.TextureScaleOffsetProperty(normalMapForMatCap);
						m_MaterialEditor.RangeProperty(rotate_NormalMapForMatCapUV, "Rotate NormalMapUV");
						EditorGUI.indentLevel--;
					}

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("MatCap on Shadow");
					if (material.GetFloat(ShaderPropIs_UseTweakMatCapOnShadow) == 0)
					{
						if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_UseTweakMatCapOnShadow, 1);
						}
					}
					else
					{
						if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_UseTweakMatCapOnShadow, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					if (material.GetFloat(ShaderPropIs_UseTweakMatCapOnShadow) == 1)
					{
						EditorGUI.indentLevel++;
						m_MaterialEditor.RangeProperty(tweakMatCapOnShadow, "MatCap Power on Shadow");
						EditorGUI.indentLevel--;
					}

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("MatCap Projection Camera");
					if (material.GetFloat(ShaderPropIs_Ortho) == 0)
					{
						if (GUILayout.Button("Perspective", middleButtonStyle))
						{
							material.SetFloat(ShaderPropIs_Ortho, 1);
						}
					}
					else
					{
						if (GUILayout.Button("Orthographic", middleButtonStyle))
						{
							material.SetFloat(ShaderPropIs_Ortho, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
				}

				EditorGUILayout.Space();
				m_MaterialEditor.TexturePropertySingleLine(Styles.matCapMaskText, set_MatcapMask);
				m_MaterialEditor.TextureScaleOffsetProperty(set_MatcapMask);
				m_MaterialEditor.RangeProperty(tweak_MatcapMaskLevel, "MatCap Mask Level");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Inverse Matcap Mask");
				if (material.GetFloat(ShaderPropInverse_MatcapMask) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropInverse_MatcapMask, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropInverse_MatcapMask, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				//EditorGUI.indentLevel--;
			}
		}

		private void GUI_AngelRing(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("AngelRing Projection");
			if (material.GetFloat(ShaderPropAngelRing) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropAngelRing, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropAngelRing, 0);
				}
			}
			EditorGUILayout.EndHorizontal();


			if (material.GetFloat(ShaderPropAngelRing) == 1)
			{
				EditorGUILayout.Space();
				m_MaterialEditor.TexturePropertySingleLine(Styles.angelRingText, angelRing_Sampler, angelRing_Color);
				m_MaterialEditor.RangeProperty(ar_OffsetU, "Offset U");
				m_MaterialEditor.RangeProperty(ar_OffsetV, "Offset V");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Use α channel as Clipping");

				if (material.GetFloat(ShaderPropARSampler_AlphaOn) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropARSampler_AlphaOn, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropARSampler_AlphaOn, 0);
					}
				}
				EditorGUILayout.EndHorizontal();
			}

		}

		private void ApplyQueueAndRenderType(Material material)
		{
			var stencilMode = (ToonStencilMode)material.GetInt(ShaderPropStencilMode);
			if (_autoRenderQueue == 1)
			{
				material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
			}

			const string OPAQUE = "Opaque";
			const string TRANSPARENTCUTOUT = "TransparentCutOut";
			const string TRANSPARENT = "Transparent";
			const string RENDERTYPE = "RenderType";
			const string IGNOREPROJECTION = "IgnoreProjection";
			const string DO_IGNOREPROJECTION = "True";
			const string DONT_IGNOREPROJECTION = "False";
			var renderType = OPAQUE;
			var ignoreProjection = DONT_IGNOREPROJECTION;

			if (_Transparent_Setting == Transparent.On)
			{
				renderType = TRANSPARENT;
				ignoreProjection = DO_IGNOREPROJECTION;
			}
			else
			{
				TransClippingMode transClippingMode = (TransClippingMode)material.GetInt(ShaderPropClippingMode);
				if (transClippingMode == TransClippingMode.Off)
				{
				}
				else
				{
					renderType = TRANSPARENTCUTOUT;

				}
			}

			if (_autoRenderQueue == 1)
			{
				if (_Transparent_Setting == Transparent.On)
				{
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
				}
				else if (stencilMode == ToonStencilMode.StencilMask)
				{
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest - 1;
				}
				else if (stencilMode == ToonStencilMode.StencilOut)
				{
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
				}
			}
			else
			{
				material.renderQueue = _renderQueue;
			}

			material.SetOverrideTag(RENDERTYPE, renderType);
			material.SetOverrideTag(IGNOREPROJECTION, ignoreProjection);
		}

		private void ApplyMatCapMode(Material material)
		{
			if (material.GetInt(ShaderPropClippingMode) == 0)
			{
				if (material.GetFloat(ShaderPropMatCap) == 1)
				{
					material.EnableKeyword(ShaderPropMatCap);
				}
				else
				{
					material.DisableKeyword(ShaderPropMatCap);
				}
			}
			else
			{
				material.DisableKeyword(ShaderPropMatCap);
			}
		}

		private void ApplyAngelRing(Material material)
		{
			int angelRingEnabled = material.GetInt(ShaderPropAngelRing);
			if (angelRingEnabled == 0)
			{
				material.DisableKeyword(ShaderDefineANGELRING_ON);
				material.EnableKeyword(ShaderDefineANGELRING_OFF);
			}
			else
			{
				material.EnableKeyword(ShaderDefineANGELRING_ON);
				material.DisableKeyword(ShaderDefineANGELRING_OFF);
			}
		}

		private void ApplyStencilMode(Material material)
		{
			ToonStencilMode mode = (ToonStencilMode)(material.GetInt(ShaderPropStencilMode));
			switch (mode)
			{
				case ToonStencilMode.Off:
					material.SetInt(ShaderPropStencilComp, (int)StencilCompFunction.Disabled);
					material.SetInt(ShaderPropStencilOpPass, (int)StencilType.Keep);
					material.SetInt(ShaderPropStencilOpFail, (int)StencilType.Keep);
					break;
				case ToonStencilMode.StencilMask:
					material.SetInt(ShaderPropStencilComp, (int)StencilCompFunction.Always);
					material.SetInt(ShaderPropStencilOpPass, (int)StencilType.Replace);
					material.SetInt(ShaderPropStencilOpFail, (int)StencilType.Replace);
					break;
				case ToonStencilMode.StencilOut:
					material.SetInt(ShaderPropStencilComp, (int)StencilCompFunction.NotEqual);
					material.SetInt(ShaderPropStencilOpPass, (int)StencilType.Keep);
					material.SetInt(ShaderPropStencilOpFail, (int)StencilType.Keep);
					break;
			}
		}

		private void ApplyClippingMode(Material material)
		{
			material.DisableKeyword(ShaderDefineIS_CLIPPING_OFF);
			material.DisableKeyword(ShaderDefineIS_CLIPPING_MODE);
			material.DisableKeyword(ShaderDefineIS_CLIPPING_TRANSMODE);
			switch (material.GetInt(ShaderPropClippingMode))
			{
				case 0:
					material.EnableKeyword(ShaderDefineIS_TRANSCLIPPING_OFF);
					material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_ON);
					break;
				default:
					material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_OFF);
					material.EnableKeyword(ShaderDefineIS_TRANSCLIPPING_ON);
					break;

			}
		}

		private void GUI_Emissive(Material material)
		{
			GUILayout.Label("Bloom Post-Processing Effect Necessary", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			m_MaterialEditor.TexturePropertySingleLine(Styles.emissiveTexText, emissive_Tex, emissive_Color);
			m_MaterialEditor.TextureScaleOffsetProperty(emissive_Tex);

			int _EmissiveMode_Setting = material.GetInt("_EMISSIVE");
			if ((int)EmissiveMode.SimpleEmissive == _EmissiveMode_Setting)
			{
				emissiveMode = EmissiveMode.SimpleEmissive;
			}
			else if ((int)EmissiveMode.EmissiveAnimation == _EmissiveMode_Setting)
			{
				emissiveMode = EmissiveMode.EmissiveAnimation;
			}
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Emissive Animation");
			if (emissiveMode == EmissiveMode.SimpleEmissive)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat("_EMISSIVE", 1);
					material.EnableKeyword("_EMISSIVE_ANIMATION");
					material.DisableKeyword("_EMISSIVE_SIMPLE");
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat("_EMISSIVE", 0);
					material.EnableKeyword("_EMISSIVE_SIMPLE");
					material.DisableKeyword("_EMISSIVE_ANIMATION");
				}
			}
			EditorGUILayout.EndHorizontal();

			if (emissiveMode == EmissiveMode.EmissiveAnimation)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginHorizontal();
				m_MaterialEditor.FloatProperty(base_Speed, "Base Speed (Time)");
				if (!_SimpleUI)
				{
					if (material.GetFloat(ShaderPropIs_ViewCoord_Scroll) == 0)
					{
						if (GUILayout.Button("UV Coord Scroll", shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_ViewCoord_Scroll, 1);
						}
					}
					else
					{
						if (GUILayout.Button("View Coord Scroll", shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_ViewCoord_Scroll, 0);
						}
					}
				}
				EditorGUILayout.EndHorizontal();

				m_MaterialEditor.RangeProperty(scroll_EmissiveU, "Scroll U/X direction");
				m_MaterialEditor.RangeProperty(scroll_EmissiveV, "Scroll V/Y direction");
				m_MaterialEditor.FloatProperty(rotate_EmissiveUV, "Rotate around UV center");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("PingPong Move for Base");
				if (material.GetFloat(ShaderPropIs_PingPong_Base) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_PingPong_Base, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_PingPong_Base, 0);
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;

				if (!_SimpleUI)
				{
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("ColorShift with Time");
					if (material.GetFloat(ShaderPropIs_ColorShift) == 0)
					{
						if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_ColorShift, 1);
						}
					}
					else
					{
						if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_ColorShift, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel++;
					if (material.GetFloat(ShaderPropIs_ColorShift) == 1)
					{
						m_MaterialEditor.ColorProperty(colorShift, "Destination Color");
						m_MaterialEditor.FloatProperty(colorShift_Speed, "ColorShift Speed (Time)");
					}
					EditorGUI.indentLevel--;

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("ViewShift of Color");
					if (material.GetFloat(ShaderPropIs_ViewShift) == 0)
					{
						if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_ViewShift, 1);
						}
					}
					else
					{
						if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_ViewShift, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel++;
					if (material.GetFloat(ShaderPropIs_ViewShift) == 1)
					{
						m_MaterialEditor.ColorProperty(viewShift, "ViewShift Color");
					}
					EditorGUI.indentLevel--;
				}
			}
			EditorGUILayout.Space();
		}

		private void SetupOverDrawTransparentObject(Material material)
		{
			var srpDefaultLightModeTag = material.GetTag("LightMode", false, SRPDefaultLightModeName);
			if (srpDefaultLightModeTag == SRPDefaultLightModeName)
			{
				material.SetShaderPassEnabled(SRPDefaultLightModeName, true);
				material.SetInt(SRPDefaultColorMask, 0);
				material.SetInt(SRPDefaultCullMode, (int)CullingMode.BackCulling);
			}
		}

		private void SetuOutline(Material material)
		{
			var srpDefaultLightModeTag = material.GetTag("LightMode", false, SRPDefaultLightModeName);
			if (srpDefaultLightModeTag == SRPDefaultLightModeName)
			{
				material.SetInt(SRPDefaultColorMask, 15);
				material.SetInt(SRPDefaultCullMode, (int)CullingMode.FrontCulling);
			}
		}

		private void GUI_Outline(Material material)
		{
			var srpDefaultLightModeTag = material.GetTag("LightMode", false, SRPDefaultLightModeName);
			bool isOutlineEnabled = true;
			if (srpDefaultLightModeTag == SRPDefaultLightModeName)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Outline");
				if (isOutlineEnabled = material.GetShaderPassEnabled(SRPDefaultLightModeName))
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetShaderPassEnabled(SRPDefaultLightModeName, false);
					}
				}
				else
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetShaderPassEnabled(SRPDefaultLightModeName, true);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			if (!isOutlineEnabled)
			{
				return;
			}
			int _OutlineMode_Setting = material.GetInt(ShaderPropOutline);
			if ((int)OutlineMode.NormalDirection == _OutlineMode_Setting)
			{
				outlineMode = OutlineMode.NormalDirection;
			}
			else if ((int)OutlineMode.PositionScaling == _OutlineMode_Setting)
			{
				outlineMode = OutlineMode.PositionScaling;
			}
			outlineMode = (OutlineMode)EditorGUILayout.EnumPopup("Outline Mode", outlineMode);
			if (outlineMode == OutlineMode.NormalDirection)
			{
				material.SetFloat(ShaderPropOutline, 0);
				material.EnableKeyword("_OUTLINE_NML");
				material.DisableKeyword("_OUTLINE_POS");
			}
			else if (outlineMode == OutlineMode.PositionScaling)
			{
				material.SetFloat(ShaderPropOutline, 1);
				material.EnableKeyword("_OUTLINE_POS");
				material.DisableKeyword("_OUTLINE_NML");
			}

			m_MaterialEditor.FloatProperty(outline_Width, "Outline Width");
			m_MaterialEditor.ColorProperty(outline_Color, "Outline Color");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Blend BaseColor");
			if (material.GetFloat(ShaderPropIs_BlendBaseColor) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_BlendBaseColor, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_BlendBaseColor, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			m_MaterialEditor.TexturePropertySingleLine(Styles.outlineSamplerText, outline_Sampler);
			m_MaterialEditor.FloatProperty(offset_Z, "Camera Z-axis");

			EditorGUILayout.Space();
			if (!_SimpleUI)
			{
				_AdvancedOutline_Foldout = FoldoutSubMenu(_AdvancedOutline_Foldout, "● Advanced Outline Settings");
				if (_AdvancedOutline_Foldout)
				{
					EditorGUI.indentLevel++;
					GUILayout.Label("Camera Distance for Outline Width");
					m_MaterialEditor.FloatProperty(farthest_Distance, "● Farthest Distance");
					m_MaterialEditor.FloatProperty(nearest_Distance, "● Nearest Distance");
					EditorGUI.indentLevel--;

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Use Outline Texture");
					if (material.GetFloat(ShaderPropIs_OutlineTex) == 0)
					{
						if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_OutlineTex, 1);
						}
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ShaderPropIs_OutlineTex, 0);
						}
						EditorGUILayout.EndHorizontal();
						m_MaterialEditor.TexturePropertySingleLine(Styles.outlineTexText, outlineTex);
					}

					if (outlineMode == OutlineMode.NormalDirection)
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Use Baked Normal");
						if (material.GetFloat(ShaderPropIs_BakedNormal) == 0)
						{
							if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
							{
								material.SetFloat(ShaderPropIs_BakedNormal, 1);
							}
							EditorGUILayout.EndHorizontal();
						}
						else
						{
							if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
							{
								material.SetFloat(ShaderPropIs_BakedNormal, 0);
							}
							EditorGUILayout.EndHorizontal();
							m_MaterialEditor.TexturePropertySingleLine(Styles.bakedNormalOutlineText, bakedNormal);
						}
					}
				}
			}
		}

		private void GUI_LightColorContribution(Material material)
		{
			GUILayout.Label("Realtime LightColor Contribution to each colors", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Base Color");

			if (material.GetFloat(ShaderPropIsLightColor_Base) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIsLightColor_Base, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIsLightColor_Base, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("1st ShadeColor");
			if (material.GetFloat(ShaderPropIs_LightColor_1st_Shade) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("2nd ShadeColor");
			if (material.GetFloat(ShaderPropIs_LightColor_2nd_Shade) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("HighColor");
			if (material.GetFloat(ShaderPropIs_LightColor_HighColor) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_HighColor, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_HighColor, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("RimLight");
			if (material.GetFloat(ShaderPropIs_LightColor_RimLight) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_RimLight, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_RimLight, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Ap_RimLight");
			if (material.GetFloat(ShaderPropIs_LightColor_Ap_RimLight) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_Ap_RimLight, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_Ap_RimLight, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("MatCap");
			if (material.GetFloat(ShaderPropIs_LightColor_MatCap) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_MatCap, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_MatCap, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Angel Ring");
			if (material.GetFloat(ShaderPropIs_LightColor_AR) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_AR, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_LightColor_AR, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.HasProperty(ShaderPropOutline))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Outline");
				if (material.GetFloat(ShaderPropIs_LightColor_Outline) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_LightColor_Outline, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropIs_LightColor_Outline, 0);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Space();
		}

		private void GUI_AdditionalLightingSettings(Material material)
		{
			m_MaterialEditor.RangeProperty(gi_Intensity, "GI Intensity");
			m_MaterialEditor.RangeProperty(unlit_Intensity, "Unlit Intensity");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("SceneLights Hi-Cut Filter");
			//GUILayout.Space(60);
			if (material.GetFloat(ShaderPropIs_Filter_LightColor) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_Filter_LightColor, 1);
					material.SetFloat(ShaderPropIsLightColor_Base, 1);
					material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 1);
					material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 1);
					if (material.HasProperty(ShaderPropOutline))
					{
						material.SetFloat(ShaderPropIs_LightColor_Outline, 1);
					}
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_Filter_LightColor, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Built-in Light Direction");
			//GUILayout.Space(60);
			if (material.GetFloat(ShaderPropIs_BLD) == 0)
			{
				if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_BLD, 1);
				}
			}
			else
			{
				if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ShaderPropIs_BLD, 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			if (material.GetFloat(ShaderPropIs_BLD) == 1)
			{
				GUILayout.Label("Built-in Light Direction Settings");
				EditorGUI.indentLevel++;
				m_MaterialEditor.RangeProperty(offset_X_Axis_BLD, "● Offset X-Axis Direction");
				m_MaterialEditor.RangeProperty(offset_Y_Axis_BLD, "● Offset Y-Axis Direction");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("● Inverse Z-Axis Direction");
				if (material.GetFloat(ShaderPropInverse_Z_Axis_BLD) == 0)
				{
					if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropInverse_Z_Axis_BLD, 1);
					}
				}
				else
				{
					if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ShaderPropInverse_Z_Axis_BLD, 0);
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
		}

		public void DoPopup(GUIContent label, MaterialProperty property, string[] options)
		{
			DoPopup(label, property, options, m_MaterialEditor);
		}

		public static void DoPopup(GUIContent label, MaterialProperty property, string[] options, MaterialEditor materialEditor)
		{
			if (property == null)
			{
				throw new System.ArgumentNullException("property");
			}

			EditorGUI.showMixedValue = property.hasMixedValue;
			var mode = property.floatValue;
			EditorGUI.BeginChangeCheck();
			mode = EditorGUILayout.Popup(label, (int)mode, options);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo(label.text);
				property.floatValue = mode;
			}

			EditorGUI.showMixedValue = false;
		}
	}

}