using System.Collections;
using UnityEngine;

namespace Overheat.Common.Audio
{
	public struct AudioPlaybackParameters
	{
		public Vector3 Position;
		public Quaternion Rotation;
		public Transform Transform;
		public bool DestroyOnStop;
	}

	public sealed class AudioPlayback : MonoBehaviour
	{
		private static AudioPlayback instance;

		void Start()
		{
			instance = this;
		}

		private IEnumerator DestroyOnPlaybackEnd(AudioSource source)
		{
			float currentTime = source.time;
			float clipLength = source.clip.length;
			float timeLeft = clipLength - currentTime;

			yield return new WaitForSeconds(timeLeft);

			Destroy(source.gameObject);
		}

		public static void DestroyObjectOnPlaybackEnd(AudioSource audioSource)
		{
			instance.StartCoroutine(instance.DestroyOnPlaybackEnd(audioSource));
		}

		/*
		public static void PlaySound(GameObject prefab, in AudioPlaybackParameters parameters)
		{
			bool resetClip = false;

			if (prefab.TryGetComponent(out RandomSoundPlayback randomSoundPlayback)) {
				randomSoundPlayback.RandomizeClip();
				resetClip = true;
			}

			var gameObject = Instantiate(prefab);
			var transform = gameObject.transform;
			var audioSource = gameObject.GetComponent<AudioSource>();

			if (parameters.Transform != null) {
				transform = parameters.Transform;
			}

			transform.localPosition = parameters.Position;

			if (parameters.Rotation != default) {
				transform.localRotation = parameters.Rotation;
			}

			DestroyObjectOnPlaybackEnd(audioSource);

			if (resetClip) {
				prefab.GetComponent<AudioSource>().clip = null;
			}
		}
		*/
	}
}
