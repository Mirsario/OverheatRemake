using Overheat.Common.Camera;
using Overheat.Core.Signals;
using UnityEngine;
using UnityEngine.Rendering;

namespace Overheat
{
	[RequireComponent(typeof(Signals))]
    public sealed class PlayerController : MonoBehaviour
    {
		public Signals ControlledEntity;
		public Camera Camera;

		[SerializeField, HideInInspector] private Signals lastControlledEntity;
		[SerializeField, HideInInspector] private Signals signals;

		void Awake()
		{
			signals = GetComponent<Signals>();
			UpdateEntity();
		}

		void FixedUpdate()
		{
			UpdateEntity();
		}

		private void UpdateEntity()
		{
			if (ControlledEntity == lastControlledEntity) {
				return;
			}

			var cameraPosition = Vector3.zero;

			if (ControlledEntity.TryGetComponent(out ViewDescription viewDescription)) {
				cameraPosition = viewDescription.EyePosition;

				if (viewDescription.ThirdPersonModel != null) {
					foreach (var renderer in viewDescription.ThirdPersonModel.GetComponentsInChildren<Renderer>()) {
						renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
					}
				}

				if (viewDescription.Viewmodel == null && viewDescription.ViewmodelPrefab != null) {
					viewDescription.Viewmodel = Instantiate(viewDescription.ViewmodelPrefab, parent: Camera.transform);
					viewDescription.Viewmodel.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

					var viewmodel = viewDescription.Viewmodel.GetComponent<Viewmodel>();
					viewmodel.CameraTransform = transform;
					viewmodel.Body = ControlledEntity;
				}
			}

			transform.SetParent(ControlledEntity.transform);
			transform.SetLocalPositionAndRotation(cameraPosition, Quaternion.identity);

			signals.Proxy = ControlledEntity;
			lastControlledEntity = ControlledEntity;
		}
	}
}
