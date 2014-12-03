using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Change : MonoBehaviour
	{
		public Image Old;
		public InputField OldField;
		public Image New;
		public InputField NewField;
		public Image Confirm;
		public InputField ConfirmField;
		public Button ChangeButton;
		private void OnEnable()
		{
			Connection.ChangeFailEvent += ChangeFail;
			Connection.ChangeSucceedEvent += ChangeSucceed;
		}
		private void OnDisable()
		{
			Connection.ChangeFailEvent -= ChangeFail;
			Connection.ChangeSucceedEvent -= ChangeSucceed;
		}
		public void ChangeClicked()
		{
			Audio.Instance.PlayClick();
			string oldPass = OldField.text;
			string newPass = NewField.text;
			string confirm = ConfirmField.text;
			ClearError();
			if (!Connection.ValidPassword(oldPass))
			{
				Error(Old.gameObject);
				return;
			}
			if (!Connection.ValidPassword(newPass))
			{
				Error(New.gameObject);
				return;
			}
			if (!Connection.ValidPassword(confirm) || !confirm.Equals(newPass))
			{
				Error(Confirm.gameObject);
				return;
			}
			DisableInput();
			Connection.Change(newPass, oldPass);
		}
		private void Error(GameObject o)
		{
			Audio.Instance.PlayError();
			o.GetComponent<Animator>().SetBool(Constants.AnimatorError, true);
		}
		public void ClearError()
		{
			Old.GetComponent<Animator>().SetBool(Constants.AnimatorError, false);
			New.GetComponent<Animator>().SetBool(Constants.AnimatorError, false);
			Confirm.GetComponent<Animator>().SetBool(Constants.AnimatorError, false);
		}
		private void ChangeFail(string reason)
		{
			Audio.Instance.PlayError();
			Dialog.Instance.Show(reason);
			EnableInput();
		}
		private void ChangeSucceed()
		{
			Connect.Instance.SpringBack();
			OldField.text = string.Empty;
			NewField.text = string.Empty;
			ConfirmField.text = string.Empty;
			EnableInput();
		}
		public void DisableInput()
		{
			ChangeButton.GetComponent<Animator>().SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		public void EnableInput()
		{
			ChangeButton.GetComponent<Animator>().SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
