using Overheat.Core.Signals;
using Overheat.Core.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Overheat.Common.AI
{
	[RequireComponent(typeof(Signals))]
	public sealed class UseRandomly : MonoBehaviour
	{
		public InputActionReference Input;
		public int Rate = 60;

		private Signals signals;

		void Start()
		{
			Assert.IsNotNull(Input);
		}

		void Awake()
		{
			signals = GetComponent<Signals>();
		}

		void Update()
		{
			if (Input == null) {
				return;
			}

			signals.Value(Input, TimeSystem.FixedUpdateCount % (ulong)Rate == 0 ? 1f : 0f);
		}
	}
}
