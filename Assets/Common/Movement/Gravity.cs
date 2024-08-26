using UnityEngine;

namespace Overheat.Common.Movement
{
	[RequireComponent(typeof(Velocity))]
	[DefaultExecutionOrder(-10)]
	public sealed class Gravity : MonoBehaviour
	{
		public float Strength = 10f;
		public bool ApplyGravityWhenOnGround;

		private Velocity velocity;
		private CollisionInfo collisionInfo;

		void Awake()
		{
			velocity = GetComponent<Velocity>();
			collisionInfo = GetComponent<CollisionInfo>();
		}

		void FixedUpdate()
		{
			if (ApplyGravityWhenOnGround || !collisionInfo.IsOnGround()) {
				velocity.Value += Strength * Time.fixedDeltaTime * Vector3.down;
			}
		}
	}
}
