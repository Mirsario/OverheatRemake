using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Projectiles
{
	/// <summary>
	/// Handles movement & collision, nothing else.
	/// </summary>
	public sealed class Projectile : MonoBehaviour
	{
		public float Speed = 1f;
		public bool DestroyOnCollision = true;
		public bool ParentOnCollision = true;

		void Awake()
		{
			transform.parent = null;
		}

		void FixedUpdate()
		{
			if (Speed <= 0f) {
				return;
			}

			var position = transform.position;
			var direction = transform.forward;
			float step = Speed * Time.fixedDeltaTime;
			int layerMask = PhysicsCollisionMatrix.GetMask(gameObject.layer);

			if (Physics.Raycast(position, direction, out var hitInfo, step, layerMask)) {
				transform.position = hitInfo.point;

				if (ParentOnCollision) {
					//var oldScale = transform.lossyScale;
					transform.parent = hitInfo.transform;
					//var parentScale = transform.parent.lossyScale;
					//transform.localScale = new Vector3(
					//	oldScale.x / parentScale.x,
					//	oldScale.y / parentScale.y,
					//	oldScale.z / parentScale.z
					//);
				}

				if (DestroyOnCollision) {
					Destroy(gameObject);
				} else {
					enabled = false;
				}
				return;
			}

			transform.position += direction * step;
		}
	}
}
