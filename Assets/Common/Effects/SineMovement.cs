using System;
using UnityEngine;

namespace Overheat.Common.Effects
{
	public sealed class SineMovement : MonoBehaviour
	{
		public float RotationSpeed = 90f;
		public float Speed = 4f;
		public float MovementRange = 0.1f;
		
		[HideInInspector]
		public Vector3 BasePosition;

		private void Start()
		{
			BasePosition = transform.localPosition;
		}

		void Update()
		{
			float sine = MathF.Sin(Time.time * Speed);
			var offset = new Vector3(0f, sine * MovementRange * 0.5f, 0f);

			transform.localPosition = BasePosition + offset;
			transform.localEulerAngles = new Vector3(0f, Time.time * RotationSpeed, 0f);
		}
	}
}
