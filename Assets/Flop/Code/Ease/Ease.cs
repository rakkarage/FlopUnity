using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	// http://www.robertpenner.com/easing/
	public enum EaseType
	{
		Linear,
		SineIn, SineOut, SineInOut,
		QuadIn, QuadOut, QuadInOut,
		CubicIn, CubicOut, CubicInOut,
		QuartIn, QuartOut, QuartInOut,
		QuintIn, QuintOut, QuintInOut,
		ExpoIn, ExpoOut, ExpoInOut,
		CircIn, CircOut, CircInOut,
		BackIn, BackOut, BackInOut,
		ElasticIn, ElasticOut, ElasticInOut,
		BounceIn, BounceOut, BounceInOut,
		Spring
	}
	public static class Ease
	{
		private readonly static Dictionary<EaseType, Func<float, float, float, float>> Types = new Dictionary<EaseType, Func<float, float, float, float>>
		{
			{EaseType.Linear, Mathf.Lerp},
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
			{EaseType.BackIn, BackIn},
			{EaseType.BackOut, BackOut},
			{EaseType.BackInOut, BackInOut},
			{EaseType.ElasticIn, ElasticIn},
			{EaseType.ElasticOut, ElasticOut},
			{EaseType.ElasticInOut, ElasticInOut},
			{EaseType.BounceIn, BounceIn},
			{EaseType.BounceOut, BounceOut},
			{EaseType.BounceInOut, BounceInOut},
			{EaseType.Spring, Spring}
		};
		private const float HalfPi = Mathf.PI * .5f;
		private const float DoublePi = Mathf.PI * 2f;
        public static IEnumerator Go(MonoBehaviour m, float start, float end, float time, UnityAction<float> update,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			var i = GoCoroutine(start, end, time, update, complete, type, delay, repeat, pingPong);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoCoroutine(float start, float end, float time, UnityAction<float> update,
			UnityAction complete, EaseType type, float delay, int repeat, bool pingPong)
		{
			var counter = repeat;
			while (repeat == 0 || counter > 0)
			{
				if (delay > 0f)
					yield return new WaitForSeconds(delay);
				var t = 0f;
				while (t <= 1f)
				{
					t += Time.deltaTime / time;
					update(Types[type](start, end, Mathf.Clamp01(t)));
					yield return null;
				}
				if (pingPong)
				{
					if (delay > 0f)
						yield return new WaitForSeconds(delay);
					t = 0f;
					while (t <= 1f)
					{
						t += Time.deltaTime / time;
						update(Types[type](end, start, Mathf.Clamp01(t)));
						yield return null;
					}
				}
				if (repeat != 0)
					counter--;
				if (repeat == 0 && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}

		private static float GetAlpha(MonoBehaviour m)
		{
			var image = m.GetComponent<Image>();
			if (image != null)
				return image.color.a;
			var canvasGroup = m.GetComponent<CanvasGroup>();
			if (canvasGroup != null)
				return canvasGroup.alpha;
			return Camera.main.backgroundColor.a;
		}
		public static IEnumerator GoAlphaTo(MonoBehaviour m, float to, float time, UnityAction<float> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			return GoAlpha(m, GetAlpha(m), to, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoAlphaBy(MonoBehaviour m, float by, float time, UnityAction<float> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			var alpha = GetAlpha(m);
			return GoAlpha(m, alpha, alpha + by, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoAlpha(MonoBehaviour m, float start, float end, float time, UnityAction<float> update,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			var i = GoAlphaCoroutine(m, start, end, time, update, complete, type, delay, repeat, pingPong);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoAlphaCoroutine(Component o, float start, float end, float time, UnityAction<float> update,
			UnityAction complete, EaseType type, float delay, int repeat, bool pingPong)
		{
			var image = o.GetComponent<Image>();
			var canvasGroup = o.GetComponent<CanvasGroup>();
			var camera = Camera.main;
			Action<float> setAlpha = value =>
			{
				if (image != null)
					image.color = image.color.SetAlpha(value);
				else if (canvasGroup != null)
					canvasGroup.alpha = value;
				else
					camera.backgroundColor = camera.backgroundColor.SetAlpha(value);
			};
			var counter = repeat;
			while (repeat == 0 || counter > 0)
			{
				if (delay > 0f)
					yield return new WaitForSeconds(delay);
				var t = 0f;
				while (t <= 1f)
				{
					t += Time.deltaTime / time;
					var p = Types[type](start, end, Mathf.Clamp01(t));
					setAlpha(p);
					if (update != null)
						update(p);
					yield return null;
				}
				setAlpha(end);
				if (pingPong)
				{
					if (delay > 0f)
						yield return new WaitForSeconds(delay);
					t = 0f;
					while (t <= 1f)
					{
						t += Time.deltaTime / time;
						var p = Types[type](end, start, Mathf.Clamp01(t));
						setAlpha(p);
						if (update != null)
							update(p);
						yield return null;
					}
					setAlpha(start);
				}
				if (repeat != 0)
					counter--;
				if (repeat == 0 && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		public static float SineIn(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, 1f - Mathf.Cos(time * HalfPi));
		}
		public static float SineOut(float start, float end, float time)
		{
			return Mathf.Lerp(start, end, Mathf.Sin(time * HalfPi));
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
			return Mathf.Lerp(start, end, -((time -= 1f) * time * time * time - 1f));
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
				return Mathf.Lerp(start, end, -.5f * (Mathf.Sqrt(1f - time * time) - 1f));
			return Mathf.Lerp(start, end, .5f * (Mathf.Sqrt(1f - (time -= 2f) * time) + 1f));
		}
		public static float BackIn(float start, float end, float time)
		{
			const float s = 1.70158f;
			end -= start;
			return end * time * time * ((s + 1f) * time - s) + start;
		}
		public static float BackOut(float start, float end, float time)
		{
			const float s = 1.70158f;
			end -= start;
			return end * (--time * time * ((s + 1f) * time + s) + 1f) + start;
		}
		public static float BackInOut(float start, float end, float time)
		{
			const float s = 1.70158f * 1.525f;
			end -= start;
			if ((time /= .5f) < 1f)
				return end * .5f * (time * time * ((s + 1f) * time - s)) + start;
			return end * .5f * ((time -= 2) * time * ((s + 1f) * time  + s) + 2f) + start;
		}
		public static float ElasticIn(float start, float end, float time)
		{
			const float p = .3f;
			const float s = p / 4f;
			end -= start;
			return end * -(Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p)) + start;
		}
		public static float ElasticOut(float start, float end, float time)
		{
			const float p = .3f;
			const float s = p / 4f;
			end -= start;
			return end * Mathf.Pow(2f, -10f * time) * Mathf.Sin((time - s) * DoublePi / p) + end + start;
		}
		public static float ElasticInOut(float start, float end, float time)
		{
			const float p = .3f * 1.5f;
			const float s = p / 4f;
			end -= start;
			if ((time /= .5f) < 1f)
				return -.5f * (end * Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p)) + start;
			return end * Mathf.Pow(2f, -10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p) * .5f + end + start;
		}
		public static float BounceIn(float start, float end, float time)
		{
			end -= start;
			return end - BounceOut(0f, end, 1f - time) + start;
		}
		public static float BounceOut(float start, float end, float time)
		{
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
		public static float Spring(float start, float end, float time)
		{
			time = Mathf.Clamp01(time);
			time = (Mathf.Sin(time * Mathf.PI * (.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + (1.2f * (1f - time)));
			return start + (end - start) * time;
		}
	}
	public static class Ease3
	{
		private readonly static Dictionary<EaseType, Func<Vector3, Vector3, float, Vector3>> Types = new Dictionary<EaseType, Func<Vector3, Vector3, float, Vector3>>
		{
			{EaseType.Linear, Vector3.Lerp},
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
			{EaseType.BackIn, (start, end, time) => new Vector3(Ease.BackIn(start.x, end.x, time), Ease.BackIn(start.y, end.y, time), Ease.BackIn(start.z, end.z, time))},
			{EaseType.BackOut, (start, end, time) => new Vector3(Ease.BackOut(start.x, end.x, time), Ease.BackOut(start.y, end.y, time), Ease.BackOut(start.z, end.z, time))},
			{EaseType.BackInOut, (start, end, time) => new Vector3(Ease.BackInOut(start.x, end.x, time), Ease.BackInOut(start.y, end.y, time), Ease.BackInOut(start.z, end.z, time))},
			{EaseType.ElasticIn, (start, end, time) => new Vector3(Ease.ElasticIn(start.x, end.x, time), Ease.ElasticIn(start.y, end.y, time), Ease.ElasticIn(start.z, end.z, time))},
			{EaseType.ElasticOut, (start, end, time) => new Vector3(Ease.ElasticOut(start.x, end.x, time), Ease.ElasticOut(start.y, end.y, time), Ease.ElasticOut(start.z, end.z, time))},
			{EaseType.ElasticInOut, (start, end, time) => new Vector3(Ease.ElasticInOut(start.x, end.x, time), Ease.ElasticInOut(start.y, end.y, time), Ease.ElasticInOut(start.z, end.z, time))},
			{EaseType.BounceIn, (start, end, time) => new Vector3(Ease.BounceIn(start.x, end.x, time), Ease.BounceIn(start.y, end.y, time), Ease.BounceIn(start.z, end.z, time))},
			{EaseType.BounceOut, (start, end, time) => new Vector3(Ease.BounceOut(start.x, end.x, time), Ease.BounceOut(start.y, end.y, time), Ease.BounceOut(start.z, end.z, time))},
			{EaseType.BounceInOut, (start, end, time) => new Vector3(Ease.BounceInOut(start.x, end.x, time), Ease.BounceInOut(start.y, end.y, time), Ease.BounceInOut(start.z, end.z, time))},
			{EaseType.Spring, (start, end, time) => new Vector3(Ease.Spring(start.x, end.x, time), Ease.Spring(start.y, end.y, time), Ease.Spring(start.z, end.z, time))}
		};
		public static IEnumerator Go(MonoBehaviour m, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			var i = GoCoroutine(start, end, time, update, complete, type, delay, repeat, pingPong);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoCoroutine(Vector3 start, Vector3 end, float time, UnityAction<Vector3> update,
			UnityAction complete, EaseType type, float delay, int repeat, bool pingPong)
		{
			var counter = repeat;
			while (repeat == 0 || counter > 0)
			{
				if (delay > 0f)
					yield return new WaitForSeconds(delay);
				var t = 0f;
				while (t <= 1f)
				{
					t += Time.deltaTime / time;
					update(Types[type](start, end, Mathf.Clamp01(t)));
					yield return null;
				}
				if (pingPong)
				{
					if (delay > 0f)
						yield return new WaitForSeconds(delay);
					t = 0f;
					while (t <= 1f)
					{
						t += Time.deltaTime / time;
						update(Types[type](end, start, Mathf.Clamp01(t)));
						yield return null;
					}
				}
				if (repeat != 0)
					counter--;
				if (repeat == 0 && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		public static IEnumerator GoPositionTo(MonoBehaviour m, Vector3 to, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			return GoPosition(m, m.transform.localPosition, to, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoPositionBy(MonoBehaviour m, Vector3 by, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			return GoPosition(m, m.transform.localPosition, m.transform.localPosition + by, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoPosition(MonoBehaviour m, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			var i = GoPositionCoroutine(m, start, end, time, update, complete, type, delay, repeat, pingPong);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoPositionCoroutine(Component o, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update,
			UnityAction complete, EaseType type, float delay, int repeat, bool pingPong)
		{
			var counter = repeat;
			while (repeat == 0 || counter > 0)
			{
				if (delay > 0f)
					yield return new WaitForSeconds(delay);
				var t = 0f;
				while (t <= 1f)
				{
					t += Time.deltaTime / time;
					var p = Types[type](start, end, Mathf.Clamp01(t));
					o.transform.localPosition = p;
					if (update != null)
						update(p);
					yield return null;
				}
				o.transform.localPosition = end;
				if (pingPong)
				{
					if (delay > 0f)
						yield return new WaitForSeconds(delay);
					t = 0f;
					while (t <= 1f)
					{
						t += Time.deltaTime / time;
						var p = Types[type](end, start, Mathf.Clamp01(t));
						o.transform.localPosition = p;
						if (update != null)
							update(p);
						yield return null;
					}
					o.transform.localPosition = start;
				}
				if (repeat != 0)
					counter--;
				if (repeat == 0 && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		public static IEnumerator GoRotationTo(MonoBehaviour m, Vector3 to, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			return GoRotation(m, m.transform.localEulerAngles, to, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoRotationBy(MonoBehaviour m, Vector3 by, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			return GoRotation(m, m.transform.localEulerAngles, m.transform.localEulerAngles + by, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoRotation(MonoBehaviour m, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			var i = GoRotationCoroutine(m, start, end, time, update, complete, type, delay, repeat, pingPong);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoRotationCoroutine(Component o, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update,
			UnityAction complete, EaseType type, float delay, int repeat, bool pingPong)
		{
			var counter = repeat;
			while (repeat == 0 || counter > 0)
			{
				if (delay > 0f)
					yield return new WaitForSeconds(delay);
				var t = 0f;
				while (t <= 1f)
				{
					t += Time.deltaTime / time;
					var p = Types[type](start, end, Mathf.Clamp01(t));
					o.transform.localEulerAngles = p;
					if (update != null)
						update(p);
					yield return null;
				}
				o.transform.localEulerAngles = end;
				if (pingPong)
				{
					if (delay > 0f)
						yield return new WaitForSeconds(delay);
					t = 0f;
					while (t <= 1f)
					{
						t += Time.deltaTime / time;
						var p = Types[type](end, start, Mathf.Clamp01(t));
						o.transform.localEulerAngles = p;
						if (update != null)
							update(p);
						yield return null;
					}
					o.transform.localEulerAngles = start;
				}
				if (repeat != 0)
					counter--;
				if (repeat == 0 && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
		public static IEnumerator GoScaleTo(MonoBehaviour m, Vector3 to, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			return GoScale(m, m.transform.localScale, to, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoScaleBy(MonoBehaviour m, Vector3 by, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			return GoScale(m, m.transform.localScale, m.transform.localScale + by, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoScale(MonoBehaviour m, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			var i = GoScaleCoroutine(m, start, end, time, update, complete, type, delay, repeat, pingPong);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoScaleCoroutine(Component o, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update,
			UnityAction complete, EaseType type, float delay, int repeat, bool pingPong)
		{
			var counter = repeat;
			while (repeat == 0 || counter > 0)
			{
				if (delay > 0f)
					yield return new WaitForSeconds(delay);
				var t = 0f;
				while (t <= 1f)
				{
					t += Time.deltaTime / time;
					var p = Types[type](start, end, Mathf.Clamp01(t));
					o.transform.localScale = p;
					if (update != null)
						update(p);
					yield return null;
				}
				o.transform.localScale = end;
				if (pingPong)
				{
					if (delay > 0f)
						yield return new WaitForSeconds(delay);
					t = 0f;
					while (t <= 1f)
					{
						t += Time.deltaTime / time;
						var p = Types[type](end, start, Mathf.Clamp01(t));
						o.transform.localScale = p;
						if (update != null)
							update(p);
						yield return null;
					}
					o.transform.localScale = start;
				}
				if (repeat != 0)
					counter--;
				if (repeat == 0 && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}

		private static Color GetColor(MonoBehaviour m)
		{
			var image = m.GetComponent<Image>();
			return (image == null) ? Camera.main.backgroundColor : image.color;
		}
		public static IEnumerator GoColorTo(MonoBehaviour m, Vector3 to, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			return GoColor(m, GetColor(m).GetVector(), to, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoColorBy(MonoBehaviour m, Vector3 by, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			var color = GetColor(m).GetVector();
			return GoColor(m, color, color + by, time, update, complete, type, delay, repeat, pingPong);
		}
		public static IEnumerator GoColor(MonoBehaviour m, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update = null,
			UnityAction complete = null, EaseType type = EaseType.Linear, float delay = 0f, int repeat = 1, bool pingPong = false)
		{
			var i = GoColorCoroutine(m, start, end, time, update, complete, type, delay, repeat, pingPong);
			m.StartCoroutine(i);
			return i;
		}
		private static IEnumerator GoColorCoroutine(Component o, Vector3 start, Vector3 end, float time, UnityAction<Vector3> update,
			UnityAction complete, EaseType type, float delay, int repeat, bool pingPong)
		{
			var image = o.GetComponent<Image>();
			var camera = Camera.main;
			Action<Vector3> setColor = value =>
			{
				if (image == null)
					camera.backgroundColor = value.GetColor();
				else
					image.color = value.GetColor();
			};
			var counter = repeat;
			while (repeat == 0 || counter > 0)
			{
				if (delay > 0f)
					yield return new WaitForSeconds(delay);
				var t = 0f;
				while (t <= 1f)
				{
					t += Time.deltaTime / time;
					var p = Types[type](start, end, Mathf.Clamp01(t));
					setColor(p);
					if (update != null)
						update(p);
					yield return null;
				}
				setColor(end);
				if (pingPong)
				{
					if (delay > 0f)
						yield return new WaitForSeconds(delay);
					t = 0f;
					while (t <= 1f)
					{
						t += Time.deltaTime / time;
						var p = Types[type](end, start, Mathf.Clamp01(t));
						setColor(p);
						if (update != null)
							update(p);
						yield return null;
					}
					setColor(start);
				}
				if (repeat != 0)
					counter--;
				if (repeat == 0 && complete != null)
					complete();
			}
			if (repeat != 0 && complete != null)
				complete();
		}
	}
}
