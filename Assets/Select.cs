using UnityEngine;
﻿using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Select : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData e)
	{
		var texts = GetComponentsInChildren<Text>();
		Flow.Instance.DragToIndex(int.Parse(texts[1].text));
	}
}
