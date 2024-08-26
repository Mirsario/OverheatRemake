using System.Runtime.CompilerServices;
using UnityEngine;

namespace Overheat.Core.Utilities
{
	public static class ComponentExtensions
	{
		public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
		{
			var result = obj.GetComponent<T>();
			return result != null ? result : obj.AddComponent<T>();
		}

		public static T GetOrAddComponent<T>(this Component obj) where T : Component
		{
			var result = obj.GetComponent<T>();
			return result != null ? result : obj.gameObject.AddComponent<T>();
		}
	}
}
