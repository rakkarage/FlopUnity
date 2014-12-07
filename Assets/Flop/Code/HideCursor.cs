using UnityEngine;
using UnityEngine.EventSystems;
public class HideCursor : MonoBehaviour
{
	void Start()
	{
#if !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_EDITOR
		Screen.showCursor = false;
#endif
	}
}
