using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;
using UnityEngine.UI;

public class ScoreScreenState : IStateBase
{
    private GameStateMachine gameStateMachine_Ref;
    

    public ScoreScreenState(GameStateMachine gameStateMachine_Ref)
    {
        Debug.Log("SCORESCREEN");
        this.gameStateMachine_Ref = gameStateMachine_Ref;

        if (gameStateMachine_Ref.PlayerOneWon)
        {
            SetTextAndPicture(gameStateMachine_Ref.PlayerCharacterArray[0]);
        }
        else
        {
            SetTextAndPicture(gameStateMachine_Ref.PlayerCharacterArray[1]);
        }
        Debug.Log("ScoreScreen done!");
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameStateMachine_Ref.ChangeState(new BeginState(gameStateMachine_Ref));
        }
    }
    private void SetTextAndPicture(PlayerCharacter playerCharacter)
    {

        GameObject scoreScreen_Ref = gameStateMachine_Ref.scoreScreen_Ref;

        scoreScreen_Ref.transform.GetChild(1).GetComponent<Image>().sprite =
            playerCharacter.MyPicture;
        scoreScreen_Ref.transform.GetChild(2).GetComponent<PrintText>().myString =
            playerCharacter.MyVictoryPhrase;
        scoreScreen_Ref.transform.GetChild(2).GetComponent<PrintText>().MakeCharArray();
    }
}
