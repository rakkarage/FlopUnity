using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public static class Constants
	{
		public static Color ErrorRed = new Color(.75f, .5f, .5f);
		public static Color ErrorGreen = new Color(.5f, .75f, .5f);
		public static Color ErrorBlue = new Color(.5f, .5f, .75f);
		public static Color ErrorYellow = new Color(.75f, .75f, .5f);
		public static Color ButtonBlue = new Color32(159, 176, 255, 255);
		public static Color HenryBlue = new Color32(59, 67, 82, 255);
		public static string Error = "Error";
		public const float SpringTime = .5f;
		public static int OffsetInterface = 3072;
		public static Vector3 OffsetSignIn = new Vector3(0f, OffsetInterface, 0f);
		public static Vector3 OffsetRegister = new Vector3(OffsetInterface, OffsetInterface, 0f);
		public static Vector3 OffsetReset = new Vector3(-OffsetInterface, OffsetInterface, 0f);
		public static Vector3 OffsetAccount = new Vector3(-OffsetInterface, 0f, 0f);
		public static Vector3 OffsetChange = new Vector3(-(OffsetInterface * 2), 0f, 0f);
		public static Vector2 OffsetDialog = new Vector2(OffsetInterface, 0f);
		public static int AnimatorCompute = Animator.StringToHash("Compute");
		public static int AnimatorError = Animator.StringToHash("Error");
	}
	public static class Utility
	{
		public static void Assert(bool condition)
		{
			if (Debug.isDebugBuild)
			{
				if (!condition) throw new Exception();
			}
		}
		public static void LogError(Exception e)
		{
			Debug.Log("<color=red>Error: </color>" + e);
		}
		public static void Spring(MonoBehaviour m, Vector2 p)
		{
			Ease3.GoPosition(m, m.gameObject, m.gameObject.transform.localPosition, new Vector3(p.x, p.y, m.gameObject.transform.position.z), Constants.SpringTime, 0f, EaseType.Spring);
		}
		public static Color RandomColor()
		{
			return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		}
		public static void RandomColor(Text[] texts)
		{
			Color c = Utility.RandomColor();
			foreach (var text in texts)
			{
				if (text.color == Color.white)
				{
					text.color = c;
				}
				else
				{
					text.color = Color.white;
				}
			}
		}
		public static void ResetColor(Text[] texts)
		{
			if (texts != null)
			{
				foreach (var text in texts)
					text.color = Color.white;
			}
		}
	}
	public static class Prefs
	{
		private const string _emailKey = "email";
		public static string Email
		{
			get
			{
				return PlayerPrefs.GetString(_emailKey, null);
			}
			set
			{
				PlayerPrefs.SetString(_emailKey, value);
				PlayerPrefs.Save();
			}
		}
	}
	public static class EmailValidator
	{
		private static readonly Regex ValidEmailRegex = CreateValidEmailRegex();
		private const string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
			+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
			+ @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
		private static Regex CreateValidEmailRegex()
		{
			return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
		}
		public static bool IsValid(string email)
		{
			return ValidEmailRegex.IsMatch(email);
		}
	}
	public static class Vector3Extensions
	{
		public static Color GetColor(this Vector3 v)
		{
			return new Color(v.x, v.y, v.z);
		}
	}
	public static class ColorExtensions
	{
		public static Vector3 GetVector(this Color c)
		{
			return new Vector3(c.r, c.g, c.b);
		}
		public static Color SetAlpha(this Color c, float a)
		{
			c = new Color(c.r, c.g, c.b, a);
			return c;
		}
	}
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
		public static void SetInteractable(this GameObject o, bool interactable)
		{
			Selectable[] controls = o.GetComponentsInChildren<Selectable>();
			foreach (var i in controls)
			{
				i.interactable = interactable;
			}
		}
	}
}
