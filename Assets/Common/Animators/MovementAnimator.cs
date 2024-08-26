using Overheat.Common.Movement;
using UnityEngine;

namespace Overheat.Common.Animators
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Velocity))]
	[RequireComponent(typeof(CollisionInfo))]
	public sealed class MovementAnimator : MonoBehaviour
	{
		private Animator animator;
		private Velocity velocity;
		private CollisionInfo collisionInfo;
		private int moveSpeedProperty;
		private int onGroundProperty;

		void Awake()
		{
			animator = GetComponent<Animator>();
			velocity = GetComponent<Velocity>();
			collisionInfo = GetComponent<CollisionInfo>();

			moveSpeedProperty = Animator.StringToHash("MoveSpeed");
			onGroundProperty = Animator.StringToHash("OnGround");
		}

		void Update()
		{
			animator.SetFloat(moveSpeedProperty, new Vector2(velocity.Value.x, velocity.Value.z).magnitude);
			animator.SetBool(onGroundProperty, collisionInfo.IsOnGround());
		}
	}
}
