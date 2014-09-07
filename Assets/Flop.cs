using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Flop : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public float Offset = 64f;
	private Transform _t;
	private float _current = 0;
	private const int _limitSide = 4;
	private const int _limit = (_limitSide * 2) + 1;
	private List<int> _data = Enumerable.Range(111, 7).ToList();
	private Dictionary<int, Transform> _views = new Dictionary<int, Transform>();
	protected override void Start()
	{
		base.Start();
		_t = transform;
		for (int i = 0; (i < _data.Count) && (i < _limitSide); i++)
		{
			Add(i);
		}
		gameObject.SortChildren();
	}
	private void Add(int i)
	{
		GameObject o = Pool.Instance.Enter();
		var offset = i * Offset;
		o.transform.localPosition = new Vector3(offset, _t.localPosition.y, offset);
		UpdateName(o, i);
		_views.Add(i, o.transform);
	}
	private void Remove(GameObject o)
	{
		Pool.Instance.Exit(o);
	}
	public void OnBeginDrag(PointerEventData e)
	{
		Drag(e.delta.x);
	}
	public void OnDrag(PointerEventData e)
	{
		Drag(e.delta.x);
	}
	public void OnEndDrag(PointerEventData e)
	{
		Drag(e.delta.x);
	}
	private int GetViewIndex(float delta)
	{
		return Mathf.RoundToInt(delta + _limitSide);
	}
	private float GetDelta(float target, int dataIndex)
	{
		return (target - dataIndex) * -1;
	}
	private void Drag(float offset)
	{
		_current += offset;
		for (int i = 0; i < _data.Count; i++)
		{
			var x = _current + (i * Offset);
			var visible = Mathf.Abs(x) < (_limitSide * Offset);
			var negative = x < transform.localPosition.x;

			Transform t;
			_views.TryGetValue(i, out t);
			if (t == null)
			{
				if (visible)
					Add(i);
			}
			else
			{
				if (!visible)
					Remove(t.gameObject);
				else
					t.localPosition = new Vector3(x, transform.localPosition.y, negative ? -x : x);
			}
		}
		gameObject.SortChildren();
	}
	private void UpdateName(GameObject view, int index)
	{
		string data = string.Format("{0:X}", _data[index]);
		string text = string.Format("{0}[{1}]", index, data);
		view.name = text;
		Text[] texts = view.GetComponentsInChildren<Text>(true);
		texts[0].text = data;
		texts[1].text = index.ToString();
	}
	public void Prev()
	{
		if (_current > 0)
		{
			// Snap(Mathf.RoundToInt(_current) - 1);
		}
	}
	public void Next()
	{
		if (_current < _data.Count - 1)
		{
			// Snap(Mathf.RoundToInt(_current) + 1);
		}
	}
}
