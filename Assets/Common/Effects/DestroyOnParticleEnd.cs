using System.Collections;
using UnityEngine;

namespace Overheat.Common.Effects
{
	public sealed class DestroyOnParticleEnd : MonoBehaviour
	{
		void Awake()
		{
			if (TryGetComponent(out ParticleSystem particleSystem)) {
				StartCoroutine(DestroyOnPlaybackEnd(particleSystem));
			}

			enabled = false;
		}

		private static IEnumerator DestroyOnPlaybackEnd(ParticleSystem particleSystem)
		{
			yield return new WaitForSeconds(particleSystem.main.duration - particleSystem.time);

			Destroy(particleSystem.gameObject);
		}
	}
}
