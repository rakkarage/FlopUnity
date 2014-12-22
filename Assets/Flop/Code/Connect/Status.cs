using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Status : Singleton<Status>
	{
		[SerializeField]
		private MonoBehaviour _tip = null;
		[SerializeField]
		private Text _email = null;
		private Image _image;
		private bool _highlight = false;
		private const float Time = .333f;
		private void Awake()
		{
			_image = GetComponent<Image>();
		}
		private void Start()
		{
			UpdateColor();
		}
		private void OnEnable()
		{
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
			_image.StopAllCoroutines();
			var color = (Connection.Connected ? Constants.StatusGreen : Constants.StatusRed).SetAlpha(_highlight ? .75f : .25f);
            Ease4.GoColorTo(_image, color.GetVector4(), Time, null, null, EaseType.SineInOut, 0f, 1, false, true);
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
			_highlight = true;
			UpdateColor();
			ShowTooltip();
		}
		public void PointerExit()
		{
			_highlight = false;
			UpdateColor();
			HideTooltip();
		}
		private void ShowTooltip()
		{
			if (!Connection.Connected) return;
			_tip.gameObject.SetActive(true);
			_tip.StopAllCoroutines();
			Ease.GoAlphaTo(_tip, 1f, Time, null, null, EaseType.BounceOut, 0f, 1, false, true);
			Ease3.GoRotationTo(_tip, new Vector3(0f, 0f, 0f), Time, null, null, EaseType.BounceOut, 0f, 1, false, true);
		}
		private void HideTooltip()
		{
			if (!_tip.gameObject.activeInHierarchy) return;
			_tip.StopAllCoroutines();
			Ease.GoAlphaTo(_tip, 0f, Time * .5f, null, null, EaseType.CircOut, 0f, 1, false, true);
			Ease3.GoRotationTo(_tip, new Vector3(0f, 0f, 180f), Time, null, () => { _tip.gameObject.SetActive(false); }, EaseType.CircOut, 0f, 1, false, true);
		}
	}
}
