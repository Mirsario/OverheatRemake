using Overheat.Core.Signals;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Overheat.Common.AI
{
	[RequireComponent(typeof(Signals))]
	public sealed class MoveRandomly : MonoBehaviour
	{
		public InputActionReference Input;
		public float Rotation;

		private Signals signals;

		private void Start()
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

			Rotation = Random.Range(0f, 360f);

			Vector3 vector = Quaternion.AngleAxis(Rotation, Vector3.up) * Vector3.forward;
			Vector2 moveVector = new Vector2(vector.x, vector.z);

			signals.Values2D(Input, moveVector);
		}
	}
}
