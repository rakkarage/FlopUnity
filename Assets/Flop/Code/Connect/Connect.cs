using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	public class Connect : Singleton<Connect>
	{
		public static UnityAction<string> EmailChangedEvent;
		private static readonly Vector3 _offsetSignIn = new Vector3(0f, Constants.Offset, 0f);
		private static readonly Vector3 _offsetRegister = new Vector3(Constants.Offset, Constants.Offset, 0f);
		private static readonly Vector3 _offsetReset = new Vector3(-Constants.Offset, Constants.Offset, 0f);
		private static readonly Vector3 _offsetAccount = new Vector3(-Constants.Offset, 0f, 0f);
		private static readonly Vector3 _offsetChange = new Vector3(-(Constants.Offset * 2f), 0f, 0f);
		private void OnEnable()
		{
			StartCoroutine(UpdateEmail());
		}
		private IEnumerator UpdateEmail()
		{
			yield return null;
			string email = Prefs.Email;
			if (!string.IsNullOrEmpty(email))
			{
				SetEmail(email);
			}
		}
		public void SetEmail(string email)
		{
			Prefs.Email = email;
			if (EmailChangedEvent != null)
				EmailChangedEvent(email);
		}
		public void SpringSignIn()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(this, _offsetSignIn);
			Flow.Instance.FadeBack();
		}
		public void SpringRegister()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(this, _offsetRegister);
			Flow.Instance.FadeBack();
		}
		public void SpringReset()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(this, _offsetReset);
			Flow.Instance.FadeBack();
		}
		public void SpringAccount()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(this, _offsetAccount);
			Flow.Instance.FadeBack();
		}
		public void SpringChange()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(this, _offsetChange);
			Flow.Instance.FadeBack();
		}
		public void SpringBack()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Status.Instance.UpdateColor();
			Utility.Spring(this, Vector3.zero);
			Flow.Instance.FadeFore();
		}
	}
}
