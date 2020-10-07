using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPA
{
	[CreateAssetMenu(menuName = "FPA/Param")]
	public class SceneRendererSettings : ScriptableObject
	{
		/// <summary>
		/// 鮮やかな赤の閾値
		/// </summary>
		public float alertBrightness = 85f;

		/// <summary>
		/// 点滅最大回数
		/// </summary>
		public int brightnessCount = 5;

		/// <summary>
		/// 点滅判定時間
		/// </summary>
		public float brightnessInterval = 1f;
	}

}

