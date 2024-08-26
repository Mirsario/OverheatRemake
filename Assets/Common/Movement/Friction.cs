using System;
using UnityEngine;

namespace Overheat.Common.Movement
{
	[DefaultExecutionOrder(-10)]
	[RequireComponent(typeof(Velocity))]
	[RequireComponent(typeof(CollisionInfo))]
	public sealed class Friction : MonoBehaviour
	{
		public float AirStrength = 1.0f;
		public float GroundStrength = 1.0f;
		public float StopSpeed = 0.001f;

		private Velocity velocity;
		private CollisionInfo collisionInfo;

		void Awake()
		{
			velocity = GetComponent<Velocity>();
			collisionInfo = GetComponent<CollisionInfo>();
		}

		void FixedUpdate()
		{
			float strength = collisionInfo.IsOnGround() ? GroundStrength : AirStrength;

			ApplyFriction(ref velocity.Value, strength, StopSpeed);
		}

		public static void ApplyFriction(ref Vector3 velocity, float friction, float stopSpeed)
		{
			float speed = new Vector2(velocity.x, velocity.z).magnitude;
			float drop = Math.Max(speed, stopSpeed) * friction * Time.fixedDeltaTime;

			float newSpeed = Math.Max(0f, speed - drop);

			if (newSpeed != 0f) {
				newSpeed /= speed;
			}

			velocity.x *= newSpeed;
			velocity.z *= newSpeed;
		}
	}
}
