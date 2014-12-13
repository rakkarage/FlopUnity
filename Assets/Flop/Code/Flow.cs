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
		[SerializeField]
		private bool _big = false;
		[SerializeField]
		private Vector3 _offset = new Vector3(32f, -3f, 16f);
		[SerializeField]
		private bool _absoluteY = true;
		[SerializeField]
		private float _insetZ = .5f;
		[SerializeField]
		private float _time = .333f;
		[SerializeField]
		private int _limit = 6;
		[SerializeField]
		private Transform _lookAt = null;
		[SerializeField]
		private Button _prev = null;
		[SerializeField]
		private Button _next = null;
		[SerializeField]
		private Scrollbar _scrollbar = null;
		private Text _scrollbarText;
		private Fade _fade;
		private CanvasScaler _scaler;
		private Pool _pool;
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
			var count = _big ? _dataBig.Count : _data.Count;
			_views = new Dictionary<int, Transform>(count);
			_scrollbarText = _scrollbar.GetComponentInChildren<Text>();
			_fade = GetComponentInParent<Fade>();
			_scaler = GetComponentInParent<CanvasScaler>();
			_pool = GetComponent<Pool>();
			_dataMax = count - 1;
			_max = (_offset.x * (_limit - 2f));
			_min = -(_dataMax * _offset.x + _max);
			_scrollbar.numberOfSteps = count;
			for (var i = 0; (i < count) && (i < _limit); i++)
				Add(i);
			UpdateAll();
			LoadPage();
			Ease3.GoRotation(this, new Vector3(-360f, 0f, 0f), 1f, 0f, EaseType.Spring);
		}
		private void OnEnable()
		{
			_scrollbar.onValueChanged.AddListener(OnScrollChanged);
			Data.LoadedEvent += LoadPage;
		}
		private void OnDisable()
		{
			_scrollbar.onValueChanged.RemoveListener(OnScrollChanged);
			Data.LoadedEvent -= LoadPage;
		}
		private void LoadPage()
		{
			var page = Data.Instance.Page;
			var pageBig = Data.Instance.PageBig;
			EaseTo(_big ? (pageBig == -1 ? 0 : pageBig) : (page == -1 ? 1 : page));
		}
		private void Add(int i)
		{
			GameObject o = _pool.Enter();
			o.GetComponent<LookAt>().Target = _lookAt;
			var button = o.GetComponent<Button>();
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => { EaseTo(o.transform); });
			UpdateItem(o, i * _offset.x);
			UpdateName(o, i);
			_views.Add(i, o.transform);
		}
		private void Remove(int i, Transform t)
		{
			_views.Remove(i);
			_pool.Exit(t.gameObject);
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
			_ease = Ease.Go(this, _current, to * -_offset.x, _time, 0f, EaseType.Spring, DragTo, null);
		}
		private void DragTo(int i)
		{
			DragTo(i * -_offset.x);
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
			var min = Mathf.Min(old, current) - _limit;
			if (min < 0) min = 0;
			var max = Mathf.Max(old, current) + _limit;
			if (max > _dataMax) max = _dataMax;
			bool back = current < old;
			for (var i = (back ? max : min); (back ? (i >= min) : (i <= max)); i = (back ? i - 1 : i + 1))
			{
				var x = _current + (i * _offset.x);
				var ax = Mathf.Abs(x);
				var lx = _limit * _offset.x;
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
			return Mathf.Clamp(Mathf.RoundToInt(-(_current / _offset.x)), 0, _dataMax);
		}
		private void UpdateItem(GameObject o, float x)
		{
			var xl = _limit * _offset.x;
			var xn = x / xl;
			var axn = Mathf.Abs(x) / xl;
			var y = (_absoluteY ? axn : xn) * (_limit * _offset.y);
			var z = (axn - _insetZ) * (_limit * _offset.z);
			o.transform.localPosition = new Vector3(x, y, z);
			o.GetComponent<CanvasGroup>().alpha = 1 - axn;
		}
		private void UpdateName(GameObject o, int i)
		{
			o.name = i.ToString();
			var data = _big ? _dataBig[i].ToString("x") :  string.Format("{0}", (char)_data[i]);
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
			_prev.interactable = (current > 0);
			_next.interactable = (current < _dataMax);
		}
		private void UpdateScroll()
		{
			_ignore = true;
			float current = GetCurrent();
			_scrollbar.value = current / _dataMax;
			_scrollbarText.text = _big ? current.ToString() : (current + 32).ToString();
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
			if (_big)
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
			var time = Mathf.Clamp(Mathf.Abs(_inertia * .1f), 0f, _big ? 33f : 3.33f);
			_inertiaDecayEase = Ease.Go(this, _inertia, 0f, time, 0f, EaseType.Linear, i => _inertia = i, null);
		}
		public void OnEndDrag(PointerEventData e)
		{
			if (Mathf.Abs(_inertia) > .0333f)
			{
				var time = Mathf.Clamp(Mathf.Abs(_inertia * .1f), 0f, _big ? 33f : 3.33f);
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
		public void FadeOut()
		{
			_fade.FadeTo(.25f, 1f);
		}
		public void FadeIn()
		{
			_fade.FadeTo(1f, 1f);
		}
	}
}
