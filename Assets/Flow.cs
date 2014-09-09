﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Flow : Singleton<Flow>, IEndDragHandler, IDragHandler
{
	public float Offset = 32f;
	public int Limit = 6;
	public Button PrevButton;
	public Button NextButton;
	public Scrollbar Scrollbar;
	public Text Text;
	private Transform _t;
	private bool _ignore = false;
	private float _current;
	private static List<int> _data = Enumerable.Range(111, 100).ToList();
	private Dictionary<int, Transform> _views = new Dictionary<int, Transform>();
	private void Start()
	{
		_t = transform;
		Scrollbar.numberOfSteps = _data.Count;
		for (int i = 0; (i < _data.Count) && (i < Limit); i++)
		{
			Add(i);
		}
		gameObject.SortChildren();
		UpdateButtons();
		UpdateScroll();
	}
	private void OnEnable()
	{
		Scrollbar.onValueChanged.AddListener(OnScrollChanged);
	}
	private void OnDisable()
	{
		Scrollbar.onValueChanged.RemoveListener(OnScrollChanged);
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
	public void DragTo(Transform t)
	{
		DragTo(_views.SingleOrDefault(x => x.Value == t).Key);
	}
	private void DragTo(int i)
	{
		DragTo(i * Offset * -1f);
	}
	private void DragTo(float delta)
	{
		_current = 0;
		Drag(delta);
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
		UpdateScroll();
	}
	public int GetCurrent()
	{
		return Mathf.Clamp(Mathf.RoundToInt((_current / Offset) * -1f), 0, _data.Count - 1);
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
	private void UpdateButtons()
	{
		var current = GetCurrent();
		PrevButton.interactable = (current > 0);
		NextButton.interactable = (current < _data.Count - 1);
	}
	private void UpdateScroll()
	{
		_ignore = true;
		float current = GetCurrent();
		Scrollbar.value = (current / (_data.Count - 1));
		Text.text = (current + 1).ToString();
		_ignore = false;
	}
	public void OnScrollChanged(float scroll)
	{
		if (!_ignore)
			DragTo(Mathf.RoundToInt(scroll * (_data.Count - 1)));
	}
	public void OnDrag(PointerEventData e)
	{
		Drag(e.delta.x);
	}
	public void OnEndDrag(PointerEventData e)
	{
		DragTo(GetCurrent());
	}
	public void OnPrev()
	{
		DragTo(GetCurrent() - 1);
	}
	public void OnNext()
	{
		DragTo(GetCurrent() + 1);
	}
}
