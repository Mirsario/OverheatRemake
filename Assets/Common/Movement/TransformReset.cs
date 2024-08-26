using UnityEngine;

namespace Overheat.Common.Movement
{
	public sealed class TransformReset : MonoBehaviour
	{
		[HideInInspector] public bool ResetLocalPosition;
		[HideInInspector] public bool ResetLocalRotation;
		[HideInInspector] public bool ResetLocalScale;
		[HideInInspector] public bool ResetOnRenderUpdate;
		[HideInInspector] public bool ResetOnFixedUpdate;

		void Update()
		{
			if (ResetOnRenderUpdate) {
				Reset();
			}
		}

		void FixedUpdate()
		{
			if (ResetOnFixedUpdate) {
				Reset();
			}
		}

		public void Reset()
		{
			if (ResetLocalPosition && ResetLocalRotation) {
				transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			} else if (ResetLocalPosition) {
				transform.localPosition = Vector3.zero;
			} else if (ResetLocalRotation) {
				transform.localRotation = Quaternion.identity;
			}

			if (ResetLocalScale) {
				transform.localScale = Vector3.one;
			}
		}
	}
}
