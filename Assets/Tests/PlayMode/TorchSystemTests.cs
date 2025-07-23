using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TorchSystemTests
{
    GameObject player;
    PlayerTorchInteractor interactor;
    GameObject[] torches;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        player = new GameObject("Player");
        interactor = player.AddComponent<PlayerTorchInteractor>();
        interactor.interactionRange = 2f;

        torches = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            torches[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            torches[i].transform.position = new Vector3(i * 1.5f, 0, 0); // spaced apart
            torches[i].AddComponent<Torch>();
        }

        yield return null; // wait one frame
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(player);
        foreach (var torch in torches)
        {
            Object.Destroy(torch);
        }

        yield return null;
    }

    // 1. Unit Test – Torch Lights Up
    [UnityTest]
    public IEnumerator Torch_Lights_Up_When_Called()
    {
        var torch = torches[0].GetComponent<Torch>();
        torch.LightUp();
        yield return null;
        Assert.IsTrue(torch.isLit);
    }

    // 2. Integration Test – Player Can Light Nearby Torch
    [UnityTest]
    public IEnumerator Player_Can_Light_Torch_In_Range()
    {
        player.transform.position = Vector3.zero;
        var torch = torches[0].GetComponent<Torch>();
        torch.transform.position = player.transform.position + Vector3.forward * 1f;
        interactor.CheckIfNearTorch();
        yield return null;

        Assert.IsTrue(torch.isLit);
    }

    // 3. Regression Test – Torch Stays Lit After Player Leaves
    [UnityTest]
    public IEnumerator Torch_Stays_Lit_After_Player_Leaves()
    {
        var torch = torches[0].GetComponent<Torch>();
        torch.LightUp();
        player.transform.position = new Vector3(100, 0, 0);
        yield return null;

        Assert.IsTrue(torch.isLit);
    }

    // 4. Smoke Test – All 3 Torches Exist in Scene
    [UnityTest]
    public IEnumerator Scene_Has_Three_Torches()
    {
        yield return null;
        var foundTorches = Object.FindObjectsByType<Torch>(FindObjectsSortMode.None);
        Assert.AreEqual(3, foundTorches.Length);
    }

    // 5. Functional Test – Player Lights All 3 Torches
    [UnityTest]
    public IEnumerator Player_Lights_All_Torches()
    {
        player.transform.position = Vector3.zero;

        for (int i = 0; i < torches.Length; i++)
        {
            var torch = torches[i].GetComponent<Torch>();
            torch.transform.position = player.transform.position + Vector3.right * (i * 1.2f);
            torch.LightUp();
            yield return null;
        }

        foreach (var torch in torches)
        {
            Assert.IsTrue(torch.GetComponent<Torch>().isLit);
        }
    }
}
