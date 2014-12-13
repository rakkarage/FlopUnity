using UnityEngine;
namespace ca.HenrySoftware.Flop
{
	public class Audio : Singleton<Audio>
	{
		[SerializeField]
		private AudioClip _click = null;
		[SerializeField]
		private AudioClip _error = null;
		private AudioSource _source;
		private const float _volumeMin = .5f;
		private const float _volumeMax = 1f;
		private const float _pitchMin = .75f;
		private const float _pitchMax = 1.25f;
		private void Awake()
		{
			_source = GetComponent<AudioSource>();
		}
		private float RandomVolume()
		{
			return Random.Range(_volumeMin, _volumeMax);
		}
		private float RandomPitch()
		{
			return Random.Range(_pitchMin, _pitchMax);
		}
		private void RandomPlay(AudioClip sound)
		{
			var oldPitch = _source.pitch;
			_source.pitch = RandomPitch();
			_source.PlayOneShot(sound, RandomVolume());
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
