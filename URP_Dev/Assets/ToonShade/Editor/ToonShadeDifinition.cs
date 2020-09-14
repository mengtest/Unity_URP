using UnityEngine;
using UnityEditor;

namespace ToonShade
{

	public static class ToonShadeDifinition
	{
		public static string ShaderDefineSHADINGGRADEMAP = "_SHADINGGRADEMAP";
		public static string ShaderDefineANGELRING_ON = "_IS_ANGELRING_ON";
		public static string ShaderDefineANGELRING_OFF = "_IS_ANGELRING_OFF";
		public static string ShaderDefine_USE_RAYTRACING_SHADOW = "USE_RAYTRACING_SHADOW";
		public static string ShaderPropAngelRing = "_AngelRing";
		public static string ShaderPropRTHS = "_RTHS";
		public static string ShaderPropMatCap = "_MatCap";
		public static string ShaderPropClippingMode = "_ClippingMode";
		public static string ShaderPropClippingMask = "_ClippingMask";
		public static string ShaderPropSimpleUI = "_simpleUI";
		public static string ShaderPropAutoRenderQueue = "_AutoRenderQueue";
		public static string ShaderPropStencilMode = "_StencilMode";
		public static string ShaderPropStencilNo = "_StencilNo";
		public static string ShaderPropTransparentEnabled = "_TransparentEnabled";
		public static string ShaderPropStencilComp = "_StencilComp";
		public static string ShaderPropStencilOpPass = "_StencilOpPass";
		public static string ShaderPropStencilOpFail = "_StencilOpFail";
		public static string ShaderPropStencilWriteMask = "_StencilWriteMask";
		public static string ShaderPropStencilReadMask = "_StencilReadMask";
		public static string ShaderPropOutline = "_OUTLINE";
		public static string ShaderPropNormalMapToHighColor = "_Is_NormalMapToHighColor";
		public static string ShaderPropIsNormalMapToRimLight = "_Is_NormalMapToRimLight";
		public static string ShaderPropSetSystemShadowsToBase = "_Set_SystemShadowsToBase";
		public static string ShaderPropIsFilterHiCutPointLightColor = "_Is_Filter_HiCutPointLightColor";
		public static string ShaderPropInverseClipping = "_Inverse_Clipping";
		public static string ShaderPropIsBaseMapAlphaAsClippingMask = "_IsBaseMapAlphaAsClippingMask";
		public static string ShaderPropIsLightColor_Base = "_Is_LightColor_Base";
		public static string ShaderPropCameraRolling_Stabilizer = "_CameraRolling_Stabilizer";
		public static string ShaderPropIs_Ortho = "_Is_Ortho";
		public static string ShaderPropGI_Intensity = "_GI_Intensity";
		public static string ShaderPropUnlit_Intensity = "_Unlit_Intensity";
		public static string ShaderPropIs_Filter_LightColor = "_Is_Filter_LightColor";
		public static string ShaderPropIs_LightColor_1st_Shade = "_Is_LightColor_1st_Shade";
		public static string ShaderPropIs_LightColor_2nd_Shade = "_Is_LightColor_2nd_Shade";
		public static string ShaderPropIs_LightColor_HighColor = "_Is_LightColor_HighColor";
		public static string ShaderPropIs_LightColor_RimLight = "_Is_LightColor_RimLight";
		public static string ShaderPropIs_LightColor_Ap_RimLight = "_Is_LightColor_Ap_RimLight";
		public static string ShaderPropIs_LightColor_MatCap = "_Is_LightColor_MatCap";
		public static string ShaderPropIs_LightColor_AR = "_Is_LightColor_AR";
		public static string ShaderPropIs_LightColor_Outline = "_Is_LightColor_Outline";
		public static string ShaderPropInverse_MatcapMask = "_Inverse_MatcapMask";
		public static string ShaderPropUse_BaseAs1st = "_Use_BaseAs1st";
		public static string ShaderPropUse_1stAs2nd = "_Use_1stAs2nd";
		public static string ShaderPropIs_NormalMapToBase = "_Is_NormalMapToBase";
		public static string ShaderPropIs_ColorShift = "_Is_ColorShift";
		public static string ShaderPropRimLight = "_RimLight";
		public static string ShaderPropRimLight_FeatherOff = "_RimLight_FeatherOff";
		public static string ShaderPropAp_RimLight_FeatherOff = "_Ap_RimLight_FeatherOff";
		public static string ShaderPropIs_BlendAddToMatCap = "_Is_BlendAddToMatCap";
		public static string ShaderPropARSampler_AlphaOn = "_ARSampler_AlphaOn";
		public static string ShaderPropIs_UseTweakHighColorOnShadow = "_Is_UseTweakHighColorOnShadow";

