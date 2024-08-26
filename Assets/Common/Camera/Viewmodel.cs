using System;
using Overheat.Common.Movement;
using Overheat.Core.Signals;
using Overheat.Core.Utilities;
using UnityEngine;
using NQuaternion = System.Numerics.Quaternion;

namespace Overheat.Common.Camera
{
	[RequireComponent(typeof(TransformReset))]
	public sealed class Viewmodel : MonoBehaviour
	{
		public Signals Body;
		public Transform CameraTransform;
		public bool InverseRotation;
		public float RotationDamping = 0.1f;
		public float RotationMultiplier = 1.0f;
		public float BobbingYRange = 0.25f;
		public float BobbingSpeedMultiplier = 0.5f;
		public float BobbingDamping = 0.1f;
		public float OffsetDamping = 0.1f;
		public Vector3 Offset;

		private float sineAccumulator;
		private float bobbingIntensity;
		private NQuaternion dampedRotation;

		void OnEnable()
		{
			var resetter = GetComponent<TransformReset>();
			resetter.ResetLocalPosition = true;
			resetter.ResetLocalRotation = true;
			resetter.ResetOnRenderUpdate = true;
		}

		void LateUpdate()
		{
			//UpdateBobbing();

			var targetRotation = CameraTransform.rotation.ToNumerics();
			dampedRotation = MathUtils.Damp(dampedRotation, targetRotation, RotationDamping * RotationDamping, Time.deltaTime);
			var localRotation = dampedRotation * NQuaternion.Inverse(targetRotation);
			if (InverseRotation) { localRotation = NQuaternion.Inverse(localRotation); }
			if (RotationMultiplier != 1f) { localRotation = NQuaternion.Lerp(NQuaternion.Identity, localRotation, RotationMultiplier); }
			//var rotation = (localRotation * targetRotation).ToUnity();

			Offset = MathUtils.Damp(Offset, Vector3.zero, OffsetDamping * OffsetDamping, Time.deltaTime);
			var position = Vector3.zero;
			//position += GetBobbingVector();
			//position += (localRotation * targetRotation).ToUnity() * Offset;
			position += Offset;

			transform.SetLocalPositionAndRotation(transform.localPosition + position, transform.localRotation * localRotation.ToUnity());
		}

		public void UpdateBobbing()
		{
			float speed = 1f;
			float targetIntensity = 0f;

			if (Body.TryGetComponent(out Velocity velocity)) {
				speed = new Vector2(velocity.Value.x, velocity.Value.z).magnitude;

				if (!Body.TryGetComponent(out CollisionInfo collisionInfo) || collisionInfo.IsOnGround()) {
					targetIntensity = MathF.Min(1f, speed);
				}
			}

			bobbingIntensity = MathUtils.Damp(bobbingIntensity, targetIntensity, BobbingDamping * BobbingDamping, Time.deltaTime);
			sineAccumulator += Time.deltaTime * BobbingSpeedMultiplier * speed;
		}

		public Vector3 GetBobbingVector()
		{
			float step = MathF.Sin(sineAccumulator);
			var vector = Vector3.zero;

			vector.y = step * BobbingYRange * bobbingIntensity * 0.5f;

			return vector;
		}
	}
}
