using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Reset : MonoBehaviour
	{
		public Image Email;
		public InputField EmailField;
		public Button ResetButton;
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
				EmailField.text = email;
				EmailField.MoveTextEnd(false);
			}
		}
		public void ResetClicked()
		{
			Audio.Instance.PlayClick();
			string email = EmailField.text;
			Connect.Instance.SetEmail(email);
			ClearError();
			if (!Connection.ValidEmail(email))
			{
				Error(Email.gameObject);
				return;
			}
			DisableInput();
			Connection.Reset(email);
		}
		private void Error(GameObject o)
		{
			Audio.Instance.PlayError();
			o.GetComponent<Animator>().SetBool(Constants.AnimatorError, true);
		}
		public void ClearError()
		{
			Email.GetComponent<Animator>().SetBool(Constants.AnimatorError, false);
		}
		private void ResetFail(string reason)
		{
			Audio.Instance.PlayError();
			Dialog.Instance.Show(reason);
			EnableInput();
		}
		private void ResetSucceed()
		{
			Connect.Instance.SpringSignIn();
			EnableInput();
		}
		public void DisableInput()
		{
			ResetButton.GetComponent<Animator>().SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		public void EnableInput()
		{
			ResetButton.GetComponent<Animator>().SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
