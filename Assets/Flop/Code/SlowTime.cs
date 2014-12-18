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
			StopAllCoroutines();
			Ease3.Go(this, _status.color.GetVector(), (_slow ? Constants.StatusRed : Constants.StatusGreen).GetVector(), 1f, (i) => { _status.color = i.GetColor(); }, null, EaseType.ExpoOut);
		}
	}
}
