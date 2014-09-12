using System.Collections;
﻿using System.Collections.Generic;
﻿using UnityEngine;
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
	public static void Go(MonoBehaviour o, float start, float end, float t, UnityAction<float> callback, Type type)
	{
		o.StopAllCoroutines();
		o.StartCoroutine(GoCoroutine(start, end, t, callback, type));
	}
	private static IEnumerator GoCoroutine(float start, float end, float t, UnityAction<float> callback, Type type)
	{
		var i = 0f;
		while (i <= 1f)
		{
			i += Time.deltaTime / t;
            callback(_types[type](start, end, i));
			yield return null;
		}
	}
    public static float Hermite(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, t * t * (3f - 2f * t));
    }
    public static float Sinerp(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(t * Mathf.PI * .5f));
    }
    public static float Coserp(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, 1f - Mathf.Cos(t * Mathf.PI * .5f));
    }
    public static float Spring(float start, float end, float t)
    {
        t = Mathf.Clamp01(t);
        t = (Mathf.Sin(t * Mathf.PI * (.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + (1.2f * (1f - t)));
        return start + (end - start) * t;
    }
}
