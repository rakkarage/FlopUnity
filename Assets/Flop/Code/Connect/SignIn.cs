using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class SignIn : MonoBehaviour
	{
		public Image Email;
		public InputField EmailField;
		public Image Password;
		public InputField PasswordField;
		public Button SignInButton;
		private void OnEnable()
		{
			Connect.EmailChangedEvent += HandleEmailChanged;
			Connection.SignInFailEvent += SignInFail;
			Connection.SignInSucceedEvent += SignInSucceed;
		}
		private void OnDisable()
		{
			Connect.EmailChangedEvent -= HandleEmailChanged;
			Connection.SignInFailEvent -= SignInFail;
			Connection.SignInSucceedEvent -= SignInSucceed;
		}
		private void HandleEmailChanged(string email)
		{
			if (!string.IsNullOrEmpty(email))
			{
				EmailField.text = email;
				EmailField.MoveTextEnd(false);
			}
		}
		public void SignInClicked()
		{
			Audio.Instance.PlayClick();
			string email = EmailField.text;
			string password = PasswordField.text;
			Connect.Instance.SetEmail(email);
			ClearError();
			if (!Connection.ValidEmail(email))
			{
				Error(Email.gameObject);
				return;
			}
			if (!Connection.ValidPassword(password))
			{
				Error(Password.gameObject);
				return;
			}
			DisableInput();
			Connection.SignIn(email, password);
		}
		private void Error(GameObject o)
		{
			Audio.Instance.PlayError();
			o.GetComponent<Animator>().SetBool(Constants.AnimatorError, true);
		}
		public void ClearError()
		{
			Email.GetComponent<Animator>().SetBool(Constants.AnimatorError, false);
			Password.GetComponent<Animator>().SetBool(Constants.AnimatorError, false);
		}
		private void SignInFail(string reason)
		{
			Audio.Instance.PlayError();
			Dialog.Instance.ShowError(reason);
			EnableInput();
		}
		private void SignInSucceed()
		{
			Connect.Instance.SpringBack();
			PasswordField.text = string.Empty;
			EnableInput();
		}
		public void DisableInput()
		{
			SignInButton.GetComponent<Animator>().SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		public void EnableInput()
		{
			SignInButton.GetComponent<Animator>().SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
