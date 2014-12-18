namespace ca.HenrySoftware.Flop
{
	public class EaserAlpha : Easer<float>
	{
		public override void Go()
		{
			if (By)
				Ease.GoAlphaBy(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat);
			else
				Ease.GoAlphaTo(this, Value, Time, null, Complete.Invoke, Type, Delay, Repeat);
		}
	}
}
