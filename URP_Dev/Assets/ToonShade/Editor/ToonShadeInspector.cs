using UnityEngine;
using UnityEditor;

namespace ToonShade
{
	public sealed class ToonShadeInspector : ShaderGUI
	{
		public enum _UTS_Technique
		{
			DoubleSideWithFeather,
			ShadingGradeMap,
			OutlineObject
		}

		public enum _OutlineMode
		{
			NormalDirection,
			PositionScaling
		}

		public enum _CullingMode
		{
			CullingOff,
			FrontCulling,
			BackCulling
		}

		public enum _EmissiveMode
		{
			SimpleEmissive,
			EmissiveAnimation
		}

		public _OutlineMode outlineMode;
		public _CullingMode cullingMode;
		public _EmissiveMode emissiveMode;

		public GUILayoutOption[] shortButtonStyle = new GUILayoutOption[] { GUILayout.Width(130) };
		public GUILayoutOption[] middleButtonStyle = new GUILayoutOption[] { GUILayout.Width(130) };

		static float _UTS2VersionNumber = 2.075f;
		static int _StencilNo_Setting;
		static bool _OriginalInspector = false;
		static bool _SimpleUI = false;

		bool _Use_VrcRecommend = false;
		bool _RemovedUnusedKeywordsMessage = false;

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
		static bool _LightColorContribution_Foldout = false;
		static bool _AdditionalLightingSettings_Foldout = false;

		private MaterialProperty clippingMask = null;
		private MaterialProperty clipping_Level = null;
		private MaterialProperty tweak_transparency = null;
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

		private MaterialEditor m_MaterialEditor;

		public void FindProperties(MaterialProperty[] props)
		{
			clippingMask = FindProperty("_ClippingMask", props, false);
			clipping_Level = FindProperty("_Clipping_Level", props, false);
			tweak_transparency = FindProperty("_Tweak_transparency", props, false);
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
			baseColor_Step = FindProperty("_BaseColor_Step", props);
			baseShade_Feather = FindProperty("_BaseShade_Feather", props);
			shadeColor_Step = FindProperty("_ShadeColor_Step", props);
			first2nd_Shades_Feather = FindProperty("_1st2nd_Shades_Feather", props);
			first_ShadeColor_Step = FindProperty("_1st_ShadeColor_Step", props);
			first_ShadeColor_Feather = FindProperty("_1st_ShadeColor_Feather", props);
			second_ShadeColor_Step = FindProperty("_2nd_ShadeColor_Step", props);
			second_ShadeColor_Feather = FindProperty("_2nd_ShadeColor_Feather", props);
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
			gi_Intensity = FindProperty("_GI_Intensity", props);
			unlit_Intensity = FindProperty("_Unlit_Intensity", props);
			offset_X_Axis_BLD = FindProperty("_Offset_X_Axis_BLD", props);
			offset_Y_Axis_BLD = FindProperty("_Offset_Y_Axis_BLD", props);
		}

		private static void Line()
		{
			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
		}

		private static bool Foldout(bool display, string title)
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

		private static bool FoldoutSubMenu(bool display, string title)
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

			if (material.GetFloat("_utsVersion") < _UTS2VersionNumber)
			{
				if (material.GetTexture("_BaseMap") != null)
				{
					material.SetTexture("_MainTex", material.GetTexture("_BaseMap"));
					material.SetFloat("_utsVersion", _UTS2VersionNumber);
				}
				else
				{
					material.SetFloat("_utsVersion", _UTS2VersionNumber);
				}
			}

			CheckUtsTechnique(material);

			EditorGUILayout.BeginHorizontal();

