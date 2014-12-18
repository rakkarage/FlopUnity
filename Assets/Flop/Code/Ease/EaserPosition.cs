namespace ca.HenrySoftware.Flop
{
	public class EaserPosition : Easer
	{
		public override void Go()
		{
			if (_by)
				Ease3.GoPositionBy(this, _value, _time, null, _complete.Invoke, _type, _delay, _repeat);
			else
				Ease3.GoPositionTo(this, _value, _time, null, _complete.Invoke, _type, _delay, _repeat);
		}
	}
}
