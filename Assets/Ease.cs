using System.Collections;
﻿using UnityEngine;
using UnityEngine.Events;
public static class Ease
{
	public static void Go(MonoBehaviour o, float start, float end, float time, UnityAction<float> callback)
	{
		o.StopAllCoroutines();
		o.StartCoroutine(GoCoroutine(o, start, end, time, callback));
	}
	public static IEnumerator GoCoroutine(MonoBehaviour o, float start, float end, float time, UnityAction<float> callback)
	{
		var i = 0f;
		while (i <= 1f)
		{
			i += Time.deltaTime / time;
			callback(Spring(start, end, i));
			yield return null;
		}
	}
    public static float Spring(float start, float end, float i)
    {
        i = Mathf.Clamp01(i);
        i = (Mathf.Sin(i * Mathf.PI * (.2f + 2.5f * i * i * i)) * Mathf.Pow(1f - i, 2.2f) + i) * (1f + (1.2f * (1f - i)));
        return start + (end - start) * i;
    }
}
