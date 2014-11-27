using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum EaseType
{
	Linear,
	Hermite,
	Sinerp,
	Coserp,
	Spring,
	BounceIn,
	BounceOut,
	BounceInOut
}
public static class Ease
{
	private delegate float EaseHandler(float start, float end, float t);
	private static Dictionary<EaseType, EaseHandler> _types = new Dictionary<EaseType, EaseHandler>
	{
		{EaseType.Linear, Mathf.Lerp},
		{EaseType.Hermite, Hermite},
		{EaseType.Sinerp, Sinerp},
		{EaseType.Coserp, Coserp},
		{EaseType.Spring, Spring},
		{EaseType.BounceIn, BounceIn},
		{EaseType.BounceOut, BounceOut},
		{EaseType.BounceInOut, BounceInOut}
	};
	public static void Go(MonoBehaviour m, float start, float end, float time, float delay, EaseType type, UnityAction<float> update, UnityAction complete)
	{
		m.StartCoroutine(GoCoroutine(start, end, time, delay, type, update, complete));
	}
	private static IEnumerator GoCoroutine(float start, float end, float time, float delay, EaseType type, UnityAction<float> update, UnityAction complete)
	{
		if (delay > 0f)
		yield return new WaitForSeconds(delay);
		var i = 0f;
		while (i <= 1f)
		{
			i += Time.deltaTime / time;
			update(_types[type](start, end, i));
			yield return null;
		}
		if (complete != null)
		complete();
	}
	private static float Hermite(float start, float end, float time)
	{
		return Mathf.Lerp(start, end, time * time * (3f - 2f * time));
	}
	private static float Sinerp(float start, float end, float time)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(time * Mathf.PI * .5f));
	}
	private static float Coserp(float start, float end, float time)
	{
		return Mathf.Lerp(start, end, 1f - Mathf.Cos(time * Mathf.PI * .5f));
	}
	private static float Spring(float start, float end, float time)
	{
		time = Mathf.Clamp01(time);
		time = (Mathf.Sin(time * Mathf.PI * (.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + (1.2f * (1f - time)));
		return start + (end - start) * time;
	}
	public static float BounceIn(float start, float end, float time)
	{
		end -= start;
		return end - BounceOut(0f, end, 1f - time) + start;
	}
	public static float BounceOut(float start, float end, float time)
	{
		time /= 1f;
		end -= start;
		if (time < (1f / 2.75f))
		return end * (7.5625f * time * time) + start;
		if (time < (2f / 2.75f))
		return end * (7.5625f * (time -= (1.5f / 2.75f)) * time + .75f) + start;
		if (time < (2.5f / 2.75f))
		return end * (7.5625f * (time -= (2.25f / 2.75f)) * time + .9375f) + start;
		return end * (7.5625f * (time -= (2.625f / 2.75f)) * time + .984375f) + start;
	}
	public static float BounceInOut(float start, float end, float time)
	{
		end -= start;
		if (time < .5f)
		return BounceIn(0f, end, time * 2f) * .5f + start;
		return BounceOut(0f, end, time * 2f - 1f) * .5f + end * .5f + start;
	}
}
public static class Ease3
{
	private delegate Vector3 EaseHandler(Vector3 start, Vector3 end, float t);
	private static Dictionary<EaseType, EaseHandler> _types = new Dictionary<EaseType, EaseHandler>
	{
		{EaseType.Linear, Vector3.Lerp},
		{EaseType.Hermite, Hermite},
		{EaseType.Sinerp, Sinerp},
		{EaseType.Coserp, Coserp},
		{EaseType.Spring, Spring},
		{EaseType.BounceIn, BounceIn},
		{EaseType.BounceOut, BounceOut},
		{EaseType.BounceInOut, BounceInOut}
	};
	public static void Go(MonoBehaviour m, Vector3 start, Vector3 end, float time, float delay, EaseType type, UnityAction<Vector3> update, UnityAction complete)
	{
		m.StartCoroutine(GoCoroutine(start, end, time, delay, type, update, complete));
	}
	private static IEnumerator GoCoroutine(Vector3 start, Vector3 end, float time, float delay, EaseType type, UnityAction<Vector3> update, UnityAction complete)
	{
		if (delay > 0f)
		yield return new WaitForSeconds(delay);
		var i = 0f;
		while (i <= 1f)
		{
			i += Time.deltaTime / time;
			update(_types[type](start, end, i));
			yield return null;
		}
		if (complete != null)
		complete();
	}
	public static void GoPosition(MonoBehaviour m, GameObject o, Vector3 start, Vector3 end, float time, float delay, EaseType type)
	{
		m.StartCoroutine(GoPositionCoroutine(o, start, end, time, delay, type));
	}
	private static IEnumerator GoPositionCoroutine(GameObject o, Vector3 start, Vector3 end, float time, float delay, EaseType type)
	{
		if (delay > 0f)
		yield return new WaitForSeconds(delay);
		var i = 0f;
		while (i <= 1f)
		{
			i += Time.deltaTime / time;
			o.transform.localPosition = _types[type](start, end, i);
			yield return null;
		}
		o.transform.localPosition = end;
	}
	public static void GoRotation(MonoBehaviour m, GameObject o, Vector3 start, Vector3 end, float time, float delay, EaseType type)
	{
		m.StartCoroutine(GoRotationCoroutine(o, start, end, time, delay, type));
	}
	private static IEnumerator GoRotationCoroutine(GameObject o, Vector3 start, Vector3 end, float time, float delay, EaseType type)
	{
		if (delay > 0f)
		yield return new WaitForSeconds(delay);
		var i = 0f;
		while (i <= 1f)
		{
			i += Time.deltaTime / time;
			o.transform.localRotation = Quaternion.Euler(_types[type](start, end, i));
			yield return null;
		}
		o.transform.localRotation = Quaternion.Euler(end);
	}
	public static void GoScale(MonoBehaviour m, GameObject o, Vector3 start, Vector3 end, float time, float delay, EaseType type)
	{
		m.StartCoroutine(GoScaleCoroutine(o, start, end, time, delay, type));
	}
	private static IEnumerator GoScaleCoroutine(GameObject o, Vector3 start, Vector3 end, float time, float delay, EaseType type)
	{
		if (delay > 0f)
		yield return new WaitForSeconds(delay);
		var i = 0f;
		while (i <= 1f)
		{
			i += Time.deltaTime / time;
			o.transform.localScale = _types[type](start, end, i);
			yield return null;
		}
		o.transform.localScale = end;
	}
	private static Vector3 Hermite(Vector3 start, Vector3 end, float time)
	{
		return Vector3.Lerp(start, end, time * time * (3f - 2f * time));
	}
	private static Vector3 Sinerp(Vector3 start, Vector3 end, float time)
	{
		return Vector3.Lerp(start, end, Mathf.Sin(time * Mathf.PI * .5f));
	}
	private static Vector3 Coserp(Vector3 start, Vector3 end, float time)
	{
		return Vector3.Lerp(start, end, 1f - Mathf.Cos(time * Mathf.PI * .5f));
	}
	private static Vector3 Spring(Vector3 start, Vector3 end, float time)
	{
		time = Mathf.Clamp01(time);
		time = (Mathf.Sin(time * Mathf.PI * (.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + (1.2f * (1f - time)));
		return start + (end - start) * time;
	}
	public static Vector3 BounceIn(Vector3 start, Vector3 end, float time)
	{
		end -= start;
		return end - BounceOut(Vector3.zero, end, 1f - time) + start;
	}
	public static Vector3 BounceOut(Vector3 start, Vector3 end, float time)
	{
		time /= 1f;
		end -= start;
		if (time < (1f / 2.75f))
		return end * (7.5625f * time * time) + start;
		if (time < (2f / 2.75f))
		return end * (7.5625f * (time -= (1.5f / 2.75f)) * time + .75f) + start;
		if (time < (2.5f / 2.75f))
		return end * (7.5625f * (time -= (2.25f / 2.75f)) * time + .9375f) + start;
		return end * (7.5625f * (time -= (2.625f / 2.75f)) * time + .984375f) + start;
	}
	public static Vector3 BounceInOut(Vector3 start, Vector3 end, float time)
	{
		end -= start;
		if (time < .5f)
		return BounceIn(Vector3.zero, end, time * 2f) * .5f + start;
		return BounceOut(Vector3.zero, end, time * 2f - 1f) * .5f + end * .5f + start;
	}
}
