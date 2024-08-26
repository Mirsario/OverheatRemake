using UnityEngine;

namespace Overheat.Core.Signals
{
	[RequireComponent(typeof(Signals))]
	public sealed class InputLayoutSignalProvider : MonoBehaviour
	{
		private Signals signals;

		void Awake()
		{
			signals = GetComponent<Signals>();
		}

		void Update()
		{
			var values = signals.Values;

			for (int i = 0; i < values.Length; i++) {
				values[i].Value = default;
			}

			for (int i = 0; i < SignalSystem.Actions.Length; i++) {
				var action = SignalSystem.Actions[i];

				values[i].Value = action.ReadValueAsObject() switch {
					float f => new Vector3(f, 0f, 0f),
					Vector2 v => new Vector3(v.x, v.y, 0f),
					Vector3 v => new Vector3(v.x, v.y, v.z),
					_ => Vector3.zero,
				};
			}
		}
	}
}
