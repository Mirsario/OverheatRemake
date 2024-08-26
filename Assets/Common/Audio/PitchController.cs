using UnityEngine;

namespace Overheat.Common.Audio
{
	public sealed class PitchController : MonoBehaviour
	{
		[SerializeField, HideInInspector] private float initialPitch;
		private AudioSource audioSource;

		public Vector2 PitchRange = new(0.9f, 1.2f);

		void Awake()
		{
			if (TryGetComponent(out audioSource) && !audioSource.isPlaying) {
				initialPitch = Random.Range(PitchRange.x, PitchRange.y);
				Update();
				audioSource.Play();
			}
		}

		void Update()
		{
			audioSource.pitch = initialPitch * Time.timeScale;
		}
	}
}
