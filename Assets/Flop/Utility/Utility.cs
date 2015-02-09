using System.Linq;
using UnityEngine;
namespace ca.HenrySoftware
{
	public static class VectorExtensions
	{
		public static Color GetColor(this Vector3 v)
		{
			return new Color(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y), Mathf.Clamp01(v.z));
		}
		public static Color GetColor(this Vector4 v)
		{
			return new Color(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y), Mathf.Clamp01(v.z), Mathf.Clamp01(v.w));
		}
	}
	public static class ColorExtensions
	{
		public static Vector3 GetVector3(this Color c)
		{
			return new Vector3(c.r, c.g, c.b);
		}
		public static Vector4 GetVector4(this Color c)
		{
			return new Vector4(c.r, c.g, c.b, c.a);
		}
		public static Color SetAlpha(this Color c, float a)
		{
			return new Color(c.r, c.g, c.b, a);
		}
	}
	public static class GameObjectExtensions
	{
		public static void SortChildren(this GameObject o)
		{
			var children = o.GetComponentsInChildren<Transform>(true).ToList();
			children.Remove(o.transform);
			children.Sort(Compare);
			for (var i = 0; i < children.Count; i++)
				children[i].SetSiblingIndex(i);
		}
		private static int Compare(Transform lhs, Transform rhs)
		{
			if (lhs == rhs) return 0;
			var test = rhs.gameObject.activeInHierarchy.CompareTo(lhs.gameObject.activeInHierarchy);
			if (test != 0) return test;
			if (lhs.localPosition.z < rhs.localPosition.z) return 1;
			if (lhs.localPosition.z > rhs.localPosition.z) return -1;
			if (lhs.localPosition.y < rhs.localPosition.y) return 1;
			if (lhs.localPosition.y > rhs.localPosition.y) return -1;
			if (lhs.localPosition.x < rhs.localPosition.x) return 1;
			if (lhs.localPosition.x > rhs.localPosition.x) return -1;
			return 0;
		}
	}
}
