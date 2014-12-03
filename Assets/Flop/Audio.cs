using UnityEngine;
using System.Collections;
namespace ca.HenrySoftware.Flop
{
	public class Audio : Singleton<Audio>
	{
		public float VolumeMin = .777f;
		public float VolumeMax = 1f;
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
		public void PlayButton0()
		{
			_source.PlayOneShot(Button0, RandomVolume());
		}
		public void PlayButton1()
		{
			_source.PlayOneShot(Button1, RandomVolume());
		}
		public void PlayClick()
		{
			_source.PlayOneShot(Click, RandomVolume());
		}
		public void PlayError()
		{
			_source.PlayOneShot(Error, RandomVolume());
		}
	}
}
