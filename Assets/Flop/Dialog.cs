using UnityEngine;
using UnityEngine.UI;
namespace ca.HenrySoftware.Flop
{
	public class Dialog : Singleton<Dialog>
	{
		public Text Title;
		public Text Text;
		private MonoBehaviour _m;
		private void Start()
		{
			_m = GetComponent<Dialog>();
		}
		public void Show(string text)
		{
			Title.text = Constants.Error;
			Text.text = text;
			Spring();
		}
		public void Spring()
		{
			Utility.Spring(_m, Constants.OffsetDialog);
		}
		public void SpringBack()
		{
			Utility.Spring(_m, Vector3.zero);
		}
	}
}
