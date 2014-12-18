using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	public abstract class Easer : MonoBehaviour
	{
		[SerializeField]
		protected bool _start = true;
		[SerializeField]
		protected bool _by = true;
		[SerializeField]
		protected Vector3 _value = Vector3.zero;
		[SerializeField]
		protected EaseType _type = EaseType.Linear;
		[SerializeField]
		protected float _time = 1f;
		[SerializeField]
		protected float _delay = 0f;
		[SerializeField, Range(-1, 100), Tooltip("< 0 : Infinite, 0 : PingPong, > 0 : Repeat")]
		protected int _repeat = 1;
		[SerializeField]
		protected UnityEvent Complete;
		private void Start()
		{
			if (_start) Go();
		}
		public abstract void Go();
	}
}
