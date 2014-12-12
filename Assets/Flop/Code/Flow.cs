using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Flow : Singleton<Flow>, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		public bool Big = false;
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
		private Fade _fade;
		private CanvasScaler _scaler;
		private IEnumerator _ease;
		private IEnumerator _inertiaDecayEase;
		private IEnumerator _inertiaEase;
		private float _inertia;
		private float _max;
		private float _min;
		private bool _ignore;
		private float _current;
		private int _dataMax;
		private static readonly List<int> _data = Enumerable.Range(32, 95).ToList();
		private static readonly List<int> _dataBig = Enumerable.Range(1000000, 1000000).ToList();
		private Dictionary<int, Transform> _views;
		private void Start()
		{
			var count = Big ? _dataBig.Count : _data.Count;
			_views = new Dictionary<int, Transform>(count);
			_fade = GetComponentInParent<Fade>();
			_scaler = GetComponentInParent<CanvasScaler>();
			_dataMax = count - 1;
			_max = (Offset.x * (Limit - 2f));
			_min = -(_dataMax * Offset.x + _max);
			Scrollbar.numberOfSteps = count;
			for (var i = 0; (i < count) && (i < Limit); i++)
				Add(i);
			UpdateAll();
			LoadPage();
			Ease3.GoRotation(this, new Vector3(-360f, 0f, 0f), 1f, 0f, EaseType.Spring);
		}
		private void OnEnable()
		{
			Scrollbar.onValueChanged.AddListener(OnScrollChanged);
			Data.LoadedEvent += LoadPage;
		}
		private void OnDisable()
		{
			Scrollbar.onValueChanged.RemoveListener(OnScrollChanged);
			Data.LoadedEvent -= LoadPage;
		}
		private void LoadPage()
		{
			var page = Data.Instance.Page;
			var pageBig = Data.Instance.PageBig;
			EaseTo(Big ? (pageBig == -1 ? 0 : pageBig) : (page == -1 ? 1 : page));
		}
		private void Add(int i)
		{
			GameObject o = Pool.Enter();
			o.GetComponent<LookAt>().Target = LookAt;
			var button = o.GetComponent<Button>();
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => { EaseTo(o.transform); });
			UpdateItem(o, i * Offset.x);
			UpdateName(o, i);
			_views.Add(i, o.transform);
		}
		private void Remove(int i, Transform t)
		{
			_views.Remove(i);
			Pool.Exit(t.gameObject);
		}
		public void EaseBy(int by)
		{
			EaseTo(GetCurrent() + by);
		}
		public void EaseTo(Transform t)
		{
			EaseTo(_views.SingleOrDefault(x => x.Value == t).Key);
		}
		private void EaseTo(int to)
		{
			if (_ease != null)
				StopCoroutine(_ease);
			_ease = Ease.Go(this, _current, to * -Offset.x, Time, 0f, EaseType.Spring, DragTo, null);
		}
		private void DragTo(int i)
		{
			DragTo(i * -Offset.x);
		}
		private void DragTo(float delta)
		{
			var old = GetCurrent();
			_current = 0;
			Drag(delta, old);
		}
		private void Drag(float delta)
		{
			Drag(delta, GetCurrent());
		}
		private void Drag(float delta, int old)
		{
			_current = Mathf.Clamp(_current + delta, _min, _max);
			var current = GetCurrent();
			var min = Mathf.Min(old, current) - Limit;
			if (min < 0) min = 0;
			var max = Mathf.Max(old, current) + Limit;
			if (max > _dataMax) max = _dataMax;
			bool back = current < old;
			for (var i = (back ? max : min); (back ? (i >= min) : (i <= max)); i = (back ? i - 1 : i + 1))
			{
				var x = _current + (i * Offset.x);
				var ax = Mathf.Abs(x);
				var lx = Limit * Offset.x;
				var visible = ax < lx;
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
			var data = Big ? _dataBig[i].ToString("x") :  string.Format("{0}", (char)_data[i]);
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
			Text.text = Big ? current.ToString() : (current + 32).ToString();
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
			if (Big)
				Data.Instance.PageBig = GetCurrent();
			else
				Data.Instance.Page = GetCurrent();
		}
		public void Stop()
		{
			_inertia = 0f;
			if (_inertiaEase != null)
				StopCoroutine(_inertiaEase);
			if (_inertiaDecayEase != null)
				StopCoroutine(_inertiaDecayEase);
		}
		public void OnPointerDown(PointerEventData e)
		{
			Stop();
		}
		public void OnPointerUp(PointerEventData e)
		{
			if (Mathf.Abs(_inertia) < 0.001f)
				Snap();
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
			var time = Mathf.Clamp(Mathf.Abs(_inertia * .1f), 0f, Big ? 33f : 3.33f);
			_inertiaDecayEase = Ease.Go(this, _inertia, 0f, time, 0f, EaseType.Linear, i => _inertia = i, null);
		}
		public void OnEndDrag(PointerEventData e)
		{
			if (Mathf.Abs(_inertia) > .0333f)
			{
				var time = Mathf.Clamp(Mathf.Abs(_inertia * .1f), 0f, Big ? 33f : 3.33f);
				_inertiaEase = Ease.Go(this, -_inertia, 0f, time, 0f, EaseType.Linear, Drag, Snap);
			}
			else
				Snap();
		}
		private void Snap()
		{
			_inertia = 0f;
			EaseTo(GetCurrent());
		}
		public void OnPrev()
		{
			Audio.Instance.PlayClick();
			Stop();
			Prev();
		}
		public void Prev()
		{
			EaseBy(-1);
		}
		public void OnNext()
		{
			Audio.Instance.PlayClick();
			Stop();
			Next();
		}
		public void Next()
		{
			EaseBy(1);
		}
		public void FadeBack()
		{
			_fade.FadeTo(.25f, 1f);
		}
		public void FadeFore()
		{
			_fade.FadeTo(1f, 1f);
		}
	}
}
