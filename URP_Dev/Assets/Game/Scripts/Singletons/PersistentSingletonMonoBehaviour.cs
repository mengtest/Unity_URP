using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Singletons
{
	public abstract class PersistentSingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		private const string FIXED_PREFAB_PATH = "Prefabs/Singletons/";

		private static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					string[] parts = typeof(T).ToString().Split('.');
					// In case of namespaces
					string name = parts[parts.Length - 1];

					// Remove duplicates in scene if they exist
					T[] duplicates = FindObjectsOfType<T>();
					if (duplicates.Length > 0)
					{
						Debug.LogError(string.Format("You cannot have {0} preloaded in the scene!", name));
						for (int i = 0, n = duplicates.Length; i < n; i++)
						{
							Destroy(duplicates[i].gameObject);
						}
					}

					GameObject obj;

					// Check default prefab location
					string path = FIXED_PREFAB_PATH + name;
					GameObject prefab = Resources.Load<GameObject>(path);
					if (prefab)
					{
						Debug.Log(string.Format("Loading prefab for {0}", name));
						obj = Instantiate(prefab);
						instance = obj.GetComponent<T>();
						obj.name = name;

					}
					else
					{
						// Create new game object in current scene
						Debug.Log(string.Format("Creating new GameObject for {0}", name));
						obj = new GameObject(name);
						instance = obj.AddComponent<T>();
					}

					DontDestroyOnLoad(obj);
				}
				return instance;
			}
		}

		public void Initialize()
		{
			// HACK: Empty method to secure instance
		}

		public virtual void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}
	}
}
