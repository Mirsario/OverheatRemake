using System;
using UnityEngine;

namespace Overheat.Core.Utilities
{
	public sealed class TimeSystem : MonoBehaviour
	{
		public static ulong FixedUpdateCount { get; private set; }

		void FixedUpdate()
		{
			FixedUpdateCount++;
		}
	}
}
