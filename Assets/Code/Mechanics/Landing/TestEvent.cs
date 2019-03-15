using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestEvent : MonoBehaviour
{
    private UnityAction someListener; //We declare the variable for a listener.

    void Awake()
    {
        someListener = new UnityAction(SomeFunction);
    }



    //Do OnEnable and OnDisable, don't do it on Awake, and Disable before destroying the GameObject.
    void OnEnable()
    {
        EventManager.StartListening("testevent", someListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("testevent", someListener);
    }

    void SomeFunction()
    {
        Debug.Log("This is a function which shows us that our Test listener is working.");
    }
}
