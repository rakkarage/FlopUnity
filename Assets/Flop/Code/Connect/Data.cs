using Parse;
using System.Collections;
using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Data : Singleton<Data>
	{
		private int _page = -1;
		public int Page
		{
			get { return _page; }
			set
			{
				_page = value;
				StopAllCoroutines();
				StartCoroutine(Save());
			}
		}
		private int _pageBig = -1;
		public int PageBig
		{
			get { return _pageBig; }
			set
			{
				_pageBig = value;
				StopAllCoroutines();
				StartCoroutine(Save());
			}
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
					p["pageBig"] = _pageBig;
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
							_page = t.Result.Get<int>("page");
							_pageBig = t.Result.Get<int>("pageBig");
						});
					}
				});
			}
		}
	}
}
