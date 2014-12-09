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
		public static int maxThreads = 8;
		private static Loom _current;
		private static bool initialized;
		private static int numThreads;
		private List<Action> _actions = new List<Action>();
		private int _count;
		private List<Action> _currentActions = new List<Action>();
		private List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();
		private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
		public static Loom Current
		{
			get
			{
				Initialize();
				return _current;
			}
		}
		public static void QueueOnMainThread(Action action)
		{
			QueueOnMainThread(action, 0f);
		}
		public static void QueueOnMainThread(Action action, float time)
		{
			if (time != 0)
			{
				lock (Current._delayed)
				{
					Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
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
			while (numThreads >= maxThreads)
			{
				Thread.Sleep(1);
			}
			Interlocked.Increment(ref numThreads);
			ThreadPool.QueueUserWorkItem(RunAction, a);
			return null;
		}
		private static void Initialize()
		{
			if (!initialized)
			{
				if (!Application.isPlaying)
					return;
				initialized = true;
				var g = new GameObject("Loom");
				_current = g.AddComponent<Loom>();
			}
		}
		private static void RunAction(object action)
		{
			try
			{
				((Action)action)();
			}
			catch
			{
			}
			finally
			{
				Interlocked.Decrement(ref numThreads);
			}
		}
		private void Awake()
		{
			_current = this;
			initialized = true;
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
				_currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
				foreach (var item in _currentDelayed)
					_delayed.Remove(item);
			}
			foreach (var delayed in _currentDelayed)
			{
				delayed.action();
			}
		}
		public struct DelayedQueueItem
		{
			public Action action;
			public float time;
		}
	}
}
