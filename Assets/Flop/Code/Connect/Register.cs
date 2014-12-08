using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Register : MonoBehaviour
	{
		public Image Email;
		public InputField EmailField;
		public Image Password;
		public InputField PasswordField;
		public Image Confirm;
		public InputField ConfirmField;
		public Button RegisterButton;
		private void OnEnable()
		{
			Connect.EmailChangedEvent += HandleEmailChanged;
			Connection.RegisterFailEvent += RegisterFail;
			Connection.RegisterSucceedEvent += RegisterSucceed;
		}
		private void OnDisable()
		{
			Connect.EmailChangedEvent -= HandleEmailChanged;
			Connection.RegisterFailEvent -= RegisterFail;
			Connection.RegisterSucceedEvent -= RegisterSucceed;
		}
		private void HandleEmailChanged(string email)
		{
			if (!string.IsNullOrEmpty(email))
			{
				EmailField.text = email;
				EmailField.MoveTextEnd(false);
			}
		}
		public void RegisterClicked()
		{
			Audio.Instance.PlayClick();
			string email = EmailField.text;
			string password = PasswordField.text;
			string confirm = ConfirmField.text;
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
			if (!Connection.ValidPassword(confirm) || !confirm.Equals(password))
			{
				Error(Confirm.gameObject);
				return;
			}
			DisableInput();
			Connection.Register(email, password);
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
			Confirm.GetComponent<Animator>().SetBool(Constants.AnimatorError, false);
		}
		private void RegisterFail(string reason)
		{
			Audio.Instance.PlayError();
			Dialog.Instance.ShowError(reason);
			EnableInput();
		}
		private void RegisterSucceed()
		{
			Connect.Instance.SpringBack();
			PasswordField.text = string.Empty;
			ConfirmField.text = string.Empty;
			EnableInput();
		}
		public void DisableInput()
		{
			RegisterButton.GetComponent<Animator>().SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		public void EnableInput()
		{
			RegisterButton.GetComponent<Animator>().SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
