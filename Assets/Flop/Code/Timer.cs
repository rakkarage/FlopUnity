using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Timer : MonoBehaviour
	{
		[SerializeField]
		private Image _statusSlow;
		private bool _slow;
		[SerializeField]
		private Image _statusPause;
		private bool _pause;
		[SerializeField]
		private Image _statusFast;
		private bool _fast;
		private readonly Color _colorOff = Color.white;
		private readonly Color _colorOn = Constants.ButtonBlue;
		private void Start()
		{
			_statusSlow.color = _colorOff;
			_statusPause.color = _colorOff;
			_statusFast.color = _colorOff;
		}
		public void Slow()
		{
			_pause = false;
			_statusPause.color = _colorOff;
			_fast = false;
			_statusFast.color = _colorOff;
			Time.timeScale = (_slow = !_slow) ? .1f : 1f;
			Ease3.GoColorTo(_statusSlow, (_slow ? _colorOn : _colorOff).GetVector(), .333f, i => { _statusSlow.color = i.GetColor(); }, null, EaseType.Linear, 0f, 1, false, true);
		}
		public void Pause()
		{
			Time.timeScale = (_pause = !_pause) ? 0f : _slow ? .1f : _fast ? 2f : 1f;
			_statusPause.color = _pause ? _colorOn : _colorOff;
		}
		public void Fast()
		{
			_slow = false;
			_statusSlow.color = _colorOff;
			_pause = false;
			_statusPause.color = _colorOff;
			Time.timeScale = (_fast = !_fast) ? 2f : 1f;
			Ease3.GoColorTo(_statusFast, (_fast ? _colorOn : _colorOff).GetVector(), .333f, i => { _statusFast.color = i.GetColor(); }, null, EaseType.Linear, 0f, 1, false, true);
		}
	}
}
