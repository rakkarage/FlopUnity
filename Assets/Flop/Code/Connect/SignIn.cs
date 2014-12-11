using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class SignIn : MonoBehaviour
	{
		public GameObject Email;
		private InputField _emailField;
		private Animator _emailAnimator;
		public GameObject Password;
		private InputField _passwordField;
		private Animator _passwordAnimator;
		public Animator SignInButton;
		private void Awake()
		{
			_emailField = Email.GetComponent<InputField>();
			_emailAnimator = Email.GetComponent<Animator>();
			_passwordField = Password.GetComponent<InputField>();
			_passwordAnimator = Password.GetComponent<Animator>();
		}
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
				_emailField.text = email;
				_emailField.MoveTextEnd(false);
			}
		}
		public void SignInClicked()
		{
			Audio.Instance.PlayClick();
			string email = _emailField.text;
			string password = _passwordField.text;
			Connect.Instance.SetEmail(email);
			ClearError();
			if (!Connection.ValidEmail(email))
			{
				Error(_emailAnimator);
				return;
			}
			if (!Connection.ValidPassword(password))
			{
				Error(_passwordAnimator);
				return;
			}
			DisableInput();
			Connection.SignIn(email, password);
		}
		private void Error(Animator a)
		{
			Audio.Instance.PlayError();
			a.SetBool(Constants.AnimatorError, true);
		}
		public void ClearError()
		{
			_emailAnimator.SetBool(Constants.AnimatorError, false);
			_passwordAnimator.SetBool(Constants.AnimatorError, false);
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
			_passwordField.text = string.Empty;
			EnableInput();
		}
		public void DisableInput()
		{
			SignInButton.SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		public void EnableInput()
		{
			SignInButton.SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
