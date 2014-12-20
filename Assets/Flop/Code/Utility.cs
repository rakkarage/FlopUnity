using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public static class Constants
	{
		public const string Error = "Error";
		public const int Offset = 3072;
		public static readonly int AnimatorCompute = Animator.StringToHash("Compute");
		public static readonly int AnimatorError = Animator.StringToHash("Error");
		public static readonly Color StatusRed = new Color(.75f, .5f, .5f);
		public static readonly Color StatusGreen = new Color(.5f, .75f, .5f);
		public static readonly Color StatusBlue = new Color(.5f, .5f, .75f);
		public static readonly Color StatusYellow = new Color(.75f, .75f, .5f);
		public static readonly Color ButtonBlue = new Color32(159, 176, 255, 255);
		public static readonly Color HenryBlue = new Color32(59, 67, 82, 255);
	}
	public static class Utility
	{
		public static IEnumerator RealTimeWait(float time)
		{
			var start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + time)
				yield return null;
		}
		public static void LogError(Exception e)
		{
			Debug.Log("<color=red>Error:</color> " + e);
		}
		public static void Spring(MonoBehaviour m, Vector2 p)
		{
			Ease3.GoPositionTo(m, new Vector3(p.x, p.y, 0f), .5f, null, null, EaseType.Spring);
		}
		public static Color RandomColor()
		{
			return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		}
		public static void RandomColor(IEnumerable<Text> texts)
		{
			if (texts == null) return;
			var c = RandomColor();
			foreach (var text in texts)
			{
				text.color = text.color == Color.white ? c : Color.white;
			}
		}
		public static void ResetColor(IEnumerable<Text> texts)
		{
			if (texts == null) return;
			foreach (var text in texts)
				text.color = Color.white;
		}
	}
	public static class Prefs
	{
		private const string EmailKey = "email";
		public static string Email
		{
			get { return PlayerPrefs.GetString(EmailKey, null); }
			set
			{
				PlayerPrefs.SetString(EmailKey, value);
				PlayerPrefs.Save();
			}
		}
	}
	public static class Valid
	{
		private const string EmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
			+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
			+ @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
		private static readonly Regex EmailRegex = new Regex(EmailPattern, RegexOptions.IgnoreCase);
		public static bool Email(string email)
		{
			return EmailRegex.IsMatch(email);
		}
	}
	public static class Vector3Extensions
	{
		public static Color GetColor(this Vector3 v)
		{
			return new Color(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y), Mathf.Clamp01(v.z));
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
		public static void SetInteractable(this GameObject o, bool interactable)
		{
			foreach (var i in o.GetComponentsInChildren<Selectable>())
				i.interactable = interactable;
		}
	}
}
