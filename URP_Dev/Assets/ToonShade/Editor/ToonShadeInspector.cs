using UnityEngine;
using UnityEditor;

namespace ToonShade
{
	public sealed class ToonShadeInspector : ShaderGUI
	{
		public int autoRenderQueue = 1;
		public int renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
		public ToonShadeDifinition.OutlineMode outlineMode;
		public ToonShadeDifinition.CullingMode cullingMode;
		public ToonShadeDifinition.EmissiveMode emissiveMode;


		public GUILayoutOption[] shortButtonStyle = new GUILayoutOption[] { GUILayout.Width(130) };
		public GUILayoutOption[] middleButtonStyle = new GUILayoutOption[] { GUILayout.Width(130) };


		private static ToonShadeDifinition.Transparent TransparentSetting;
		private static int stencilNoSetting;
		private static bool originalInspector = false;
		private static bool simpleUI = false;

		/// <summary>
		/// unused props
		/// </summary>
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
			bool bRet = (ToonShadeDifinition.TransClippingMode)material.GetInt(ToonShadeDifinition.ShaderPropClippingMode) != ToonShadeDifinition.TransClippingMode.Off;
			return bRet;
		}

		private bool ClippingModePropertyAvailable
		{
			get
			{
				Material material = m_MaterialEditor.target as Material;
				return material.GetInt(ToonShadeDifinition.ShaderPropClippingMode) != 0;
			}
		}

		private bool StencilShaderPropertyAvailable => true;


