using UnityEngine;
using UnityEngine.InputSystem;

namespace Overheat.Common.Camera
{
	public sealed class CursorController : MonoBehaviour
	{
		public InputActionReference InputEscape;
		public InputActionReference InputFocus;

		void Update()
		{
			bool isLocked = Cursor.lockState == CursorLockMode.Locked;
			bool shouldBeLocked = isLocked;

			if ((isLocked ? InputEscape : InputFocus).action.IsPressed()) {
				shouldBeLocked = !shouldBeLocked;
			}

			// Spamming this prevents OS-related bugs.
			SetCursorLocked(shouldBeLocked);
		}

		private static void SetCursorLocked(bool value)
		{
			Cursor.visible = !value;
			Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}
