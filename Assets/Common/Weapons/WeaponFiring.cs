using System;
using System.Collections.Generic;
using System.Linq;
using Overheat.Common.Projectiles;
using Overheat.Core.Signals;
using Overheat.Core.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Overheat.Common.Weapons
{
	[Serializable]
	public struct WeaponShot
	{
		public GameObject Projectile;
		public float DelayInSeconds;
		public bool InstantiateOnlyChildren;
		[HideInInspector] public float TimeInSeconds;
	}

	[RequireComponent(typeof(Signals))]
	public sealed class WeaponFiring : MonoBehaviour
	{
		public struct OnFireArgs
		{
			public Vector3 Point;
			public Vector3 Offset;
		}


		public InputActionReference Input;
		public Cooldown Cooldown;
		public float CooldownMultiplier = 1f;
		public WeaponShot[] Shots = Array.Empty<WeaponShot>();

		public UnityEvent<OnFireArgs> OnFire = new();

		[SerializeField, HideInInspector] private Signals signals;
		[SerializeField, HideInInspector] private WeaponFiringPositions weaponFiringPositions;
		[SerializeField, HideInInspector] private List<WeaponShot> pendingShots = new();

		void Awake()
		{
			signals = GetComponent<Signals>();
			weaponFiringPositions = GetComponent<WeaponFiringPositions>();
		}

		void FixedUpdate()
		{
			float time = Time.fixedTime;

			if (signals.IsActive(Input) && (Cooldown == null || !Cooldown.IsActive)) {
				pendingShots.AddRange(Shots.Select(shot => {
					shot.TimeInSeconds = time + shot.DelayInSeconds;
					return shot;
				}));

				if (Cooldown != null) {
					Cooldown.SetMultiplied(CooldownMultiplier);
				}
			}

			for (int i = 0; i < pendingShots.Count; i++) {
				var shot = pendingShots[i];
				if (time >= shot.TimeInSeconds) {
					Fire(in shot);
					pendingShots.RemoveAt(i--);
				}
			}
		}

		public void Fire(in WeaponShot shot)
		{
			OnFireArgs args;
			args.Point = signals.Proxy.transform.position;
			args.Offset = weaponFiringPositions != null ? weaponFiringPositions.GetNextFiringPositionOffset() : default;

			OnFire.Invoke(args);
			CreateProjectile(in shot, in args);
		}

		private void CreateProjectile(in WeaponShot shot, in OnFireArgs args)
		{
			var firePosition = transform.TransformPoint(args.Offset);
			var fireRotation = signals.LookRotation; //transform.rotation;
			var owner = TryGetComponent(out OwnerInfo ownerInfo) ? ownerInfo.Owner : null;

			void UpdateProjectile(GameObject obj)
			{
				if (owner != null && obj.TryGetComponent(out Projectile projectile)) {
					projectile.GetOrAddComponent<OwnerInfo>().Owner = owner;
				}
			}

			if (shot.InstantiateOnlyChildren) {
				var prefabTransform = shot.Projectile.transform;
				int prefabChildCount = prefabTransform.childCount;

				for (int i = 0; i < prefabChildCount; i++) {
					UpdateProjectile(Instantiate(prefabTransform.GetChild(i).gameObject, firePosition, fireRotation, transform));
				}
			} else {
				UpdateProjectile(Instantiate(shot.Projectile, firePosition, fireRotation, transform));
			}
		}
	}
}
