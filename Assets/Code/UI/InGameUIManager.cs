using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    private GameStateMachine gameStateMachine_Ref;

    private int playerOneScore;
    private int playerTwoScore;

    
    void Start()
    {
        gameStateMachine_Ref = GameStateMachine.GetInstance();     
    }
    public void UpdateUI()
    {

    }
   
    
    public int PlayerOneScore { set { playerOneScore = value; } }
    public int PlayerTwoScore { set { playerTwoScore = value; } }
}
