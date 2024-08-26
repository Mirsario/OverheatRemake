using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Offsets
{
	[RequireComponent(typeof(TransformReset))]
	public sealed class DragOffset : MonoBehaviour
	{
		public Transform TransformOverride;
		public bool InverseRotation;
		public float RotationDamping = 0.1f;
		public float RotationMultiplier = 1.0f;

		private Quaternion dampedRotation;

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

			var targetRotation = baseTransform.rotation;

			dampedRotation = MathUtils.Damp(dampedRotation, targetRotation, RotationDamping * RotationDamping, Time.deltaTime);

			var localRotation = Quaternion.Inverse(targetRotation) * dampedRotation;

			if (InverseRotation) { localRotation = Quaternion.Inverse(localRotation); }
			if (RotationMultiplier != 1f) { localRotation = Quaternion.Lerp(Quaternion.identity, localRotation, RotationMultiplier); }

			transform.localRotation = localRotation;
		}
	}
}
