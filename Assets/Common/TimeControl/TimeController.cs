using System;
using Overheat.Common.Weapons;
using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.TimeControl
{
	/// <summary>
	/// Implements the game's time control mechanic.
	/// </summary>
	public sealed class TimeController : MonoBehaviour
	{
		public float MinTimeScale = 0.1f;
		public float MaxTimeScale = 1.0f;
		public float CurrentTimeScale = 1.0f;
		public float TimeScaleDamping = 0.1f;
		public int UpdatesPerSecond = 60;

		void FixedUpdate()
		{
			float factor = CalculateTargetTimeFactor(transform.position);
			float targetTimeScale = Mathf.Lerp(MinTimeScale, MaxTimeScale, factor);

			CurrentTimeScale = CurrentTimeScale < targetTimeScale
				? MathUtils.Damp(CurrentTimeScale, targetTimeScale, TimeScaleDamping * TimeScaleDamping, Time.fixedUnscaledDeltaTime)
				: targetTimeScale;
			
			Time.timeScale = CurrentTimeScale;
			//Time.fixedDeltaTime = timeScale / UpdatesPerSecond;
		}

		private float CalculateTargetTimeFactor(Vector3 point)
		{
			float result = 1.0f;
			var expectedOwner = transform.parent != null ? transform.parent.gameObject : gameObject;

			foreach (var danger in FindObjectsByType<Danger>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
				var dangerPoint = danger.transform.position;
				float rangeSquared = danger.Range * danger.Range;
				float distanceSquared = Vector3.SqrMagnitude(point - dangerPoint);

				if (distanceSquared >= rangeSquared) {
					continue;
				}

				if (danger.TryGetComponent(out OwnerInfo ownerInfo) && ownerInfo.Owner == expectedOwner) {
					continue;
				}

				float distanceFactor = Mathf.Clamp01(Mathf.Sqrt(distanceSquared) / danger.Range);
				//float angleFactor = danger.AngleFactor >= 1f ? 1f : 1f - ((Vector3.Dot(danger.transform.forward, (point - dangerPoint).normalized) + 1f) * 0.5f);
				//float factor = Mathf.Lerp(distanceFactor, 1f, angleFactor);
				float factor = Mathf.Clamp01(danger.DistanceCurve.Evaluate(distanceFactor));

				result = MathF.Min(result, factor);
				//result *= factor;
			}

			return result;
		}
	}
}
