using System.Collections.Generic;
using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Pool : MonoBehaviour
	{
		public int Count = 12;
		public GameObject Prefab;
		private List<GameObject> _pool;
		private const string _name = "Pool";
		private void Awake()
		{
			_pool = new List<GameObject>(Count);
			for (var i = 0; i < Count; i++)
				_pool.Add(New());
		}
		private GameObject New()
		{
			var o = Instantiate(Prefab) as GameObject;
			if (o != null)
			{
				o.transform.SetParent(gameObject.transform, false);
				o.transform.position = Vector3.zero;
				o.name = _name;
				o.SetActive(false);
			}
			return o;
		}
		public GameObject Enter()
		{
			GameObject o = null;
			for (int i = 0; (i < _pool.Count) && (o == null); i++)
			{
				if (!_pool[i].activeInHierarchy)
					o = _pool[i];
			}
			if (o == null)
			{
				o = New();
				_pool.Add(o);
			}
			o.SetActive(true);
			return o;
		}
		public void Exit(GameObject o)
		{
			if (o != null)
			{
				o.name = _name;
				o.SetActive(false);
			}
		}
	}
}
