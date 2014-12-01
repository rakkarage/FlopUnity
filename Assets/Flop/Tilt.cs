using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Tilt : MonoBehaviour
	{
		public Vector2 Range = new Vector2(1f, 1f);
		private Transform _t;
		private Quaternion _q;
		private Vector2 _r;
		private void Start()
		{
			_t = transform;
			_q = _t.localRotation;
		}
		private void Update()
		{
			var p = Input.mousePosition;
			var halfWidth = Screen.width * .5f;
			var halfHeight = Screen.height * .5f;
			var x = Mathf.Clamp((p.x - halfWidth) / halfWidth, -1f, 1f);
			var y = Mathf.Clamp((p.y - halfHeight) / halfHeight, -1f, 1f);
			_r = Vector2.Lerp(_r, new Vector2(x, y), Time.deltaTime * 5f);
			_t.localRotation = _q * Quaternion.Euler(-_r.y * Range.y, _r.x * Range.x, 0f);
		}
	}
}
