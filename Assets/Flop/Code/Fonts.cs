using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace ca.HenrySoftware.Flop
{
	public class Fonts : MonoBehaviour, IPointerClickHandler
	{
		private Text[] _children;
		private void Start()
		{
			_children = GetComponentsInChildren<Text>();
		}
		public void OnPointerClick(PointerEventData e)
		{
			UpdateColor();
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
	}
}
