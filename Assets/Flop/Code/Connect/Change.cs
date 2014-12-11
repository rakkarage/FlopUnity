using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Change : MonoBehaviour
	{
		public GameObject Old;
		private InputField _oldField;
		private Animator _oldAnimator;
		public GameObject New;
		private InputField _newField;
		private Animator _newAnimator;
		public GameObject Confirm;
		private InputField _confirmField;
		private Animator _confirmAnimator;
		public Animator ChangeButton;
		private void Awake()
		{
			_oldField = Old.GetComponent<InputField>();
			_oldAnimator = Old.GetComponent<Animator>();
			_newField = New.GetComponent<InputField>();
			_newAnimator = New.GetComponent<Animator>();
			_confirmField = Confirm.GetComponent<InputField>();
			_confirmAnimator = Confirm.GetComponent<Animator>();
		}
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
			var oldPass = _oldField.text;
			var newPass = _newField.text;
			var confirm = _confirmField.text;
			ClearError();
			if (!Connection.ValidPassword(oldPass))
			{
				Error(_oldAnimator);
				return;
			}
			if (!Connection.ValidPassword(newPass))
			{
				Error(_newAnimator);
				return;
			}
			if (!Connection.ValidPassword(confirm) || !confirm.Equals(newPass))
			{
				Error(_confirmAnimator);
				return;
			}
			DisableInput();
			Connection.Change(newPass, oldPass);
		}
		private void Error(Animator a)
		{
			Audio.Instance.PlayError();
			a.SetBool(Constants.AnimatorError, true);
		}
		public void ClearError()
		{
			_oldAnimator.SetBool(Constants.AnimatorError, false);
			_newAnimator.SetBool(Constants.AnimatorError, false);
			_confirmAnimator.SetBool(Constants.AnimatorError, false);
		}
		private void ChangeFail(string reason)
		{
			Audio.Instance.PlayError();
			Dialog.Instance.ShowError(reason);
			EnableInput();
		}
		private void ChangeSucceed()
		{
			Connect.Instance.SpringBack();
			_oldField.text = string.Empty;
			_newField.text = string.Empty;
			_confirmField.text = string.Empty;
			EnableInput();
		}
		public void DisableInput()
		{
			ChangeButton.SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		public void EnableInput()
		{
			ChangeButton.SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
