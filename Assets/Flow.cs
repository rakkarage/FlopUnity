using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Flow : UIBehaviour, IEndDragHandler, IDragHandler
{
	public float Offset = 32f;
	public int Limit = 4;
	public Button PrevButton;
	public Button NextButton;
	public Scrollbar Scrollbar;
	private Transform _t;
	private float _current = 0f;
	private static List<int> _data = Enumerable.Range(111, 33).ToList();
	private Dictionary<int, Transform> _views = new Dictionary<int, Transform>();
	protected override void Start()
	{
		base.Start();
		_t = transform;
		for (int i = 0; (i < _data.Count) && (i < Limit); i++)
		{
			Add(i);
		}
		gameObject.SortChildren();
		UpdateButtons();
	}
	private void Add(int i)
	{
		GameObject o = Pool.Instance.Enter();
		var offset = i * Offset;
		o.transform.localPosition = new Vector3(offset, _t.localPosition.y, offset);
		UpdateName(o, i);
		_views.Add(i, o.transform);
	}
	private void Remove(int i, Transform t)
	{
		_views.Remove(i);
		Pool.Instance.Exit(t.gameObject);
	}
	private void UpdateName(GameObject o, int i)
	{
		var text0 = i.ToString();
		var text1 = string.Format("{0:X}", _data[i]);
		o.name = text0;
		Text[] texts = o.GetComponentsInChildren<Text>(true);
		texts[0].text = text1;
		texts[1].text = text0;
	}
	private void Drag(float delta)
	{
		Transform t;
		var max = (Offset * (Limit - .5f));
		var min = -((_data.Count - 1) * Offset + max);
		_current = Mathf.Clamp(_current + delta, min, max);
		for (int i = 0; i < _data.Count; i++)
		{
			var x = _current + (i * Offset);
			var visible = Mathf.Abs(x) < (Limit * Offset);
			var negative = x < transform.localPosition.x;
			_views.TryGetValue(i, out t);
			if (t == null)
			{
				if (visible)
					Add(i);
			}
			else
			{
				if (!visible)
					Remove(i, t);
			}
			_views.TryGetValue(i, out t);
			if (t != null)
				t.localPosition = new Vector3(x, transform.localPosition.y, negative ? -x : x);
		}
		gameObject.SortChildren();
		UpdateButtons();
	}
	private void Snap()
	{
		var i = Mathf.Clamp(Mathf.RoundToInt(_current / Offset), -(_data.Count - 1), 0);
		var delta = i * Offset;
		_current = 0f;
		Drag(delta);
		UpdateButtons();
	}
	private void UpdateButtons()
	{
		var half = Offset * .5f;
		PrevButton.interactable = (_current <= 0 - half);
		NextButton.interactable = (_current >= -(_data.Count - 1) * Offset + half);
	}
	public void OnDrag(PointerEventData e)
	{
		Drag(e.delta.x);
	}
	public void OnEndDrag(PointerEventData e)
	{
		Snap();
	}
	public void OnScrollChanged(float scroll)
	{
		var temp = (_data.Count * Offset) * scroll;
		Debug.Log(temp);
		Drag(temp);
	}
	public void Prev()
	{
		Drag(Offset);
		UpdateButtons();
	}
	public void Next()
	{
		Drag(-Offset);
		UpdateButtons();
	}
}
