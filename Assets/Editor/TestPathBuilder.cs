using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestPathBuilder {

    /// <summary>
    /// The test algorithms are located in Assets/scripts/Game/PathAlgorithm/TestAlgorithms.cs
    /// </summary>
	[Test]
	public void TestPathBuilder_AlgorithmDiscovery() {
        List<IPathAlgorithm> algos = PathBuilder.DiscoverAlgotithmsByType(AlgorithmType.TEST);

        Assert.AreEqual(1, algos.Count);
        Assert.AreEqual(typeof(TestAlgorithmSuccess), algos[0].GetType());
	}
}