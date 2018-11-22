using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestRunMode {


	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator TestRunModeWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame

		var m = MainThreadDispatcher.Instance;
		
		
			
		yield return null;
	}
}
