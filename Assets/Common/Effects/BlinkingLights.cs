using UnityEngine;

namespace Overheat.Common.Effects
{
	[RequireComponent(typeof(Light))]
	public sealed class BlinkingLights : MonoBehaviour
	{
		public Vector2 BlinkLengthRange = new(0.1f, 0.2f); 
		public Vector2 BlinkDelayRange = new(0.02f, 2.0f);
		public float BlinkIntensity = 0.25f;

		private new Light light;
		private float nextBlinkTime;
		private float blinkEndTime;

		[SerializeField, HideInInspector]
		private float baseIntensity;

		void Awake()
		{
			light = GetComponent<Light>();
			baseIntensity = light.intensity;
		}

		void Update()
		{
			float currentTime = Time.time;

			if (currentTime >= blinkEndTime) {
				light.intensity = baseIntensity;

				blinkEndTime = float.PositiveInfinity;
				nextBlinkTime = currentTime + Random.Range(BlinkDelayRange.x, BlinkDelayRange.y);
			} else if (currentTime >= nextBlinkTime) {
				light.intensity = BlinkIntensity;

				nextBlinkTime = float.PositiveInfinity;
				blinkEndTime = currentTime + Random.Range(BlinkLengthRange.x, BlinkLengthRange.y);
			}
		}
	}
}
