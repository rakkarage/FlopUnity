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
			{
				EmailField.text = email;
				EmailField.MoveTextEnd(false);
			}
		}
		public void SignOutClicked()
		{
			Audio.Instance.PlayClick();
			Connection.SignOut();
			Connect.Instance.SpringBack();
		}
	}
}
