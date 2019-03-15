using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary; //Created a dictionary.

    private static EventManager eventManager; //Created an instance of the eventManager.

    private static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                //^ If there is no eventmanager, we try to find it.

                if (!eventManager)
                {
                    Debug.LogError("There needs to be an active EventManager script on a GameObject in your scene.");
                }

                else
                {
                    eventManager.Init();
                    //If we found the eventmanager, we go to the function to initialize it.
                }
            }

            return eventManager;
        }
    }



    void Init() //Here is where we initialize the eventManager.
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
            //If the dictionary is null, we create it.
        }
    }


    public static void StartListening(string eventName, UnityAction listener) //We start listening, we take an event, and an action.
    {

        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
            //Adds listener to the event if we have an entry.
        }

        //Adds new entry

        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);

            //Or we create a new entry and add a listener to that new entry.

        }

    }


    public static void StopListening(string eventName, UnityAction listener) //We stop listening, we find the event if it exists.
    {
        if (eventManager == null) return; //If it doesn't exist, it returns null.
        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
            //If the event exists, we remove the listener from it.
        }
    }


    //Trigger event on string name.

    public static void TriggerEvent(string eventName) //Here is where we trigger our event,
    {
        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {

            thisEvent.Invoke(); //Call all functions that are listening to this event.
        }
    }


}
