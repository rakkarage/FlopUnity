using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Status : Singleton<Status>
	{
		[SerializeField]
		private GameObject _tip = null;
		[SerializeField]
		private Text _email = null;
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
				_email.text = email;
		}
		public void UpdateColor()
		{
			var alpha = _highlight ? .75f : .25f;
            _image.color = Connection.Connected ? Constants.StatusGreen.SetAlpha(alpha) : Constants.StatusRed.SetAlpha(alpha);
		}
		public void StatusClicked()
		{
			Audio.Instance.PlayClick();
			if (Connection.Connected)
				Connect.Instance.SpringAccount();
			else
				Connect.Instance.SpringSignIn();
		}
		public void PointerEnter()
		{
			if (Connection.Connected)
				_tip.SetActive(true);
			_highlight = true;
			UpdateColor();
		}
		public void PointerExit()
		{
			_tip.SetActive(false);
			_highlight = false;
			UpdateColor();
		}
	}
}
