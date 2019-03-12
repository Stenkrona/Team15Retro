using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{

    private GameStateMachine gameStateMachine_Ref;


    void Start()
    {
        gameStateMachine_Ref = Camera.main.GetComponent<GameStateMachine>();
    }

    public void MainMenuButtonPressed()
    {
        if(gameStateMachine_Ref != null)
        {
            gameStateMachine_Ref.ChangeState(new MainMenuState(gameStateMachine_Ref));
        }
        else
        {
            Debug.Log("MainMenuButtonPressed is missing a Reference to GameStateMachine");
        }
    }
   
}
