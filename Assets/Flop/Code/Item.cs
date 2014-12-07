using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Item : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
	{
		public Flow Flow;
		private Transform _t;
		private Text[] _children;
		private void Start()
		{
			_t = transform;
			_children = GetComponentsInChildren<Text>();
		}
		public void OnPointerDown(PointerEventData e)
		{
			Flow.Stop();
		}
		public void OnPointerClick(PointerEventData e)
		{
			e.Use();
			UpdateColor();
			var direction = _t.localPosition.x > 0 ? 360f : -360f;
			Ease3.GoRotation(this, gameObject, new Vector3(0f, direction, 0f), 1f, 0f, EaseType.Spring);
			Audio.Instance.PlayClick();
			Flow.TweenTo(_t);
		}
		public void UpdateColor()
		{
			Color c = Utility.RandomColor();
			foreach (var text in _children)
			{
				if (text.color == Color.white)
				{
					text.color = c;
				}
				else
				{
					text.color = Color.white;
				}
			}
		}
		public void ResetColor()
		{
			if (_children != null)
			{
				foreach (var text in _children)
					text.color = Color.white;
			}
		}
	}
}
