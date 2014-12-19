using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class EaserColor : Easer<Vector3>
	{
		public override void Go()
		{
			if (By)
				Ease3.GoColorBy(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
			else
				Ease3.GoColorTo(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong, RealTime);
		}
	}
}
