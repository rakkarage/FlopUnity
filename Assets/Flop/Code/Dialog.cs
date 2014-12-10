﻿using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Dialog : Singleton<Dialog>
	{
		public Text Title;
		public Text Text;
		private static readonly Vector3 _offsetDialog = new Vector3(Constants.Offset, 0f, 0f);
		public void ShowError(string text)
		{
			Title.text = Constants.Error;
			Text.text = text;
			Spring();
		}
		public void Spring()
		{
			Utility.Spring(this, _offsetDialog);
		}
		public void SpringBack()
		{
			Utility.Spring(this, Vector3.zero);
		}
	}
}
