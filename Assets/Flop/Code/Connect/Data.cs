using Parse;
using System.Collections;
using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Data : Singleton<Data>
	{
		private MonoBehaviour _m;
		private void Awake()
		{
			_m = GetComponent<Data>();
		}
		public void Set(int data)
		{
			_m.StopAllCoroutines();
			_m.StartCoroutine(FinishSet(data));
		}
		private IEnumerator FinishSet(int data)
		{
			yield return new WaitForSeconds(1);
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
		public void Get()
		{
			if (Connection.Connected)
			{
				var task = ParseObject.GetQuery("Data").WhereEqualTo("userId", ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
				task.ContinueWith(t =>
				{
					if (t.Result != null)
					{
						Loom.QueueOnMainThread(() => { Flow.Instance.TweenBy(t.Result.Get<int>("page")); });
					}
					else
					{
						Loom.QueueOnMainThread(() => { Flow.Instance.Next(); });
					}
				});
			}
		}
	}
}
