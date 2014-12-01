using System.Linq;
using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public static class GameObjectExtensions
	{
		// public static void SortChildren(this GameObject o)
		// {
		// 	var children = o.GetComponentsInChildren<Transform>(true);
		// 	var sorted = from child in children
		// 				 orderby child.gameObject.activeInHierarchy descending, child.localPosition.z descending
		// 				 where child != o.transform
		// 				 select child;
		// 	for (int i = 0; i < sorted.Count(); i++)
		// 		sorted.ElementAt(i).SetSiblingIndex(i);
		// }
		public static void SortChildren(this GameObject o)
		{
			var children = o.GetComponentsInChildren<Transform>(true).ToList();
			children.Remove(o.transform);
			children.Sort(Compare);
			for (int i = 0; i < children.Count; i++)
				children[i].SetSiblingIndex(i);
		}
		private static int Compare(Transform lhs, Transform rhs)
		{
			if (lhs == rhs) return 0;
			var test = rhs.gameObject.activeInHierarchy.CompareTo(lhs.gameObject.activeInHierarchy);
			if (test != 0) return test;
			if (lhs.localPosition.z < rhs.localPosition.z) return 1;
			if (lhs.localPosition.z > rhs.localPosition.z) return -1;
			return 0;
		}
	}
}
