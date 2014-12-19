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
		private readonly Color _colorOff = Color.white.SetAlpha(.5f);
		private readonly Color _colorOn = Constants.ButtonBlue.SetAlpha(.5f);
		private readonly Vector3 _scaleTo = new Vector3(1.333f, 1.333f, 1f);
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
			_statusSlow.StopAllCoroutines();
			Ease3.GoColorTo(_statusSlow, (_slow ? _colorOn : _colorOff).GetVector(), Time, null, null, EaseType.SineInOut, 0f, 1, false, true);
			Ease3.GoScaleTo(_statusSlow, _scaleTo, Time, null, () =>
			{
				Ease3.GoScaleTo(_statusSlow, Vector3.one, Time, null, null, EaseType.BackInOut, 0f, 1, false, true);
			}, EaseType.BackInOut, 0f, 1, false, true);
		}
		public void Pause()
		{
			UnityEngine.Time.timeScale = (_pause = !_pause) ? 0f : _slow ? .1f : _fast ? 2f : 1f;
			_statusPause.StopAllCoroutines();
			Ease3.GoColorTo(_statusPause, (_pause ? _colorOn : _colorOff).GetVector(), Time, null, null, EaseType.SineInOut, 0f, 1, false, true);
			Ease3.GoScaleTo(_statusPause, _scaleTo, Time, null, () =>
			{
				Ease3.GoScaleTo(_statusPause, Vector3.one, Time, null, null, EaseType.BackInOut, 0f, 1, false, true);
			}, EaseType.BackInOut, 0f, 1, false, true);
		}
		public void Fast()
		{
			_slow = false;
			_statusSlow.color = _colorOff;
			_pause = false;
			_statusPause.color = _colorOff;
			UnityEngine.Time.timeScale = (_fast = !_fast) ? 2f : 1f;
			_statusFast.StopAllCoroutines();
			Ease3.GoColorTo(_statusFast, (_fast ? _colorOn : _colorOff).GetVector(), Time, null, null, EaseType.SineInOut, 0f, 1, false, true);
			Ease3.GoScaleTo(_statusFast, _scaleTo, Time, null, () =>
			{
				Ease3.GoScaleTo(_statusFast, Vector3.one, Time, null, null, EaseType.BackInOut, 0f, 1, false, true);
			}, EaseType.BackInOut, 0f, 1, false, true);
		}
	}
}
