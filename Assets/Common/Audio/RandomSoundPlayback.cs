using System;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace Overheat.Common.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public sealed class RandomSoundPlayback : MonoBehaviour
	{
		public AudioClip[] Clips = Array.Empty<AudioClip>();

		private byte[] history;
		private byte historySize;
		private byte historyIndex;

		void Awake()
		{
			RandomizeClip();
		}

		public void RandomizeClip()
		{
			GetComponent<AudioSource>().clip = GetRandomClip();
		}

		public AudioClip GetRandomClip()
		{
			if (history?.Length is not > 0) {
				history = new byte[Clips.Length / 2];
			}

			byte index = GetRandomClipIndex();

			history[historyIndex] = index;
			historyIndex = (byte)((historyIndex + 1) % history.Length);

			if (historySize < history.Length) {
				historySize++;
			}

			return Clips[index];
		}

		private byte GetRandomClipIndex()
		{
			if (historySize == 0) {
				return (byte)UnityRandom.Range(0, Clips.Length);
			}

			Span<byte> pool = stackalloc byte[Clips.Length - historySize];

			byte i = 0;
			byte poolIndex = 0;

			while (poolIndex < pool.Length) {
				for (byte j = 0; j < historySize; j++) {
					if (history[j] == i) {
						goto SkipIndex;
					}
				}

				pool[poolIndex++] = i;

				SkipIndex: i++;
			}

			return pool[UnityRandom.Range(0, pool.Length)];
		}
	}
}
