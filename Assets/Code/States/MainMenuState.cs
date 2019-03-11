using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;

public class MainMenuState : IStateBase
{
    private GameStateMachine gameStateMachine;
    private GameObject canvas_ref;
    private bool debugMode;
    private float timeBeingActive;
    public MainMenuState (GameStateMachine gameStateMachine_Ref)
    {
        gameStateMachine = gameStateMachine_Ref;
        debugMode = gameStateMachine.debugMode;

           if(debugMode){
            Debug.Log("------------Running MainMenuState Constructor-------------");
        }
        if(gameStateMachine.Canvas_Ref != null){
            canvas_ref = gameStateMachine.Canvas_Ref;
        }
        else
        {
            Debug.Log("MainMenuState cannot get a reference to the canvas");
        }

            timeBeingActive = 0;
       
        if(debugMode){
            Debug.Log("------------MainMenuState Constructor Done-------------");
        }
    }

    public void StateUpdate(){
        PlayerInput();
        TimeTracker();
    }
    public void ShowIt(){
        gameStateMachine.TurnOnCanvasSection(1);
    }
    public void PlayerInput(){

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameStateMachine.ChangeState(new PlayState(gameStateMachine, true));
        }
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

    }
    private void TimeTracker()
    {
        timeBeingActive += Time.deltaTime;
    }
}
