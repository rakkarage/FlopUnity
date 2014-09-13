using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Flow : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public Vector3 Offset = new Vector3(32f, -3f, 16f);
	public bool AbsoluteY = true;
	public float InsetZ = .5f;
	public float Time = .333f;
	public int Limit = 6;
	public Pool Pool;
	public Transform LookAt;
	public Button PrevButton;
	public Button NextButton;
	public Scrollbar Scrollbar;
	public Text Text;
	private ReferenceResolution _r;
	private float _velocity;
	private MonoBehaviour _m;
	private float _max;
	private float _min;
	private bool _ignore;
	private float _current;
	private int _dataMax;
	private static List<int> _data = Enumerable.Range(32, 95).ToList();
	private Dictionary<int, Transform> _views = new Dictionary<int, Transform>(_data.Count);
	private AudioSource _audio;
	private void Start()
	{
		_m = GetComponent<Flow>();
		_r = GetComponentInParent<ReferenceResolution>();
		_audio = GetComponent<AudioSource>();
		_dataMax = _data.Count - 1;
		_max = (Offset.x * (Limit - 2f));
		_min = -(_dataMax * Offset.x + _max);
		Scrollbar.numberOfSteps = _data.Count;
		for (int i = 0; (i < _data.Count) && (i < Limit); i++)
			Add(i);
		UpdateAll();
		Next();
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
		GameObject o = Pool.Enter();
		o.GetComponent<LookAt>().Target = LookAt;
		o.GetComponent<Button>().onClick.AddListener(delegate { TweenTo(o.transform); });
		o.GetComponent<Item>().Flow = this;
		UpdateItem(o, i * Offset.x);
		UpdateName(o, i);
		_views.Add(i, o.transform);
	}
	private void Remove(int i, Transform t)
	{
		_views.Remove(i);
		Pool.Exit(t.gameObject);
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
		Ease.Go(_m, _current, to * -Offset.x, Time, DragTo, null, Ease.Type.Spring);
	}
	private void DragTo(int i)
	{
		DragTo(i * -Offset.x);
	}
	private void DragTo(float delta)
	{
		_current = 0;
		Drag(delta);
	}
	private void Drag(float delta)
	{
		Transform t;
		_current = Mathf.Clamp(_current + delta, _min, _max);
		for (int i = 0; i < _data.Count; i++)
		{
			var x = _current + (i * Offset.x);
			var ax = Mathf.Abs(x);
			var lx = Limit * Offset.x;
			var visible = ax < lx;
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
				UpdateItem(t.gameObject, x);
		}
		UpdateAll();
	}
	private int GetCurrent()
	{
		return Mathf.Clamp(Mathf.RoundToInt(-(_current / Offset.x)), 0, _dataMax);
	}
	private void UpdateItem(GameObject o, float x)
	{
		var xl = Limit * Offset.x;
		var xn = x / xl;
		var axn = Mathf.Abs(x) / xl;
		var y = (AbsoluteY ? axn : xn) * (Limit * Offset.y);
		var z = (axn - InsetZ) * (Limit * Offset.z);
		o.transform.localPosition = new Vector3(x, y, z);
		o.GetComponent<CanvasGroup>().alpha = 1 - axn;
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
		NextButton.interactable = (current < _dataMax);
	}
	private void UpdateScroll()
	{
		_ignore = true;
		float current = GetCurrent();
		Scrollbar.value = current / _dataMax;
		Text.text = (current + 32).ToString();
		_ignore = false;
	}
	private void OnScrollChanged(float scroll)
	{
		if (!_ignore)
		{
			_m.StopAllCoroutines();
			DragTo(Mathf.RoundToInt(scroll * _dataMax));
		}
	}
	public void OnBeginDrag(PointerEventData e)
	{
		_m.StopAllCoroutines();
	}
	public void OnDrag(PointerEventData e)
	{
		var temp = _current;
		if (_r != null)
			Drag(e.delta.x * _r.resolution.x / Screen.width);
		else
			Drag(e.delta.x);
		_velocity = temp - _current;
	}
	public void OnEndDrag(PointerEventData e)
	{
		if (!e.used)
		{
			if (!Mathf.Approximately(_velocity, 0f))
				Ease.Go(_m, -_velocity, 0f, Mathf.Abs(_velocity * .1f), Drag, Snap, Ease.Type.Linear);
			else
				Snap();
		}
	}
	private void Snap()
	{
		_velocity = 0f;
		Tween(GetCurrent());
	}
	public void OnPrev()
	{
		PlayClick();
		Prev();
	}
	private void Prev()
	{
		TweenBy(-1);
	}
	public void OnNext()
	{
		PlayClick();
		Next();
	}
	private void Next()
	{
		TweenBy(1);
	}
	public void PlayClick()
	{
		_audio.Play();
	}
}
