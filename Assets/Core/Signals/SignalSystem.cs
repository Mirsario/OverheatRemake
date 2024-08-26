using System;
using System.Collections.Generic;
using System.Linq;
using Overheat.Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Overheat.Core.Signals
{
	public sealed class SignalSystem : MonoBehaviour
	{
		private static readonly Dictionary<InputAction, int> actionIds = new();

		private static InputAction[] actions = Array.Empty<InputAction>();

		public static int ActionCount { get; private set; }

		public static ReadOnlySpan<InputAction> Actions => new(actions, 0, ActionCount);

		public InputActionAsset InputLayout;
		public string[] ActionMapNames = { "Game" }; // There's no InputActionMapReference as of now.

		void Start()
		{
			InputLayout.Enable();

			foreach (var actionMap in InputLayout.actionMaps) {
				if (!ActionMapNames.Contains(actionMap.name)) {
					continue;
				}

				foreach (var action in actionMap.actions) {
					int id = ActionCount++;

					actionIds[action] = id;
				}
			}

			EnsureSize();

			foreach (var pair in actionIds) {
				actions[pair.Value] = pair.Key;
			}
		}

		public static int GetActionId(InputAction action)
		{
			if (actionIds.TryGetValue(action, out int id)) {
				return id;
			}

			throw new InvalidOperationException($"Input action '{action.name}' is not part of the default input layout.");
		}

		private static void EnsureSize()
		{
			if (ActionCount > actions.Length) {
				Array.Resize(ref actions, (int)BitOperations.RoundUpToPowerOf2((uint)ActionCount));
			}
		}
	}
}
