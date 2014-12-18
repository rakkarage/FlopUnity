using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	public abstract class Easer : MonoBehaviour
	{
		[SerializeField]
		protected bool Begin = true;
		[SerializeField]
		protected bool By = true;
		[SerializeField]
		protected Vector3 Value = Vector3.zero;
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
