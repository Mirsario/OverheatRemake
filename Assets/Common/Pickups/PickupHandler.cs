using Overheat.Common.Pickups;
using UnityEngine;
using UnityEngine.Events;

namespace Overheat.Common.Weapons
{
	public sealed class PickupHandler : MonoBehaviour
	{
		public struct PickupCollisionArguments
		{
			public PickupInfo Pickup;
			public PickupHandler Handler;
			public PickupContainer Container;

			public void DestroyPickupContainer()
			{
				Destroy(Container.gameObject);
			}
		}

		[SerializeField, HideInInspector] public UnityEvent<PickupCollisionArguments> OnPickupCollision = new();

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out PickupContainer pickup) && pickup.Item != null) {
				PickupCollisionArguments args;

				args.Pickup = pickup.Item;
				args.Handler = this;
				args.Container = pickup;

				OnPickupCollision.Invoke(args);
			}
		}
	}
}
