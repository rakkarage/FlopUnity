using System.Collections.Generic;
using UnityEngine;
public class Pool : Singleton<Pool>
{
	public int Count = 10;
	public GameObject Prefab;
	private List<GameObject> _pool;
	private const string _name = "Pool";
	private void Awake()
	{
		_pool = new List<GameObject>(Count);
		for (int i = 0; i < Count; i++)
		{
			_pool.Add(New());
		}
	}
	private GameObject New()
	{
		GameObject o = Instantiate(Prefab) as GameObject;
		o.transform.SetParent(gameObject.transform, false);
		o.transform.position = Vector3.zero;
		o.name = _name;
		o.SetActive(false);
		return o;
	}
	public GameObject Enter()
	{
		GameObject o = null;
		for (int i = 0; (i < _pool.Count) && (o == null); i++)
		{
			if (!_pool[i].activeInHierarchy)
			{
				o = _pool[i];
			}
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
