using Overheat.Core.Signals;
using UnityEngine;

namespace Overheat.Common.Looking
{
	public sealed class FaceLookPosition : MonoBehaviour
	{
		public Signals Signals;
		public bool OnlyYAxis;

		void Awake()
		{
			if (Signals == null) {
				Signals = GetComponent<Signals>();
			}
		}

		void LateUpdate()
		{
			//var direction = Signals.LookDirection; //(Signals.LookPosition - transform.position).normalized;
			var rotation = Signals.LookRotation;

			if (OnlyYAxis) {
				var euler = transform.eulerAngles;

				euler.y = rotation.eulerAngles.y;

				transform.eulerAngles = euler;
			} else {
				transform.rotation = rotation;
			}
		}
	}
}
