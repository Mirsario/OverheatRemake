using Overheat.Core.Signals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Overheat.Common.Looking
{
	[Tooltip("Controls a Signals component's look angles in a first-person camera manner based on its inputs.")]
	[RequireComponent(typeof(Signals))]
	public sealed class FirstPersonLook : MonoBehaviour
	{
		public InputActionReference Input;

		[Tooltip("Multiplier for the inputs.")]
		public float SensitivityFactor = 1.0f;
		[Tooltip("Whether to apply the look angle to the current object's transform")]
		public bool ApplyToTransform;

		private Signals signals;
		//[SerializeField, HideInInspector]
		//private Vector3 lastDirection;

		void OnEnable()
		{
			signals = GetComponent<Signals>();
		}

		void Update()
		{
			//if (signals.LookDirection is Vector3 lookDirection && lookDirection != lastDirection) {
			//	EulerAngles = Quaternion.LookRotation(lookDirection).eulerAngles;
			//}

			ref float pitch = ref signals.LookAngles.x;
			ref float yaw = ref signals.LookAngles.y;

			if (IsCameraRotationAllowed()) {
				var offset = GetInputLookRotation();

				//rotation *= Quaternion.AngleAxis(offset.y * SensitivityFactor, transform.right);
				//rotation = Quaternion.AngleAxis(offset.x * SensitivityFactor, Vector3.up) * Quaternion.AngleAxis(offset.y * SensitivityFactor, Vector3.right) * rotation;
				
				pitch -= offset.y * SensitivityFactor;
				yaw += offset.x * SensitivityFactor;
				pitch = Mathf.Clamp(pitch, -89.99f, 89.99f);
				yaw = Mathf.Repeat(yaw, 360f);
			}

			var ray = new Ray(transform.position, signals.LookRotation * Vector3.forward);
			if (Physics.Raycast(ray, out var raycast)) {
				signals.LookPosition = raycast.point;
			} else {
				signals.LookPosition = ray.origin + ray.direction;
			}

			if (ApplyToTransform) {
				transform.rotation = signals.LookRotation;
				//transform.eulerAngles = EulerAngles;
			}

			//lastDirection = ray.direction;
		}

		void LateUpdate()
		{
			if (ApplyToTransform) {
				transform.rotation = signals.LookRotation;
				//transform.eulerAngles = EulerAngles;
			}
		}

		private Vector2 GetInputLookRotation()
		{
			return signals.Values2D(Input);
		}

		private bool IsCameraRotationAllowed()
		{
			return Cursor.lockState == CursorLockMode.Locked;
		}
	}
}
