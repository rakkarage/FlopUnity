using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	// http://www.robertpenner.com/easing/
	public enum EaseType
	{
		Linear,
		Spring,
		SineIn, SineOut, SineInOut,
		QuadIn, QuadOut, QuadInOut,
		CubicIn, CubicOut, CubicInOut,
		QuartIn, QuartOut, QuartInOut,
		QuintIn, QuintOut, QuintInOut,
		ExpoIn, ExpoOut, ExpoInOut,
		CircIn, CircOut, CircInOut,
		BounceIn, BounceOut, BounceInOut
	}
	public static class Ease
	{
		private delegate float Handler(float start, float end, float time);
		private readonly static Dictionary<EaseType, Handler> Types = new Dictionary<EaseType, Handler>
		{
			{EaseType.Linear, Mathf.Lerp},
			{EaseType.Spring, Spring},
			{EaseType.SineIn, SineIn},
			{EaseType.SineOut, SineOut},
			{EaseType.SineInOut, SineInOut},
			{EaseType.QuadIn, QuadIn},
			{EaseType.QuadOut, QuadOut},
			{EaseType.QuadInOut, QuadInOut},
			{EaseType.CubicIn, CubicIn},
			{EaseType.CubicOut, CubicOut},
			{EaseType.CubicInOut, CubicInOut},
			{EaseType.QuartIn, QuartIn},
			{EaseType.QuartOut, QuartOut},
			{EaseType.QuartInOut, QuartInOut},
			{EaseType.QuintIn, QuintIn},
			{EaseType.QuintOut, QuintOut},
			{EaseType.QuintInOut, QuintInOut},
			{EaseType.ExpoIn, ExpoIn},
			{EaseType.ExpoOut, ExpoOut},
			{EaseType.ExpoInOut, ExpoInOut},
			{EaseType.CircIn, CircIn},
			{EaseType.CircOut, CircOut},
			{EaseType.CircInOut, CircInOut},
			{EaseType.BounceIn, BounceIn},
			{EaseType.BounceOut, BounceOut},
			{EaseType.BounceInOut, BounceInOut}
		};
		public static IEnumerator Go(MonoBehaviour m, float start, float end, float time, UnityAction<float> update, UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f)
		{
			var i = GoCoroutine(start, end, time, update, complete, type, delay);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoCoroutine(float start, float end, float time, UnityAction<float> update, UnityAction complete, EaseType type, float delay)
		{
			if (delay > 0f)
				yield return new WaitForSeconds(delay);
			var i = 0f;
			while (i <= 1f)
			{
				i += Time.deltaTime / time;
				update(Types[type](start, end, Mathf.Clamp01(i)));
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
			return Mathf.Lerp(start, end, 1f - Mathf.Cos(time * Constants.HalfPI));
		}
		public static float SineOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, Mathf.Sin(time * Constants.HalfPI));
		}
		public static float SineInOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, .5f * (1f - Mathf.Cos(Mathf.PI * time)));
		}
		public static float QuadIn(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, time * time);
		}
		public static float QuadOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, -time * (time - 2f));
		}
		public static float QuadInOut(float start, float end, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(start, end, .5f * time * time);
			return Mathf.Lerp(start, end, -.5f * (((--time) * (time - 2f) - 1f)));
		}
		public static float CubicIn(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, time * time * time);
		}
		public static float CubicOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, (time -= 1f) * time * time + 1f);
		}
		public static float CubicInOut(float start, float end, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(start, end, .5f * time * time * time);
			return Mathf.Lerp(start, end, .5f * ((time -= 2) * time * time + 2f));
		}
		public static float QuartIn(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, time * time * time * time);
		}
		public static float QuartOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, (time -= 1f) * time * time * time - 1f);
		}
		public static float QuartInOut(float start, float end, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(start, end, .5f * time * time * time * time);
			return Mathf.Lerp(start, end, -.5f * ((time -= 2f) * time * time * time - 2f));
		}
		public static float QuintIn(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, time * time * time * time * time);
		}
		public static float QuintOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, (time -= 1f) * time * time * time * time + 1f);
		}
		public static float QuintInOut(float start, float end, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(start, end, .5f * time * time * time * time * time);
			return Mathf.Lerp(start, end, .5f * ((time -= 2f) * time * time * time * time + 2f));
		}
		public static float ExpoIn(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, Mathf.Pow(2f, 10f * (time - 1f)));
		}
		public static float ExpoOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, -Mathf.Pow(2f, -10f * time) + 1f);
		}
		public static float ExpoInOut(float start, float end, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(start, end, .5f * Mathf.Pow(2f, 10f * (time - 1f)));
			return Mathf.Lerp(start, end, .5f * (-Mathf.Pow(2f, -10f * --time) + 2f));
		}
		public static float CircIn(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, -(Mathf.Sqrt(1f - time * time) - 1f));
		}
		public static float CircOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, Mathf.Sqrt(1f - (time -= 1f) * time));
		}
		public static float CircInOut(float start, float end, float time)
		{
			if ((time /= .5f) < 1f)
				return Mathf.Lerp(start, end, -.5f * (Mathf.Sqrt(1 - time * time) - 1));
			return Mathf.Lerp(start, end, .5f * (Mathf.Sqrt(1 - (time -= 2) * time) + 1));
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
		private delegate Vector3 Handler(Vector3 start, Vector3 end, float time);
		private readonly static Dictionary<EaseType, Handler> Types = new Dictionary<EaseType, Handler>
		{
			{EaseType.Linear, Vector3.Lerp},
			{EaseType.Spring, (start, end, time) => new Vector3(Ease.Spring(start.x, end.x, time), Ease.Spring(start.y, end.y, time), Ease.Spring(start.z, end.z, time))},
			{EaseType.SineIn, (start, end, time) => new Vector3(Ease.SineIn(start.x, end.x, time), Ease.SineIn(start.y, end.y, time), Ease.SineIn(start.z, end.z, time))},
			{EaseType.SineOut, (start, end, time) => new Vector3(Ease.SineOut(start.x, end.x, time), Ease.SineOut(start.y, end.y, time), Ease.SineOut(start.z, end.z, time))},
			{EaseType.SineInOut, (start, end, time) => new Vector3(Ease.SineInOut(start.x, end.x, time), Ease.SineInOut(start.y, end.y, time), Ease.SineInOut(start.z, end.z, time))},
			{EaseType.QuadIn, (start, end, time) => new Vector3(Ease.QuadIn(start.x, end.x, time), Ease.QuadIn(start.y, end.y, time), Ease.QuadIn(start.z, end.z, time))},
			{EaseType.QuadOut, (start, end, time) => new Vector3(Ease.QuadOut(start.x, end.x, time), Ease.QuadOut(start.y, end.y, time), Ease.QuadOut(start.z, end.z, time))},
			{EaseType.QuadInOut, (start, end, time) => new Vector3(Ease.QuadInOut(start.x, end.x, time), Ease.QuadInOut(start.y, end.y, time), Ease.QuadInOut(start.z, end.z, time))},
			{EaseType.CubicIn, (start, end, time) => new Vector3(Ease.CubicIn(start.x, end.x, time), Ease.CubicIn(start.y, end.y, time), Ease.CubicIn(start.z, end.z, time))},
			{EaseType.CubicOut, (start, end, time) => new Vector3(Ease.CubicOut(start.x, end.x, time), Ease.CubicOut(start.y, end.y, time), Ease.CubicOut(start.z, end.z, time))},
			{EaseType.CubicInOut, (start, end, time) => new Vector3(Ease.CubicInOut(start.x, end.x, time), Ease.CubicInOut(start.y, end.y, time), Ease.CubicInOut(start.z, end.z, time))},
			{EaseType.QuartIn, (start, end, time) => new Vector3(Ease.QuartIn(start.x, end.x, time), Ease.QuartIn(start.y, end.y, time), Ease.QuartIn(start.z, end.z, time))},
			{EaseType.QuartOut, (start, end, time) => new Vector3(Ease.QuartOut(start.x, end.x, time), Ease.QuartOut(start.y, end.y, time), Ease.QuartOut(start.z, end.z, time))},
			{EaseType.QuartInOut, (start, end, time) => new Vector3(Ease.QuartInOut(start.x, end.x, time), Ease.QuartInOut(start.y, end.y, time), Ease.QuartInOut(start.z, end.z, time))},
			{EaseType.QuintIn, (start, end, time) => new Vector3(Ease.QuintIn(start.x, end.x, time), Ease.QuintIn(start.y, end.y, time), Ease.QuintIn(start.z, end.z, time))},
			{EaseType.QuintOut, (start, end, time) => new Vector3(Ease.QuintOut(start.x, end.x, time), Ease.QuintOut(start.y, end.y, time), Ease.QuintOut(start.z, end.z, time))},
			{EaseType.QuintInOut, (start, end, time) => new Vector3(Ease.QuintInOut(start.x, end.x, time), Ease.QuintInOut(start.y, end.y, time), Ease.QuintInOut(start.z, end.z, time))},
			{EaseType.ExpoIn, (start, end, time) => new Vector3(Ease.ExpoIn(start.x, end.x, time), Ease.ExpoIn(start.y, end.y, time), Ease.ExpoIn(start.z, end.z, time))},
			{EaseType.ExpoOut, (start, end, time) => new Vector3(Ease.ExpoOut(start.x, end.x, time), Ease.ExpoOut(start.y, end.y, time), Ease.ExpoOut(start.z, end.z, time))},
			{EaseType.ExpoInOut, (start, end, time) => new Vector3(Ease.ExpoInOut(start.x, end.x, time), Ease.ExpoInOut(start.y, end.y, time), Ease.ExpoInOut(start.z, end.z, time))},
			{EaseType.CircIn, (start, end, time) => new Vector3(Ease.CircIn(start.x, end.x, time), Ease.CircIn(start.y, end.y, time), Ease.CircIn(start.z, end.z, time))},
			{EaseType.CircOut, (start, end, time) => new Vector3(Ease.CircOut(start.x, end.x, time), Ease.CircOut(start.y, end.y, time), Ease.CircOut(start.z, end.z, time))},
			{EaseType.CircInOut, (start, end, time) => new Vector3(Ease.CircInOut(start.x, end.x, time), Ease.CircInOut(start.y, end.y, time), Ease.CircInOut(start.z, end.z, time))},
			{EaseType.BounceIn, (start, end, time) => new Vector3(Ease.BounceIn(start.x, end.x, time), Ease.BounceIn(start.y, end.y, time), Ease.BounceIn(start.z, end.z, time))},
			{EaseType.BounceOut, (start, end, time) => new Vector3(Ease.BounceOut(start.x, end.x, time), Ease.BounceOut(start.y, end.y, time), Ease.BounceOut(start.z, end.z, time))},
			{EaseType.BounceInOut, (start, end, time) => new Vector3(Ease.BounceInOut(start.x, end.x, time), Ease.BounceInOut(start.y, end.y, time), Ease.BounceInOut(start.z, end.z, time))}
		};
		public static IEnumerator Go(MonoBehaviour m, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update, UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f)
		{
			var i = GoCoroutine(start, end, time, update, complete, type, delay);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoCoroutine(Vector3 start, Vector3 end, float time, UnityAction<Vector3> update, UnityAction complete, EaseType type, float delay)
		{
			if (delay > 0f)
				yield return new WaitForSeconds(delay);
			var i = 0f;
			while (i <= 1f)
			{
				i += Time.deltaTime / time;
				update(Types[type](start, end, Mathf.Clamp01(i)));
				yield return null;
			}
			if (complete != null)
				complete();
		}
		public static IEnumerator GoPosition(MonoBehaviour m, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update = null, UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f)
		{
			return GoPosition(m, m.gameObject, start, end, time, update, complete, type, delay);
		}
		public static IEnumerator GoPosition(MonoBehaviour m, GameObject o, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update, UnityAction complete, EaseType type = EaseType.Linear, float delay = 0f)
		{
			var i = GoPositionCoroutine(o, start, end, time, update, complete, type, delay);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoPositionCoroutine(GameObject o, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update, UnityAction complete, EaseType type, float delay)
		{
			if (delay > 0f)
				yield return new WaitForSeconds(delay);
			var i = 0f;
			while (i <= 1f)
			{
				i += Time.deltaTime / time;
				var p = Types[type](start, end, Mathf.Clamp01(i));
				o.transform.localPosition = p;
				if (update != null)
					update(p);
				yield return null;
			}
			o.transform.localPosition = end;
			if (complete != null)
				complete();
		}
		public static IEnumerator GoRotation(MonoBehaviour m, Vector3 angle, float time, UnityAction<Vector3> update = null, UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f)
		{
			return GoRotation(m, m.gameObject, angle, time, update, complete, type, delay);
		}
		public static IEnumerator GoRotation(MonoBehaviour m, GameObject o, Vector3 angle, float time, UnityAction<Vector3> update, UnityAction complete, EaseType type = EaseType.Linear, float delay = 0f)
		{
			var i = GoRotationCoroutine(o, angle, time, update, complete, type, delay);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoRotationCoroutine(GameObject o, Vector3 angle, float time, UnityAction<Vector3> update, UnityAction complete, EaseType type, float delay)
		{
			if (delay > 0f)
				yield return new WaitForSeconds(delay);
			var i = 0f;
			while (i <= 1f)
			{
				i += Time.deltaTime / time;
				var p = Types[type](Vector3.zero, angle, Mathf.Clamp01(i));
				o.transform.localEulerAngles = p;
				if (update != null)
					update(p);
				yield return null;
			}
			o.transform.localEulerAngles = Types[type](Vector3.zero, angle, 1f);
			if (complete != null)
				complete();
		}
		public static IEnumerator GoScale(MonoBehaviour m, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update = null, UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f)
		{
			return GoScale(m, m.gameObject, start, end, time, update, complete, type, delay);
		}
		public static IEnumerator GoScale(MonoBehaviour m, GameObject o, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update, UnityAction complete, EaseType type = EaseType.Linear, float delay = 0f)
		{
			var i = GoScaleCoroutine(o, start, end, time, update, complete, type, delay);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoScaleCoroutine(GameObject o, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update, UnityAction complete, EaseType type, float delay)
		{
			if (delay > 0f)
				yield return new WaitForSeconds(delay);
			var i = 0f;
			while (i <= 1f)
			{
				i += Time.deltaTime / time;
				var p = Types[type](start, end, Mathf.Clamp01(i));
				o.transform.localScale = p;
				if (update != null)
					update(p);
				yield return null;
			}
			o.transform.localScale = end;
			if (complete != null)
				complete();
		}
	}
}
