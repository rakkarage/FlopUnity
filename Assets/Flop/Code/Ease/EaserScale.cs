namespace ca.HenrySoftware.Flop
{
	public class EaserScale : Easer
	{
		public override void Go()
		{
			if (_by)
				Ease3.GoPositionTo(this, _value, _time, null, Complete.Invoke, _type, _delay, _repeat);
			else
				Ease3.GoPositionBy(this, _value, _time, null, Complete.Invoke, _type, _delay, _repeat);
		}
	}
}
