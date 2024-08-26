using System;
using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Weapons
{
	public sealed class Cooldown : MonoBehaviour
	{
		[SerializeField, HideInInspector]
		private float activeUntilTime;

		public bool IsActive => activeUntilTime > Time.fixedTime;

		public void Set(float time)
		{
			activeUntilTime = Math.Max(activeUntilTime, Time.fixedTime + time);
		}
	}
}
