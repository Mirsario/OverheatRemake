using System;
using UnityEngine;

namespace Overheat.Common.EFfects
{
	public sealed class CooldownEffects : MonoBehaviour
	{
		[Tooltip("If true, the gameobject will be destroyed after cooling down.")]
		public bool DestroyAfterCooldown;

		[Tooltip("Whether to clone the material before modifying it.")]
		public bool CloneMaterial = true;

		[Tooltip("The current heat factor.")]
		public float Intensity = 1f;

		[Tooltip("Time in seconds that the cooling down process should last.")]
		public float CooldownTime = 1f;

		[Tooltip("If set, the cooldown will not start until the provided component is disabled.")]
		public MonoBehaviour BaseBehavior;

		[Tooltip("The light to modify color for.")]
		public Light Light;

		[Tooltip("The renderer to modify material's emission of.")]
		public Renderer Renderer;

		[Tooltip("The material to use after completely cooling down. Providing this improves performance.")]
		public Material FinalMaterial;

		private Material material;
		private int emissionColorParameter;
		private float baseLightIntensity;
		private Color baseMaterialEmission;

		void Awake()
		{
			if (Light != null) {
				baseLightIntensity = Light.intensity;
			}

			if (Renderer != null) {
				if (CloneMaterial) {
					material = Renderer.material = Instantiate(Renderer.material);
				} else {
					material = Renderer.material;
				}

				emissionColorParameter = Shader.PropertyToID("_EmissionColor");
				baseMaterialEmission = material.GetColor(emissionColorParameter);
			}
		}

		void Update()
		{
			if (BaseBehavior != null && BaseBehavior.enabled) {
				return;
			}

			Intensity = MathF.Max(0f, Intensity - (Time.deltaTime / CooldownTime));

			float factor = Intensity * Intensity * Intensity;

			if (Light != null) {
				Light.intensity = baseLightIntensity * factor;
			}

			if (material != null) {
				material.SetColor(emissionColorParameter, Color.Lerp(Color.clear, baseMaterialEmission, factor));
			}

			if (Intensity <= 0f) {
				if (FinalMaterial != null) {
					Renderer.material = FinalMaterial;
				}

				if (CloneMaterial) {
					Destroy(material);
				}

				if (Light != null) {
					Light.enabled = false;
				}

				if (DestroyAfterCooldown) {
					Destroy(gameObject);
				}

				material = null;
				enabled = false;
			}
		}
	}
}
