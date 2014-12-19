using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Pause : MonoBehaviour
	{
		[SerializeField]
		private Image _statusSlow;
		private bool _slow;
		[SerializeField]
		private Image _statusStop;
		private bool _stop;
		[SerializeField]
		private Image _statusFast;
		private bool _fast;
		private readonly Color _colorOff = Color.white;
		private readonly Color _colorOn = Constants.ButtonBlue;
		private void Start()
		{
			_statusSlow.color = _colorOff;
			_statusStop.color = _colorOff;
			_statusFast.color = _colorOff;
		}
		public void Slow()
		{
			_stop = false;
			_statusStop.color = _colorOff;
			_fast = false;
			_statusFast.color = _colorOff;
			Time.timeScale = (_slow = !_slow) ? .1f : 1f;
			Ease3.GoColorTo(_statusSlow, (_slow ? _colorOn : _colorOff).GetVector(), 1f, i => { _statusSlow.color = i.GetColor(); }, null, EaseType.ExpoOut);
		}
		public void Stop()
		{
			Time.timeScale = (_stop = !_stop) ? 0f : _slow ? .1f : _fast ? 2f : 1f;
			_statusStop.color = _stop ? _colorOn : _colorOff;
		}
		public void Fast()
		{
			_slow = false;
			_statusSlow.color = _colorOff;
			_stop = false;
			_statusStop.color = _colorOff;
			Time.timeScale = (_fast = !_fast) ? 2f : 1f;
			Ease3.GoColorTo(_statusFast, (_fast ? _colorOn : _colorOff).GetVector(), 1f, i => { _statusFast.color = i.GetColor(); }, null, EaseType.ExpoOut);
		}
	}
}