		public static string ShaderPropIs_SpecularToHighColor = "_Is_SpecularToHighColor";
		public static string ShaderPropIs_BlendAddToHiColor = "_Is_BlendAddToHiColor";

		public static string ShaderPropAdd_Antipodean_RimLight = "_Add_Antipodean_RimLight";
		public static string ShaderPropLightDirection_MaskOn = "_LightDirection_MaskOn";

		public static string ShaderProp1st_ShadeColor_Step = "_1st_ShadeColor_Step";
		public static string ShaderPropBaseColor_Step = "_BaseColor_Step";
		public static string ShaderProp1st_ShadeColor_Feather = "_1st_ShadeColor_Feather";
		public static string ShaderPropBaseShade_Feather = "_BaseShade_Feather";
		public static string ShaderProp2nd_ShadeColor_Step = "_2nd_ShadeColor_Step";
		public static string ShaderPropShadeColor_Step = "_ShadeColor_Step";
		public static string ShaderProp2nd_ShadeColor_Feather = "_2nd_ShadeColor_Feather";
		public static string ShaderProp1st2nd_Shades_Feather = "_1st2nd_Shades_Feather";
		public static string ShaderPropIs_NormalMapForMatCap = "_Is_NormalMapForMatCap";
		public static string ShaderPropIs_UseTweakMatCapOnShadow = "_Is_UseTweakMatCapOnShadow";
		public static string ShaderPropIs_ViewCoord_Scroll = "_Is_ViewCoord_Scroll";
		public static string ShaderPropIs_PingPong_Base = "_Is_PingPong_Base";

		public static string ShaderPropIs_ViewShift = "_Is_ViewShift";
		public static string ShaderPropIs_BlendBaseColor = "_Is_BlendBaseColor";
		public static string ShaderPropIs_OutlineTex = "_Is_OutlineTex";
		public static string ShaderPropIs_BakedNormal = "_Is_BakedNormal";
		public static string ShaderPropIs_BLD = "_Is_BLD";
		public static string ShaderPropInverse_Z_Axis_BLD = "_Inverse_Z_Axis_BLD";

		public static string ShaderDefineIS_OUTLINE_CLIPPING_NO = "_IS_OUTLINE_CLIPPING_NO";
		public static string ShaderDefineIS_OUTLINE_CLIPPING_YES = "_IS_OUTLINE_CLIPPING_YES";

		public static string ShaderDefineIS_CLIPPING_OFF = "_IS_CLIPPING_OFF";
		public static string ShaderDefineIS_CLIPPING_MODE = "_IS_CLIPPING_MODE";
		public static string ShaderDefineIS_CLIPPING_TRANSMODE = "_IS_CLIPPING_TRANSMODE";

		public static string ShaderDefineIS_TRANSCLIPPING_OFF = "_IS_TRANSCLIPPING_OFF";
		public static string ShaderDefineIS_TRANSCLIPPING_ON = "_IS_TRANSCLIPPING_ON";


		public static string STR_ONSTATE = "Active";
		public static string STR_OFFSTATE = "Off";

		public static string SRPDefaultLightModeName = "SRPDefaultUnlit";
		public static string SRPDefaultColorMask = "_SPRDefaultUnlitColorMask";
		public static string SRPDefaultCullMode = "_SRPDefaultUnlitColMode";


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

		public static void Line()
		{
			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
		}

		public static bool Foldout(bool display, string title)
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

		public static bool FoldoutSubMenu(bool display, string title)
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

	public static class Styles
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


}
