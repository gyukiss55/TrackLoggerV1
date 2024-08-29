using System.Collections;
using UnityEngine;


public class TimerExample : MonoBehaviour
{
    private UDPClient uDPClient;

    void Start()
    {
        GameObject targetObject = GameObject.Find("Canvas");

        if (targetObject != null)
        {
            // Get the ScriptB component and call the method
            uDPClient = targetObject.GetComponent<UDPClient>();
        }
        else
        {
            Debug.LogWarning("uDPLogger is null!");
        }
        // Start the coroutine to call the function every second
        StartCoroutine(CallFunctionEverySecond());
    }

    IEnumerator CallFunctionEverySecond()
    {
        while (true)
        {
            // Call the function you want to execute
            if (uDPClient != null)
                uDPClient.OnTrigger();

            // Wait for 1 second
            yield return new WaitForSeconds(1f);
        }
    }

    void YourFunction()
    {
        // Your repeated action here
        Debug.Log("Function called at: " + Time.time);
    }
}
