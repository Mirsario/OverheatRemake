using System;
using Overheat.Common.Audio;
using Overheat.Common.Movement;
using UnityEngine;

namespace Overheat.Common.Effects
{
	[Serializable]
	public sealed class AnimationFootsteps : StateMachineBehaviour
	{
		public enum FootstepPositionType : byte
		{
			Fixed,
			InverseKinematics,
		}

		[Serializable]
		public struct FootstepEntry
		{
			public float NormalizedTime;
			public FootstepPositionType PositionType;
			public Vector3 FixedPosition;
			public AvatarIKGoal HumanoidIK;
		}

		public GameObject Sound;
		public bool CheckIfOnGround;
		public FootstepEntry[] Footsteps = Array.Empty<FootstepEntry>();

		private float previousNormalizedTime;

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			// Don't play footsteps if the object reports itself not grounded
			if (CheckIfOnGround && animator.TryGetComponent(out CollisionInfo collisionInfo) && !collisionInfo.IsOnGround()) {
				return;
			}

			float currentNormalizedTime = stateInfo.normalizedTime;

			if (previousNormalizedTime > currentNormalizedTime) {
				previousNormalizedTime = 0f;
			}

			for (int i = 0; i < Footsteps.Length; i++) {
				ref readonly var footstep = ref Footsteps[i];

				if (currentNormalizedTime >= footstep.NormalizedTime && previousNormalizedTime <= footstep.NormalizedTime) {
					TriggerFootstep(animator, in footstep);
				}
			}

			previousNormalizedTime = currentNormalizedTime;
		}

		private void TriggerFootstep(Animator animator, in FootstepEntry entry)
		{
			Vector3 footstepPosition;

			switch (entry.PositionType) {
				/*
				case FootstepPositionType.InverseKinematics:
					if ((footstepPosition = animator.GetIKPosition(entry.HumanoidIK)) != default) {
						break;
					}

					goto default;
				*/
				default:
					footstepPosition = animator.transform.position + entry.FixedPosition;
					break;
			};

			Instantiate(Sound, footstepPosition, Quaternion.identity);
			//AudioPlayback.PlaySound(Sound, new AudioPlaybackParameters {
			//	Position = footstepPosition,
			//	DestroyOnStop = true,
			//});
		}
	}
}
