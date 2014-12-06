using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Dialog : Singleton<Dialog>
	{
		public Text Title;
		public Text Text;
		public void Show(string text)
		{
			Title.text = Constants.Error;
			Text.text = text;
			Spring();
		}
		public void Spring()
		{
			Utility.Spring(this, Constants.OffsetDialog);
		}
		public void SpringBack()
		{
			Utility.Spring(this, Vector3.zero);
		}
	}
}
