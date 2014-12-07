using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
			if (!e.used && _selectable.interactable)
				Audio.Instance.PlayButton1();
		}
		public void OnPointerExit(PointerEventData e)
		{
			if (!e.used && _selectable.interactable)
				Audio.Instance.PlayButton0();
		}
	}
}
