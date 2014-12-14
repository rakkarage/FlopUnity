using Parse;
using System;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	public static class Connection
	{
		public static bool Connected
		{
			get { return ParseUser.CurrentUser != null; }
		}
		public static string Email
		{
			get { return Connected ? ParseUser.CurrentUser.Email : string.Empty; }
		}
		public static bool ValidEmail(string email)
		{
			return !string.IsNullOrEmpty(email) && Valid.Email(email);
		}
		public static bool ValidPassword(string password)
		{
			return !string.IsNullOrEmpty(password) && (password.Length > 3);
		}
		// register
		public static UnityAction<string> RegisterFailEvent;
		public static UnityAction RegisterSucceedEvent;
		public static void Register(string email, string password)
		{
			try
			{
				var user = new ParseUser() { Username = email, Email = email, Password = password };
				user.SignUpAsync().ContinueWith(t =>
				{
					if (t.IsFaulted || t.IsCanceled)
						Loom.QueueOnMainThread(() => { RegisterFail(t.Exception); });
					else
						Loom.QueueOnMainThread(RegisterSucceed);
				});
			}
			catch (Exception e)
			{
				RegisterFail(e);
			}
		}
		private static void RegisterFail(Exception e)
		{
			Utility.LogError(e);
			if (RegisterFailEvent != null) RegisterFailEvent(e.Message);
		}
		private static void RegisterSucceed()
		{
			if (RegisterSucceedEvent != null) RegisterSucceedEvent();
		}
		// sign in
		public static UnityAction<string> SignInFailEvent;
		public static UnityAction SignInSucceedEvent;
		public static void SignIn(string email, string password)
		{
			try
			{
				ParseUser.LogInAsync(email, password).ContinueWith(t =>
				{
					if (t.IsFaulted || t.IsCanceled)
						Loom.QueueOnMainThread(() => { SignInFail(t.Exception); });
					else
						Loom.QueueOnMainThread(SignInSucceed);
				});
			}
			catch (Exception e)
			{
				SignInFail(e);
			}
		}
		private static void SignInFail(Exception e)
		{
			Utility.LogError(e);
			if (SignInFailEvent != null) SignInFailEvent(e.Message);
		}
		private static void SignInSucceed()
		{
			if (SignInSucceedEvent != null) SignInSucceedEvent();
		}
		// sign out
		public static UnityAction SignOutEvent;
		public static void SignOut()
		{
			ParseUser.LogOut();
			if (SignOutEvent != null) SignOutEvent();
		}
		// reset password
		public static UnityAction<string> ResetFailEvent;
		public static UnityAction ResetSucceedEvent;
		public static void Reset(string email)
		{
			try
			{
				ParseUser.RequestPasswordResetAsync(email).ContinueWith(t =>
				{
					if (t.IsFaulted || t.IsCanceled)
						Loom.QueueOnMainThread(() => { ResetFail(t.Exception); });
					else
						Loom.QueueOnMainThread(ResetSucceed);
				});
			}
			catch (Exception e)
			{
				ResetFail(e);
			}
		}
		private static void ResetFail(Exception e)
		{
			Utility.LogError(e);
			if (ResetFailEvent != null) ResetFailEvent(e.Message);
		}
		private static void ResetSucceed()
		{
			if (ResetSucceedEvent != null) ResetSucceedEvent();
		}
		// change password
		public static UnityAction<string> ChangeFailEvent;
		public static UnityAction ChangeSucceedEvent;
		public static void Change(string newPassword, string oldPassword)
		{
			try
			{
				ParseUser.CurrentUser.Password = newPassword;
				ParseUser.CurrentUser.SaveAsync().ContinueWith(t =>
				{
					if (t.IsFaulted || t.IsCanceled)
					{
						Loom.QueueOnMainThread(() => { ChangeFail(t.Exception); });
					}
					else
					{
						Loom.QueueOnMainThread(ChangeSucceed);
					}
				});
			}
			catch (Exception e)
			{
				ChangeFail(e);
			}
		}
		private static void ChangeFail(Exception e)
		{
			Utility.LogError(e);
			if (ChangeFailEvent != null) ChangeFailEvent(e.Message);
		}
		private static void ChangeSucceed()
		{
			if (ChangeSucceedEvent != null) ChangeSucceedEvent();
		}
	}
}
