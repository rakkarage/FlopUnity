using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	public enum EaseType { Linear, Spring, SineIn, SineOut, SineInOut, BounceIn, BounceOut, BounceInOut }
	public static class Ease
	{
		private delegate float EaseHandler(float start, float end, float time);
		private readonly static Dictionary<EaseType, EaseHandler> _types = new Dictionary<EaseType, EaseHandler>
		{
			{EaseType.Linear, Mathf.Lerp},
			{EaseType.Spring, Spring},
			{EaseType.SineIn, SineIn},
			{EaseType.SineOut, SineOut},
			{EaseType.SineInOut, SineInOut},
			{EaseType.BounceIn, BounceIn},
			{EaseType.BounceOut, BounceOut},
			{EaseType.BounceInOut, BounceInOut}
		};
		public static IEnumerator Go(MonoBehaviour m, float start, float end, float time, float delay, EaseType type, UnityAction<float> update, UnityAction complete)
		{
			IEnumerator i = GoCoroutine(start, end, time, delay, type, update, complete);
			m.StartCoroutine(i);
			return i;
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
		public static float Spring(float start, float end, float time)
		{
			time = Mathf.Clamp01(time);
			time = (Mathf.Sin(time * Mathf.PI * (.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + (1.2f * (1f - time)));
			return start + (end - start) * time;
		}
		public static float SineIn(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, 1f - Mathf.Cos(time * Mathf.PI * .5f));
		}
		public static float SineOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, Mathf.Sin(time * Mathf.PI * .5f));
		}
		public static float SineInOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, time * time * (3f - 2f * time));
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
		private delegate Vector3 EaseHandler(Vector3 start, Vector3 end, float time);
		private readonly static Dictionary<EaseType, EaseHandler> _types = new Dictionary<EaseType, EaseHandler>
		{
			{EaseType.Linear, Vector3.Lerp},
			{EaseType.Spring, Spring},
			{EaseType.SineIn, SineIn},
			{EaseType.SineOut, SineOut},
			{EaseType.SineInOut, SineInOut},
			{EaseType.BounceIn, BounceIn},
			{EaseType.BounceOut, BounceOut},
			{EaseType.BounceInOut, BounceInOut}
		};
		public static IEnumerator Go(MonoBehaviour m, Vector3 start, Vector3 end, float time, float delay, EaseType type, UnityAction<Vector3> update, UnityAction complete)
		{
			IEnumerator i = GoCoroutine(start, end, time, delay, type, update, complete);
			m.StartCoroutine(i);
			return i;
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
		public static IEnumerator GoPosition(MonoBehaviour m, Vector3 start, Vector3 end, float time, float delay, EaseType type)
		{
			return GoPosition(m, m.gameObject, start, end, time, delay, type);
		}
		public static IEnumerator GoPosition(MonoBehaviour m, GameObject o, Vector3 start, Vector3 end, float time, float delay, EaseType type)
		{
			IEnumerator i = GoPositionCoroutine(o, start, end, time, delay, type);
			m.StartCoroutine(i);
			return i;
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
		public static IEnumerator GoRotation(MonoBehaviour m, Vector3 angle, float time, float delay, EaseType type)
		{
			return GoRotation(m, m.gameObject, angle, time, delay, type);
		}
		public static IEnumerator GoRotation(MonoBehaviour m, GameObject o, Vector3 angle, float time, float delay, EaseType type)
		{
			IEnumerator i = GoRotationCoroutine(o, angle, time, delay, type);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoRotationCoroutine(GameObject o, Vector3 angle, float time, float delay, EaseType type)
		{
			if (delay > 0f)
				yield return new WaitForSeconds(delay);
			var i = 0f;
			while (i <= 1f)
			{
				i += Time.deltaTime / time;
				o.transform.localEulerAngles = _types[type](Vector3.zero, angle, i);
				yield return null;
			}
			o.transform.localEulerAngles = _types[type](Vector3.zero, angle, 1f);
		}
		public static IEnumerator GoScale(MonoBehaviour m, Vector3 start, Vector3 end, float time, float delay, EaseType type)
		{
			return GoScale(m, m.gameObject, start, end, time, delay, type);
		}
		public static IEnumerator GoScale(MonoBehaviour m, GameObject o, Vector3 start, Vector3 end, float time, float delay, EaseType type)
		{
			IEnumerator i = GoScaleCoroutine(o, start, end, time, delay, type);
			m.StartCoroutine(i);
			return i;
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
		public static Vector3 Spring(Vector3 start, Vector3 end, float time)
		{
			return new Vector3(Ease.Spring(start.x, end.x, time), Ease.Spring(start.y, end.y, time), Ease.Spring(start.z, end.z, time));
		}
		public static Vector3 SineIn(Vector3 start, Vector3 end, float time)
		{
			return new Vector3(Ease.SineIn(start.x, end.x, time), Ease.SineIn(start.y, end.y, time), Ease.SineIn(start.z, end.z, time));
		}
		public static Vector3 SineOut(Vector3 start, Vector3 end, float time)
		{
			return new Vector3(Ease.SineOut(start.x, end.x, time), Ease.SineOut(start.y, end.y, time), Ease.SineOut(start.z, end.z, time));
		}
		public static Vector3 SineInOut(Vector3 start, Vector3 end, float time)
		{
			return new Vector3(Ease.SineInOut(start.x, end.x, time), Ease.SineInOut(start.y, end.y, time), Ease.SineInOut(start.z, end.z, time));
		}
		public static Vector3 BounceIn(Vector3 start, Vector3 end, float time)
		{
			return new Vector3(Ease.BounceIn(start.x, end.x, time), Ease.BounceIn(start.y, end.y, time), Ease.BounceIn(start.z, end.z, time));
		}
		public static Vector3 BounceOut(Vector3 start, Vector3 end, float time)
		{
			return new Vector3(Ease.BounceOut(start.x, end.x, time), Ease.BounceOut(start.y, end.y, time), Ease.BounceOut(start.z, end.z, time));
		}
		public static Vector3 BounceInOut(Vector3 start, Vector3 end, float time)
		{
			return new Vector3(Ease.BounceInOut(start.x, end.x, time), Ease.BounceInOut(start.y, end.y, time), Ease.BounceInOut(start.z, end.z, time));
		}
	}
}
