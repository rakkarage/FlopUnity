using System.Collections.Generic;
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
	private float _time = .333f;
	private bool _ignore;
	private float _current;
	private static List<int> _data = Enumerable.Range(32, 95).ToList();
	private Dictionary<int, Transform> _views = new Dictionary<int, Transform>();
	private void Start()
	{
		_t = transform;
		Scrollbar.numberOfSteps = _data.Count;
		for (int i = 0; (i < _data.Count) && (i < Limit); i++)
			Add(i);
		UpdateAll();
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
    	o.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, offset / (Limit * Offset));
		UpdateName(o, i);
		_views.Add(i, o.transform);
	}
	private void Remove(int i, Transform t)
	{
		_views.Remove(i);
		Pool.Instance.Exit(t.gameObject);
	}
	public void TweenTo(Transform t)
	{
		Tween(_views.SingleOrDefault(x => x.Value == t).Key);
	}
	private void TweenBy(int by)
	{
		Tween(GetCurrent() + by);
	}
	private void Tween(int to)
	{
		Ease.Go(GetComponent<Flow>(), _current, to * -Offset, _time, DragTo);
	}
	private void DragTo(int i)
	{
		DragTo(i * -Offset);
	}
	private void DragTo(float delta)
	{
		_current = 0;
		Drag(delta);
	}
	private void Drag(float delta)
	{
		Transform t;
		var max = (Offset * (Limit - 2f));
		var min = -((_data.Count - 1) * Offset + max);
		_current = Mathf.Clamp(_current + delta, min, max);
		for (int i = 0; i < _data.Count; i++)
		{
			var x = _current + (i * Offset);
            var ax = Mathf.Abs(x);
			var visible = ax < (Limit * Offset);
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
			{
                t.localPosition = new Vector3(x, t.localPosition.y, ax);
                t.gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, ax / (Limit * Offset));
			}
		}
		UpdateAll();
	}
	public int GetCurrent()
	{
		return Mathf.Clamp(Mathf.RoundToInt(-(_current / Offset)), 0, _data.Count - 1);
	}
	private void UpdateName(GameObject o, int i)
	{
		o.name = i.ToString();
		var data = string.Format("{0}", (char)_data[i]);
		foreach (var text in o.GetComponentsInChildren<Text>(true))
			text.text = data;
	}
	private void UpdateAll()
	{
		gameObject.SortChildren();
		UpdateButtons();
		UpdateScroll();
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
		if (!e.used)
			Tween(GetCurrent());
	}
	public void OnPrev()
	{
		TweenBy(-1);
	}
	public void OnNext()
	{
		TweenBy(1);
	}
}
