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
		private readonly Vector3 _scaleBy = new Vector3(.333f, .333f, 0f);
		private const float Time = .333f;
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
			UnityEngine.Time.timeScale = (_slow = !_slow) ? .1f : 1f;
			Ease3.GoColorTo(_statusSlow, (_slow ? _colorOn : _colorOff).GetVector(), Time, i => { _statusSlow.color = i.GetColor(); }, null, EaseType.Linear, 0f, 1, false, true);
			Ease3.GoScaleBy(_statusSlow, _scaleBy, Time, null, null, EaseType.BackInOut, 0f, 1, true, true);
		}
		public void Pause()
		{
			UnityEngine.Time.timeScale = (_pause = !_pause) ? 0f : _slow ? .1f : _fast ? 2f : 1f;
			Ease3.GoColorTo(_statusPause, (_pause ? _colorOn : _colorOff).GetVector(), Time, i => { _statusPause.color = i.GetColor(); }, null, EaseType.Linear, 0f, 1, false, true);
			Ease3.GoScaleBy(_statusPause, _scaleBy, Time, null, null, EaseType.BackInOut, 0f, 1, true, true);
		}
		public void Fast()
		{
			_slow = false;
			_statusSlow.color = _colorOff;
			_pause = false;
			_statusPause.color = _colorOff;
			UnityEngine.Time.timeScale = (_fast = !_fast) ? 2f : 1f;
			Ease3.GoColorTo(_statusFast, (_fast ? _colorOn : _colorOff).GetVector(), Time, i => { _statusFast.color = i.GetColor(); }, null, EaseType.Linear, 0f, 1, false, true);
			Ease3.GoScaleBy(_statusFast, _scaleBy, Time, null, null, EaseType.BackInOut, 0f, 1, true, true);
		}
	}
}
