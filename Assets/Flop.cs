using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Flop : UIBehaviour, IDragHandler
{
	public float Offset = 64f;
	public Transform LookAt;
	private float _current = 0;
	private const int _limitSide = 4;
	private const int _limit = (_limitSide * 2) + 1;
	private List<int> _data = Enumerable.Range(111, 100).ToList();
	private List<GameObject> _views = Enumerable.Repeat((GameObject)null, _limit).ToList();
	private List<int> _tweens = Enumerable.Repeat(0, _limit).ToList();
	private int _tweenInertia;
	protected override void Start()
	{
		base.Start();
		for (int i = 0; (i < _data.Count) && (i < _limitSide); i++)
		{
			int viewIndex = GetViewIndex(GetDelta(_current, i));
			string data = string.Format("{0:X}", _data[i]);
			string text = string.Format("{0}[{1}]", viewIndex, data);
			GameObject o = Pool.Instance.Enter();
			o.name = text;
			Vector3 p = new Vector3(i * Offset, transform.localPosition.y, i * Offset);
			o.transform.localPosition = p;
			Text[] texts = o.GetComponentsInChildren<Text>(true);
			texts[0].text = data;
			texts[1].text = viewIndex.ToString();
			o.GetComponent<LookAt>().Target = LookAt;
			o.SetActive(true);
			_views[viewIndex] = o;
		}
		Order();
	}
	public void Drag(PointerEventData e)
	{
		foreach (Transform i in transform)
		{
			var x = i.localPosition.x + e.delta.x;
			Drag(x, i);
		}
		Order();
	}
	private void Order()
	{
		var children = GetComponentsInChildren<Transform>(true);
		var sorted = from child in children orderby child.gameObject.activeInHierarchy descending, child.localPosition.z descending select child;
		for (int i = 0; i < sorted.Count(); i++)
		{
			sorted.ElementAt(i).SetSiblingIndex(i);
		}
	}
	private void Drag(float x, Transform t)
	{
		t.localPosition = new Vector3(x, transform.localPosition.y, x < 0 ? -x : x);
	}
	public void OnDrag(PointerEventData e)
	{
		Drag(e);
	}
	private bool IsVisible(float delta)
	{
		return Mathf.Abs(delta) < _limitSide;
	}
	private int GetDataIndex(int viewIndex)
	{
		return Mathf.RoundToInt(viewIndex - _limitSide + _current);
	}
	private int GetViewIndex(float delta)
	{
		return Mathf.RoundToInt(delta + _limitSide);
	}
	private float GetDelta(float target, int dataIndex)
	{
		return (target - dataIndex) * -1;
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
		return GetDataIndex(closestIndex);
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
}
