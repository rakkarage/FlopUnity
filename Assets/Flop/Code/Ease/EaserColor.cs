using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class EaserColor : Easer<Color>
	{
		public override void Go()
		{
			if (By)
				Ease3.GoColorBy(this, Value.GetVector(), Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
			else
				Ease3.GoColorTo(this, Value.GetVector(), Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
		}
	}
}
