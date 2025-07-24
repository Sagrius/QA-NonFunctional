using System.Collections;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;

public class NonFunctionalTests
{

    #region Stress Test

    [Header("Stress Test")]
    private GameObject _stressPrefab;
    private const int STRESS_QUANITITY = 10000;
    private const string STRESS_OBJECT_NAME = "Torch";


    [SetUp]
    public void Setup()
    {
        _stressPrefab = Resources.Load<GameObject>(STRESS_OBJECT_NAME);

        if (_stressPrefab == null)
        {
            Debug.LogError($"Failed to load prefab {STRESS_OBJECT_NAME}");
        }
    }

    [Test, Performance]
    public void StressTest()
    {
        Measure.Method(StressForLoop)

       .WarmupCount(5)
       .MeasurementCount(20)
       .GC()
       .Run();

    }

    public void StressForLoop()
    {
        for (int i = 0; i < STRESS_QUANITITY; i++)
        {
            Object.Instantiate(_stressPrefab);
        }
    }

    #endregion

    #region FPS test

    private const int FPS_FRAMES_COUNT = 500;

    [UnityTest, Performance]
    public IEnumerator MeasuresFpsAndStability()
    {
        yield return Measure.Frames()

        .WarmupCount(50)
        .MeasurementCount(FPS_FRAMES_COUNT)
        .Run();

    }

    //FPS = 1000 / FrameTime.
    //StdDev < 1ms is excellent , StdDev > 1ms is bad
    #endregion

    #region Load Test

    private const string SceneToLoad = "TestScene";

    [UnityTest]
    public IEnumerator SceneLoadsProperly()
    {
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(SceneToLoad);

        yield return sceneLoadOperation;

        Assert.AreEqual(SceneToLoad, SceneManager.GetActiveScene().name, "The loaded scene is not the active scene.");
    }
    #endregion

}