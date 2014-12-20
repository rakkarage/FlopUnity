using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	// http://answers.unity3d.com/questions/305882/how-do-i-invoke-functions-on-the-main-thread.html
	public class Loom : MonoBehaviour
	{
		public static int MaxThreads = 8;
		private static Loom _current;
		private static bool _initialized;
		private static int _numThreads;
		private readonly List<Action> _actions = new List<Action>();
		private int _count;
		private readonly List<Action> _currentActions = new List<Action>();
		private readonly List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();
		private readonly List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
		public static Loom Current
		{
			get
			{
				Initialize();
				return _current;
			}
		}
		public static void QueueOnMainThread(Action action, float time = 0f)
		{
			if (Math.Abs(time) > 0.001f)
			{
				lock (Current._delayed)
				{
					Current._delayed.Add(new DelayedQueueItem { Time = Time.time + time, Action = action });
				}
			}
			else
			{
				lock (Current._actions)
				{
					Current._actions.Add(action);
				}
			}
		}
		public static Thread RunAsync(Action a)
		{
			Initialize();
			while (_numThreads >= MaxThreads)
			{
				Thread.Sleep(1);
			}
			Interlocked.Increment(ref _numThreads);
			ThreadPool.QueueUserWorkItem(RunAction, a);
			return null;
		}
		private static void Initialize()
		{
			if (_initialized) return;
			if (!Application.isPlaying) return;
			_initialized = true;
			var g = new GameObject("Loom");
			_current = g.AddComponent<Loom>();
		}
		private static void RunAction(object action)
		{
			try
			{
				((Action)action)();
			}
			catch
			{
				// ignored
			}
			finally
			{
				Interlocked.Decrement(ref _numThreads);
			}
		}
		private void Awake()
		{
			_current = this;
			_initialized = true;
		}
		private void OnDisable()
		{
			if (_current == this)
			{
				_current = null;
			}
		}
		private void Update()
		{
			lock (_actions)
			{
				_currentActions.Clear();
				_currentActions.AddRange(_actions);
				_actions.Clear();
			}
			foreach (var a in _currentActions)
			{
				a();
			}
			lock (_delayed)
			{
				_currentDelayed.Clear();
				_currentDelayed.AddRange(_delayed.Where(d => d.Time <= Time.time));
				foreach (var item in _currentDelayed)
					_delayed.Remove(item);
			}
			foreach (var delayed in _currentDelayed)
			{
				delayed.Action();
			}
		}
		private struct DelayedQueueItem
		{
			public Action Action;
			public float Time;
		}
	}
}
