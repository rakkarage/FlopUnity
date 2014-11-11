using UnityEngine;
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
		Ease.Go(GetComponent<Fade>(), start, end, Time, HandleFade, null, Ease.Type.Sinerp);
	}
	private void HandleFade(float alpha, MonoBehaviour sender)
	{
		_group.alpha = alpha;
	}
}
