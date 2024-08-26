using Overheat.Common.Movement;
using Overheat.Common.Weapons;
using UnityEngine;

namespace Overheat.Common.Animators
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(WeaponHandler))]
	public sealed class CombatAnimator : MonoBehaviour
	{
		private Animator animator;
		private WeaponHandler weaponHandler;
		private int inCombatProperty;
		private int holdingGunProperty;

		void Awake()
		{
			animator = GetComponent<Animator>();
			weaponHandler = GetComponent<WeaponHandler>();

			inCombatProperty = Animator.StringToHash("InCombat");
			holdingGunProperty = Animator.StringToHash("HoldingGun");
		}

		void Update()
		{
			animator.SetBool(inCombatProperty, true);
			animator.SetBool(holdingGunProperty, weaponHandler.CurrentWeapon != null);
		}
	}
}
