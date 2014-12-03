using UnityEngine;
using System.Collections;
namespace ca.HenrySoftware.Flop
{
	public class Audio : Singleton<Audio>
	{
		public float VolumeMin = .5f;
		public float VolumeMax = 1f;
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
		private float RandomPitch()
		{
			return Random.Range(PitchMin, PitchMax);
		}
		private void RandomSound(AudioClip sound)
		{
			var oldPitch = _source.pitch;
			_source.pitch = RandomPitch();
			_source.PlayOneShot(sound, RandomVolume());
			_source.pitch = oldPitch;
		}
		public void PlayButton0()
		{
			RandomSound(Button0);
		}
		public void PlayButton1()
		{
			RandomSound(Button1);
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
