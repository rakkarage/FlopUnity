using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	[RequireComponent(typeof(Pool))]
	public abstract class Flow<T> : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private Vector3 _offset = new Vector3(32f, -3f, 16f);
		[SerializeField] private bool _absoluteY = true;
		[SerializeField] private float _insetZ = .5f;
		[SerializeField] private int _limit = 6;
		[SerializeField] private Transform _lookAt = null;
		private Pool _pool;
		private CanvasScaler _scaler;
		private IEnumerator _ease;
		private IEnumerator _inertiaDecayEase;
		private IEnumerator _inertiaEase;
		private float _time = .333f;
		private float _inertia;
		private float _inertiaLimit = 3.33f;
		private float _max;
		private float _min;
		private float _current;
		private int _dataMax;
		private Dictionary<int, Transform> _views;
		protected abstract List<T> Data { get; }
		protected virtual int StartAt { get { return 0; } }
		protected bool First { get { return GetCurrent() == 0; } }
		protected bool Last { get { return GetCurrent() == _dataMax; } }
		protected virtual void Start()
		{
			_pool = GetComponent<Pool>();
			_scaler = GetComponentInParent<CanvasScaler>();
			Make(StartAt);
		}
		protected void Clear()
		{
			for (var i = _views.Count - 1; i >= 0; i--)
				Exit(i);
		}
		protected void Make()
		{
			Make(GetCurrent());
		}
		protected void Make(int at)
		{
			if (_views != null)
				Clear();
			_views = new Dictionary<int, Transform>(Data.Count);
			_dataMax = Data.Count - 1;
			_max = (_offset.x * (_limit - 2f));
			_min = -(_dataMax * _offset.x + _max);
			for (var i = 0; (i < Data.Count) && (i < _limit); i++)
				Enter(i);
			EaseTo(at);
			UpdateCurrent(GetCurrent());
		}
		protected abstract void UpdateCurrent(int current);
		protected abstract void Apply(GameObject o, T d);
		private void Enter(int i)
		{
			var o = _pool.Enter();
			var l = o.GetComponent<LookAt>();
			if (l != null)
			{
				l.Target = _lookAt == null ? Camera.main.transform : _lookAt;
				StartCoroutine(Face(l));
			}
			var button = o.GetComponent<Button>();
			if (button != null)
			{
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() => { EaseTo(o.transform); });
			}
			o.name = i.ToString();
			var item = o.GetComponent<Item>();
			item.Stop = Stop;
			UpdateItem(o, i * _offset.x);
			_views.Add(i, o.transform);
			Apply(o, Data[i]);
		}
		private void Exit(int i)
		{
			var t = _views[i];
			_views.Remove(i);
			_pool.Exit(t.gameObject);
		}
		private IEnumerator Face(LookAt l)
		{
			yield return new WaitForFixedUpdate();
			l.Face();
		}
		protected float GetPercent(float i)
		{
			return i / _dataMax;
		}
		protected void Scroll(float percent)
		{
			Stop();
			DragTo(Mathf.RoundToInt(percent * _dataMax));
		}
		public void EaseTo(Transform t)
		{
			EaseTo(_views.SingleOrDefault(x => x.Value == t).Key);
		}
		private void EaseTo(int to)
		{
			if (_ease != null)
				StopCoroutine(_ease);
			_ease = Ease.Go(this, _current, to * -_offset.x, _time, DragTo, null, EaseType.Spring);
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
			var back = current < old;
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
						Enter(i);
				}
				else
				{
					if (!visible)
						Exit(i);
				}
				_views.TryGetValue(i, out t);
				if (t != null)
					UpdateItem(t.gameObject, x);
			}
			gameObject.SortChildren();
			UpdateCurrent(GetCurrent());
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
			var group = o.GetComponent<CanvasGroup>();
			if (group != null)
				group.alpha = 1 - axn;
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
			if (Mathf.Approximately(_inertia, 0f))
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
			var time = Mathf.Clamp(Mathf.Abs(_inertia * .1f), 0f, _inertiaLimit);
			_inertiaDecayEase = Ease.Go(this, _inertia, 0f, time, i => _inertia = i);
		}
		public void OnEndDrag(PointerEventData e)
		{
			if (Mathf.Abs(_inertia) > .0333f)
			{
				var time = Mathf.Clamp(Mathf.Abs(_inertia * .1f), 0f, _inertiaLimit);
				_inertiaEase = Ease.Go(this, -_inertia, 0f, time, Drag, Snap, EaseType.SineOut);
			}
			else
				Snap();
		}
		private void Snap()
		{
			Stop();
			EaseTo(GetCurrent());
		}
		public void Prev()
		{
			Stop();
			EaseTo(GetCurrent() - 1);
		}
		public void Next()
		{
			Stop();
			EaseTo(GetCurrent() + 1);
		}
	}
}
