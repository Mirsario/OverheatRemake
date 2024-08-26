using UnityEngine;

namespace Overheat.Common.Instancing
{
	public sealed class CreateOnDestroy : MonoBehaviour
	{
		public GameObject Object;

		void OnDestroy()
		{
			// Don't ruin scene cleanups.
			if (!gameObject.scene.isLoaded) {
				return;
			}

			var obj = Instantiate(Object, transform.position, transform.rotation, transform.parent);
			obj.transform.localScale = transform.localScale;
		}
	}
}
