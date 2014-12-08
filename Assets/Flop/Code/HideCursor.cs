using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class HideCursor : MonoBehaviour
	{
		void Start()
		{
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
			Screen.showCursor = false;
#endif
		}
	}
}
