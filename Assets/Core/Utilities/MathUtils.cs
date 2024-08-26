using System;
using UnityEngine;
using UQuaternion = UnityEngine.Quaternion;
using NQuaternion = System.Numerics.Quaternion;
using System.Runtime.CompilerServices;

namespace Overheat.Core.Utilities
{
	public static class MathUtils
	{
		// See this:
		// https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DampLerpStep(float smoothing, float dt)
		{
			return 1f - Mathf.Pow(smoothing, dt);
		}

		public static float Damp(float source, float destination, float smoothing, float dt)
		{
			return Mathf.Lerp(source, destination, DampLerpStep(smoothing, dt));
		}

		public static Vector2 Damp(Vector2 source, Vector2 destination, float smoothing, float dt)
		{
			return new(
				Damp(source.x, destination.x, smoothing, dt),
				Damp(source.y, destination.y, smoothing, dt)
			);
		}

		public static Vector3 Damp(Vector3 source, Vector3 destination, float smoothing, float dt)
		{
			return new(
				Damp(source.x, destination.x, smoothing, dt),
				Damp(source.y, destination.y, smoothing, dt),
				Damp(source.z, destination.z, smoothing, dt)
			);
		}

		public static UQuaternion Damp(UQuaternion q1, UQuaternion q2, float smoothing, float dt)
		{
			return UQuaternion.Lerp(q1, q2, DampLerpStep(smoothing, dt));
		}

		public static NQuaternion Damp(NQuaternion q1, NQuaternion q2, float smoothing, float dt)
		{
			return NQuaternion.Lerp(q1, q2, DampLerpStep(smoothing, dt));
		}

		public static NQuaternion ToNumerics(this UQuaternion q)
			=> new(q.x, q.y, q.z, q.w);

		public static UQuaternion ToUnity(this NQuaternion q)
			=> new(q.X, q.Y, q.Z, q.W);
	}
}
