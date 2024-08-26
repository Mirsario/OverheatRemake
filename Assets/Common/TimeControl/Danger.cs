using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.TimeControl
{
	/// <summary>
	/// Describes an object's danger, which interacts with the time mechanics.
	/// </summary>
	public sealed class Danger : MonoBehaviour
	{
		//public float AngleFactor = 1f;
		public float Range = 1.0f;
		public float Factor = 1.0f;
		public AnimationCurve DistanceCurve;
	}
}
