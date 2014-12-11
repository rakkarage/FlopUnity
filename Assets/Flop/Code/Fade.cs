using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	[RequireComponent(typeof(CanvasGroup))]
	public class Fade : MonoBehaviour
	{
		public bool In = true;
		public float Time = 3f;
		private CanvasGroup _group;
		private void Start()
		{
			var start = In ? 0f : 1f;
			var end = In ? 1f : 0f;
			_group = GetComponent<CanvasGroup>();
			_group.alpha = start;
			Ease.Go(this, start, end, Time, 0f, EaseType.SineInOut, HandleFade, null);
		}
		private void HandleFade(float alpha)
		{
			_group.alpha = alpha;
		}
		public void FadeTo(float alpha, float time)
		{
			StopAllCoroutines();
			Ease.Go(this, _group.alpha, alpha, time, 0f, EaseType.SineInOut, HandleFade, null);
		}
	}
}
