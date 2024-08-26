using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Offsets
{
	[RequireComponent(typeof(TransformReset))]
	public sealed class VisualOffset : MonoBehaviour
	{
		public float DecayDamping = 0.1f;
		public Vector3 Offset;

		void OnEnable()
		{
			var resetter = GetComponent<TransformReset>();
			resetter.ResetLocalPosition = true;
			resetter.ResetOnRenderUpdate = true;
		}

		void LateUpdate()
		{
			transform.localPosition += Offset;
			Offset = MathUtils.Damp(Offset, Vector3.zero, DecayDamping * DecayDamping, Time.deltaTime);
		}
	}
}
