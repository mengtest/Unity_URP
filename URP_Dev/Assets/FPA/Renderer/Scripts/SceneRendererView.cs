using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPA
{
	public class SceneRendererView : PersistentSingletonMonoBehaviour<SceneRendererView>
	{
		private SceneRenderer sceneRenderer = default;



		public void SetTarget(SceneRenderer newSceneRenderer)
		{
			sceneRenderer = newSceneRenderer;
		}
	}

}
