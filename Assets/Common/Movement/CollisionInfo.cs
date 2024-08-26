using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Movement
{
	[DefaultExecutionOrder(-100)]
	public sealed class CollisionInfo : MonoBehaviour
	{
		[SerializeField] private ulong lastGroundCollisionTicks;

		public float MinGroundDot = 0.8f;
		public Vector3 GroundNormal { get; private set; } = Vector3.up;

		void OnCollisionEnter(Collision collision)
			=> OnCollision(collision);

		void OnCollisionStay(Collision collision)
			=> OnCollision(collision);

		void OnCollisionExit(Collision collision)
			=> OnCollision(collision);

		void FixedUpdate()
		{
			if (!IsOnGround()) {
				GroundNormal = Vector3.up;
			}
		}

		public bool IsOnGround()
		{
			return lastGroundCollisionTicks >= TimeSystem.FixedUpdateCount - 2;
		}

		public void ForceLeaveGround()
		{
			lastGroundCollisionTicks = 0;
		}

		private void OnCollision(Collision collision)
		{
			if (IsAGroundCollision(collision, transform.up)) {
				lastGroundCollisionTicks = TimeSystem.FixedUpdateCount;
			}
		}

		private bool IsAGroundCollision(Collision collision, Vector3 transformUp)
		{
			int contactCount = collision.contactCount;
			ContactPoint? flattestContact = null;
			float flattestContactDot = float.NegativeInfinity;

			for (int i = 0; i < contactCount; i++) {
				var contact = collision.GetContact(i);
				float normalDot = Vector3.Dot(contact.normal, transformUp);

				if (normalDot > flattestContactDot) {
					flattestContact = contact;
					flattestContactDot = normalDot;
				}
			}

			if (flattestContact is ContactPoint groundContact) {
				//Debug.Log($"flattestContactDot: {flattestContactDot:0.00}");
				if (flattestContactDot > MinGroundDot) {
					GroundNormal = groundContact.normal;
					//Debug.Log($"GroundNormal: {GroundNormal:0.00}");
					return true;
				}

				GroundNormal = Vector3.up;
			}

			return false;
		}
	}
}
