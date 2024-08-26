using Overheat.Common.Looking;
using Overheat.Common.Offsets;
using Overheat.Core.Signals;
using Overheat.Core.Utilities;
using UnityEngine;

namespace Overheat.Common.Weapons
{
	[RequireComponent(typeof(Signals))]
	public sealed class WeaponHandler : MonoBehaviour
	{
		public WeaponInfo CurrentWeapon;

		private Signals signals;

		void Awake()
		{
			signals = GetComponent<Signals>();

			if (TryGetComponent(out PickupHandler pickupHandler)) {
				pickupHandler.OnPickupCollision.AddListener(PickupWeapon);
			}
		}

		public void PickupWeapon(PickupHandler.PickupCollisionArguments args)
		{
			if (CurrentWeapon != null) {
				return;
			}

			if (!args.Pickup.TryGetComponent(out WeaponInfo weaponInfo)) {
				return;
			}

			var weaponObject = Instantiate(args.Pickup.gameObject);

			SetWeapon(weaponObject.GetComponent<WeaponInfo>());

			args.DestroyPickupContainer();

			//TODO: Replace with proper animations.
			if (TryGetComponent(out ViewDescription viewDescription)
			&& viewDescription.Viewmodel is Viewmodel viewmodel
			&& viewmodel.TryGetComponent(out VisualOffset visualOffset)) {
				visualOffset.Offset.y -= 2f;
			}
		}

		public void SetWeapon(WeaponInfo weapon)
		{
			if (CurrentWeapon != null) {
				CurrentWeapon.GetComponent<Signals>().Proxy = null;
				CurrentWeapon.GetComponent<OwnerInfo>().Owner = null;
				CurrentWeapon.transform.parent = null;
			}

			CurrentWeapon = weapon;

			if (CurrentWeapon != null) {
				var weaponParent = GetWeaponParentTransform();

				CurrentWeapon.GetComponent<Signals>().Proxy = signals;
				CurrentWeapon.GetOrAddComponent<OwnerInfo>().Owner = gameObject;
				CurrentWeapon.transform.parent = weaponParent;
				CurrentWeapon.transform.localPosition = default;
				CurrentWeapon.transform.localRotation = Quaternion.identity;
			}
		}

		public Transform GetWeaponParentTransform()
		{
			if (TryGetComponent(out ViewDescription viewDescription)) {
				if (viewDescription.Viewmodel != null && viewDescription.Viewmodel.TryGetComponent(out WeaponHandlingDescription weaponDescription)) {
					return weaponDescription.WeaponParent;
				}

				if (TryGetComponent(out weaponDescription)) {
					return weaponDescription.WeaponParent;
				}
			}

			return transform;
		}
	}
}
