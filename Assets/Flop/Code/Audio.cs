using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Audio : Singleton<Audio>
	{
		public AudioClip Button0;
		public AudioClip Button1;
		public AudioClip Click;
		public AudioClip Error;
		private AudioSource _source;
		private float _volumeMin = .5f;
		private float _volumeMax = 1f;
		private float _volumeLowMin = .3f;
		private float _volumeLowMax = .6f;
		private float _pitchMin = .75f;
		private float _pitchMax = 1.25f;
		private void Awake()
		{
			_source = GetComponent<AudioSource>();
		}
		private float RandomVolume()
		{
			return Random.Range(_volumeMin, _volumeMax);
		}
		private float RandomVolumeLow()
		{
			return Random.Range(_volumeLowMin, _volumeLowMax);
		}
		private float RandomPitch()
		{
			return Random.Range(_pitchMin, _pitchMax);
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
