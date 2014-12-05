using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace ca.HenrySoftware.Flop
{
	[RequireComponent(typeof(Selectable))]
	public class SoundOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		private Selectable _selectable;
		private void Awake()
		{
			_selectable = GetComponent<Selectable>();
		}
		public void OnPointerEnter(PointerEventData e)
		{
			if (_selectable.interactable)
				Audio.Instance.PlayButton1();
	    }
		public void OnPointerExit(PointerEventData e)
		{
			if (_selectable.interactable)
				Audio.Instance.PlayButton0();
		}
	}
}
