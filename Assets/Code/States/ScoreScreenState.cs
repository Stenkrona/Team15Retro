using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;

public class ScoreScreenState : IStateBase
{
    private GameStateMachine gameStateMachine_Ref;

    public ScoreScreenState(GameStateMachine gameStateMachine_Ref)
    {
        this.gameStateMachine_Ref = gameStateMachine_Ref;
    }

    public void StateUpdate()
    {
        PlayerInput();
    }
    public void ShowIt()
    {
        gameStateMachine_Ref.TurnOnCanvasSection(4);
    }
    public void PlayerInput()
    {

    }
}
