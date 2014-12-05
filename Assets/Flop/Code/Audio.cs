using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Audio : Singleton<Audio>
	{
		public float VolumeMin = .5f;
		public float VolumeMax = 1f;
		public float VolumeLowMin = .3f;
		public float VolumeLowMax = .6f;
		public float PitchMin = .75f;
		public float PitchMax = 1.25f;
		public AudioClip Button0;
		public AudioClip Button1;
		public AudioClip Click;
		public AudioClip Error;
		private AudioSource _source;
		private void Awake()
		{
			_source = GetComponent<AudioSource>();
		}
		private float RandomVolume()
		{
			return Random.Range(VolumeMin, VolumeMax);
		}
		private float RandomVolumeLow()
		{
			return Random.Range(VolumeLowMin, VolumeLowMax);
		}
		private float RandomPitch()
		{
			return Random.Range(PitchMin, PitchMax);
		}
		private void RandomSound(AudioClip sound)
		{
			RandomSound(sound, false);
		}
		private void RandomSound(AudioClip sound, bool low)
		{
			var oldPitch = _source.pitch;
			_source.pitch = RandomPitch();
			_source.PlayOneShot(sound, low ? RandomVolumeLow() : RandomVolume());
			_source.pitch = oldPitch;
		}
		public void PlayButton0()
		{
			RandomSound(Button0, true);
		}
		public void PlayButton1()
		{
			RandomSound(Button1, true);
		}
		public void PlayClick()
		{
			RandomSound(Click);
		}
		public void PlayError()
		{
			RandomSound(Error);
		}
	}
}
