using Overheat.Common.Movement;
using UnityEngine;

namespace Overheat.Common.Camera
{
	[RequireComponent(typeof(TransformReset))]
	public sealed class ViewBobbing : MonoBehaviour
	{
		[SerializeField, HideInInspector] private Velocity velocity;
		[SerializeField, HideInInspector] private CollisionInfo collisionInfo;
		[SerializeField, HideInInspector] private float accumulatedBobbing;

		public CameraRotationEffect VerticalBobbing = new(0.03f, 0.1f);
		public CameraRotationEffect HorizontalBobbing = new(0.03f, 0.1f);
		public CameraRotationEffect BobbingAccumulation = new(1.00f, 0.1f);
		public Vector2 BobbingMultiplier = new(0.5f, 1.0f);

		void OnEnable()
		{
			var resetter = GetComponent<TransformReset>();
			resetter.ResetLocalPosition = true;
			resetter.ResetOnRenderUpdate = true;
		}

		void Update()
		{
			float deltaTime = Time.deltaTime;

			if (velocity != null || (velocity = GetComponentInParent<Velocity>()) != null) {
				var globalVelocity = velocity.Value;
				float target = new Vector3(globalVelocity.x, 0f, globalVelocity.z).magnitude;

				if (collisionInfo != null || (collisionInfo = GetComponentInParent<CollisionInfo>()) != null) {
					if (!collisionInfo.IsOnGround()) {
						target = 0f;
					}
				}

				VerticalBobbing.Target = target;
				HorizontalBobbing.Target = target;
				BobbingAccumulation.Target = target;
			}

			VerticalBobbing.Update(deltaTime);
			HorizontalBobbing.Update(deltaTime);
			BobbingAccumulation.Update(deltaTime);

			accumulatedBobbing += deltaTime * BobbingAccumulation.Get();
			transform.localPosition += new Vector3(
				Mathf.Sin(accumulatedBobbing * BobbingMultiplier.x) * HorizontalBobbing.Get(),
				Mathf.Sin(accumulatedBobbing * BobbingMultiplier.y) * VerticalBobbing.Get(),
				0f
			);
		}
	}
}
