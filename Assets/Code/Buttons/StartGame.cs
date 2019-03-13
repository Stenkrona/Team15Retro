using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private GameStateMachine gameStateMachine_Ref;


   void Start()
    {
        gameStateMachine_Ref = Camera.main.GetComponent<GameStateMachine>();
    }

    public void StartGameButtonPressed()
    {
        if (gameStateMachine_Ref != null)
        {
            gameStateMachine_Ref.ChangeState(new PlayState(gameStateMachine_Ref, true));
        }
        else
        {
            Debug.Log("StartGame Button is missing a reference to GameStateMachine");
        }
    }
}
