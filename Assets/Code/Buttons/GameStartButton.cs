using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButton : MonoBehaviour
{
    private GameStateMachine gameStateMachine_Ref;

    void Start()
    {
        gameStateMachine_Ref = Camera.main.GetComponent<GameStateMachine>();    
    }

    
    public void StartGameButtonPressed()
    {
        gameStateMachine_Ref.ChangeState(new PlayState(gameStateMachine_Ref, true));
    }
}
