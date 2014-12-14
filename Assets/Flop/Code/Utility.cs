using System;
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
		public static Color StatusRed = new Color(.75f, .5f, .5f);
		public static Color StatusGreen = new Color(.5f, .75f, .5f);
		public static Color StatusBlue = new Color(.5f, .5f, .75f);
		public static Color StatusYellow = new Color(.75f, .75f, .5f);
		public static Color ButtonBlue = new Color32(159, 176, 255, 255);
		public static Color HenryBlue = new Color32(59, 67, 82, 255);
		public static readonly int AnimatorCompute = Animator.StringToHash("Compute");
		public static readonly int AnimatorError = Animator.StringToHash("Error");
	}
	public static class Utility
	{
		public static void LogError(Exception e)
		{
			Debug.Log("<color=red>Error:</color> " + e);
		}
		public static void Spring(MonoBehaviour m, Vector2 p)
		{
			var t = m.transform;
			Ease3.GoPosition(m, t.localPosition, new Vector3(p.x, p.y, t.position.z), .5f, EaseType.Spring);
		}
		public static Color RandomColor()
		{
			return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		}
		public static void RandomColor(IEnumerable<Text> texts)
		{
			if (texts != null)
			{
				var c = RandomColor();
				foreach (var text in texts)
				{
					text.color = text.color == Color.white ? c : Color.white;
				}
			}
		}
		public static void ResetColor(IEnumerable<Text> texts)
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
		private const string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
			+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
			+ @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
		private static Regex CreateValidEmailRegex()
		{
			return new Regex(_validEmailPattern, RegexOptions.IgnoreCase);
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
