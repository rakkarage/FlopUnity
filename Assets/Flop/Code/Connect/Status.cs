using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Status : Singleton<Status>
	{
		public Text EmailText;
		public GameObject Tip;
		private Image _image;
		private bool _highlight = false;
		private void Awake()
		{
			_image = GetComponent<Image>();
		}
		private void OnEnable()
		{
			UpdateColor();
			Connect.EmailChangedEvent += HandleEmailChanged;
		}
		private void OnDisable()
		{
			Connect.EmailChangedEvent -= HandleEmailChanged;
		}
		private void HandleEmailChanged(string email)
		{
			if (!string.IsNullOrEmpty(email))
				EmailText.text = email;
		}
		public void UpdateColor()
		{
			if (Connection.Connected)
			{
				_image.color = Constants.StatusGreen.SetAlpha(_highlight ? 1f : .5f);
			}
			else
			{
				_image.color = Constants.StatusRed.SetAlpha(_highlight ? 1f : .5f);
			}
		}
		public void StatusClicked()
		{
			Audio.Instance.PlayClick();
			if (Connection.Connected)
				Connect.Instance.SpringAccount();
			else
				Connect.Instance.SpringSignIn();
		}
		public void Enter()
		{
			if (Connection.Connected)
				Tip.SetActive(true);
			_highlight = true;
			UpdateColor();
		}
		public void Exit()
		{
			Tip.SetActive(false);
			_highlight = false;
			UpdateColor();
		}
	}
}