		public static GUIContent transparentModeText = new GUIContent("Transparent Mode", "Transparent  mode that fits you. ");
		public static GUIContent clippingmodeModeText0 = new GUIContent("Clipping Mode", "Select clipping mode that fits you. ");
		public static GUIContent clippingmodeModeText1 = new GUIContent("Trans Clipping", "Select clipping mode that fits you. ");
		public static GUIContent stencilmodeModeText = new GUIContent("Stencil Mode", "Select stencil mode that fits you. ");

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
		{
			EditorGUIUtility.fieldWidth = 0;
			FindProperties(props);
			m_MaterialEditor = materialEditor;
			Material material = materialEditor.target as Material;

			TransparentSetting = (ToonShadeDifinition.Transparent)material.GetInt(ToonShadeDifinition.ShaderPropTransparentEnabled);
			stencilNoSetting = material.GetInt(ToonShadeDifinition.ShaderPropStencilNo);

			EditorGUILayout.BeginHorizontal();

			if (material.HasProperty(ToonShadeDifinition.ShaderPropSimpleUI))
			{
				var selectedUI = material.GetInt(ToonShadeDifinition.ShaderPropSimpleUI);
				if (selectedUI == 2)
				{
					originalInspector = true;
				}
				else if (selectedUI == 1)
				{
					simpleUI = true;
				}
				if (originalInspector)
				{
					if (GUILayout.Button("Change CustomUI", middleButtonStyle))
					{
						originalInspector = false;
						material.SetInt(ToonShadeDifinition.ShaderPropSimpleUI, 0);
					}
					EditorGUILayout.EndHorizontal();
					m_MaterialEditor.PropertiesDefaultGUI(props);
					return;
				}
				if (GUILayout.Button("Show All properties", middleButtonStyle))
				{
					originalInspector = true;
					material.SetInt(ToonShadeDifinition.ShaderPropSimpleUI, 2);
				}
			}

			EditorGUILayout.EndHorizontal();
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.Space();
			autoRenderQueue = material.GetInt(ToonShadeDifinition.ShaderPropAutoRenderQueue);
			renderQueue = material.renderQueue;

			EditorGUILayout.Space();
			_BasicShaderSettings_Foldout = ToonShadeDifinition.Foldout(_BasicShaderSettings_Foldout, "Basic Shader Settings");
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
			_BasicThreeColors_Foldout = ToonShadeDifinition.Foldout(_BasicThreeColors_Foldout, "【Basic Three Colors Settings】");
			if (_BasicThreeColors_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_BasicThreeColors(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_StepAndFeather_Foldout = ToonShadeDifinition.Foldout(_StepAndFeather_Foldout, "【Basic Band Step Settings】");
			if (_StepAndFeather_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_StepAndFeather(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_HighColor_Foldout = ToonShadeDifinition.Foldout(_HighColor_Foldout, "【HighColor Settings】");
			if (_HighColor_Foldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				GUI_HighColor(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_RimLight_Foldout = ToonShadeDifinition.Foldout(_RimLight_Foldout, "【RimLight Settings】");
			if (_RimLight_Foldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				GUI_RimLight(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_MatCap_Foldout = ToonShadeDifinition.Foldout(_MatCap_Foldout, "【MatCap Settings】");
			if (_MatCap_Foldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				GUI_MatCap(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_AngelRing_Foldout = ToonShadeDifinition.Foldout(_AngelRing_Foldout, "【AngelRing Settings】");
			if (_AngelRing_Foldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				GUI_AngelRing(material);
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();
			_Emissive_Foldout = ToonShadeDifinition.Foldout(_Emissive_Foldout, "【Emissive Settings】");
			if (_Emissive_Foldout)
			{
				EditorGUI.indentLevel++;
				GUI_Emissive(material);
				EditorGUI.indentLevel--;
			}


			EditorGUILayout.Space();
			if (material.HasProperty(ToonShadeDifinition.ShaderPropOutline) && TransparentSetting != ToonShadeDifinition.Transparent.On)
			{
				SetuOutline(material);
				_Outline_Foldout = ToonShadeDifinition.Foldout(_Outline_Foldout, "【Outline Settings】");
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


			if (!simpleUI)
			{
				_LightColorContribution_Foldout = ToonShadeDifinition.Foldout(_LightColorContribution_Foldout, "【LightColor Contribution to Materials】");
				if (_LightColorContribution_Foldout)
				{
					EditorGUI.indentLevel++;
					GUI_LightColorContribution(material);
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.Space();

				_AdditionalLightingSettings_Foldout = ToonShadeDifinition.Foldout(_AdditionalLightingSettings_Foldout, "【Environmental Lighting Contributions Setups】");
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

		private void FindProperties(MaterialProperty[] props)
		{
			transparentMode = FindProperty(ToonShadeDifinition.ShaderPropTransparentEnabled, props);
			clippingMask = FindProperty(ToonShadeDifinition.ShaderPropClippingMask, props);
			clippingMode = FindProperty(ToonShadeDifinition.ShaderPropClippingMode, props);
			clipping_Level = FindProperty("_Clipping_Level", props, false);
			tweak_transparency = FindProperty("_Tweak_transparency", props, false);
			stencilMode = FindProperty(ToonShadeDifinition.ShaderPropStencilMode, props);
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
			baseColor_Step = FindProperty(ToonShadeDifinition.ShaderPropBaseColor_Step, props);
			baseShade_Feather = FindProperty(ToonShadeDifinition.ShaderPropBaseShade_Feather, props);
			shadeColor_Step = FindProperty(ToonShadeDifinition.ShaderPropShadeColor_Step, props);
			first2nd_Shades_Feather = FindProperty(ToonShadeDifinition.ShaderProp1st2nd_Shades_Feather, props);
			first_ShadeColor_Step = FindProperty(ToonShadeDifinition.ShaderProp1st_ShadeColor_Step, props);
			first_ShadeColor_Feather = FindProperty(ToonShadeDifinition.ShaderProp1st_ShadeColor_Feather, props);
			second_ShadeColor_Step = FindProperty(ToonShadeDifinition.ShaderProp2nd_ShadeColor_Step, props);
			second_ShadeColor_Feather = FindProperty(ToonShadeDifinition.ShaderProp2nd_ShadeColor_Feather, props);
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
			gi_Intensity = FindProperty(ToonShadeDifinition.ShaderPropGI_Intensity, props);
			unlit_Intensity = FindProperty(ToonShadeDifinition.ShaderPropUnlit_Intensity, props);
			offset_X_Axis_BLD = FindProperty("_Offset_X_Axis_BLD", props);
			offset_Y_Axis_BLD = FindProperty("_Offset_Y_Axis_BLD", props);
		}

		private void GUI_SetRTHS(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Raytraced Hard Shadow");
			var isRTHSenabled = material.IsKeywordEnabled(ToonShadeDifinition.ShaderDefine_USE_RAYTRACING_SHADOW);

			if (isRTHSenabled)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.DisableKeyword(ToonShadeDifinition.ShaderDefine_USE_RAYTRACING_SHADOW);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.EnableKeyword(ToonShadeDifinition.ShaderDefine_USE_RAYTRACING_SHADOW);
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

			if (autoRenderQueue == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetInt(ToonShadeDifinition.ShaderPropAutoRenderQueue, autoRenderQueue = 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetInt(ToonShadeDifinition.ShaderPropAutoRenderQueue, autoRenderQueue = 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUI.BeginDisabledGroup(autoRenderQueue == 1);
			renderQueue = (int)EditorGUILayout.IntField("Render Queue", renderQueue);
			EditorGUI.EndDisabledGroup();
		}

		private void GUI_SetCullingMode(Material material)
		{
			const string _CullMode = "_CullMode";
			int _CullMode_Setting = material.GetInt(_CullMode);
			if ((int)ToonShadeDifinition.CullingMode.CullingOff == _CullMode_Setting)
			{
				cullingMode = ToonShadeDifinition.CullingMode.CullingOff;
			}
			else if ((int)ToonShadeDifinition.CullingMode.FrontCulling == _CullMode_Setting)
			{
				cullingMode = ToonShadeDifinition.CullingMode.FrontCulling;
			}
			else
			{
				cullingMode = ToonShadeDifinition.CullingMode.BackCulling;
			}
			cullingMode = (ToonShadeDifinition.CullingMode)EditorGUILayout.EnumPopup("Culling Mode", cullingMode);
			if (_CullMode_Setting != (int)cullingMode)
			{
				switch (cullingMode)
				{
					case ToonShadeDifinition.CullingMode.CullingOff:
						material.SetInt(_CullMode, 0);
						break;
					case ToonShadeDifinition.CullingMode.FrontCulling:
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
			DoPopup(transparentModeText, transparentMode, System.Enum.GetNames(typeof(ToonShadeDifinition.Transparent)));
			const string _ZWriteMode = "_ZWriteMode";
			const string _ZOverDrawMode = "_ZOverDrawMode";

			if (TransparentSetting == ToonShadeDifinition.Transparent.On)
			{
				material.SetInt(ToonShadeDifinition.ShaderPropClippingMode, (int)ToonShadeDifinition.TransClippingMode.On);
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
			DoPopup(stencilmodeModeText, stencilMode, System.Enum.GetNames(typeof(ToonShadeDifinition.ToonStencilMode)));

			int _Current_StencilNo = stencilNoSetting;
			_Current_StencilNo = (int)EditorGUILayout.IntField("Stencil No.", _Current_StencilNo);
			if (stencilNoSetting != _Current_StencilNo)
			{
				material.SetInt(ToonShadeDifinition.ShaderPropStencilNo, _Current_StencilNo);
			}

		}

		private void GUI_SetClippingMask(Material material)
		{
			//GUILayout.Label("Options for Clipping or TransClipping features", EditorStyles.boldLabel);
			m_MaterialEditor.TexturePropertySingleLine(Styles.clippingMaskText, clippingMask);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Inverse Clipping Mask");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropInverseClipping) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropInverseClipping, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropInverseClipping, 0);
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
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIsBaseMapAlphaAsClippingMask) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIsBaseMapAlphaAsClippingMask, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIsBaseMapAlphaAsClippingMask, 0);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		private void GUI_OptionMenu(Material material)
		{
			GUILayout.Label("Option Menu", EditorStyles.boldLabel);
			if (material.HasProperty(ToonShadeDifinition.ShaderPropSimpleUI))
			{
				simpleUI = (material.GetInt(ToonShadeDifinition.ShaderPropSimpleUI) == 1) ? true : false;
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Current UI Type");
			if (simpleUI == false)
			{
				if (GUILayout.Button("Pro / Full Control", middleButtonStyle))
				{
					material.SetInt(ToonShadeDifinition.ShaderPropSimpleUI, 1);
				}
			}
			else
			{
				if (GUILayout.Button("Biginner", middleButtonStyle))
				{
					material.SetInt(ToonShadeDifinition.ShaderPropSimpleUI, 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("VRChat Recommendation");

			if (GUILayout.Button("Apply Settings", middleButtonStyle))
			{
				//Set_Vrchat_Recommendation(material);
				//_Use_VrcRecommend = true;
			}
			EditorGUILayout.EndHorizontal();
			//if (_Use_VrcRecommend)
			//{
			//	EditorGUILayout.HelpBox("UTS2 : Applied VRChat Recommended Settings.", MessageType.Info);
			//}
		}

		private void Set_Vrchat_Recommendation(Material material)
		{
			material.SetFloat(ToonShadeDifinition.ShaderPropIsLightColor_Base, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_1st_Shade, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_2nd_Shade, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_HighColor, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_RimLight, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_Ap_RimLight, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_MatCap, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_AR, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_Outline, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropSetSystemShadowsToBase, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIsFilterHiCutPointLightColor, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropCameraRolling_Stabilizer, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_Ortho, 0);
			material.SetFloat(ToonShadeDifinition.ShaderPropGI_Intensity, 0);
			material.SetFloat(ToonShadeDifinition.ShaderPropUnlit_Intensity, 1);
			material.SetFloat(ToonShadeDifinition.ShaderPropIs_Filter_LightColor, 1);
		}

		private void GUI_BasicThreeColors(Material material)
		{
			EditorGUILayout.BeginHorizontal();
			m_MaterialEditor.TexturePropertySingleLine(Styles.baseColorText, mainTex, baseColor);
			material.SetColor("_Color", material.GetColor("_BaseColor"));

			if (material.GetFloat(ToonShadeDifinition.ShaderPropUse_BaseAs1st) == 0)
			{
				if (GUILayout.Button("No Sharing", middleButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropUse_BaseAs1st, 1);
				}
			}
			else
			{
				if (GUILayout.Button("With 1st ShadeMap", middleButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropUse_BaseAs1st, 0);
				}
			}
			GUILayout.Space(60);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			m_MaterialEditor.TexturePropertySingleLine(Styles.firstShadeColorText, firstShadeMap, firstShadeColor);
			if (material.GetFloat(ToonShadeDifinition.ShaderPropUse_1stAs2nd) == 0)
			{
				if (GUILayout.Button("No Sharing", middleButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropUse_1stAs2nd, 1);
				}
			}
			else
			{
				if (GUILayout.Button("With 2nd ShadeMap", middleButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropUse_1stAs2nd, 0);
				}
			}
			GUILayout.Space(60);
			EditorGUILayout.EndHorizontal();
			m_MaterialEditor.TexturePropertySingleLine(Styles.secondShadeColorText, secondShadeMap, secondShadeColor);
			EditorGUILayout.Space();
			_NormalMap_Foldout = ToonShadeDifinition.FoldoutSubMenu(_NormalMap_Foldout, "NormalMap Settings");
			if (_NormalMap_Foldout)
			{
				m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, normalMap, bumpScale);
				m_MaterialEditor.TextureScaleOffsetProperty(normalMap);

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("3 Basic Colors");
				if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_NormalMapToBase) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_NormalMapToBase, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_NormalMapToBase, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("HighColor");
				if (material.GetFloat(ToonShadeDifinition.ShaderPropNormalMapToHighColor) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropNormalMapToHighColor, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropNormalMapToHighColor, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("RimLight");
				if (material.GetFloat(ToonShadeDifinition.ShaderPropIsNormalMapToRimLight) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIsNormalMapToRimLight, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIsNormalMapToRimLight, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space();
			}

			_ShadowControlMaps_Foldout = ToonShadeDifinition.FoldoutSubMenu(_ShadowControlMaps_Foldout, "Shadow Control Maps");
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

			if (!simpleUI)
			{
				GUI_SystemShadows(material);

				if (material.HasProperty("_StepOffset"))
				{
					_AdditionalLookdevs_Foldout = ToonShadeDifinition.FoldoutSubMenu(_AdditionalLookdevs_Foldout, "Additional Settings");
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
			if (material.GetFloat(ToonShadeDifinition.ShaderPropSetSystemShadowsToBase) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropSetSystemShadowsToBase, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropSetSystemShadowsToBase, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat(ToonShadeDifinition.ShaderPropSetSystemShadowsToBase) == 1)
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
			material.SetFloat(ToonShadeDifinition.ShaderPropBaseColor_Step, material.GetFloat(ToonShadeDifinition.ShaderProp1st_ShadeColor_Step));
			material.SetFloat(ToonShadeDifinition.ShaderPropBaseShade_Feather, material.GetFloat(ToonShadeDifinition.ShaderProp1st_ShadeColor_Feather));
			material.SetFloat(ToonShadeDifinition.ShaderPropShadeColor_Step, material.GetFloat(ToonShadeDifinition.ShaderProp2nd_ShadeColor_Step));
			material.SetFloat(ToonShadeDifinition.ShaderProp1st2nd_Shades_Feather, material.GetFloat(ToonShadeDifinition.ShaderProp2nd_ShadeColor_Feather));

			EditorGUILayout.Space();
		}

		private void GUI_AdditionalLookdevs(Material material)
		{
			//GUILayout.Label("Settings for PointLights in ForwardAdd Pass");
			//EditorGUI.indentLevel++;
			m_MaterialEditor.RangeProperty(stepOffset, "Step Offset for PointLights");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("PointLights Hi-Cut Filter");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIsFilterHiCutPointLightColor) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIsFilterHiCutPointLightColor, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
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

			if (!simpleUI)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Specular Mode");
				if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_SpecularToHighColor) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_SpecularToHighColor, 1);
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_BlendAddToHiColor, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_SpecularToHighColor, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Color Blend Mode");
				if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_BlendAddToHiColor) == 0)
				{
					if (GUILayout.Button("Multiply", shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_BlendAddToHiColor, 1);
					}
				}
				else
				{
					if (GUILayout.Button("Additive", shortButtonStyle))
					{
						if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_SpecularToHighColor) == 1)
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_BlendAddToHiColor, 1);
						}
						else
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_BlendAddToHiColor, 0);
						}
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("ShadowMask on HihgColor");
				if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_UseTweakHighColorOnShadow) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_UseTweakHighColorOnShadow, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_UseTweakHighColorOnShadow, 0);
					}
				}
				EditorGUILayout.EndHorizontal();

				if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_UseTweakHighColorOnShadow) == 1)
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
			if (material.GetFloat(ToonShadeDifinition.ShaderPropRimLight) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropRimLight, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropRimLight, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat(ToonShadeDifinition.ShaderPropRimLight) == 1)
			{
				//EditorGUI.indentLevel++;
				//GUILayout.Label("RimLight Settings", EditorStyles.boldLabel);
				m_MaterialEditor.ColorProperty(rimLightColor, "RimLight Color");
				m_MaterialEditor.RangeProperty(rimLight_Power, "RimLight Power");

				if (!simpleUI)
				{
					m_MaterialEditor.RangeProperty(rimLight_InsideMask, "RimLight Inside Mask");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("RimLight FeatherOff");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropRimLight_FeatherOff) == 0)
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropRimLight_FeatherOff, 1);
						}
					}
					else
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropRimLight_FeatherOff, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("LightDirection Mask");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropLightDirection_MaskOn) == 0)
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropLightDirection_MaskOn, 1);
						}
					}
					else
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropLightDirection_MaskOn, 0);
						}
					}
					EditorGUILayout.EndHorizontal();

					if (material.GetFloat(ToonShadeDifinition.ShaderPropLightDirection_MaskOn) == 1)
					{
						EditorGUI.indentLevel++;
						m_MaterialEditor.RangeProperty(tweak_LightDirection_MaskLevel, "LightDirection MaskLevel");

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Antipodean(Ap)_RimLight");
						if (material.GetFloat(ToonShadeDifinition.ShaderPropAdd_Antipodean_RimLight) == 0)
						{
							if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
							{
								material.SetFloat(ToonShadeDifinition.ShaderPropAdd_Antipodean_RimLight, 1);
							}
						}
						else
						{
							if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
							{
								material.SetFloat(ToonShadeDifinition.ShaderPropAdd_Antipodean_RimLight, 0);
							}
						}
						EditorGUILayout.EndHorizontal();

						if (material.GetFloat(ToonShadeDifinition.ShaderPropAdd_Antipodean_RimLight) == 1)
						{
							EditorGUI.indentLevel++;
							GUILayout.Label("Ap_RimLight Settings", EditorStyles.boldLabel);
							m_MaterialEditor.ColorProperty(ap_RimLightColor, "Ap_RimLight Color");
							m_MaterialEditor.RangeProperty(ap_RimLight_Power, "Ap_RimLight Power");

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PrefixLabel("Ap_RimLight FeatherOff");
							if (material.GetFloat(ToonShadeDifinition.ShaderPropAp_RimLight_FeatherOff) == 0)
							{
								if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
								{
									material.SetFloat(ToonShadeDifinition.ShaderPropAp_RimLight_FeatherOff, 1);
								}
							}
							else
							{
								if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
								{
									material.SetFloat(ToonShadeDifinition.ShaderPropAp_RimLight_FeatherOff, 0);
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
			if (material.GetFloat(ToonShadeDifinition.ShaderPropMatCap) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropMatCap, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropMatCap, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.GetFloat(ToonShadeDifinition.ShaderPropMatCap) == 1)
			{
				//GUILayout.Label("MatCap Settings", EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.matCapSamplerText, matCap_Sampler, matCapColor);
				m_MaterialEditor.TextureScaleOffsetProperty(matCap_Sampler);

				if (!simpleUI)
				{

					m_MaterialEditor.RangeProperty(blurLevelMatcap, "Blur Level of MatCap Sampler");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Color Blend Mode");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_BlendAddToMatCap) == 0)
					{
						if (GUILayout.Button("Multipy", shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_BlendAddToMatCap, 1);
						}
					}
					else
					{
						if (GUILayout.Button("Additive", shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_BlendAddToMatCap, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					m_MaterialEditor.RangeProperty(tweak_MatCapUV, "Scale MatCapUV");
					m_MaterialEditor.RangeProperty(rotate_MatCapUV, "Rotate MatCapUV");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("CameraRolling Stabilizer");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropCameraRolling_Stabilizer) == 0)
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropCameraRolling_Stabilizer, 1);
						}
					}
					else
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropCameraRolling_Stabilizer, 0);
						}
					}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("NormalMap for MatCap");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_NormalMapForMatCap) == 0)
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_NormalMapForMatCap, 1);
						}
					}
					else
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_NormalMapForMatCap, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_NormalMapForMatCap) == 1)
					{
						EditorGUI.indentLevel++;
						m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, normalMapForMatCap, bumpScaleMatcap);
						m_MaterialEditor.TextureScaleOffsetProperty(normalMapForMatCap);
						m_MaterialEditor.RangeProperty(rotate_NormalMapForMatCapUV, "Rotate NormalMapUV");
						EditorGUI.indentLevel--;
					}

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("MatCap on Shadow");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_UseTweakMatCapOnShadow) == 0)
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_UseTweakMatCapOnShadow, 1);
						}
					}
					else
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_UseTweakMatCapOnShadow, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_UseTweakMatCapOnShadow) == 1)
					{
						EditorGUI.indentLevel++;
						m_MaterialEditor.RangeProperty(tweakMatCapOnShadow, "MatCap Power on Shadow");
						EditorGUI.indentLevel--;
					}

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("MatCap Projection Camera");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_Ortho) == 0)
					{
						if (GUILayout.Button("Perspective", middleButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_Ortho, 1);
						}
					}
					else
					{
						if (GUILayout.Button("Orthographic", middleButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_Ortho, 0);
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
				if (material.GetFloat(ToonShadeDifinition.ShaderPropInverse_MatcapMask) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropInverse_MatcapMask, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropInverse_MatcapMask, 0);
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
			if (material.GetFloat(ToonShadeDifinition.ShaderPropAngelRing) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropAngelRing, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropAngelRing, 0);
				}
			}
			EditorGUILayout.EndHorizontal();


			if (material.GetFloat(ToonShadeDifinition.ShaderPropAngelRing) == 1)
			{
				EditorGUILayout.Space();
				m_MaterialEditor.TexturePropertySingleLine(Styles.angelRingText, angelRing_Sampler, angelRing_Color);
				m_MaterialEditor.RangeProperty(ar_OffsetU, "Offset U");
				m_MaterialEditor.RangeProperty(ar_OffsetV, "Offset V");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Use α channel as Clipping");

				if (material.GetFloat(ToonShadeDifinition.ShaderPropARSampler_AlphaOn) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropARSampler_AlphaOn, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropARSampler_AlphaOn, 0);
					}
				}
				EditorGUILayout.EndHorizontal();
			}

		}

		private void GUI_Emissive(Material material)
		{
			GUILayout.Label("Bloom Post-Processing Effect Necessary", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			m_MaterialEditor.TexturePropertySingleLine(Styles.emissiveTexText, emissive_Tex, emissive_Color);
			m_MaterialEditor.TextureScaleOffsetProperty(emissive_Tex);

			int _EmissiveMode_Setting = material.GetInt("_EMISSIVE");
			if ((int)ToonShadeDifinition.EmissiveMode.SimpleEmissive == _EmissiveMode_Setting)
			{
				emissiveMode = ToonShadeDifinition.EmissiveMode.SimpleEmissive;
			}
			else if ((int)ToonShadeDifinition.EmissiveMode.EmissiveAnimation == _EmissiveMode_Setting)
			{
				emissiveMode = ToonShadeDifinition.EmissiveMode.EmissiveAnimation;
			}
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Emissive Animation");
			if (emissiveMode == ToonShadeDifinition.EmissiveMode.SimpleEmissive)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat("_EMISSIVE", 1);
					material.EnableKeyword("_EMISSIVE_ANIMATION");
					material.DisableKeyword("_EMISSIVE_SIMPLE");
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat("_EMISSIVE", 0);
					material.EnableKeyword("_EMISSIVE_SIMPLE");
					material.DisableKeyword("_EMISSIVE_ANIMATION");
				}
			}
			EditorGUILayout.EndHorizontal();

			if (emissiveMode == ToonShadeDifinition.EmissiveMode.EmissiveAnimation)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginHorizontal();
				m_MaterialEditor.FloatProperty(base_Speed, "Base Speed (Time)");
				if (!simpleUI)
				{
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_ViewCoord_Scroll) == 0)
					{
						if (GUILayout.Button("UV Coord Scroll", shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_ViewCoord_Scroll, 1);
						}
					}
					else
					{
						if (GUILayout.Button("View Coord Scroll", shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_ViewCoord_Scroll, 0);
						}
					}
				}
				EditorGUILayout.EndHorizontal();

				m_MaterialEditor.RangeProperty(scroll_EmissiveU, "Scroll U/X direction");
				m_MaterialEditor.RangeProperty(scroll_EmissiveV, "Scroll V/Y direction");
				m_MaterialEditor.FloatProperty(rotate_EmissiveUV, "Rotate around UV center");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("PingPong Move for Base");
				if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_PingPong_Base) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_PingPong_Base, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_PingPong_Base, 0);
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;

				if (!simpleUI)
				{
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("ColorShift with Time");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_ColorShift) == 0)
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_ColorShift, 1);
						}
					}
					else
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_ColorShift, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel++;
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_ColorShift) == 1)
					{
						m_MaterialEditor.ColorProperty(colorShift, "Destination Color");
						m_MaterialEditor.FloatProperty(colorShift_Speed, "ColorShift Speed (Time)");
					}
					EditorGUI.indentLevel--;

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("ViewShift of Color");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_ViewShift) == 0)
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_ViewShift, 1);
						}
					}
					else
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_ViewShift, 0);
						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel++;
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_ViewShift) == 1)
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
			var srpDefaultLightModeTag = material.GetTag("LightMode", false, ToonShadeDifinition.SRPDefaultLightModeName);
			bool isOutlineEnabled = true;
			if (srpDefaultLightModeTag == ToonShadeDifinition.SRPDefaultLightModeName)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Outline");
				if (isOutlineEnabled = material.GetShaderPassEnabled(ToonShadeDifinition.SRPDefaultLightModeName))
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetShaderPassEnabled(ToonShadeDifinition.SRPDefaultLightModeName, false);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetShaderPassEnabled(ToonShadeDifinition.SRPDefaultLightModeName, true);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			if (!isOutlineEnabled)
			{
				return;
			}

			int _OutlineMode_Setting = material.GetInt(ToonShadeDifinition.ShaderPropOutline);
			if ((int)ToonShadeDifinition.OutlineMode.NormalDirection == _OutlineMode_Setting)
			{
				outlineMode = ToonShadeDifinition.OutlineMode.NormalDirection;
			}
			else if ((int)ToonShadeDifinition.OutlineMode.PositionScaling == _OutlineMode_Setting)
			{
				outlineMode = ToonShadeDifinition.OutlineMode.PositionScaling;
			}

			outlineMode = (ToonShadeDifinition.OutlineMode)EditorGUILayout.EnumPopup("Outline Mode", outlineMode);
			if (outlineMode == ToonShadeDifinition.OutlineMode.NormalDirection)
			{
				material.SetFloat(ToonShadeDifinition.ShaderPropOutline, 0);
				material.EnableKeyword("_OUTLINE_NML");
				material.DisableKeyword("_OUTLINE_POS");
			}
			else if (outlineMode == ToonShadeDifinition.OutlineMode.PositionScaling)
			{
				material.SetFloat(ToonShadeDifinition.ShaderPropOutline, 1);
				material.EnableKeyword("_OUTLINE_POS");
				material.DisableKeyword("_OUTLINE_NML");
			}

			m_MaterialEditor.FloatProperty(outline_Width, "Outline Width");
			m_MaterialEditor.ColorProperty(outline_Color, "Outline Color");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Blend BaseColor");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_BlendBaseColor) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_BlendBaseColor, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_BlendBaseColor, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			m_MaterialEditor.TexturePropertySingleLine(Styles.outlineSamplerText, outline_Sampler);
			m_MaterialEditor.FloatProperty(offset_Z, "Camera Z-axis");

			EditorGUILayout.Space();
			if (!simpleUI)
			{
				_AdvancedOutline_Foldout = ToonShadeDifinition.FoldoutSubMenu(_AdvancedOutline_Foldout, "Advanced Outline Settings");
				if (_AdvancedOutline_Foldout)
				{
					EditorGUI.indentLevel++;
					GUILayout.Label("Camera Distance for Outline Width");
					m_MaterialEditor.FloatProperty(farthest_Distance, "Farthest Distance");
					m_MaterialEditor.FloatProperty(nearest_Distance, "Nearest Distance");
					EditorGUI.indentLevel--;

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Use Outline Texture");
					if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_OutlineTex) == 0)
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_OutlineTex, 1);
						}
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
						{
							material.SetFloat(ToonShadeDifinition.ShaderPropIs_OutlineTex, 0);
						}
						EditorGUILayout.EndHorizontal();
						m_MaterialEditor.TexturePropertySingleLine(Styles.outlineTexText, outlineTex);
					}

					if (outlineMode == ToonShadeDifinition.OutlineMode.NormalDirection)
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Use Baked Normal");
						if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_BakedNormal) == 0)
						{
							if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
							{
								material.SetFloat(ToonShadeDifinition.ShaderPropIs_BakedNormal, 1);
							}
							EditorGUILayout.EndHorizontal();
						}
						else
						{
							if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
							{
								material.SetFloat(ToonShadeDifinition.ShaderPropIs_BakedNormal, 0);
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

			if (material.GetFloat(ToonShadeDifinition.ShaderPropIsLightColor_Base) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIsLightColor_Base, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIsLightColor_Base, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("1st ShadeColor");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_1st_Shade) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_1st_Shade, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_1st_Shade, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("2nd ShadeColor");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_2nd_Shade) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_2nd_Shade, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_2nd_Shade, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("HighColor");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_HighColor) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_HighColor, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_HighColor, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("RimLight");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_RimLight) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_RimLight, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_RimLight, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Ap_RimLight");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_Ap_RimLight) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_Ap_RimLight, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_Ap_RimLight, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("MatCap");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_MatCap) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_MatCap, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_MatCap, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Angel Ring");
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_AR) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_AR, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_AR, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			if (material.HasProperty(ToonShadeDifinition.ShaderPropOutline))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Outline");
				if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_Outline) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_Outline, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_Outline, 0);
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
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_Filter_LightColor) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_Filter_LightColor, 1);
					material.SetFloat(ToonShadeDifinition.ShaderPropIsLightColor_Base, 1);
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_1st_Shade, 1);
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_2nd_Shade, 1);
					if (material.HasProperty(ToonShadeDifinition.ShaderPropOutline))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropIs_LightColor_Outline, 1);
					}
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_Filter_LightColor, 0);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Built-in Light Direction");
			//GUILayout.Space(60);
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_BLD) == 0)
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_BLD, 1);
				}
			}
			else
			{
				if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
				{
					material.SetFloat(ToonShadeDifinition.ShaderPropIs_BLD, 0);
				}
			}
			EditorGUILayout.EndHorizontal();
			if (material.GetFloat(ToonShadeDifinition.ShaderPropIs_BLD) == 1)
			{
				GUILayout.Label("Built-in Light Direction Settings");
				EditorGUI.indentLevel++;
				m_MaterialEditor.RangeProperty(offset_X_Axis_BLD, "Offset X-Axis Direction");
				m_MaterialEditor.RangeProperty(offset_Y_Axis_BLD, "Offset Y-Axis Direction");

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Inverse Z-Axis Direction");
				if (material.GetFloat(ToonShadeDifinition.ShaderPropInverse_Z_Axis_BLD) == 0)
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_OFFSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropInverse_Z_Axis_BLD, 1);
					}
				}
				else
				{
					if (GUILayout.Button(ToonShadeDifinition.STR_ONSTATE, shortButtonStyle))
					{
						material.SetFloat(ToonShadeDifinition.ShaderPropInverse_Z_Axis_BLD, 0);
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
		}

		private void ApplyQueueAndRenderType(Material material)
		{
			var stencilMode = (ToonShadeDifinition.ToonStencilMode)material.GetInt(ToonShadeDifinition.ShaderPropStencilMode);
			if (autoRenderQueue == 1)
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

			if (TransparentSetting == ToonShadeDifinition.Transparent.On)
			{
				renderType = TRANSPARENT;
				ignoreProjection = DO_IGNOREPROJECTION;
			}
			else
			{
				ToonShadeDifinition.TransClippingMode transClippingMode = (ToonShadeDifinition.TransClippingMode)material.GetInt(ToonShadeDifinition.ShaderPropClippingMode);
				if (transClippingMode == ToonShadeDifinition.TransClippingMode.Off)
				{
				}
				else
				{
					renderType = TRANSPARENTCUTOUT;

				}
			}

			if (autoRenderQueue == 1)
			{
				if (TransparentSetting == ToonShadeDifinition.Transparent.On)
				{
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
				}
				else if (stencilMode == ToonShadeDifinition.ToonStencilMode.StencilMask)
				{
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest - 1;
				}
				else if (stencilMode == ToonShadeDifinition.ToonStencilMode.StencilOut)
				{
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
				}
			}
			else
			{
				material.renderQueue = renderQueue;
			}

			material.SetOverrideTag(RENDERTYPE, renderType);
			material.SetOverrideTag(IGNOREPROJECTION, ignoreProjection);
		}

		private void ApplyMatCapMode(Material material)
		{
			if (material.GetInt(ToonShadeDifinition.ShaderPropClippingMode) == 0)
			{
				if (material.GetFloat(ToonShadeDifinition.ShaderPropMatCap) == 1)
				{
					material.EnableKeyword(ToonShadeDifinition.ShaderPropMatCap);
				}
				else
				{
					material.DisableKeyword(ToonShadeDifinition.ShaderPropMatCap);
				}
			}
			else
			{
				material.DisableKeyword(ToonShadeDifinition.ShaderPropMatCap);
			}
		}

		private void ApplyAngelRing(Material material)
		{
			int angelRingEnabled = material.GetInt(ToonShadeDifinition.ShaderPropAngelRing);
			if (angelRingEnabled == 0)
			{
				material.DisableKeyword(ToonShadeDifinition.ShaderDefineANGELRING_ON);
				material.EnableKeyword(ToonShadeDifinition.ShaderDefineANGELRING_OFF);
			}
			else
			{
				material.EnableKeyword(ToonShadeDifinition.ShaderDefineANGELRING_ON);
				material.DisableKeyword(ToonShadeDifinition.ShaderDefineANGELRING_OFF);
			}
		}

		private void ApplyStencilMode(Material material)
		{
			ToonShadeDifinition.ToonStencilMode mode = (ToonShadeDifinition.ToonStencilMode)(material.GetInt(ToonShadeDifinition.ShaderPropStencilMode));
			switch (mode)
			{
				case ToonShadeDifinition.ToonStencilMode.Off:
					material.SetInt(ToonShadeDifinition.ShaderPropStencilComp, (int)ToonShadeDifinition.StencilCompFunction.Disabled);
					material.SetInt(ToonShadeDifinition.ShaderPropStencilOpPass, (int)ToonShadeDifinition.StencilType.Keep);
					material.SetInt(ToonShadeDifinition.ShaderPropStencilOpFail, (int)ToonShadeDifinition.StencilType.Keep);
					break;
				case ToonShadeDifinition.ToonStencilMode.StencilMask:
					material.SetInt(ToonShadeDifinition.ShaderPropStencilComp, (int)ToonShadeDifinition.StencilCompFunction.Always);
					material.SetInt(ToonShadeDifinition.ShaderPropStencilOpPass, (int)ToonShadeDifinition.StencilType.Replace);
					material.SetInt(ToonShadeDifinition.ShaderPropStencilOpFail, (int)ToonShadeDifinition.StencilType.Replace);
					break;
				case ToonShadeDifinition.ToonStencilMode.StencilOut:
					material.SetInt(ToonShadeDifinition.ShaderPropStencilComp, (int)ToonShadeDifinition.StencilCompFunction.NotEqual);
					material.SetInt(ToonShadeDifinition.ShaderPropStencilOpPass, (int)ToonShadeDifinition.StencilType.Keep);
					material.SetInt(ToonShadeDifinition.ShaderPropStencilOpFail, (int)ToonShadeDifinition.StencilType.Keep);
					break;
			}
		}

		private void ApplyClippingMode(Material material)
		{
			material.DisableKeyword(ToonShadeDifinition.ShaderDefineIS_CLIPPING_OFF);
			material.DisableKeyword(ToonShadeDifinition.ShaderDefineIS_CLIPPING_MODE);
			material.DisableKeyword(ToonShadeDifinition.ShaderDefineIS_CLIPPING_TRANSMODE);
			switch (material.GetInt(ToonShadeDifinition.ShaderPropClippingMode))
			{
				case 0:
					material.EnableKeyword(ToonShadeDifinition.ShaderDefineIS_TRANSCLIPPING_OFF);
					material.DisableKeyword(ToonShadeDifinition.ShaderDefineIS_TRANSCLIPPING_ON);
					break;
				default:
					material.DisableKeyword(ToonShadeDifinition.ShaderDefineIS_TRANSCLIPPING_OFF);
					material.EnableKeyword(ToonShadeDifinition.ShaderDefineIS_TRANSCLIPPING_ON);
					break;

			}
		}	

		private void SetupOverDrawTransparentObject(Material material)
		{
			var srpDefaultLightModeTag = material.GetTag("LightMode", false, ToonShadeDifinition.SRPDefaultLightModeName);
			if (srpDefaultLightModeTag == ToonShadeDifinition.SRPDefaultLightModeName)
			{
				material.SetShaderPassEnabled(ToonShadeDifinition.SRPDefaultLightModeName, true);
				material.SetInt(ToonShadeDifinition.SRPDefaultColorMask, 0);
				material.SetInt(ToonShadeDifinition.SRPDefaultCullMode, (int)ToonShadeDifinition.CullingMode.BackCulling);
			}
		}

		private void SetuOutline(Material material)
		{
			var srpDefaultLightModeTag = material.GetTag("LightMode", false, ToonShadeDifinition.SRPDefaultLightModeName);
			if (srpDefaultLightModeTag == ToonShadeDifinition.SRPDefaultLightModeName)
			{
				material.SetInt(ToonShadeDifinition.SRPDefaultColorMask, 15);
				material.SetInt(ToonShadeDifinition.SRPDefaultCullMode, (int)ToonShadeDifinition.CullingMode.FrontCulling);
			}
		}

		private void DoPopup(GUIContent label, MaterialProperty property, string[] options)
		{
			ToonShadeDifinition.DoPopup(label, property, options, m_MaterialEditor);
		}
	}

}