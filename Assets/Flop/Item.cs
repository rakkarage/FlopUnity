using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
    public class Item : MonoBehaviour, IPointerDownHandler
	{
        public UnityAction Stop;
        public void OnPointerDown(PointerEventData e)
        {
            if (Stop != null)
                Stop();
        }
	}
}
