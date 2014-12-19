using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Item : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
	{
		private Transform _t;
		private Text[] _children;
		private void Awake()
		{
			_t = transform;
			_children = GetComponentsInChildren<Text>();
		}
		private void OnDisable()
		{
			if (_t != null)
			{
				_t.localPosition = Vector3.zero;
				_t.localRotation = Quaternion.identity;
			}
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
			var to = Mathf.Approximately(_t.localPosition.x, 0f) ?
				new Vector3(e.position.y > (Screen.height * .5f) ? 360f : -360f, 0f, 0f) :
				new Vector3(0f, _t.localPosition.x > 0 ? 360f : -360f, 0f);
			Ease3.GoRotation(this, Vector3.zero, to, 1f, null, null, EaseType.Spring);
			Flow.Instance.EaseTo(_t);
		}
	}
}
