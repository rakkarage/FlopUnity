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
			Audio.Instance.PlayClick();
			StopAllCoroutines();
			Utility.RandomColor(_children);
			var direction = Mathf.Approximately(_t.localPosition.x, 0f) ?
				new Vector3(e.position.y > (Screen.height * .5f) ? 360f : -360f, 0f, 0f) :
				new Vector3(0f, _t.localPosition.x > 0 ? 360f : -360f, 0f);
			Ease3.GoRotation(this, direction, 1f, EaseType.Spring);
			Flow.Instance.EaseTo(_t);
		}
	}
}
