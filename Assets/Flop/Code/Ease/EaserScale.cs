namespace ca.HenrySoftware.Flop
{
	public class EaserScale : Easer
	{
		public override void Go()
		{
			if (_by)
				Ease3.GoScaleTo(this, _value, _time, null, _complete.Invoke, _type, _delay, _repeat);
			else
				Ease3.GoScaleBy(this, _value, _time, null, _complete.Invoke, _type, _delay, _repeat);
		}
	}
}
