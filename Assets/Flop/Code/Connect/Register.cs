using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Register : MonoBehaviour
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
		private GameObject _confirm = null;
		private InputField _confirmField;
		private Animator _confirmAnimator;
		[SerializeField]
		private Animator _registerButton = null;
		private void Awake()
		{
			_emailField = _email.GetComponent<InputField>();
			_emailAnimator = _email.GetComponent<Animator>();
			_passwordField = _password.GetComponent<InputField>();
			_passwordAnimator = _password.GetComponent<Animator>();
			_confirmField = _confirm.GetComponent<InputField>();
			_confirmAnimator = _confirm.GetComponent<Animator>();
		}
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
				_emailField.text = email;
		}
		public void RegisterClicked()
		{
			Audio.Instance.PlayClick();
			string email = _emailField.text;
			string password = _passwordField.text;
			string confirm = _confirmField.text;
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
			if (!Connection.ValidPassword(confirm) || !confirm.Equals(password))
			{
				Error(_confirmAnimator);
				return;
			}
			DisableInput();
			Connection.Register(email, password);
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
			_confirmAnimator.SetBool(Constants.AnimatorError, false);
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
			_passwordField.text = string.Empty;
			_confirmField.text = string.Empty;
			EnableInput();
		}
		private void DisableInput()
		{
			_registerButton.SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		private void EnableInput()
		{
			_registerButton.SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
