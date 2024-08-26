using System;
using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Weapons
{
	public sealed class Cooldown : MonoBehaviour
	{
		public float DefaultCooldown = 1.0f;

		[SerializeField, HideInInspector]
		private float activeUntilTime;

		public bool IsActive => activeUntilTime > Time.fixedTime;

		public void SetDefault()
			=> SetExact(DefaultCooldown);

		public void SetMultiplied(float multiplier)
			=> SetExact(DefaultCooldown * multiplier);

		public void SetExact(float time)
		{
			activeUntilTime = Math.Max(activeUntilTime, Time.fixedTime + time);
		}
	}
}
