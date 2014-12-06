using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Flow : Singleton<Flow>, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
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
		private CanvasScaler _scaler;
		private float _inertia;
		private float _max;
		private float _min;
		private bool _ignore;
		private float _current;
		private int _dataMax;
		private static List<int> _data = Enumerable.Range(32, 95).ToList();
		private Dictionary<int, Transform> _views = new Dictionary<int, Transform>(_data.Count);
		private void Start()
		{
			_scaler = GetComponentInParent<CanvasScaler>();
			_dataMax = _data.Count - 1;
			_max = (Offset.x * (Limit - 2f));
			_min = -(_dataMax * Offset.x + _max);
			Scrollbar.numberOfSteps = _data.Count;
			for (int i = 0; (i < _data.Count) && (i < Limit); i++)
				Add(i);
			UpdateAll();
			TweenBy(Data.Instance.Page);
			Ease3.GoRotation(this, gameObject, gameObject.transform.localRotation.eulerAngles, new Vector3(360f, 0f, 0f), Constants.SpringTime, 0f, EaseType.Spring);
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
			var button = o.GetComponent<Button>();
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => { TweenTo(o.transform); });
			Item item = o.GetComponent<Item>();
			item.Flow = this;
			item.ResetColor();
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
		public void TweenBy(int by)
		{
			Tween(GetCurrent() + by);
		}
		private void Tween(int to)
		{
			Ease.Go(this, _current, to * -Offset.x, Time, 0f, EaseType.Spring, (i) => { DragTo(i); }, null);
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
				Audio.Instance.PlayClick();
				Stop();
				DragTo(Mathf.RoundToInt(scroll * _dataMax));
			}
			Data.Instance.Page = GetCurrent();
		}
		public void Stop()
		{
			_inertia = 0f;
			StopAllCoroutines();
		}
		public void OnPointerDown(PointerEventData e)
		{
			Stop();
		}
		public void OnBeginDrag(PointerEventData e)
		{
			Stop();
		}
		public void OnDrag(PointerEventData e)
		{
			Stop();
			var temp = _current;
			if (_scaler != null)
				Drag(e.delta.x * _scaler.referenceResolution.x / Screen.width);
			else
				Drag(e.delta.x);
			_inertia = temp - _current;
			var time = Mathf.Clamp(Mathf.Abs(_inertia * .1f), 0, 3.33f);
			Ease.Go(this, _inertia, 0f, time, 0f, EaseType.Linear, (i) => { _inertia = i; }, null);
		}
		public void OnEndDrag(PointerEventData e)
		{
			if (!e.used)
			{
				if (Mathf.Abs(_inertia) > .0333f)
				{
					var time = Mathf.Clamp(Mathf.Abs(_inertia * .1f), 0, 3.33f);
					Ease.Go(this, -_inertia, 0f, time, 0f, EaseType.Linear, (i) => { Drag(i); }, Snap);
				}
				else
					Snap();
			}
		}
		private void Snap()
		{
			_inertia = 0f;
			Tween(GetCurrent());
		}
		public void OnPrev()
		{
			Audio.Instance.PlayClick();
			Stop();
			Prev();
		}
		public void Prev()
		{
			TweenBy(-1);
		}
		public void OnNext()
		{
			Audio.Instance.PlayClick();
			Stop();
			Next();
		}
		public void Next()
		{
			TweenBy(1);
		}
	}
}
