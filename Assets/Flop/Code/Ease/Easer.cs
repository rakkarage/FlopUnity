using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	public abstract class Easer<T> : MonoBehaviour
	{
		[SerializeField]
		protected bool Begin = true;
		[SerializeField]
		protected bool By = true;
		[SerializeField]
		protected T Value;
		[SerializeField]
		protected EaseType Type = EaseType.Linear;
		[SerializeField]
		protected float Time = 1f;
		[SerializeField]
		protected float Delay = 0f;
		[SerializeField, Range(-1, 100), Tooltip("< 0 : Infinite, 0 : PingPong, > 0 : Repeat")]
		protected int Repeat = 1;
		[SerializeField]
		protected UnityEvent Complete;
		private void Start()
		{
			if (Begin) Go();
		}
		public abstract void Go();
	}
}
