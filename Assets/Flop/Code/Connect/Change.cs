using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Change : MonoBehaviour
	{
		[SerializeField] private GameObject _old = null;
		private InputField _oldField;
		private Animator _oldAnimator;
		[SerializeField] private GameObject _new = null;
		private InputField _newField;
		private Animator _newAnimator;
		[SerializeField] private GameObject _confirm = null;
		private InputField _confirmField;
		private Animator _confirmAnimator;
		[SerializeField] private Animator _changeButton = null;
		private void Awake()
		{
			_oldField = _old.GetComponent<InputField>();
			_oldAnimator = _old.GetComponent<Animator>();
			_newField = _new.GetComponent<InputField>();
			_newAnimator = _new.GetComponent<Animator>();
			_confirmField = _confirm.GetComponent<InputField>();
			_confirmAnimator = _confirm.GetComponent<Animator>();
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
		private void ClearError()
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
		private void DisableInput()
		{
			_changeButton.SetBool(Constants.AnimatorCompute, true);
			gameObject.SetInteractable(false);
		}
		private void EnableInput()
		{
			_changeButton.SetBool(Constants.AnimatorCompute, false);
			gameObject.SetInteractable(true);
		}
	}
}
