using Overheat.Common.Camera;
using Overheat.Core.Signals;
using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Weapons
{
	[RequireComponent(typeof(Signals))]
	public sealed class FiringRecoil : MonoBehaviour
	{
		public Vector3 VisualOffsetMin = new(-0.01f,  0.00f, -0.02f);
		public Vector3 VisualOffsetMax = new( 0.01f,  0.00f,  0.01f);
		public Vector2 AimingOffsetMin = new(-2.00f,  0.00f);
		public Vector2 AimingOffsetMax = new(-2.00f,  0.00f);
		public Vector2 PendingAimingOffset;
		public float AimingOffsetDamping = 0.01f;
		public float AimingOffsetCutoff = 0.005f;
		public bool Additive = true;

		private Signals signals;

		void Awake()
		{
			signals = GetComponent<Signals>();
		}

		void Update()
		{
			if (PendingAimingOffset != default) {
				var offsetCut = MathUtils.Damp(default, PendingAimingOffset, AimingOffsetDamping * AimingOffsetDamping, Time.deltaTime);
				signals.LookAngles += offsetCut;
				PendingAimingOffset -= offsetCut;

				if (Mathf.Abs(PendingAimingOffset.x) < AimingOffsetCutoff) PendingAimingOffset.x = 0f;
				if (Mathf.Abs(PendingAimingOffset.y) < AimingOffsetCutoff) PendingAimingOffset.y = 0f;
			}
		}

		public void OnFire(WeaponFiring.OnFireArgs args)
		{
			if (signals.Proxy is Signals handler && handler.TryGetComponent(out ViewDescription viewDescription) && viewDescription.Viewmodel is Viewmodel viewmodel) {
				viewmodel.Offset += new Vector3(
					Random.Range(VisualOffsetMin.x, VisualOffsetMax.x),
					Random.Range(VisualOffsetMin.y, VisualOffsetMax.y),
					Random.Range(VisualOffsetMin.z, VisualOffsetMax.z)
				);
			}

			if (AimingOffsetMin != default || AimingOffsetMax != default) {
				var offset = new Vector2(
					Random.Range(AimingOffsetMin.x, AimingOffsetMax.x),
					Random.Range(AimingOffsetMin.y, AimingOffsetMax.y)
				);

				if (Additive) {
					PendingAimingOffset += offset;
				} else {
					PendingAimingOffset = offset;
				}

				//signals.LookRotation *= Quaternion.AngleAxis(Random.Range(AimingOffsetMin.x, AimingOffsetMax.x), signals.
				//signals.LookRotation = Quaternion.Euler(aimingOffset) * signals.LookRotation;
				//signals.LookDirection = (Quaternion.Euler(aimingOffset.x, aimingOffset.y, aimingOffset.z) * signals.LookDirection).normalized;
			}
		}
	}
}
