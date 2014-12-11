using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Account : MonoBehaviour
	{
		public InputField EmailField;
		private void OnEnable()
		{
			Connect.EmailChangedEvent += HandleEmailChanged;
		}
		private void OnDisable()
		{
			Connect.EmailChangedEvent -= HandleEmailChanged;
		}
		private void HandleEmailChanged(string email)
		{
			if (!string.IsNullOrEmpty(email))
				EmailField.text = email;
		}
		public void SignOutClicked()
		{
			Connection.SignOut();
			Audio.Instance.PlayClick();
			Connect.Instance.SpringBack();
		}
	}
}
