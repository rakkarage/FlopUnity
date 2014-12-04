using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Application : MonoBehaviour
	{
		void OnApplicationQuit()
		{
			Debug.Log("quit");
		}
		void OnApplicationFocus(bool focusStatus)
		{
			Debug.Log("focus");
		}
		void OnApplicationPause(bool pauseStatus)
		{
			Debug.Log("pause");
		}
	}
}
