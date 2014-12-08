using Parse;
using System.Collections;
using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Data : Singleton<Data>
	{
		private int _page = 1;
		public int Page
		{
			get { return _page; }
			set
			{
				_page = value;
				StopAllCoroutines();
				StartCoroutine(Save(_page));
			}
		}
		private void Start()
		{
			Load();
		}
		private IEnumerator Save(int page)
		{
			yield return new WaitForSeconds(1f);
			if (Connection.Connected)
			{
				var task = ParseObject.GetQuery("Data").WhereEqualTo("userId", ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
				task.ContinueWith(t =>
				{
					ParseObject p = (t.Result != null) ? t.Result : new ParseObject("Data");
					p["page"] = page;
					p["userId"] = ParseUser.CurrentUser.ObjectId;
					p.SaveAsync();
				});
			}
		}
		public void Load()
		{
			if (Connection.Connected)
			{
				var task = ParseObject.GetQuery("Data").WhereEqualTo("userId", ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
				task.ContinueWith(t =>
				{
					if (t.Result != null)
					{
						Loom.QueueOnMainThread(() => { Page = t.Result.Get<int>("page"); });
					}
				});
			}
		}
	}
}
