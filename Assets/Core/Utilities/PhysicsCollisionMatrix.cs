using UnityEngine;

namespace Overheat.Core.Utilities
{
	/// <summary>
	/// Provides access to Unity's physics collision matrix's layer masks.
	/// </summary>
	public static class PhysicsCollisionMatrix
	{
		private const int NumLayers = 32;
		private static readonly int[] masksByLayer;

		static PhysicsCollisionMatrix()
		{
			masksByLayer = new int[NumLayers];

			for (int i = 0; i < NumLayers; i++) {
				int mask = 0;

				for (int j = 0; j < NumLayers; j++) {
					if (!Physics.GetIgnoreLayerCollision(i, j)) {
						mask |= 1 << j;
					}
				}

				masksByLayer[i] = mask;
			}
		}

		public static int GetMask(int layerId)
		{
			return masksByLayer[layerId];
		}
	}
}
