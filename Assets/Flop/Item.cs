using UnityEngine;
using UnityEngine.EventSystems;
namespace ca.HenrySoftware.Flop
{
	public class Item : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
	{
		public Flow Flow;
		private Transform _t;
		private void Start()
		{
			_t = transform;
		}
		public void OnPointerDown(PointerEventData e)
		{
			Flow.Stop();
		}
		public void OnPointerClick(PointerEventData e)
		{
			e.Use();
			Audio.Instance.PlayClick();
			Flow.TweenTo(_t);
		}
	}
}
