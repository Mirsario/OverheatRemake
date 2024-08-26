using System;
using UnityEngine;
using UQuaternion = UnityEngine.Quaternion;
using NQuaternion = System.Numerics.Quaternion;

namespace Overheat.Core.Utilities
{
	public static class MathUtils
	{
		public static float Damp(float source, float destination, float smoothing, float dt)
		{
			// See this:
			// https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp

			return Mathf.Lerp(source, destination, 1f - Mathf.Pow(smoothing, dt));
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

		public static NQuaternion Damp(NQuaternion q1, NQuaternion q2, float smoothing, float dt)
		{
			const float SlerpEpsilon = 1e-6f;

			float cosOmega = NQuaternion.Dot(q1, q2);
			float sign = 1.0f;

			if (cosOmega < 0.0f) {
				cosOmega = -cosOmega;
				sign = -1.0f;
			}

			float s1, s2;
			float amount2 = 1f - Mathf.Pow(smoothing, dt);
			float amount1 = 1f - amount2;

			if (cosOmega > 1.0f - SlerpEpsilon) {
				// Too close, do straight linear interpolation.
				s1 = amount1;
				s2 = amount2 * sign;
			} else {
				float omega = MathF.Acos(cosOmega);
				float invSinOmega = 1 / MathF.Sin(omega);

				s1 = MathF.Sin(amount1 * omega) * invSinOmega;
				s2 = MathF.Sin(amount2 * omega) * invSinOmega * sign;
			}

			return q1 * s1 + q2 * s2;
		}

		public static NQuaternion ToNumerics(this UQuaternion q)
			=> new(q.x, q.y, q.z, q.w);

		public static UQuaternion ToUnity(this NQuaternion q)
			=> new(q.X, q.Y, q.Z, q.W);
	}
}
