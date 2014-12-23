using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Dialog : Singleton<Dialog>
	{
		[SerializeField] private Text _title = null;
		[SerializeField] private Text _text = null;
		private static readonly Vector3 Offset = new Vector3(Constant.Offset, 0f, 0f);
		public void ShowError(string text)
		{
			_title.text = Constant.Error;
			_text.text = text;
			Spring();
		}
		public void Spring()
		{
			Utility.Spring(this, Offset);
		}
		public void SpringBack()
		{
			Utility.Spring(this, Vector3.zero);
		}
	}
}
