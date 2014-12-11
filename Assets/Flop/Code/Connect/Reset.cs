using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Reset : MonoBehaviour
	{
		public GameObject Email;
		private InputField _emailField;
		private Animator _emailAnimator;
		public Animator ResetButton;
		private void Awake()
		{
			_emailField = Email.GetComponent<InputField>();
			_emailAnimator = Email.GetComponent<Animator>();
		}
		private void OnEnable()
		{
			Connect.EmailChangedEvent += HandleEmailChanged;
			Connection.ResetFailEvent += ResetFail;
			Connection.ResetSucceedEvent += ResetSucceed;
		}
		private void OnDisable()
		{
			Connect.EmailChangedEvent -= HandleEmailChanged;
			Connection.ResetFailEvent -= ResetFail;
			Connection.ResetSucceedEvent -= ResetSucceed;
		}
		private void HandleEmailChanged(string email)
		{
			if (!string.IsNullOrEmpty(email))
			{
				_emailField.text = email;
				_emailField.MoveTextEnd(false);
			}
		}
		public void ResetClicked()
		{
			Audio.Instance.PlayClick();
			string email = _emailField.text;
			Connect.Instance.SetEmail(email);
			ClearError();
			if (!Connection.ValidEmail(email))
			{
				Error(_emailAnimator);
				return;
			}
			DisableInput();
			Connection.Reset(email);
		}
		private void Error(Animator a)
		{
			Audio.Instance.PlayError();
			a.SetBool(Constants.AnimatorError, true);
		}
		private void ClearError()
		{
			_emailAnimator.SetBool(Constants.AnimatorError, false);
		}
		private void ResetFail(string reason)
		{
			Audio.Instance.PlayError();
			Dialog.Instance.ShowError(reason);
			EnableInput();
		}
		private void ResetSucceed()
		{
			Connect.Instance.SpringSignIn();
			EnableInput();
		}
		private void DisableInput()
		{
			ResetButton.SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		private void EnableInput()
		{
			ResetButton.SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
