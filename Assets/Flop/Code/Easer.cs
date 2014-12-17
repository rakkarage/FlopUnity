using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Easer : MonoBehaviour
	{
		public EaseType Type;
		private Vector3 _start;
		private Vector3 _end;
		private const float Time = 1f;
		private const float Delay = .333f;
		private const float Offset = 132f;
		private Transform _t;
		private void Awake()
		{
			_t = transform;
		}
		private void Start()
		{
			_start = new Vector3(-Offset, _t.localPosition.y, _t.localPosition.z);
			_end = new Vector3(Offset, _t.localPosition.y, _t.localPosition.z);
			_t.localPosition = _start;
			Fore();
		}
		private void Fore()
		{
			Ease3.GoPosition(this, _start, _end, Time, null, Back, Type, Delay);
		}
		private void Back()
		{
			Ease3.GoPosition(this, _end, _start, Time, null, Fore, Type, Delay);
		}
	}
}
