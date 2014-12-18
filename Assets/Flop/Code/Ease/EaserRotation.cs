namespace ca.HenrySoftware.Flop
{
	public class EaserRotation : Easer
	{
		public override void Go()
		{
			if (_by)
				Ease3.GoRotationTo(this, _value, _time, null, null, _type, _delay, _repeat);
			else
				Ease3.GoRotationBy(this, _value, _time, null, null, _type, _delay, _repeat);
		}
	}
}
