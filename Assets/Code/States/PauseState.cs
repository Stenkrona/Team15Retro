using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;

public class PauseState : IStateBase
{
    private GameStateMachine gameStateMachine;
    private GameObject canvas_ref;
    private bool debugMode;
    private float timeBeingActive;
   public PauseState (GameStateMachine gameStateMachine_ref){
       gameStateMachine = gameStateMachine_ref;
        debugMode = gameStateMachine.debugMode;
        if (debugMode)
        {
            Debug.Log("------------Running PauseState Constructor-------------");
        }
        canvas_ref = gameStateMachine.Canvas_Ref;
        gameStateMachine.BubbleManager_Ref.TurnOff();
      
        if (debugMode)
        {
            Debug.Log("------------PauseState Constructor Done-------------");
        }
    }
   public void StateUpdate(){
       PlayerInput();
   }
   public void ShowIt(){
       canvas_ref.transform.GetChild(3).gameObject.SetActive(true);
   }
   public void PlayerInput()
   {
       if(Input.GetKeyDown(KeyCode.Escape)){
            gameStateMachine.ChangeState(new PlayState(gameStateMachine, false));
             gameStateMachine.TurnOnCanvasSection(2);
       }
   }
}
