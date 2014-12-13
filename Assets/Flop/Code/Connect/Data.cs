using Parse;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace ca.HenrySoftware.Flop
{
	[ParseClassName("Data")]
	public class ParseData : ParseObject
	{
		[ParseFieldName("page")]
		public int Page
		{
			get { return GetProperty<int>("Page"); }
			set { SetProperty<int>(value, "Page"); }
		}
		[ParseFieldName("pageBig")]
		public int PageBig
		{
			get { return GetProperty<int>("PageBig"); }
			set { SetProperty<int>(value, "PageBig"); }
		}
		[ParseFieldName("userId")]
		public string UserId
		{
			get { return GetProperty<string>("UserId"); }
			set { SetProperty<string>(value, "UserId"); }
		}
	}
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
		private void Awake()
		{
			ParseObject.RegisterSubclass<ParseData>();
			Load();
		}
		private IEnumerator Save()
		{
			yield return new WaitForSeconds(.5f);
			if (Connection.Connected)
			{
				var task = new ParseQuery<ParseData>().WhereEqualTo("userId", ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
				task.ContinueWith(t =>
				{
					var p = t.Result ?? new ParseData();
					p.Page = _page;
					p.PageBig = _pageBig;
					p.UserId = ParseUser.CurrentUser.ObjectId;
					p.SaveAsync();
				});
			}
		}
		public static UnityAction LoadedEvent;
		private void Load()
		{
			if (Connection.Connected)
			{
				var task = new ParseQuery<ParseData>().WhereEqualTo("userId", ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
				task.ContinueWith(t =>
				{
					if (t.Result != null && !(t.IsFaulted || t.IsCanceled))
					{
						Loom.QueueOnMainThread(() =>
						{
							_page = t.Result.Page;
							_pageBig = t.Result.PageBig;
							if (LoadedEvent != null) LoadedEvent();
						});
					}
				});
			}
		}
	}
}
