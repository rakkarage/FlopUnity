using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class HideCursor : MonoBehaviour
	{
		void Start()
		{
			if (Utility.TouchPlatform)
				Screen.showCursor = false;
		}
	}
}
