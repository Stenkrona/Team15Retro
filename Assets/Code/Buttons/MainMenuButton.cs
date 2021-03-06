﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{

    private GameStateMachine gameStateMachine_Ref;

    void Start()
    {
        gameStateMachine_Ref = GameStateMachine.GetInstance();
    }

    public void MainMenuButtonPressed()
    {
        gameStateMachine_Ref.ChangeState(new MainMenuState(gameStateMachine_Ref));
    }
   
}
