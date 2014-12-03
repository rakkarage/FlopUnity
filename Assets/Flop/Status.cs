using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Status : Singleton<Status>
	{
		public Text EmailText;
		public GameObject Tip;
		private Image _image;
		private Image _child;
		private bool _highlight = false;
		private void Awake()
		{
			_image = GetComponent<Image>();
			_child = transform.FindChild("Image").GetComponent<Image>();
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
				_image.color = Constants.ErrorGreen.SetAlpha(_highlight ? .5f : 0f);
				_child.color = Constants.ErrorGreen.SetAlpha(.5f);
			}
			else
			{
				_image.color = Constants.ErrorRed.SetAlpha(_highlight ? .5f : 0f);
				_child.color = Constants.ErrorRed.SetAlpha(.5f);
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
