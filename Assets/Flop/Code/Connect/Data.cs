using Parse;
using System.Collections;
using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Data : Singleton<Data>
	{
		private bool _big = false;
		public bool Big
		{
			get { return _big; }
		}
		private int _page = -1;
		public int Page
		{
			get { return _page; }
		}
		public void SaveData(int page, bool big)
		{
			_big = big;
			_page = page;
			StopAllCoroutines();
			StartCoroutine(Save());
		}
		private void Start()
		{
			Load();
		}
		private IEnumerator Save()
		{
			yield return new WaitForSeconds(1f);
			if (Connection.Connected)
			{
				var task = ParseObject.GetQuery("Data").WhereEqualTo("userId", ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
				task.ContinueWith(t =>
				{
					ParseObject p = (t.Result != null) ? t.Result : new ParseObject("Data");
					p["page"] = _page;
					p["big"] = _big;
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
						Loom.QueueOnMainThread(() =>
						{
							_big = t.Result.Get<bool>("big");
							_page = t.Result.Get<int>("page");
						});
					}
				});
			}
		}
	}
}
