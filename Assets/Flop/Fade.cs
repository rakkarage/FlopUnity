using UnityEngine;
namespace ca.HenrySoftware.Rage
{
	[RequireComponent(typeof(CanvasGroup))]
	public class Fade : MonoBehaviour
	{
		[SerializeField] private bool _in = true;
		[SerializeField] private float _time = 3.33f;
		private CanvasGroup _group;
		private void Start()
		{
			var start = _in ? 0f : 1f;
			var end = _in ? 1f : 0f;
			_group = GetComponent<CanvasGroup>();
			_group.alpha = start;
			Ease.Go(this, start, end, _time, HandleFade);
		}
		private void HandleFade(float alpha)
		{
			_group.alpha = alpha;
		}
		public void FadeTo(float alpha, float time)
		{
			StopAllCoroutines();
			Ease.Go(this, _group.alpha, alpha, time, HandleFade, null, EaseType.CircOut);
		}
	}
}
