using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Select : MonoBehaviour, IPointerClickHandler
{
	private Transform _t;
	private void Start()
	{
		_t = transform;
	}
	public void OnPointerClick(PointerEventData e)
	{
		Flow.Instance.DragTo(_t);
	}
}
