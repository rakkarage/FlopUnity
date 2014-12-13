using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Intro : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] _activate = null;
		private Image _logo;
		private Image _foreground;
		private AudioSource _source;
		private const float _timeAnimation = 1f;
		private const float _timeDelay = .5f;
		private const float _timeDelaySound = .8f;
		private void Awake()
		{
			if (_activate != null)
				foreach (var i in _activate)
					i.SetActive(false);
		}
		private void Start()
		{
			_logo = transform.FindChild("Logo").GetComponent<Image>();
			_foreground = transform.FindChild("Fore").GetComponent<Image>();
			_source = GetComponent<AudioSource>();
			StartCoroutine(PlayDelayed(_timeDelaySound));
			Ease3.GoScale(this, _logo.gameObject, _logo.transform.localScale, new Vector3(2f, 2f, 1f), _timeAnimation, _timeDelay, EaseType.BounceOut);
			Ease3.GoRotation(this, _logo.gameObject, new Vector3(0f, 0f, 180f), _timeAnimation, _timeDelay, EaseType.BounceOut);
			Ease3.Go(this, Constants.HenryBlue.GetVector(), Color.black.GetVector(), _timeAnimation, _timeDelay, EaseType.BounceOut, HandleColor, HandleFade);
		}
		private IEnumerator PlayDelayed(float time)
		{
			yield return new WaitForSeconds(time);
			_source.Play();
		}
		private void HandleColor(Vector3 vector)
		{
			Camera.main.backgroundColor = vector.GetColor();
		}
		private void HandleFade()
		{
			Ease.Go(this, 0f, 1f, _timeAnimation, _timeAnimation, EaseType.Linear, HandleFade, HandleNext);
		}
		private void HandleFade(float value)
		{
			_foreground.color = new Color(_foreground.color.r, _foreground.color.g, _foreground.color.b, value);
		}
		public void HandleNext()
		{
			StopAllCoroutines();
			_logo.gameObject.SetActive(false);
			Ease.Go(this, _foreground.color.a, 0f, _timeAnimation, 0f, EaseType.Linear, HandleFade, HandleEnd);
			Camera.main.backgroundColor = Constants.HenryBlue;
			if (_activate != null)
				foreach (var i in _activate)
					i.SetActive(true);
		}
		public void HandleEnd()
		{
			gameObject.SetActive(false);
		}
	}
}
