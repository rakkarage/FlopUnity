using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class EaserPosition : Easer<Vector3>
	{
		public override void Go()
		{
			if (By)
				Ease3.GoPositionBy(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong);
			else
				Ease3.GoPositionTo(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat, PingPong);
		}
	}
}
