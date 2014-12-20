using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Audio : Singleton<Audio>
	{
		[SerializeField] private AudioClip _click = null;
		[SerializeField] private AudioClip _error = null;
		private AudioSource _source;
		private const float VolumeMin = .5f;
		private const float VolumeMax = 1f;
		private const float PitchMin = .75f;
		private const float PitchMax = 1.25f;
		private void Awake()
		{
			_source = GetComponent<AudioSource>();
		}
		private void RandomPlay(AudioClip sound)
		{
			var oldPitch = _source.pitch;
			_source.pitch = Random.Range(PitchMin, PitchMax);
			_source.PlayOneShot(sound, Random.Range(VolumeMin, VolumeMax));
			_source.pitch = oldPitch;
		}
		public void PlayClick()
		{
			RandomPlay(_click);
		}
		public void PlayError()
		{
			RandomPlay(_error);
		}
	}
}
