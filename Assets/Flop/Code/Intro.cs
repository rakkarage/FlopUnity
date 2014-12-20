using System.Collections;
using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Intro : MonoBehaviour
	{
		[SerializeField] private MonoBehaviour _logo;
		[SerializeField] private MonoBehaviour _foreground;
		[SerializeField] private GameObject[] _activate = null;
		private AudioSource _source;
		private const float TimeAnimation = 1f;
		private const float TimeDelay = .5f;
		private const float TimeDelaySound = .8f;
		private void Awake()
		{
			if (_activate == null) return;
			foreach (var i in _activate)
				i.SetActive(false);
		}
		private void Start()
		{
			_source = GetComponent<AudioSource>();
			StartCoroutine(PlayDelayed(TimeDelaySound));
			Ease3.GoScaleTo(_logo, new Vector3(2f, 2f, 1f), TimeAnimation, null, null, EaseType.BounceOut, TimeDelay);
			Ease3.GoRotationTo(_logo, new Vector3(0f, 0f, 180f), TimeAnimation, null, null, EaseType.BounceOut, TimeDelay);
			Ease3.GoColorTo(this, Color.black.GetVector(), TimeAnimation, null, Fade, EaseType.BounceOut, TimeDelay);
		}
		private IEnumerator PlayDelayed(float time)
		{
			yield return new WaitForSeconds(time);
			_source.Play();
		}
		private void Fade()
		{
			Ease.GoAlpha(_foreground, 0f, 1f, TimeAnimation, null, Next, EaseType.Linear, TimeAnimation);
		}
		public void Next()
		{
			StopAllCoroutines();
			_logo.gameObject.SetActive(false);
			Ease.GoAlphaTo(_foreground, 0f, TimeAnimation, null, Finish);
			Camera.main.backgroundColor = Constants.HenryBlue;
			if (_activate == null) return;
			foreach (var i in _activate)
				i.SetActive(true);
		}
		private void Finish()
		{
			gameObject.SetActive(false);
		}
	}
}
