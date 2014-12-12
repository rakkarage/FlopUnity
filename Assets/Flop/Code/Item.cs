using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Item : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
	{
		private Transform _t;
		private Text[] _children;
		private void Start()
		{
			_t = transform;
			_children = GetComponentsInChildren<Text>();
		}
		private void OnEnable()
		{
			Utility.ResetColor(_children);
		}
		public void OnPointerDown(PointerEventData e)
		{
			Flow.Instance.Stop();
		}
		public void OnPointerClick(PointerEventData e)
		{
			StopAllCoroutines();
			Utility.RandomColor(_children);
			var direction = _t.localPosition.x == 0 ? new Vector3(360f, 0f, 0f) : new Vector3(0f, _t.localPosition.x > 0 ? 360f : -360f, 0f);
			Ease3.GoRotation(this, direction, 1f, 0f, EaseType.Spring);
			Audio.Instance.PlayClick();
			Flow.Instance.EaseTo(_t);
		}
	}
}
