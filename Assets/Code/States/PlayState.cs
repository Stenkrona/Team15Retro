using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;

public class PlayState : IStateBase
{
    private GameStateMachine gameStateMachine_ref;
    private GameObject canvas_ref;
    private bool debugMode;
    private float timeBeingActive;
    private bool isDisplayingMessage;

   public PlayState(GameStateMachine gameStateMachine_Ref, bool showMessage)
    {

        gameStateMachine_ref = gameStateMachine_Ref;
        debugMode = gameStateMachine_ref.debugMode;

        if (debugMode)
        {
            Debug.Log("------------Running PlayState Constructor-------------");
        }

        canvas_ref = gameStateMachine_ref.Canvas_Ref;
        isDisplayingMessage = showMessage;

        if (!showMessage)
        {
        
        }

        if (debugMode)
        {
            Debug.Log("------------PlayState Constructor Done-------------");
        }
    }

    public void StateUpdate()
    {
        TimeTracker();

        if(!isDisplayingMessage) PlayerInput();

    }

    public void ShowIt()
    {
        gameStateMachine_ref.TurnOnCanvasSection(2);
    }

    public void PlayerInput()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
           gameStateMachine_ref.ChangeState(
           new PauseState(gameStateMachine_ref));
           gameStateMachine_ref.TurnOnCanvasSection(3);
        }

       
        
    }
    private void TimeTracker()
    {
        timeBeingActive += Time.deltaTime;

        if (timeBeingActive > 2.0f)
        {
            isDisplayingMessage = false;

            if (gameStateMachine_ref.canvas_Ref.transform.GetChild(2).GetChild(0).gameObject != null)
            {
                gameStateMachine_ref.canvas_Ref.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            }
           
            Debug.Log("PlayState has shown its message!");
        }
    }
    
}
