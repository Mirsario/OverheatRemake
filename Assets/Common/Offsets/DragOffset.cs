using Overheat.Core.Utilities;
using UnityEngine;
using NQuaternion = System.Numerics.Quaternion;

namespace Overheat.Common.Offsets
{
	[RequireComponent(typeof(TransformReset))]
	public sealed class DragOffset : MonoBehaviour
	{
		public Transform TransformOverride;
		public bool InverseRotation;
		public float RotationDamping = 0.1f;
		public float RotationMultiplier = 1.0f;

		private NQuaternion dampedRotation;

		void OnEnable()
		{
			var resetter = GetComponent<TransformReset>();
			resetter.ResetLocalRotation = true;
			resetter.ResetOnRenderUpdate = true;
		}

		void LateUpdate()
		{
			var baseTransform = TransformOverride != null ? TransformOverride.transform : transform.parent;

			if (baseTransform == null)
				return;

			var targetRotation = baseTransform.rotation.ToNumerics();

			dampedRotation = MathUtils.Damp(dampedRotation, targetRotation, RotationDamping * RotationDamping, Time.deltaTime);
			
			var localRotation = dampedRotation * NQuaternion.Inverse(targetRotation);
			
			if (InverseRotation) { localRotation = NQuaternion.Inverse(localRotation); }
			if (RotationMultiplier != 1f) { localRotation = NQuaternion.Lerp(NQuaternion.Identity, localRotation, RotationMultiplier); }

			transform.localRotation = transform.localRotation * localRotation.ToUnity();
		}
	}
}
