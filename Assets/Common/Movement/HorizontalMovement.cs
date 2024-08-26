using System;
using Overheat.Core.Signals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Overheat.Common.Movement
{
	[RequireComponent(typeof(Signals))]
	[RequireComponent(typeof(Velocity))]
	[RequireComponent(typeof(CollisionInfo))]
	public sealed class HorizontalMovement : MonoBehaviour
	{
		public bool EnableBunnyhopping = true;
		public float AirAcceleration = 0.1f;
		public float AirMaxSpeed = 1f;
		public float GroundAcceleration = 1f;
		public float GroundMaxSpeed = 10f;
		public InputActionReference Input;

		private Velocity velocity;
		private Signals signals;
		private CollisionInfo collisionInfo;

		void Awake()
		{
			signals = GetComponent<Signals>();
			velocity = GetComponent<Velocity>();
			collisionInfo = GetComponent<CollisionInfo>();
		}

		void FixedUpdate()
		{
			var movementInput = signals.Values2D(Input).normalized;

			if (movementInput == default) {
				//return;
			}

			//var groundNormalRotation = Quaternion.LookRotation(Vector3.up) * Quaternion.Inverse(Quaternion.LookRotation(collisionInfo.GroundNormal));
			//var forwardDirection = groundNormalRotation * transform.forward;
			//var rightDirection = groundNormalRotation * transform.right;
			var forwardDirection = Vector3.Cross(transform.right, collisionInfo.GroundNormal);
			var rightDirection = Vector3.Cross(-transform.forward, collisionInfo.GroundNormal);

			Debug.DrawRay(transform.position, forwardDirection * 5f, Color.blue);
			Debug.DrawRay(transform.position, rightDirection * 5f, Color.red);

			var wishDirection = ((forwardDirection * movementInput.y) + (rightDirection * movementInput.x)).normalized;
			bool isOnGround = collisionInfo.IsOnGround();
			float maxSpeed = isOnGround ? GroundMaxSpeed : AirMaxSpeed;
			float acceleration = isOnGround ? GroundAcceleration : AirAcceleration;

			ApplyAcceleration(acceleration * Time.fixedDeltaTime, wishDirection, maxSpeed);
		}

		public void ApplyAcceleration(float acceleration, Vector3 wishDirection, float wishSpeed)
		{
			if (EnableBunnyhopping) {
				ApplyQuakeAcceleration(acceleration, wishDirection, wishSpeed);
			} else {
				ApplyFixedAcceleration(acceleration, wishDirection, wishSpeed);
			}
		}

		private void ApplyQuakeAcceleration(float acceleration, Vector3 wishDirection, float wishSpeed)
		{
			float currentSpeed = Vector3.Dot(velocity.Value, wishDirection);
			float addSpeed = wishSpeed - currentSpeed;

			if (addSpeed <= 0f) {
				return;
			}

			float accelSpeed = Math.Min(acceleration * wishSpeed, addSpeed);

			velocity.Value += accelSpeed * wishDirection;
		}

		// Buggier, actually
		private void ApplyFixedAcceleration(float acceleration, Vector3 wishDirection, float wishSpeed)
		{
			var wishVelocity = wishDirection * wishSpeed;
			var pushDirection = wishVelocity - velocity.Value;
			float pushLength = pushDirection.magnitude;

			pushDirection *= pushLength; // Normalization

			float accelerationSpeed = acceleration * wishSpeed;

			if (accelerationSpeed > pushLength) {
				accelerationSpeed = pushLength;
			}

			velocity.Value += pushDirection * accelerationSpeed;
		}
	}
}
