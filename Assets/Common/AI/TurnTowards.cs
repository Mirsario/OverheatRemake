using Overheat.Core.Signals;
using UnityEngine;

namespace Overheat.Common.AI
{
	[RequireComponent(typeof(Signals))]
	public sealed class TurnTowards : MonoBehaviour
	{
		public Transform Target;

		private Signals signals;

		void Awake()
		{
			signals = GetComponent<Signals>();
		}

		void FixedUpdate()
		{
			signals.LookDirection = (Target.position - transform.position).normalized;
		}
	}
}
