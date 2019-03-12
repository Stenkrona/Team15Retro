using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGameButton : MonoBehaviour
{
    private GameStateMachine gameStateMachine_Ref;


    void Start()
    {
        gameStateMachine_Ref = Camera.main.GetComponent<GameStateMachine>();
    }

    public void ResumeGameButtonPressed()
    {
        gameStateMachine_Ref.ChangeState(new PlayState(gameStateMachine_Ref, false));
    }

}
