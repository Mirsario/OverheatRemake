#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/*
using UnityEngine;

namespace Overheat.Common.Camera
{
	internal struct CameraState
	{
		public Vector3 Position;
		public Vector3 EulerAngles;

		public void SetFromTransform(Transform transform)
		{
			Position = transform.position;
			EulerAngles = transform.eulerAngles;
		}

		public void Translate(Vector3 translation)
		{
			Position += Quaternion.Euler(EulerAngles) * translation;
		}

		public void LerpTowards(CameraState target, float positionStep, float rotationStep)
		{
			Position = Vector3.Lerp(Position, target.Position, positionStep);
			EulerAngles = Vector3.Lerp(EulerAngles, target.EulerAngles, rotationStep);
		}

		public void UpdateTransform(Transform t)
		{
			t.position = Position;
			t.eulerAngles = EulerAngles;
		}
	}

	public sealed class CameraController : MonoBehaviour
	{
		private CameraState targetCameraState = new();
		private CameraState interpolatingCameraState = new();

		[Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
		public float PositionLerpTime = 0.2f;

		[Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
		public float RotationLerpTime = 0.01f;

		[Tooltip("Speed at which the camera turns.")]
		public float Sensitivity = 1.0f;

		[Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
		public bool InvertY;

		void OnEnable()
		{
			targetCameraState.SetFromTransform(transform);
			interpolatingCameraState.SetFromTransform(transform);
		}

		void Update()
		{
			targetCameraState.Position = transform.position;

			if (IsCameraRotationAllowed()) {
				var mouseMovement = 5f * Time.deltaTime * GetInputLookRotation();

				if (!InvertY) {
					mouseMovement.y = -mouseMovement.y;
				}

				targetCameraState.EulerAngles.y += mouseMovement.x * Sensitivity;
				targetCameraState.EulerAngles.x += mouseMovement.y * Sensitivity;
			}

			// Framerate-independent interpolation
			// Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
			float positionLerpStep = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / PositionLerpTime) * Time.deltaTime);
			float rotationLerpStep = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / RotationLerpTime) * Time.deltaTime);

			interpolatingCameraState.LerpTowards(targetCameraState, positionLerpStep, rotationLerpStep);

			interpolatingCameraState.UpdateTransform(transform);
		}

		private Vector2 GetInputLookRotation()
		{
			return new Vector2(Input.GetAxis("LookX"), Input.GetAxis("LookY")) * 10;
		}

		private bool IsCameraRotationAllowed()
		{
			return Cursor.lockState == CursorLockMode.Locked;
		}
	}
}
*/
