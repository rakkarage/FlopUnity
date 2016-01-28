using ca.HenrySoftware.Flop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class FlowBasic : Flow<int>
{
	private readonly List<int> _data = Enumerable.Range(1, 10).ToList();
	protected override List<int> Data
	{
		get { return _data; }
	}
	protected override void Apply(GameObject o, int d)
	{
		var data = ((char)d).ToString();
		foreach (var text in o.GetComponentsInChildren<Text>(true))
			text.text = data;
	}
	protected override void UpdateCurrent(int current)
	{
	}
}
