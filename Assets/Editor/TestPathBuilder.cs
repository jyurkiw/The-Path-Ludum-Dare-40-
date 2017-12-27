using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestPathBuilder {

    /// <summary>
    /// The test algorithms are located in Assets/scripts/Game/PathAlgorithm/TestAlgorithms.cs
    /// </summary>
	[Test]
	public void TestPathBuilder_AlgorithmDiscovery() {
        List<IPathAlgorithm> algos = PathBuilder.DiscoverAlgotithmsByType<IPathAlgorithm>(AlgorithmType.TEST);

        Assert.AreEqual(3, algos.Count);
        Assert.Contains(typeof(TestAlgorithmSuccess), algos.Select(x => x.GetType()).ToList());
        Assert.Contains(typeof(TestHighAlgorithmSuccess), algos.Select(x => x.GetType()).ToList());
        Assert.Contains(typeof(TestLowAlgorithmSuccess), algos.Select(x => x.GetType()).ToList());
    }
}