using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class SlowTime : MonoBehaviour
	{
		[SerializeField]
		private Image _status;
		private bool _slow;
		private void Start()
		{
			_status.color = Constants.StatusGreen;
		}
		public void Go()
		{
			Time.timeScale = (_slow = !_slow) ? .1f : 1f;
			_status.color = _slow ? Constants.StatusRed : Constants.StatusGreen;
		}
	}
}
