using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Flop : UIBehaviour, IEndDragHandler, IDragHandler
{
	public float Offset = 32f;
	public int Limit = 4;
	private Transform _t;
	private float _current = 0f;
	private static List<int> _data = Enumerable.Range(111, 8).ToList();
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
		var data = string.Format("{0:X}", _data[i]);
		var text = string.Format("{0}[{1}]", i, data);
		o.name = text;
		Text[] texts = o.GetComponentsInChildren<Text>(true);
		texts[0].text = data;
		texts[1].text = i.ToString();
	}
	private void Drag(float delta)
	{
		_current += delta;
		for (int i = 0; i < _data.Count; i++)
		{
			var x = _current + (i * Offset);
			var visible = Mathf.Abs(x) < (Limit * Offset);
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
					Remove(i, t);
			}
			_views.TryGetValue(i, out t);
			if (t != null)
				t.localPosition = new Vector3(x, transform.localPosition.y, negative ? -x : x);
		}
		gameObject.SortChildren();
	}
	private void Snap()
	{
		var selected = Mathf.RoundToInt(_current / Offset);
		var delta = selected * Offset;
		_current = 0f;
		Drag(delta);
	}
	public void OnDrag(PointerEventData e)
	{
		Drag(e.delta.x);
	}
	public void OnEndDrag(PointerEventData e)
	{
		Snap();
	}
	public void Prev()
	{
		Drag(Offset);
	}
	public void Next()
	{
		Drag(-Offset);
	}
}