			if (material.HasProperty("_simpleUI"))
			{
				var selectedUI = material.GetInt("_simpleUI");
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
						material.SetInt("_simpleUI", 0);
					}
					EditorGUILayout.EndHorizontal();
					m_MaterialEditor.PropertiesDefaultGUI(props);
					return;
				}
				if (GUILayout.Button("Show All properties", middleButtonStyle))
				{
					_OriginalInspector = true;
					material.SetInt("_simpleUI", 2);
				}
			}

			EditorGUILayout.EndHorizontal();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.Space();

			_BasicShaderSettings_Foldout = Foldout(_BasicShaderSettings_Foldout, "Basic Shader Settings");
			if (_BasicShaderSettings_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_SetCullingMode(material);

				if (material.HasProperty("_StencilNo"))
				{

					GUI_SetStencilNo(material);
				}

				if (material.HasProperty("_ClippingMask"))
				{
					GUI_SetClippingMask(material);
				}
				if (material.HasProperty("_Tweak_transparency"))
				{
					GUI_SetTransparencySetting(material);
				}
				GUI_OptionMenu(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();

			_BasicThreeColors_Foldout = Foldout(_BasicThreeColors_Foldout, "【Basic Three Colors and Control Maps Setups】");
			if (_BasicThreeColors_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_BasicThreeColors(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();

			_StepAndFeather_Foldout = Foldout(_StepAndFeather_Foldout, "【Basic Lookdevs : Shading Step and Feather Settings】");
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

			_MatCap_Foldout = Foldout(_MatCap_Foldout, "【MatCap : Texture Projection Settings】");
			if (_MatCap_Foldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				GUI_MatCap(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();

			if (material.HasProperty("_AngelRing"))
			{
				_AngelRing_Foldout = Foldout(_AngelRing_Foldout, "【AngelRing Projection Settings】");
				if (_AngelRing_Foldout)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.Space();
					GUI_AngelRing(material);
					EditorGUI.indentLevel--;
				}

				EditorGUILayout.Space();
			}

			_Emissive_Foldout = Foldout(_Emissive_Foldout, "【Emissive : Self-luminescence Settings】");
			if (_Emissive_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_Emissive(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();

			if (material.HasProperty("_OUTLINE"))
			{
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

			if (EditorGUI.EndChangeCheck())
			{
				m_MaterialEditor.PropertiesChanged();
			}

		}

		private void CheckUtsTechnique(Material material)
		{
			if (material.HasProperty("_utsTechnique"))
			{
				if (material.GetInt("_utsTechnique") == (int)_UTS_Technique.DoubleSideWithFeather)
				{
					if (!material.HasProperty("_Set_1st_ShadePosition"))
					{
						material.SetInt("_utsTechnique", (int)_UTS_Technique.ShadingGradeMap);
					}
				}
				else if (material.GetInt("_utsTechnique") == (int)_UTS_Technique.ShadingGradeMap)
				{
					if (!material.HasProperty("_ShadingGradeMap"))
					{
						material.SetInt("_utsTechnique", (int)_UTS_Technique.DoubleSideWithFeather);
					}
				}
			}
		}

		private void GUI_SetCullingMode(Material material)
		{
			int _CullMode_Setting = material.GetInt("_CullMode");
			if ((int)_CullingMode.CullingOff == _CullMode_Setting)
			{
				cullingMode = _CullingMode.CullingOff;
			}
			else if ((int)_CullingMode.FrontCulling == _CullMode_Setting)
			{
				cullingMode = _CullingMode.FrontCulling;
			}
			else
			{
				cullingMode = _CullingMode.BackCulling;
			}
			cullingMode = (_CullingMode)EditorGUILayout.EnumPopup("Culling Mode", cullingMode);
			if (cullingMode == _CullingMode.CullingOff)
			{
				material.SetFloat("_CullMode", 0);
			}
			else if (cullingMode == _CullingMode.FrontCulling)
			{
				material.SetFloat("_CullMode", 1);
			}
			else
			{
				material.SetFloat("_CullMode", 2);
			}
		}

		private void GUI_SetStencilNo(Material material)
		{
			GUILayout.Label("For _StencilMask or _StencilOut Shader", EditorStyles.boldLabel);
			_StencilNo_Setting = material.GetInt("_StencilNo");
			int _Current_StencilNo = _StencilNo_Setting;
			_Current_StencilNo = (int)EditorGUILayout.IntField("Stencil No.", _Current_StencilNo);
			if (_StencilNo_Setting != _Current_StencilNo)
			{
				material.SetInt("_StencilNo", _Current_StencilNo);
			}
		}

		private void GUI_SetClippingMask(Material material)
		{
			GUILayout.Label("For _Clipping or _TransClipping Shader", EditorStyles.boldLabel);
			m_MaterialEditor.TexturePropertySingleLine(Styles.clippingMaskText, clippingMask);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Inverse Clipping Mask");
			if (material.GetFloat("_Inverse_Clipping") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Inverse_Clipping", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Inverse_Clipping", 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			m_MaterialEditor.RangeProperty(clipping_Level, "Clipping Level");
		}

		private void GUI_SetTransparencySetting(Material material)
		{

			GUILayout.Label("For _TransClipping Shader", EditorStyles.boldLabel);
			m_MaterialEditor.RangeProperty(tweak_transparency, "Transparency Level");
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Use BaseMap α as Clipping Mask");
			if (material.GetFloat("_IsBaseMapAlphaAsClippingMask") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_IsBaseMapAlphaAsClippingMask", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_IsBaseMapAlphaAsClippingMask", 0);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		private void GUI_OptionMenu(Material material)
		{
			GUILayout.Label("Option Menu", EditorStyles.boldLabel);
			if (material.HasProperty("_simpleUI"))
			{
				if (material.GetInt("_simpleUI") == 1)
				{
					_SimpleUI = true;
				}
				else
				{
					_SimpleUI = false;
				}
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Current UI Type");
			if (_SimpleUI == false)
			{
				if (GUILayout.Button("Pro / Full Control", middleButtonStyle))
				{
					material.SetInt("_simpleUI", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Biginner", middleButtonStyle))
				{
					material.SetInt("_simpleUI", 0);
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

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Remove Unused Keywords/Properties from Material");
			if (GUILayout.Button("Execute", middleButtonStyle))
			{
				RemoveUnusedKeywordsUtility(material);
				_RemovedUnusedKeywordsMessage = true;
			}
			EditorGUILayout.EndHorizontal();
			if (_RemovedUnusedKeywordsMessage)
			{
				EditorGUILayout.HelpBox("Unused Material Properties and ShaderKeywords are removed.", MessageType.Info);
			}
		}

		private void RemoveUnusedKeywordsUtility(Material material)
		{
			RemoveUnusedMaterialProperties(material);
			RemoveShaderKeywords(material);
		}

		private void RemoveShaderKeywords(Material material)
		{
			string shaderKeywords = "";
			if (material.HasProperty("_EMISSIVE"))
			{
				float outlineMode = material.GetFloat("_EMISSIVE");
				if (outlineMode == 0)
				{
					shaderKeywords = shaderKeywords + "_EMISSIVE_SIMPLE";
				}
				else
				{
					shaderKeywords = shaderKeywords + "_EMISSIVE_ANIMATION";
				}
			}
			if (material.HasProperty("_OUTLINE"))
			{
				float outlineMode = material.GetFloat("_OUTLINE");
				if (outlineMode == 0)
				{
					shaderKeywords = shaderKeywords + " _OUTLINE_NML";
				}
				else
				{
					shaderKeywords = shaderKeywords + " _OUTLINE_POS";
				}
			}
			var so = new SerializedObject(material);
			so.Update();
			so.FindProperty("m_ShaderKeywords").stringValue = shaderKeywords;
			so.ApplyModifiedProperties();
		}

		private void RemoveUnusedMaterialProperties(Material material)
		{
			var sourceProps = new SerializedObject(material);
			sourceProps.Update();
			var savedProp = sourceProps.FindProperty("m_SavedProperties");
			var texProp = savedProp.FindPropertyRelative("m_TexEnvs");
			for (int i = texProp.arraySize - 1; i >= 0; i--)
			{
				var propertyName = texProp.GetArrayElementAtIndex(i).FindPropertyRelative("first").stringValue;
				if (!material.HasProperty(propertyName))
				{
					texProp.DeleteArrayElementAtIndex(i);
				}
			}
			var floatProp = savedProp.FindPropertyRelative("m_Floats");
			for (int i = floatProp.arraySize - 1; i >= 0; i--)
			{
				var propertyName = floatProp.GetArrayElementAtIndex(i).FindPropertyRelative("first").stringValue;
				if (!material.HasProperty(propertyName))
				{
					floatProp.DeleteArrayElementAtIndex(i);
				}
			}
			var colorProp = savedProp.FindPropertyRelative("m_Colors");
			for (int i = colorProp.arraySize - 1; i >= 0; i--)
			{
				var propertyName = colorProp.GetArrayElementAtIndex(i).FindPropertyRelative("first").stringValue;
				if (!material.HasProperty(propertyName))
				{
					colorProp.DeleteArrayElementAtIndex(i);
				}
			}
			sourceProps.ApplyModifiedProperties();
		}

		private void Set_Vrchat_Recommendation(Material material)
		{
			material.SetFloat("_Is_LightColor_Base", 1);
			material.SetFloat("_Is_LightColor_1st_Shade", 1);
			material.SetFloat("_Is_LightColor_2nd_Shade", 1);
			material.SetFloat("_Is_LightColor_HighColor", 1);
			material.SetFloat("_Is_LightColor_RimLight", 1);
			material.SetFloat("_Is_LightColor_Ap_RimLight", 1);
			material.SetFloat("_Is_LightColor_MatCap", 1);
			if (material.HasProperty("_AngelRing"))
			{
				material.SetFloat("_Is_LightColor_AR", 1);
			}
			if (material.HasProperty("_OUTLINE"))
			{
				material.SetFloat("_Is_LightColor_Outline", 1);
			}
			material.SetFloat("_Set_SystemShadowsToBase", 1);
			material.SetFloat("_Is_Filter_HiCutPointLightColor", 1);
			material.SetFloat("_CameraRolling_Stabilizer", 1);
			material.SetFloat("_Is_Ortho", 0);
			material.SetFloat("_GI_Intensity", 0);
			material.SetFloat("_Unlit_Intensity", 1);
			material.SetFloat("_Is_Filter_LightColor", 1);
		}

		private void GUI_BasicThreeColors(Material material)
		{
			GUILayout.Label("3 Basic Colors Settings : Textures × Colors", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();
			m_MaterialEditor.TexturePropertySingleLine(Styles.baseColorText, mainTex, baseColor);
			if (material.HasProperty("_Color"))
			{
				material.SetColor("_Color", material.GetColor("_BaseColor"));
			}
			if (material.GetFloat("_Use_BaseAs1st") == 0)
			{
				if (GUILayout.Button("No Sharing", middleButtonStyle))
				{
					material.SetFloat("_Use_BaseAs1st", 1);
				}
			}
			else
			{
				if (GUILayout.Button("With 1st ShadeMap", middleButtonStyle))
				{
					material.SetFloat("_Use_BaseAs1st", 0);
				}
			}
			GUILayout.Space(60);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			m_MaterialEditor.TexturePropertySingleLine(Styles.firstShadeColorText, firstShadeMap, firstShadeColor);
			if (material.GetFloat("_Use_1stAs2nd") == 0)
			{
				if (GUILayout.Button("No Sharing", middleButtonStyle))
				{
					material.SetFloat("_Use_1stAs2nd", 1);
				}
			}
			else
			{
				if (GUILayout.Button("With 2nd ShadeMap", middleButtonStyle))
				{
					material.SetFloat("_Use_1stAs2nd", 0);
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
				GUILayout.Label("NormalMap Effectiveness", EditorStyles.boldLabel);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("3 Basic Colors");
				if (material.GetFloat("_Is_NormalMapToBase") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Is_NormalMapToBase", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Is_NormalMapToBase", 0);
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("HighColor");
				if (material.GetFloat("_Is_NormalMapToHighColor") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Is_NormalMapToHighColor", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Is_NormalMapToHighColor", 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("RimLight");
				if (material.GetFloat("_Is_NormalMapToRimLight") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Is_NormalMapToRimLight", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Is_NormalMapToRimLight", 0);
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
			if (material.HasProperty("_utsTechnique"))
			{
				if (material.GetInt("_utsTechnique") == (int)_UTS_Technique.DoubleSideWithFeather)
				{
					GUILayout.Label("Technipue : Double Shade With Feather", EditorStyles.boldLabel);
					m_MaterialEditor.TexturePropertySingleLine(Styles.firstPositionMapText, set_1st_ShadePosition);
					m_MaterialEditor.TexturePropertySingleLine(Styles.secondPositionMapText, set_2nd_ShadePosition);
				}
				else if (material.GetInt("_utsTechnique") == (int)_UTS_Technique.ShadingGradeMap)
				{
					GUILayout.Label("Technipue : Shading Grade Map", EditorStyles.boldLabel);
					m_MaterialEditor.TexturePropertySingleLine(Styles.shadingGradeMapText, shadingGradeMap);
					m_MaterialEditor.RangeProperty(tweak_ShadingGradeMapLevel, "ShadingGradeMap Level");
					m_MaterialEditor.RangeProperty(blurLevelSGM, "Blur Level of ShadingGradeMap");
				}
			}
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

			GUILayout.Label("System Shadows : Self Shadows Receiving", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Receive System Shadows");
			if (material.GetFloat("_Set_SystemShadowsToBase") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Set_SystemShadowsToBase", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Set_SystemShadowsToBase", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat("_Set_SystemShadowsToBase") == 1)
			{
				EditorGUI.indentLevel++;
				m_MaterialEditor.RangeProperty(tweak_SystemShadowsLevel, "System Shadows Level");
				EditorGUI.indentLevel--;
				EditorGUILayout.Space();
			}
			EditorGUILayout.Space();
		}

		private void GUI_BasicLookdevs(Material material)
		{
			if (material.HasProperty("_utsTechnique"))
			{
				if (material.GetInt("_utsTechnique") == (int)_UTS_Technique.DoubleSideWithFeather)
				{
					GUILayout.Label("Technipue : Double Shade With Feather", EditorStyles.boldLabel);
					m_MaterialEditor.RangeProperty(baseColor_Step, "BaseColor Step");
					m_MaterialEditor.RangeProperty(baseShade_Feather, "Base/Shade Feather");
					m_MaterialEditor.RangeProperty(shadeColor_Step, "ShadeColor Step");
					m_MaterialEditor.RangeProperty(first2nd_Shades_Feather, "1st/2nd_Shades Feather");
					material.SetFloat("_1st_ShadeColor_Step", material.GetFloat("_BaseColor_Step"));
					material.SetFloat("_1st_ShadeColor_Feather", material.GetFloat("_BaseShade_Feather"));
					material.SetFloat("_2nd_ShadeColor_Step", material.GetFloat("_ShadeColor_Step"));
					material.SetFloat("_2nd_ShadeColor_Feather", material.GetFloat("_1st2nd_Shades_Feather"));
				}
				else if (material.GetInt("_utsTechnique") == (int)_UTS_Technique.ShadingGradeMap)
				{
					GUILayout.Label("Technipue : Shading Grade Map", EditorStyles.boldLabel);
					m_MaterialEditor.RangeProperty(first_ShadeColor_Step, "1st ShaderColor Step");
					m_MaterialEditor.RangeProperty(first_ShadeColor_Feather, "1st ShadeColor Feather");
					m_MaterialEditor.RangeProperty(second_ShadeColor_Step, "2nd ShadeColor Step");
					m_MaterialEditor.RangeProperty(second_ShadeColor_Feather, "2nd ShadeColor Feather");
					material.SetFloat("_BaseColor_Step", material.GetFloat("_1st_ShadeColor_Step"));
					material.SetFloat("_BaseShade_Feather", material.GetFloat("_1st_ShadeColor_Feather"));
					material.SetFloat("_ShadeColor_Step", material.GetFloat("_2nd_ShadeColor_Step"));
					material.SetFloat("_1st2nd_Shades_Feather", material.GetFloat("_2nd_ShadeColor_Feather"));
				}
				else
				{
					return;
				}
			}
			EditorGUILayout.Space();
		}

		private void GUI_AdditionalLookdevs(Material material)
		{
			GUILayout.Label("Settings for PointLights in ForwardAdd Pass");
			EditorGUI.indentLevel++;
			m_MaterialEditor.RangeProperty(stepOffset, "Step Offset for PointLights");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("PointLights Hi-Cut Filter");
			if (material.GetFloat("_Is_Filter_HiCutPointLightColor") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_Filter_HiCutPointLightColor", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_Filter_HiCutPointLightColor", 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;
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
				if (material.GetFloat("_Is_SpecularToHighColor") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Is_SpecularToHighColor", 1);
						material.SetFloat("_Is_BlendAddToHiColor", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Is_SpecularToHighColor", 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Color Blend Mode");
				if (material.GetFloat("_Is_BlendAddToHiColor") == 0)
				{
					if (GUILayout.Button("Multiply", shortButtonStyle))
					{
						material.SetFloat("_Is_BlendAddToHiColor", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Additive", shortButtonStyle))
					{
						if (material.GetFloat("_Is_SpecularToHighColor") == 1)
						{
							material.SetFloat("_Is_BlendAddToHiColor", 1);
						}
						else
						{
							material.SetFloat("_Is_BlendAddToHiColor", 0);
						}
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("ShadowMask on HihgColor");
				if (material.GetFloat("_Is_UseTweakHighColorOnShadow") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Is_UseTweakHighColorOnShadow", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Is_UseTweakHighColorOnShadow", 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				if (material.GetFloat("_Is_UseTweakHighColorOnShadow") == 1)
				{
					EditorGUI.indentLevel++;
					m_MaterialEditor.RangeProperty(tweakHighColorOnShadow, "HighColor Power on Shadow");
					EditorGUI.indentLevel--;
				}
			}
			EditorGUILayout.Space();

			GUILayout.Label("HighColor Mask", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			m_MaterialEditor.TexturePropertySingleLine(Styles.highColorMaskText, set_HighColorMask);
			m_MaterialEditor.RangeProperty(tweak_HighColorMaskLevel, "HighColor Mask Level");
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();
		}

		private void GUI_RimLight(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("RimLight");
			if (material.GetFloat("_RimLight") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_RimLight", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_RimLight", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat("_RimLight") == 1)
			{
				EditorGUI.indentLevel++;
				GUILayout.Label("RimLight Settings", EditorStyles.boldLabel);
				m_MaterialEditor.ColorProperty(rimLightColor, "RimLight Color");
				m_MaterialEditor.RangeProperty(rimLight_Power, "RimLight Power");

				if (!_SimpleUI)
				{
					m_MaterialEditor.RangeProperty(rimLight_InsideMask, "RimLight Inside Mask");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("RimLight FeatherOff");
					if (material.GetFloat("_RimLight_FeatherOff") == 0)
					{
						if (GUILayout.Button("Off", shortButtonStyle))
						{
							material.SetFloat("_RimLight_FeatherOff", 1);
						}
					}
					else
					{
						if (GUILayout.Button("Active", shortButtonStyle))
						{
							material.SetFloat("_RimLight_FeatherOff", 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("LightDirection Mask");
					if (material.GetFloat("_LightDirection_MaskOn") == 0)
					{
						if (GUILayout.Button("Off", shortButtonStyle))
						{
							material.SetFloat("_LightDirection_MaskOn", 1);
						}
					}
					else
					{
						if (GUILayout.Button("Active", shortButtonStyle))
						{
							material.SetFloat("_LightDirection_MaskOn", 0);
						}
					}
					EditorGUILayout.EndHorizontal();

					if (material.GetFloat("_LightDirection_MaskOn") == 1)
					{
						EditorGUI.indentLevel++;
						m_MaterialEditor.RangeProperty(tweak_LightDirection_MaskLevel, "LightDirection MaskLevel");

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Antipodean(Ap)_RimLight");
						if (material.GetFloat("_Add_Antipodean_RimLight") == 0)
						{
							if (GUILayout.Button("Off", shortButtonStyle))
							{
								material.SetFloat("_Add_Antipodean_RimLight", 1);
							}
						}
						else
						{
							if (GUILayout.Button("Active", shortButtonStyle))
							{
								material.SetFloat("_Add_Antipodean_RimLight", 0);
							}
						}
						EditorGUILayout.EndHorizontal();

						if (material.GetFloat("_Add_Antipodean_RimLight") == 1)
						{
							EditorGUI.indentLevel++;
							GUILayout.Label("Ap_RimLight Settings", EditorStyles.boldLabel);
							m_MaterialEditor.ColorProperty(ap_RimLightColor, "Ap_RimLight Color");
							m_MaterialEditor.RangeProperty(ap_RimLight_Power, "Ap_RimLight Power");

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PrefixLabel("Ap_RimLight FeatherOff");
							if (material.GetFloat("_Ap_RimLight_FeatherOff") == 0)
							{
								if (GUILayout.Button("Off", shortButtonStyle))
								{
									material.SetFloat("_Ap_RimLight_FeatherOff", 1);
								}
							}
							else
							{
								if (GUILayout.Button("Active", shortButtonStyle))
								{
									material.SetFloat("_Ap_RimLight_FeatherOff", 0);
								}
							}
							EditorGUILayout.EndHorizontal();
							EditorGUI.indentLevel--;
						}
						EditorGUI.indentLevel--;
					}
				}
				EditorGUILayout.Space();

				GUILayout.Label("RimLight Mask", EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.rimLightMaskText, set_RimLightMask);
				m_MaterialEditor.RangeProperty(tweak_RimLightMaskLevel, "RimLight Mask Level");
				EditorGUI.indentLevel--;
				EditorGUILayout.Space();
			}
		}

		private void GUI_MatCap(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("MatCap");
			if (material.GetFloat("_MatCap") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_MatCap", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_MatCap", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat("_MatCap") == 1)
			{
				GUILayout.Label("MatCap Settings", EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.matCapSamplerText, matCap_Sampler, matCapColor);
				EditorGUI.indentLevel++;
				m_MaterialEditor.TextureScaleOffsetProperty(matCap_Sampler);

				if (!_SimpleUI)
				{

					m_MaterialEditor.RangeProperty(blurLevelMatcap, "Blur Level of MatCap Sampler");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Color Blend Mode");
					if (material.GetFloat("_Is_BlendAddToMatCap") == 0)
					{
						if (GUILayout.Button("Multipy", shortButtonStyle))
						{
							material.SetFloat("_Is_BlendAddToMatCap", 1);
						}
					}
					else
					{
						if (GUILayout.Button("Additive", shortButtonStyle))
						{
							material.SetFloat("_Is_BlendAddToMatCap", 0);
						}
					}
					EditorGUILayout.EndHorizontal();

					m_MaterialEditor.RangeProperty(tweak_MatCapUV, "Scale MatCapUV");
					m_MaterialEditor.RangeProperty(rotate_MatCapUV, "Rotate MatCapUV");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("CameraRolling Stabilizer");
					if (material.GetFloat("_CameraRolling_Stabilizer") == 0)
					{
						if (GUILayout.Button("Off", shortButtonStyle))
						{
							material.SetFloat("_CameraRolling_Stabilizer", 1);
						}
					}
					else
					{
						if (GUILayout.Button("Active", shortButtonStyle))
						{
							material.SetFloat("_CameraRolling_Stabilizer", 0);
						}
					}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("NormalMap for MatCap");
					if (material.GetFloat("_Is_NormalMapForMatCap") == 0)
					{
						if (GUILayout.Button("Off", shortButtonStyle))
						{
							material.SetFloat("_Is_NormalMapForMatCap", 1);
						}
					}
					else
					{
						if (GUILayout.Button("Active", shortButtonStyle))
						{
							material.SetFloat("_Is_NormalMapForMatCap", 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					if (material.GetFloat("_Is_NormalMapForMatCap") == 1)
					{
						EditorGUI.indentLevel++;
						GUILayout.Label("NormalMap for MatCap as SpecularMask", EditorStyles.boldLabel);
						m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, normalMapForMatCap, bumpScaleMatcap);
						m_MaterialEditor.TextureScaleOffsetProperty(normalMapForMatCap);
						m_MaterialEditor.RangeProperty(rotate_NormalMapForMatCapUV, "Rotate NormalMapUV");
						EditorGUI.indentLevel--;
					}

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("MatCap on Shadow");
					if (material.GetFloat("_Is_UseTweakMatCapOnShadow") == 0)
					{
						if (GUILayout.Button("Off", shortButtonStyle))
						{
							material.SetFloat("_Is_UseTweakMatCapOnShadow", 1);
						}
					}
					else
					{
						if (GUILayout.Button("Active", shortButtonStyle))
						{
							material.SetFloat("_Is_UseTweakMatCapOnShadow", 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					if (material.GetFloat("_Is_UseTweakMatCapOnShadow") == 1)
					{
						EditorGUI.indentLevel++;
						m_MaterialEditor.RangeProperty(tweakMatCapOnShadow, "MatCap Power on Shadow");
						EditorGUI.indentLevel--;
					}

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("MatCap Projection Camera");
					if (material.GetFloat("_Is_Ortho") == 0)
					{
						if (GUILayout.Button("Perspective", middleButtonStyle))
						{
							material.SetFloat("_Is_Ortho", 1);
						}
					}
					else
					{
						if (GUILayout.Button("Orthographic", middleButtonStyle))
						{
							material.SetFloat("_Is_Ortho", 0);
						}
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.Space();

				GUILayout.Label("MatCap Mask", EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.matCapMaskText, set_MatcapMask);
				m_MaterialEditor.TextureScaleOffsetProperty(set_MatcapMask);
				m_MaterialEditor.RangeProperty(tweak_MatcapMaskLevel, "MatCap Mask Level");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Inverse Matcap Mask");

				if (material.GetFloat("_Inverse_MatcapMask") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Inverse_MatcapMask", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Inverse_MatcapMask", 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUI.indentLevel--;
			}
		}

		private void GUI_AngelRing(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("AngelRing Projection");
			if (material.GetFloat("_AngelRing") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_AngelRing", 1);
					material.EnableKeyword("_IS_ANGELRING_ON");
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_AngelRing", 0);
					material.DisableKeyword("_IS_ANGELRING_ON");
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat("_AngelRing") == 1)
			{
				GUILayout.Label("AngelRing Sampler Settings", EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.angelRingText, angelRing_Sampler, angelRing_Color);
				EditorGUI.indentLevel++;
				m_MaterialEditor.RangeProperty(ar_OffsetU, "Offset U");
				m_MaterialEditor.RangeProperty(ar_OffsetV, "Offset V");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Use α channel as Clipping Mask");
				if (material.GetFloat("_ARSampler_AlphaOn") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_ARSampler_AlphaOn", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_ARSampler_AlphaOn", 0);
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}
		}

		private void GUI_Emissive(Material material)
		{
			GUILayout.Label("Emissive Tex × HDR Color", EditorStyles.boldLabel);
			GUILayout.Label("(Bloom Post-Processing Effect necessary)");
			EditorGUILayout.Space();
			m_MaterialEditor.TexturePropertySingleLine(Styles.emissiveTexText, emissive_Tex, emissive_Color);
			m_MaterialEditor.TextureScaleOffsetProperty(emissive_Tex);

			int _EmissiveMode_Setting = material.GetInt("_EMISSIVE");
			if ((int)_EmissiveMode.SimpleEmissive == _EmissiveMode_Setting)
			{
				emissiveMode = _EmissiveMode.SimpleEmissive;
			}
			else if ((int)_EmissiveMode.EmissiveAnimation == _EmissiveMode_Setting)
			{
				emissiveMode = _EmissiveMode.EmissiveAnimation;
			}
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Emissive Animation");

			if (emissiveMode == _EmissiveMode.SimpleEmissive)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_EMISSIVE", 1);
					material.EnableKeyword("_EMISSIVE_ANIMATION");
					material.DisableKeyword("_EMISSIVE_SIMPLE");
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_EMISSIVE", 0);
					material.EnableKeyword("_EMISSIVE_SIMPLE");
					material.DisableKeyword("_EMISSIVE_ANIMATION");
				}
			}
			EditorGUILayout.EndHorizontal();

			if (emissiveMode == _EmissiveMode.EmissiveAnimation)
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.BeginHorizontal();
				m_MaterialEditor.FloatProperty(base_Speed, "Base Speed (Time)");
				if (!_SimpleUI)
				{
					if (material.GetFloat("_Is_ViewCoord_Scroll") == 0)
					{
						if (GUILayout.Button("UV Coord Scroll", shortButtonStyle))
						{
							material.SetFloat("_Is_ViewCoord_Scroll", 1);
						}
					}
					else
					{
						if (GUILayout.Button("View Coord Scroll", shortButtonStyle))
						{
							material.SetFloat("_Is_ViewCoord_Scroll", 0);
						}
					}
				}
				EditorGUILayout.EndHorizontal();

				m_MaterialEditor.RangeProperty(scroll_EmissiveU, "Scroll U/X direction");
				m_MaterialEditor.RangeProperty(scroll_EmissiveV, "Scroll V/Y direction");
				m_MaterialEditor.FloatProperty(rotate_EmissiveUV, "Rotate around UV center");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("PingPong Move for Base");
				if (material.GetFloat("_Is_PingPong_Base") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Is_PingPong_Base", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Is_PingPong_Base", 0);
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;

				if (!_SimpleUI)
				{
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("ColorShift with Time");

					if (material.GetFloat("_Is_ColorShift") == 0)
					{
						if (GUILayout.Button("Off", shortButtonStyle))
						{
							material.SetFloat("_Is_ColorShift", 1);
						}
					}
					else
					{
						if (GUILayout.Button("Active", shortButtonStyle))
						{
							material.SetFloat("_Is_ColorShift", 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel++;
					if (material.GetFloat("_Is_ColorShift") == 1)
					{
						m_MaterialEditor.ColorProperty(colorShift, "Destination Color");
						m_MaterialEditor.FloatProperty(colorShift_Speed, "ColorShift Speed (Time)");
					}
					EditorGUI.indentLevel--;

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("ViewShift of Color");
					if (material.GetFloat("_Is_ViewShift") == 0)
					{
						if (GUILayout.Button("Off", shortButtonStyle))
						{
							material.SetFloat("_Is_ViewShift", 1);
						}
					}
					else
					{
						if (GUILayout.Button("Active", shortButtonStyle))
						{
							material.SetFloat("_Is_ViewShift", 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel++;
					if (material.GetFloat("_Is_ViewShift") == 1)
					{
						m_MaterialEditor.ColorProperty(viewShift, "ViewShift Color");
					}
					EditorGUI.indentLevel--;
				}
			}
			EditorGUILayout.Space();
		}

		private void GUI_Outline(Material material)
		{
			int _OutlineMode_Setting = material.GetInt("_OUTLINE");
			if ((int)_OutlineMode.NormalDirection == _OutlineMode_Setting)
			{
				outlineMode = _OutlineMode.NormalDirection;
			}
			else if ((int)_OutlineMode.PositionScaling == _OutlineMode_Setting)
			{
				outlineMode = _OutlineMode.PositionScaling;
			}
			outlineMode = (_OutlineMode)EditorGUILayout.EnumPopup("Outline Mode", outlineMode);
			if (outlineMode == _OutlineMode.NormalDirection)
			{
				material.SetFloat("_OUTLINE", 0);
				material.EnableKeyword("_OUTLINE_NML");
				material.DisableKeyword("_OUTLINE_POS");
			}
			else if (outlineMode == _OutlineMode.PositionScaling)
			{
				material.SetFloat("_OUTLINE", 1);
				material.EnableKeyword("_OUTLINE_POS");
				material.DisableKeyword("_OUTLINE_NML");
			}

			m_MaterialEditor.FloatProperty(outline_Width, "Outline Width");
			m_MaterialEditor.ColorProperty(outline_Color, "Outline Color");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Blend BaseColor to Outline");
			if (material.GetFloat("_Is_BlendBaseColor") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_BlendBaseColor", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_BlendBaseColor", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			m_MaterialEditor.TexturePropertySingleLine(Styles.outlineSamplerText, outline_Sampler);
			m_MaterialEditor.FloatProperty(offset_Z, "Offset Outline with Camera Z-axis");

			if (!_SimpleUI)
			{
				_AdvancedOutline_Foldout = FoldoutSubMenu(_AdvancedOutline_Foldout, "● Advanced Outline Settings");
				if (_AdvancedOutline_Foldout)
				{
					EditorGUI.indentLevel++;
					GUILayout.Label("Camera Distance for Outline Width");
					m_MaterialEditor.FloatProperty(farthest_Distance, "● Farthest Distance to vanish");
					m_MaterialEditor.FloatProperty(nearest_Distance, "● Nearest Distance to draw with Outline Width");
					EditorGUI.indentLevel--;

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Use Outline Texture");
					if (material.GetFloat("_Is_OutlineTex") == 0)
					{
						if (GUILayout.Button("Off", shortButtonStyle))
						{
							material.SetFloat("_Is_OutlineTex", 1);
						}
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						if (GUILayout.Button("Active", shortButtonStyle))
						{
							material.SetFloat("_Is_OutlineTex", 0);
						}
						EditorGUILayout.EndHorizontal();
						m_MaterialEditor.TexturePropertySingleLine(Styles.outlineTexText, outlineTex);
					}

					if (outlineMode == _OutlineMode.NormalDirection)
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Use Baked Normal for Outline");
						if (material.GetFloat("_Is_BakedNormal") == 0)
						{
							if (GUILayout.Button("Off", shortButtonStyle))
							{
								material.SetFloat("_Is_BakedNormal", 1);
							}
							EditorGUILayout.EndHorizontal();
						}
						else
						{
							if (GUILayout.Button("Active", shortButtonStyle))
							{
								material.SetFloat("_Is_BakedNormal", 0);
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
			if (material.GetFloat("_Is_LightColor_Base") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_Base", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_Base", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("1st ShadeColor");
			if (material.GetFloat("_Is_LightColor_1st_Shade") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_1st_Shade", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_1st_Shade", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("2nd ShadeColor");
			if (material.GetFloat("_Is_LightColor_2nd_Shade") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_2nd_Shade", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_2nd_Shade", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("HighColor");
			if (material.GetFloat("_Is_LightColor_HighColor") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_HighColor", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_HighColor", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("RimLight");
			if (material.GetFloat("_Is_LightColor_RimLight") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_RimLight", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_RimLight", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Ap_RimLight");
			if (material.GetFloat("_Is_LightColor_Ap_RimLight") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_Ap_RimLight", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_Ap_RimLight", 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("MatCap");

			if (material.GetFloat("_Is_LightColor_MatCap") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_MatCap", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_LightColor_MatCap", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.HasProperty("_AngelRing"))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Angel Ring");
				if (material.GetFloat("_Is_LightColor_AR") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Is_LightColor_AR", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Is_LightColor_AR", 0);
					}
				}
				EditorGUILayout.EndHorizontal();
			}

			if (material.HasProperty("_OUTLINE"))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Outline");
				if (material.GetFloat("_Is_LightColor_Outline") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Is_LightColor_Outline", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Is_LightColor_Outline", 0);
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
			if (material.GetFloat("_Is_Filter_LightColor") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_Filter_LightColor", 1);
					material.SetFloat("_Is_LightColor_Base", 1);
					material.SetFloat("_Is_LightColor_1st_Shade", 1);
					material.SetFloat("_Is_LightColor_2nd_Shade", 1);
					if (material.HasProperty("_OUTLINE"))
					{
						material.SetFloat("_Is_LightColor_Outline", 1);
					}
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_Filter_LightColor", 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Built-in Light Direction");
			if (material.GetFloat("_Is_BLD") == 0)
			{
				if (GUILayout.Button("Off", shortButtonStyle))
				{
					material.SetFloat("_Is_BLD", 1);
				}
			}
			else
			{
				if (GUILayout.Button("Active", shortButtonStyle))
				{
					material.SetFloat("_Is_BLD", 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			if (material.GetFloat("_Is_BLD") == 1)
			{
				GUILayout.Label("Built-in Light Direction Settings");
				EditorGUI.indentLevel++;
				m_MaterialEditor.RangeProperty(offset_X_Axis_BLD, "● Offset X-Axis Direction");
				m_MaterialEditor.RangeProperty(offset_Y_Axis_BLD, "● Offset Y-Axis Direction");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("● Inverse Z-Axis Direction");
				if (material.GetFloat("_Inverse_Z_Axis_BLD") == 0)
				{
					if (GUILayout.Button("Off", shortButtonStyle))
					{
						material.SetFloat("_Inverse_Z_Axis_BLD", 1);
					}
				}
				else
				{
					if (GUILayout.Button("Active", shortButtonStyle))
					{
						material.SetFloat("_Inverse_Z_Axis_BLD", 0);
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
		}

	}

}