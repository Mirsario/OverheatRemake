using System;
using UnityEngine;

namespace Overheat.Core.Signals
{
	[Serializable]
	public struct Signal
	{
		public Vector3 Value;
		public Vector3 PreviousValueRender;
		public Vector3 PreviousValueFixed;

		public Vector3 PreviousValue => Time.inFixedTimeStep ? PreviousValueFixed : Value;
	}
}
