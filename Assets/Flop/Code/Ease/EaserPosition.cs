namespace ca.HenrySoftware.Flop
{
	public class EaserPosition : Easer
	{
		public override void Go()
		{
			if (By)
				Ease3.GoPositionBy(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat);
			else
				Ease3.GoPositionTo(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat);
		}
	}
}
