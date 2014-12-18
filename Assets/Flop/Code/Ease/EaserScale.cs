using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class EaserScale : Easer<Vector3>
	{
		public override void Go()
		{
			if (By)
				Ease3.GoScaleTo(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat);
			else
				Ease3.GoScaleBy(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat);
		}
	}
}
