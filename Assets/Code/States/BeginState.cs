using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;
using UnityEngine.SceneManagement;

public class BeginState : IStateBase
{
    private GameStateMachine gameStateMachine;
    private GameObject canvas_ref;
    private bool debugMode;
    private float timeBeingActive;
    private float lifeTime;

   public BeginState (GameStateMachine gameStateMachine_Ref){
       this.gameStateMachine = gameStateMachine_Ref;
        debugMode = gameStateMachine.debugMode;
        
        if(debugMode){
            Debug.Log("------------Running BeginState Constructor-------------");
        }
        if(gameStateMachine.Canvas_Ref != null){
            canvas_ref = gameStateMachine.Canvas_Ref;
        }
        else
        {
            Debug.Log("BeginState cannot get a reference to the canvas");
        }

        if (gameStateMachine.GameOver)
            gameStateMachine.Reset();
       

            timeBeingActive = 0;
            lifeTime = 3;
        if(debugMode){
            Debug.Log("------------BeginState Constructor Done-------------");
        }
    }
    public void StateUpdate()
    {
        PlayerInput();
        TimeCounter();
    }
    public void ShowIt(){
        gameStateMachine.TurnOnCanvasSection(0);
    }
    public void PlayerInput()
    {
        if( Input.GetKey(KeyCode.Space))
        {
            gameStateMachine.ChangeState(
            new MainMenuState(gameStateMachine));
          
        }
    }
    private void TimeCounter(){
        timeBeingActive += Time.deltaTime;
        if(timeBeingActive > lifeTime) 
        { gameStateMachine.ChangeState(
            new MainMenuState(gameStateMachine));
           
           }
    }
}
