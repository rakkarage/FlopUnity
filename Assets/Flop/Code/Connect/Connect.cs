using Parse;
using System;
using System.Collections;
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
			try
			{
				if (Connection.Connected)
				{
					var task = ParseObject.GetQuery("Data").WhereEqualTo("userId", ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
					task.ContinueWith(t =>
					{
						ParseObject p = (t.Result != null) ? t.Result : new ParseObject("Data");
						p["page"] = data;
						p["userId"] = ParseUser.CurrentUser.ObjectId;
						p.SaveAsync();
					});
				}
			}
			catch (Exception e)
			{
				Utility.LogError(e);
			}
		}
		public void GetData()
		{
			try
			{
				if (Connection.Connected)
				{
					var task = ParseObject.GetQuery("Data").WhereEqualTo("userId", ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
					task.ContinueWith(t =>
					{
						if (t.Result != null)
						{
							ParseObject p = t.Result;
							var page = p.Get<int>("page");
							Loom.QueueOnMainThread(() => { Flow.Instance.TweenBy(page); });
						}
						else
						{
							Loom.QueueOnMainThread(() => { Flow.Instance.Next(); });
						}
					});
				}
			}
			catch (Exception e)
			{
				Utility.LogError(e);
			}
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
