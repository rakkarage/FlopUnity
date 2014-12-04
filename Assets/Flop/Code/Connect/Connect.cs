using Parse;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	public class Connect : Singleton<Connect>
	{
		public static UnityAction<string> EmailChangedEvent;
		private MonoBehaviour _m;
		private void Start()
		{
			_m = GetComponent<Connect>();
		}
		private void OnEnable()
		{
			StartCoroutine(UpdateEmail());
		}
		private IEnumerator UpdateEmail()
		{
			yield return null;
			string email = Prefs.Email;
			if (!string.IsNullOrEmpty(email))
			{
				SetEmail(email);
			}
		}
		public void SetData(int data)
		{
			if (Connection.Connected)
			{
				var p = new ParseObject("Data");
				p["data"] = data;
				Task saveTask = p.SaveAsync();
			}
		}
		public int GetData()
		{
			int data = 1;
			if (Connection.Connected)
			{
				ParseQuery<ParseObject> query = ParseObject.GetQuery("Data");
				query.FirstOrDefaultAsync().ContinueWith(t =>
				{
					ParseObject p = t.Result;
                    data = (int)p["data"];
				});
			}
			return data;
		}
		public void SetEmail(string email)
		{
			Prefs.Email = email;
			if (EmailChangedEvent != null)
				EmailChangedEvent(email);
		}
		public void SpringSignIn()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(_m, Constants.OffsetSignIn);
		}
		public void SpringRegister()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(_m, Constants.OffsetRegister);
		}
		public void SpringReset()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(_m, Constants.OffsetReset);
		}
		public void SpringAccount()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(_m, Constants.OffsetAccount);
		}
		public void SpringChange()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Utility.Spring(_m, Constants.OffsetChange);
		}
		public void SpringBack()
		{
			Audio.Instance.PlayClick();
			Dialog.Instance.SpringBack();
			Status.Instance.UpdateColor();
			Utility.Spring(_m, Vector3.zero);
		}
	}
}
