using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	public class Connect : Singleton<Connect>
	{
		public static UnityAction<string> EmailChangedEvent;
		private static readonly Vector3 OffsetSignIn = new Vector3(0f, Constants.Offset, 0f);
		private static readonly Vector3 OffsetRegister = new Vector3(Constants.Offset, Constants.Offset, 0f);
		private static readonly Vector3 OffsetReset = new Vector3(-Constants.Offset, Constants.Offset, 0f);
		private static readonly Vector3 OffsetAccount = new Vector3(-Constants.Offset, 0f, 0f);
		private static readonly Vector3 OffsetChange = new Vector3(-(Constants.Offset * 2f), 0f, 0f);
		private void OnEnable()
		{
			StartCoroutine(UpdateEmail());
		}
		private IEnumerator UpdateEmail()
		{
			yield return null;
			var email = Prefs.Email;
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
			Utility.Spring(this, OffsetSignIn);
			Flow.Instance.FadeOut();
		}
		public void SpringRegister()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(this, OffsetRegister);
			Flow.Instance.FadeOut();
		}
		public void SpringReset()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(this, OffsetReset);
			Flow.Instance.FadeOut();
		}
		public void SpringAccount()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(this, OffsetAccount);
			Flow.Instance.FadeOut();
		}
		public void SpringChange()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(this, OffsetChange);
			Flow.Instance.FadeOut();
		}
		public void SpringBack()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Status.Instance.UpdateColor();
			Utility.Spring(this, Vector3.zero);
			Flow.Instance.FadeIn();
		}
	}
}
