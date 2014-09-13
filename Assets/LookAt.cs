using UnityEngine;
public class LookAt : MonoBehaviour
{
	public float Damping = 3.33f;
	public Transform Target;
	private Transform _t;
	private void Start()
	{
		_t = transform;
	}
	private void OnEnable()
	{
		if (_t != null && Target != null)
			_t.localRotation = Quaternion.LookRotation(_t.localPosition - Target.localPosition);
	}
	private void LateUpdate()
	{
		var r = Quaternion.LookRotation(_t.localPosition - Target.localPosition);
		_t.localRotation = Quaternion.Slerp(_t.localRotation, r, Time.deltaTime * Damping);
	}
}
