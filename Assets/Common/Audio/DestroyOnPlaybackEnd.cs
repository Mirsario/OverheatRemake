using UnityEngine;

namespace Overheat.Common.Audio
{
	public sealed class DestroyOnPlaybackEnd : MonoBehaviour
	{
		void Awake()
		{
			if (TryGetComponent(out AudioSource audioSource)) {
				AudioPlayback.DestroyObjectOnPlaybackEnd(audioSource);
			}

			enabled = false;
		}
	}
}
