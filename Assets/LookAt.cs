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
	protected void LateUpdate()
	{
		var r = Quaternion.LookRotation(_t.position - Target.position);
		_t.localRotation = Quaternion.Slerp(_t.localRotation, r, Time.deltaTime * Damping);
	}
}
