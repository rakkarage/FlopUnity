using ca.HenrySoftware.Flop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class FlowNumber : Flow<int>
{
	[SerializeField] private Button _prev = null;
	[SerializeField] private Button _next = null;
	[SerializeField] private Scrollbar _scrollbar = null;
	private Text _scrollbarText;
	private bool _ignore;
	private readonly List<int> _data = Enumerable.Range(1000000, 1000000).ToList();
	protected override List<int> Data
	{
		get { return _data; }
	}
	protected override void Start()
	{
		_scrollbarText = _scrollbar.GetComponentInChildren<Text>();
		_scrollbar.numberOfSteps = _data.Count;
		base.Start();
	}
	private void OnEnable()
	{
		_scrollbar.onValueChanged.AddListener(OnScroll);
	}
	private void OnDisable()
	{
		_scrollbar.onValueChanged.RemoveListener(OnScroll);
	}
	private void OnScroll(float scroll)
	{
        if (!_ignore)
            Scroll(scroll);
	}
	protected override void Apply(GameObject o, int d)
	{
		var data = d.ToString("x");
        foreach (var text in o.GetComponentsInChildren<Text>(true))
			text.text = data;
	}
	protected override void UpdateCurrent(int current)
	{
		_prev.interactable = !First;
		_next.interactable = !Last;
        _ignore = true;
		_scrollbar.value = GetPercent(current);
        _ignore = false;
        _scrollbarText.text = current.ToString();
	}
}
