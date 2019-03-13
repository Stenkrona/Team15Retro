using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    private GameStateMachine gameStateMachine_Ref;
   
    void Start()
    {

        gameStateMachine_Ref = Camera.main.GetComponent<GameStateMachine>();
    }

    public void QuitGameButtonPressed()
    {
        Application.Quit();
    }

   
}
