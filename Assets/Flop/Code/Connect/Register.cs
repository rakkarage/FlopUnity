using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Register : MonoBehaviour
	{
		public GameObject Email;
		private InputField _emailField;
		private Animator _emailAnimator;
		public GameObject Password;
		private InputField _passwordField;
		private Animator _passwordAnimator;
		public GameObject Confirm;
		private InputField _confirmField;
		private Animator _confirmAnimator;
		public Animator RegisterButton;
		private void Awake()
		{
			_emailField = Email.GetComponent<InputField>();
			_emailAnimator = Email.GetComponent<Animator>();
			_passwordField = Password.GetComponent<InputField>();
			_passwordAnimator = Password.GetComponent<Animator>();
			_confirmField = Confirm.GetComponent<InputField>();
			_confirmAnimator = Confirm.GetComponent<Animator>();
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
			RegisterButton.SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		private void EnableInput()
		{
			RegisterButton.SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
