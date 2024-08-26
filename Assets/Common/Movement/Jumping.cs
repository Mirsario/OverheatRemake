using Overheat.Core.Signals;
using Overheat.Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Overheat.Common.Movement
{
	[RequireComponent(typeof(Signals))]
	[RequireComponent(typeof(Velocity))]
	[RequireComponent(typeof(CollisionInfo))]
	[DefaultExecutionOrder(100)]
	public sealed class Jumping : MonoBehaviour
	{
		public bool AllowKeyHolding = true;
		public bool StackVelocity = false;
		public float Strength = 5f;
		public float CoyoteTime = 0.2f;
		public float InputBuffering = 0.15f;
		public InputActionReference Input;
		public GameObject JumpEffect;

		private Signals signals;
		private Velocity velocity;
		private CollisionInfo collisionInfo;

		[SerializeField]
		private float pendingInputTime = -1f;

		void Awake()
		{
			signals = GetComponent<Signals>();
			velocity = GetComponent<Velocity>();
			collisionInfo = GetComponent<CollisionInfo>();
		}

		void FixedUpdate()
		{
			float time = Time.fixedTime;
			bool pressed = AllowKeyHolding ? signals.IsActive(Input) : signals.JustActivated(Input);

			if (pressed) {
				pendingInputTime = time;
			} else if (pendingInputTime < 0 || (time - pendingInputTime) > InputBuffering) {
				pendingInputTime = -1;
				return;
			}

			if (!collisionInfo.IsOnGround()) {
				return;
			}

			var previousVelocity = velocity.Value;

			if (StackVelocity) {
				velocity.Value += transform.up * Strength;
			} else {
				velocity.Value.y = Mathf.Max(velocity.Value.y, Strength);
			}

			if (JumpEffect != null && velocity.Value != previousVelocity) {
				Instantiate(JumpEffect, transform.position, Quaternion.identity);
			}

			pendingInputTime = -1;
			collisionInfo.ForceLeaveGround();
		}
	}
}
