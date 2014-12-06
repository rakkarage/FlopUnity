using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Intro : MonoBehaviour
	{
		public GameObject[] Activate;
		private GameObject _logo;
		private GameObject _foreground;
		private Image _fade;
		private AudioSource _source;
		private const float TimeAnimation = 1f;
		private const float TimeDelay = .5f;
		private const float TimeDelaySound = .8f;
		private void Awake()
		{
			foreach (var i in Activate)
				i.SetActive(false);
		}
		private void Start()
		{
			_logo = transform.FindChild("Logo").gameObject;
			_foreground = transform.FindChild("Fore").gameObject;
			_fade = _foreground.GetComponent<Image>();
			_source = GetComponent<AudioSource>();
			_source.PlayDelayed(TimeDelaySound);
			Ease3.GoScale(this, _logo, _logo.transform.localScale, new Vector3(2f, 2f, 1f), TimeAnimation, TimeDelay, EaseType.BounceOut);
			Ease3.GoRotation(this, _logo, new Vector3(0f, 0f, 180f), TimeAnimation, TimeDelay, EaseType.BounceOut);
			Ease3.Go(this, Constants.HenryBlue.GetVector(), Color.black.GetVector(), TimeAnimation, TimeDelay, EaseType.BounceOut, HandleColor, HandleFade);
		}
		private void HandleColor(Vector3 vector)
		{
			Camera.main.backgroundColor = vector.GetColor();
		}
		private void HandleFade()
		{
			Ease.Go(this, 0f, 1f, TimeAnimation, TimeAnimation, EaseType.Sinerp, HandleFade, HandleNext);
		}
		private void HandleFade(float value)
		{
			_fade.color = new Color(_fade.color.r, _fade.color.g, _fade.color.b, value);
		}
		public void HandleNext()
		{
			_source.Stop();
			StopAllCoroutines();
			_logo.SetActive(false);
			Ease.Go(this, _fade.color.a, 0f, TimeAnimation, 0f, EaseType.Sinerp, HandleFade, HandleEnd);
			Camera.main.backgroundColor = Constants.HenryBlue;
			foreach (var i in Activate)
				i.SetActive(true);
		}
		public void HandleEnd()
		{
			gameObject.SetActive(false);
		}
	}
}
