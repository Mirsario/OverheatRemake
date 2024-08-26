using System.Runtime.CompilerServices;

namespace Overheat.Core.Utilities
{
	public static class BitOperations
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint RoundUpToPowerOf2(uint value)
		{
			// Based on https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
			--value;
			value |= value >> 1;
			value |= value >> 2;
			value |= value >> 4;
			value |= value >> 8;
			value |= value >> 16;

			return value + 1;
		}
	}
}
