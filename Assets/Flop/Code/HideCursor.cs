using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class HideCursor : MonoBehaviour
	{
		void Start()
		{
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
		    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			Screen.showCursor = false;
			Screen.lockCursor = true;
#endif
		}
	}
}
