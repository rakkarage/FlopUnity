using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Flop : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public float Offset = 64f;
	private Transform _t;
	private float _time = .333f;
	private float _current = 0;
	private const int _limitSide = 4;
	private const int _limit = (_limitSide * 2) + 1;
	private List<int> _data = Enumerable.Range(111, 3).ToList();
	private List<GameObject> _views = Enumerable.Repeat((GameObject)null, _limit).ToList();
	private List<int> _tweens = Enumerable.Repeat(0, _limit).ToList();
	private int _tweenInertia;
	protected override void Start()
	{
		base.Start();
		_t = transform;
		for (int i = 0; (i < _data.Count) && (i < _limitSide); i++)
		{
			GameObject o = Pool.Instance.Enter();
			var offset = i * Offset;
			o.transform.localPosition = new Vector3(offset, _t.localPosition.y, offset);
			int viewIndex = GetViewIndex(GetDelta(_current, i));
			_views[viewIndex] = o;
			UpdateName(_views[viewIndex], viewIndex, i);
		}
		gameObject.SortChildren();
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
		// todo: inertia
		//float velocity = target.LocalTransformCenter.x - target.PreviousLocalTransformCenter.x;
		//if (Mathf.Abs(velocity) > Threshold)
		//	Inertia(velocity);
		//else
		//	Go();
		Go();
	}
	private void Drag(float offset)
	{
		float target = _current - offset;
		List<GameObject> newViews = Enumerable.Repeat((GameObject)null, _limit).ToList();
		for (int i = 0; i < _data.Count; i++)
		{
			float delta = GetDelta(target, i);
			int viewIndex = GetViewIndex(delta);
			float oldDelta = GetDelta(_current, i);
			int oldViewIndex = GetViewIndex(oldDelta);
			bool isVisible = IsVisible(delta);
			bool wasVisible = IsVisible(oldDelta);
			if (viewIndex == -1) Debug.Log("!!!ERROR!!!");
			if (wasVisible && !isVisible)
			{
				Pool.Instance.Exit(_views[viewIndex]); // todo: old?
			}
			else if (isVisible && !wasVisible)
			{
				newViews[viewIndex] = Pool.Instance.Enter();
				newViews[viewIndex].transform.localPosition = DragItem(i, delta) * Offset;
			}
			else if (isVisible)
			{
				//FlowSnapItemCancel(viewIndex);
				newViews[viewIndex] = _views[oldViewIndex];
				newViews[viewIndex].transform.localPosition = DragItem(i, delta) * Offset;
			}
			if (isVisible)
				UpdateName(newViews[viewIndex], viewIndex, i);
		}
		_views = newViews;
		_current = target;
		gameObject.SortChildren();
	}
	public void Go()
	{
		Snap(GetClosestViewIndex());
	}
	public void GoTo(GameObject o)
	{
		Snap(GetViewIndex(o));
	}
	private void SnapCancel(int viewIndex)
	{
		LeanTween.cancel(_views[viewIndex], _tweens[viewIndex]);
	}
	private void SnapTo(GameObject o, float delta, bool instant)
	{
		Vector3 to = new Vector3(delta * Offset, 0f, Mathf.Abs(delta) * Offset);
		if (instant)
			o.transform.position = to;
		else
			LeanTween.moveLocal(o, to, _time).setEase(LeanTweenType.easeSpring);
	}
	public void Snap(int target)
	{
		// Debug.Log(target);
		List<GameObject> newViews = Enumerable.Repeat((GameObject)null, _limit).ToList();
		for (int i = 0; i < _data.Count; i++)
		{
			float delta = GetDelta(target, i);
			int viewIndex = GetViewIndex(delta);
			float oldDelta = GetDelta(_current, i);
			int oldViewIndex = GetViewIndex(oldDelta);
			bool isVisible = IsVisible(delta);
			bool wasVisible = IsVisible(oldDelta);
			if (wasVisible && !isVisible)
			{
				Pool.Instance.Exit(_views[oldViewIndex]); // todo: old?
			}
			else if (isVisible && !wasVisible)
			{
				newViews[viewIndex] = Pool.Instance.Enter();
				SnapTo(newViews[viewIndex], delta, true);
			}
			else if (isVisible)
			{
				SnapCancel(oldViewIndex);
				newViews[viewIndex] = _views[oldViewIndex];
				SnapTo(newViews[viewIndex], delta, false);
			}
			if (isVisible)
				UpdateName(newViews[viewIndex], viewIndex, i);
		}
		_views = newViews;
		_current = target;
	}
	private float ClampX(int dataIndex, bool negative)
	{
		var newIndex = negative ? dataIndex : _data.Count - dataIndex - 1;
		var clamp = _data.Count * Offset + 1f;
		clamp -= (Offset * newIndex);
		return clamp;
	}
	private Vector3 DragItem(int dataIndex, float delta)
	{
		var negative = delta < 0;
		var clampX = Mathf.Clamp(delta, -ClampX(dataIndex, negative), ClampX(dataIndex, negative));
		var clampZ = Mathf.Clamp(Mathf.Abs(delta), 0f, ClampX(dataIndex, negative));
		var p = new Vector3(clampX, transform.position.y, clampZ);
		return p;
	}
	private bool IsVisible(float delta)
	{
		return Mathf.Abs(delta) < _limitSide;
	}
	private int GetDataIndex(int viewIndex)
	{
		float temp = (_current / Offset);
		Debug.Log("+++temp: " + temp);
		return Mathf.RoundToInt(viewIndex - _limitSide + temp);
	}
	private int GetViewIndex(float delta)
	{
		return Mathf.RoundToInt(delta + _limitSide);
	}
	private float GetDelta(float target, int dataIndex)
	{
		return ((target / Offset) - dataIndex) * -1;
	}
	public int GetClosestViewIndex()
	{
		int closestIndex = -1;
		float closestDistance = float.MaxValue;
		for (int i = 0; i < _views.Count; i++)
		{
			if (_views[i])
			{
				float distance = (gameObject.transform.position - _views[i].transform.localPosition).sqrMagnitude;
				if (distance < closestDistance)
				{
					closestIndex = i;
					closestDistance = distance;
				}
			}
		}
		var temp = GetDataIndex(closestIndex);
		Debug.Log(closestIndex);
		Debug.Log(temp);
		return temp;
	}
	private int GetViewIndex(GameObject view)
	{
		int found = -1;
		for (int i = 0; i < _views.Count; i++)
		{
			if (_views[i])
			{
				if (view == _views[i])
				{
					found = i;
				}
			}
		}
		return GetDataIndex(found);
	}
	private void UpdateName(GameObject view, int viewIndex, int dataIndex)
	{
		string data = string.Format("{0:X}", _data[dataIndex]);
		string text = string.Format("{0}[{1}]", viewIndex, data);
		view.name = text;
		Text[] texts = view.GetComponentsInChildren<Text>(true);
		texts[0].text = data;
		texts[1].text = viewIndex.ToString();
	}
	public void Prev()
	{
		if (_current > 0)
		{
			Snap(Mathf.RoundToInt(_current) - 1);
		}
	}
	public void Next()
	{
		if (_current < _data.Count - 1)
		{
			Snap(Mathf.RoundToInt(_current) + 1);
		}
	}
}
