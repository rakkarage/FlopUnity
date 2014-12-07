using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	[RequireComponent(typeof(CanvasGroup))]
	public class Fade : MonoBehaviour
	{
		public bool In = true;
		public float Time = 1f;
		private CanvasGroup _group;
		private void Start()
		{
			float start = In ? 0f : 1f;
			float end = In ? 1f : 0f;
			_group = GetComponent<CanvasGroup>();
			_group.alpha = start;
			Ease.Go(this, start, end, Time, 0f, EaseType.Sinerp, HandleFade, null);
		}
		private void HandleFade(float alpha)
		{
			_group.alpha = alpha;
		}
	}
}
