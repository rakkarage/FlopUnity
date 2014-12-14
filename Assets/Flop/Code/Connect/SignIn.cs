using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class SignIn : MonoBehaviour
	{
		[SerializeField]
		private GameObject _email = null;
		private InputField _emailField;
		private Animator _emailAnimator;
		[SerializeField]
		private GameObject _password = null;
		private InputField _passwordField;
		private Animator _passwordAnimator;
		[SerializeField]
		private Animator _signInButton = null;
		private void Awake()
		{
			_emailField = _email.GetComponent<InputField>();
			_emailAnimator = _email.GetComponent<Animator>();
			_passwordField = _password.GetComponent<InputField>();
			_passwordAnimator = _password.GetComponent<Animator>();
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
				_emailField.text = email;
		}
		public void SignInClicked()
		{
			Audio.Instance.PlayClick();
			var email = _emailField.text;
			var password = _passwordField.text;
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
		private void ClearError()
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
		private void DisableInput()
		{
			_signInButton.SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		private void EnableInput()
		{
			_signInButton.SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
