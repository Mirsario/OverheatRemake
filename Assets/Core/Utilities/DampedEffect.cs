using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Overheat.Core.Utilities
{
	internal static class DampedEffectInitializer
	{
		static DampedEffectInitializer()
		{
			DampedEffect<float>.Functions = new() {
				Multiply = (a, b) => a * b,
			};
			DampedEffect<Vector2>.Functions = new() {
				Multiply = (a, b) => a * b,
			};
			DampedEffect<Vector3>.Functions = new() {
				Multiply = (a, b) => a * b,
			};
			DampedEffect<Vector4>.Functions = new() {
				Multiply = (a, b) => a * b,
			};
		}
	}

	[Serializable]
	public struct DampedEffect<T>
	{
		public struct TypeFunctions
		{
			public Func<T, float, T> Multiply;
		}

		public static TypeFunctions Functions { get; set; }

		[SerializeField] public T Intensity;
		[SerializeField] public float Damping;
		[SerializeField, HideInInspector] public float CurrentFactor;
		[SerializeField, HideInInspector] public float TargetFactor;

		static DampedEffect() => RuntimeHelpers.RunClassConstructor(typeof(DampedEffectInitializer).TypeHandle);

		public DampedEffect(T intensity, float damping)
		{
			CurrentFactor = TargetFactor = 0f;
			Intensity = intensity;
			Damping = damping;
		}

		public readonly T Get()
		{
			return Functions.Multiply(Intensity, CurrentFactor);
		}

		public void Update(float deltaTime)
		{
			CurrentFactor = Damping > 0f ? MathUtils.Damp(CurrentFactor, TargetFactor, Damping * Damping, deltaTime) : TargetFactor;
			TargetFactor = 0f;
		}

		public T UpdateAndGet(float deltaTime)
		{
			Update(deltaTime);
			return Get();
		}
	}
}
