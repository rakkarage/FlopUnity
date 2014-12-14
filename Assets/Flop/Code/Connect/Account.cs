using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Account : MonoBehaviour
	{
		[SerializeField]
		private InputField _emailField = null;
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
				_emailField.text = email;
		}
		public void SignOutClicked()
		{
			Audio.Instance.PlayClick();
			Connection.SignOut();
			Connect.Instance.SpringBack();
		}
	}
}
