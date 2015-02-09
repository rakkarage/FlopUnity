using ca.HenrySoftware.Flop;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlowSprites : Flow<Sprite>
{
	[SerializeField] private Button _prev = null;
	[SerializeField] private Button _next = null;
	[SerializeField] private Scrollbar _scrollbar = null;
	private Text _scrollbarText;
	private bool _ignore;
	[SerializeField] private List<Sprite> _data;
	protected override List<Sprite> Data
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
	protected override void Apply(GameObject o, Sprite d)
	{
		o.GetComponentsInChildren<Image>(true)[1].sprite = d;
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
