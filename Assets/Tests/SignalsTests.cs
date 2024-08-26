using NUnit.Framework;
using Overheat.Core.Signals;
using UnityEngine;

namespace Overheat.Tests
{
	public class SignalsTests
	{
		[Test]
		public void AnglesTest()
		{
			var obj = new GameObject();
			var signals = obj.AddComponent<Signals>();

			Assert.AreEqual(Vector2.zero, signals.LookAngles);
			Assert.AreEqual(Vector3.forward, signals.LookDirection);
			Assert.AreEqual(Quaternion.identity, signals.LookRotation);

			signals.LookAngles = new Vector2(0f, 90f);
			Assert.That(signals.LookDirection, Is.EqualTo(Vector3.right).Within(Vector3.one * 0.01f));
			Assert.That(signals.LookRotation, Is.EqualTo(Quaternion.AngleAxis(90f, Vector3.up)).Within(0.1f));

			Object.DestroyImmediate(obj);
		}
	}
}
