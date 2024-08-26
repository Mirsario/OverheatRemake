using System;
using Overheat.Common.Movement;
using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Looking
{
	[Serializable]
	public struct CameraRotationEffect
	{
		[SerializeField] public float Intensity;
		[SerializeField] public float Damping;
		[SerializeField, HideInInspector] public float Current;
		[SerializeField, HideInInspector] public float Target;

		public CameraRotationEffect(float intensity, float damping)
		{
			Current = Target = 0f;
			Intensity = intensity;
			Damping = damping;
		}

		public readonly float Get()
		{
			return Current * Intensity;
		}

		public void Update(float deltaTime)
		{
			Current = Damping > 0f ? MathUtils.Damp(Current, Target, Damping * Damping, deltaTime) : Target;
			Target = 0f;
		}

		public float UpdateAndGet(float deltaTime)
		{
			Update(deltaTime);
			return Get();
		}
	}

	public sealed class CameraEffects : MonoBehaviour
	{
		[SerializeField] private Velocity velocity;

		public CameraRotationEffect HorizontalVelocityTilt = new(1f, 0.1f);
		public CameraRotationEffect HorizontalVelocityPitch = new(1f, 0.1f);
		public CameraRotationEffect VerticalVelocityPitch = new(1f, 0.1f);

		void Update()
		{
			var rotation = Vector3.zero;
			float deltaTime = Time.deltaTime;

			if (velocity != null || (velocity = GetComponentInParent<Velocity>()) != null) {
				var eulerAngles = transform.eulerAngles;
				var globalVelocity = velocity.Value;
				var localVelocity = Quaternion.Euler(0f, -eulerAngles.y, 0f) * globalVelocity;

				VerticalVelocityPitch.Target = localVelocity.y;
				HorizontalVelocityPitch.Target = localVelocity.z;
				HorizontalVelocityTilt.Target = -localVelocity.x;
			}

			rotation.z += HorizontalVelocityTilt.UpdateAndGet(deltaTime);
			rotation.x += HorizontalVelocityPitch.UpdateAndGet(deltaTime);
			rotation.x += VerticalVelocityPitch.UpdateAndGet(deltaTime);

			transform.localEulerAngles = rotation;
		}
	}
}
