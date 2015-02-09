﻿using UnityEngine;
namespace ca.HenrySoftware
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType(typeof(T)) as T;
					if (_instance == null)
					{
						Debug.LogError("Missing: " + typeof(T));
					}
				}
				return _instance;
			}
		}
	}
}
