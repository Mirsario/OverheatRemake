using UnityEngine;

namespace Overheat.Common.Movement
{
	[RequireComponent(typeof(Rigidbody))]
	[DefaultExecutionOrder(1000)]
	public sealed class Velocity : MonoBehaviour
	{
		public Vector3 Value;

		private new Rigidbody rigidbody;

		void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		void FixedUpdate()
		{
			rigidbody.velocity = Value;
		}

		void LateUpdate()
		{
			Value = rigidbody.velocity;
		}

		public static implicit operator Vector3(Velocity v) => v.Value;
	}
}
