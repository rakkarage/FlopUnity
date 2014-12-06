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
				_m.StopAllCoroutines();
				_m.StartCoroutine(Save(_page));
			}
		}
		private MonoBehaviour _m;
		private void Awake()
		{
			_m = GetComponent<Data>();
		}
		private void Start()
		{
			Load();
		}
		private IEnumerator Save(int page)
		{
			yield return new WaitForSeconds(1);
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
