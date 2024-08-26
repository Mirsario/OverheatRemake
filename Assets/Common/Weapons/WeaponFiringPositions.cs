using UnityEngine;

namespace Overheat.Common.Weapons
{
	public sealed class WeaponFiringPositions : MonoBehaviour
	{
		public Vector3[] FiringPositions;

		[SerializeField, HideInInspector]
		private int firingPositionIndex;

		public Vector3 GetNextFiringPositionOffset()
		{
			if (FiringPositions == null) {
				return default;
			}

			var result = FiringPositions[firingPositionIndex];

			firingPositionIndex = (firingPositionIndex + 1) % FiringPositions.Length;

			return result;
		}
	}
}
