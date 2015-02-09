using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class ItemEffect : Item, IPointerClickHandler, IPointerDownHandler
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
			ResetColor(_children);
		}
		public void OnPointerClick(PointerEventData e)
		{
			StopAllCoroutines();
			RandomColor(_children);
			var to = Mathf.Approximately(_t.localPosition.x, 0f) ?
				new Vector3(e.position.y > (Screen.height * .5f) ? 360f : -360f, 0f, 0f) :
				new Vector3(0f, _t.localPosition.x > 0 ? 360f : -360f, 0f);
			Ease3.GoRotation(this, Vector3.zero, to, 1f, null, null, EaseType.Spring);
		}
		public static Color RandomColor()
		{
			return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		}
		public static void RandomColor(IEnumerable<Text> texts)
		{
			if (texts == null) return;
			var c = RandomColor();
			foreach (var text in texts)
			{
				text.color = text.color == Color.white ? c : Color.white;
			}
		}
		public static void ResetColor(IEnumerable<Text> texts)
		{
			if (texts == null) return;
			foreach (var text in texts)
				text.color = Color.white;
		}
	}
}
