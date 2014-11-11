using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public static class Ease
{
	public enum Type
	{
		Linear,
		Hermite,
		Sinerp,
		Coserp,
		Spring
	}
	private delegate float EaseHandler(float start, float end, float t);
	private static Dictionary<Type, EaseHandler> _types = new Dictionary<Type, EaseHandler>()
	{
		{Type.Linear, Mathf.Lerp},
		{Type.Hermite, Hermite},
		{Type.Sinerp, Sinerp},
		{Type.Coserp, Coserp},
		{Type.Spring, Spring}
	};
	public static void Go(MonoBehaviour m, float start, float end, float t, UnityAction<float, MonoBehaviour> update, UnityAction complete, Type type)
	{
		m.StopAllCoroutines();
		m.StartCoroutine(GoCoroutine(m, start, end, t, update, complete, type));
	}
	private static IEnumerator GoCoroutine(MonoBehaviour m, float start, float end, float t, UnityAction<float, MonoBehaviour> update, UnityAction complete, Type type)
	{
		var i = 0f;
		while (i <= 1f)
		{
			i += Time.deltaTime / t;
			update(_types[type](start, end, i), m);
			yield return null;
		}
		if (complete != null)
			complete();
	}
	private static float Hermite(float start, float end, float t)
	{
		return Mathf.Lerp(start, end, t * t * (3f - 2f * t));
	}
	private static float Sinerp(float start, float end, float t)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(t * Mathf.PI * .5f));
	}
	private static float Coserp(float start, float end, float t)
	{
		return Mathf.Lerp(start, end, 1f - Mathf.Cos(t * Mathf.PI * .5f));
	}
	private static float Spring(float start, float end, float t)
	{
		t = Mathf.Clamp01(t);
		t = (Mathf.Sin(t * Mathf.PI * (.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + (1.2f * (1f - t)));
		return start + (end - start) * t;
	}
}
public static class Ease3
{
		public enum Type
		{
				Linear,
				Hermite,
				Sinerp,
				Coserp,
				Spring
		}
		private delegate Vector3 EaseHandler(Vector3 start, Vector3 end, float t);
		private static Dictionary<Type, EaseHandler> _types = new Dictionary<Type, EaseHandler>()
		{
				{Type.Linear, Vector3.Lerp},
				{Type.Hermite, Hermite},
				{Type.Sinerp, Sinerp},
				{Type.Coserp, Coserp},
				{Type.Spring, Spring}
		};
		public static void Go(MonoBehaviour m, Vector3 start, Vector3 end, float t, Type type)
		{
				m.StopAllCoroutines();
				m.StartCoroutine(GoCoroutine(m, start, end, t, type));
		}
		private static IEnumerator GoCoroutine(MonoBehaviour m, Vector3 start, Vector3 end, float t, Type type)
		{
				var i = 0f;
				while (i <= 1f)
				{
						i += Time.deltaTime / t;
						m.transform.localPosition = _types[type](start, end, i);
						yield return null;
				}
		}
		private static Vector3 Hermite(Vector3 start, Vector3 end, float t)
		{
				return Vector3.Lerp(start, end, t * t * (3f - 2f * t));
		}
		private static Vector3 Sinerp(Vector3 start, Vector3 end, float t)
		{
				return Vector3.Lerp(start, end, Mathf.Sin(t * Mathf.PI * .5f));
		}
		private static Vector3 Coserp(Vector3 start, Vector3 end, float t)
		{
				return Vector3.Lerp(start, end, 1f - Mathf.Cos(t * Mathf.PI * .5f));
		}
		private static Vector3 Spring(Vector3 start, Vector3 end, float t)
		{
				t = Mathf.Clamp01(t);
				t = (Mathf.Sin(t * Mathf.PI * (.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + (1.2f * (1f - t)));
				return start + (end - start) * t;
		}
}
