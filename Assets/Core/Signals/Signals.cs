using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Overheat.Core.Signals
{
	[Tooltip("Abstraction over input usable by both players and AI, and modifiable by status effects.")]
	[DefaultExecutionOrder(1000)]
	public sealed class Signals : MonoBehaviour //, ISerializationCallbackReceiver
	{
		public Signals Proxy;

		// Could instead use Memory<Signal>, all pointing to a single large array to reduce memory allocations & update costs.
		[NonSerialized] private Signal[] signals = Array.Empty<Signal>();
		[SerializeField] private (InputActionReference action, Signal signal)[] serializedSignals;

		[SerializeField] private Vector3 lookPosition;
		[SerializeField] private Vector2 lookAngles;
		//[SerializeField] private Vector3 lookDirection = Vector3.forward;

		public ref Vector3 LookPosition => ref (Proxy != null ? ref Proxy.LookPosition : ref lookPosition);
		public ref Vector2 LookAngles => ref (Proxy != null ? ref Proxy.LookAngles : ref lookAngles);
		public Quaternion LookRotation {
			get {
				var angles = LookAngles;
				return Quaternion.Euler(angles.x, angles.y, 0f);
			}
			set {
				LookAngles = (Vector2)value.eulerAngles;
			}
		}
		public Vector3 LookDirection {
			get {
				var angles = LookAngles;
				return Quaternion.Euler(angles.x, angles.y, 0f) * Vector3.forward;
			}
			set => LookRotation = Quaternion.LookRotation(value);
		}

		public Span<Signal> Values {
			get {
				if (Proxy != null) {
					return Proxy.Values;
				}

				EnsureSize();

				return signals;
			}
		}

		public ref Signal this[InputAction action] {
			get {
				if (Proxy != null) {
					return ref Proxy[action];
				}

				EnsureSize();

				int id = SignalSystem.GetActionId(action);

				return ref signals[id];
			}
		}

		// Update old values.

		void Update()
		{
			if (Proxy != null) {
				return;
			}

			for (int i = 0; i < signals.Length; i++) {
				signals[i].PreviousValueRender = signals[i].Value;
			}
		}

		void FixedUpdate()
		{
			if (Proxy != null) {
				return;
			}

			for (int i = 0; i < signals.Length; i++) {
				signals[i].PreviousValueFixed = signals[i].Value;
			}
		}

		// Getters

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Value(InputActionReference action)
		{
			var value = this[action].Value;

			return value.x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 Values2D(InputActionReference action)
		{
			var value = this[action].Value;

			return (Vector2)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 Values3D(InputActionReference action)
		{
			var value = this[action].Value;

			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float PreviousValue(InputActionReference action)
			=> this[action].PreviousValue.x;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsActive(InputActionReference action)
			=> Value(action) != 0f;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool WasActive(InputActionReference action)
			=> PreviousValue(action) != 0f;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool JustActivated(InputActionReference action)
			=> IsActive(action) && !WasActive(action);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool JustDeactivated(InputActionReference action)
			=> !IsActive(action) && WasActive(action);

		// Setters

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Value(InputActionReference action, float value)
		{
			this[action].Value.x = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Values2D(InputActionReference action, Vector2 values)
		{
			ref var value = ref this[action].Value;

			value.x = values.x;
			value.y = values.y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Values3D(InputActionReference action, Vector3 values)
		{
			this[action].Value = values;
		}

		private void EnsureSize()
		{
			if (signals.Length != SignalSystem.ActionCount) {
				Array.Resize(ref signals, SignalSystem.ActionCount);
			}
		}

		/*
		// Prepare the index-using Signals array for serialization 
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (Proxy != null) {
				return;
			}

			CheckSize();

			var actions = InputActionReference.GetTriggers();
			int length = values.Length;

			Array.Resize(ref serializedSignals, length);

			for (int i = 0; i < length; i++) {
				serializedSignals[i] = (actions[i], values[i]);
			}
		}

		// Deserialize into an optimized signal storage variant
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			CheckSize();
			Array.Clear(values, 0, values.Length);

			if (serializedSignals == null) {
				return;
			}

			foreach (var (action, signal) in serializedSignals) {
				if (action != null) {
					values[action.RuntimeId] = signal;
				}
			}
		}
		*/
	}
}
