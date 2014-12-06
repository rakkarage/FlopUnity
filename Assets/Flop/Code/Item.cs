using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
			Debug.Log(e.button);
			if (e.button == PointerEventData.InputButton.Right)
			{
				UpdateColor();
				Ease3.GoRotation(this, gameObject, gameObject.transform.localRotation.eulerAngles, new Vector3(0f, 360f, 0f), 1f, 0f, EaseType.Spring);
			}
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
