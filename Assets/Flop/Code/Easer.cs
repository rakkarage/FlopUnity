using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Easer : MonoBehaviour
	{
		public EaseType Type;
		private const float Time = 1f;
		private const float Delay = .333f;
		private Transform _t;
		private void Awake()
		{
			_t = transform;
		}
		private void Start()
		{
			_t.localPosition = new Vector3(-64f, _t.localPosition.y, _t.localPosition.z);
			Fore();
		}
		private void Fore()
		{
			var start = new Vector3(-64f, _t.localPosition.y, _t.localPosition.z);
			var end = new Vector3(64f, _t.localPosition.y, _t.localPosition.z);
			Ease3.GoPosition(this, start, end, Time, null, Back, Type, Delay);
		}
		private void Back()
		{
			var start = new Vector3(-64f, _t.localPosition.y, _t.localPosition.z);
			var end = new Vector3(64f, _t.localPosition.y, _t.localPosition.z);
			Ease3.GoPosition(this, end, start, Time, null, Fore, Type, Delay);
		}
	}
}
