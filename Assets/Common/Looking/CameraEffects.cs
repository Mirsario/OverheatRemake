using Overheat.Common.Movement;
using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Looking
{

	public sealed class CameraEffects : MonoBehaviour
	{
		[SerializeField] private Velocity velocity;

		public DampedEffect<Vector3> XVelocityRotation = new(new(0f, 0f, 1f), 0.1f);
		public DampedEffect<Vector3> YVelocityRotation = new(new(1f, 0f, 0f), 0.1f);
		public DampedEffect<Vector3> ZVelocityRotation = new(new(1f, 0f, 0f), 0.1f);

		void Update()
		{
			var rotation = Vector3.zero;
			float deltaTime = Time.deltaTime;

			if (velocity != null || (velocity = GetComponentInParent<Velocity>()) != null) {
				var eulerAngles = transform.eulerAngles;
				var globalVelocity = velocity.Value;
				var localVelocity = Quaternion.Euler(0f, -eulerAngles.y, 0f) * globalVelocity;

				XVelocityRotation.TargetFactor = -localVelocity.x;
				YVelocityRotation.TargetFactor = localVelocity.y;
				ZVelocityRotation.TargetFactor = localVelocity.z;
			}

			rotation += XVelocityRotation.UpdateAndGet(deltaTime);
			rotation += YVelocityRotation.UpdateAndGet(deltaTime);
			rotation += ZVelocityRotation.UpdateAndGet(deltaTime);

			transform.localEulerAngles = rotation;
		}
	}
}
