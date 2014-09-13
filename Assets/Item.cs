using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Item : MonoBehaviour, IPointerClickHandler
{
	public Flow Flow;
	private Transform _t;
	private void Start()
	{
		_t = transform;
	}
	public void OnPointerClick(PointerEventData e)
	{
		e.Use();
		Flow.TweenTo(_t);
	}
}
