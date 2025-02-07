using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FireBaseIniti : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(OnDependencyStatusReceived);
    }

    private void OnDependencyStatusReceived(Task<DependencyStatus> task)
    {
        try
        {
            if (!task.IsCompletedSuccessfully)
                throw new Exception("Could not resolve all Firebase dependencies", task.Exception);

            var status = task.Result;
            if (status != DependencyStatus.Available)
                throw new Exception($"Could not resolve all Firebase dependencies: {status}");

            print("Firebase initialized successfully!");
            LogTestEvent();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void LogTestEvent()
    {
        FirebaseAnalytics.LogEvent("TEST_START_APP", new Parameter("is_started", 1));
        print("Bebra");
    }

    public void LogTestEvent2()
    {
        FirebaseAnalytics.LogEvent("TEST_STORE_BUY", new Parameter("is_buyed", 1));
        print("Boba");
    }
}
