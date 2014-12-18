using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Font : MonoBehaviour, IPointerClickHandler
	{
		private Text[] _children;
		private void Start()
		{
			_children = GetComponentsInChildren<Text>();
		}
		public void OnPointerClick(PointerEventData e)
		{
			Utility.RandomColor(_children);
		}
	}
}
