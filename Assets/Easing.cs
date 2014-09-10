using UnityEngine;
public static class Easing
{
    public static float Spring(float from, float to, float i)
    {
        i = Mathf.Clamp01(i);
        i = (Mathf.Sin(i * Mathf.PI * (.2f + 2.5f * i * i * i)) * Mathf.Pow(1f - i, 2.2f) + i) * (1f + (1.2f * (1f - i)));
        return from + (to - from) * i;
    }
}
