using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class SlowTime : MonoBehaviour
	{
		private bool _slow;
		public void Go()
		{
			Time.timeScale = (_slow = !_slow) ? .1f : 1f;
		}
	}
}
