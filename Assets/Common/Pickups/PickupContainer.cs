using Overheat.Common.Weapons;
using UnityEngine;

namespace Overheat.Common.Pickups
{
	public sealed class PickupContainer : MonoBehaviour
	{
		public PickupInfo Item;
		public Transform ItemContainer;

		[SerializeField, HideInInspector]
		private GameObject instancedModel;

		private void Start()
		{
			instancedModel = Instantiate(Item.Model, ItemContainer);
		}

		void OnTriggerEnter(Collider other)
		{
			
		}

#if UNITY_EDITOR
		public void OnDrawGizmos()
		{
			if (Application.isPlaying) {
				return;
			}

			if (Item == null || Item.Model == null || ItemContainer == null) {
				return;
			}

			Gizmos.matrix = ItemContainer.localToWorldMatrix;
			Gizmos.color = new Color(1f, 1f, 1f, 0.25f);

			if (Item.Model.TryGetComponent(out MeshFilter meshFilter)) {
				Gizmos.DrawMesh(meshFilter.sharedMesh, Vector3.zero, Quaternion.identity, Vector3.one);
			}
		}
#endif
	}
}
