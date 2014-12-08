using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class HideCursor : MonoBehaviour
	{
		void Start()
		{
#if UNITY_MOBILE
			Screen.showCursor = false;
#endif
		}
	}
}
