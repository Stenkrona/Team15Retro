using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;

public class PlayState : IStateBase
{
    private GameStateMachine gameStateMachine;
    private GameObject canvas_ref;
    private bool debugMode;
    private float timeBeingActive;
    private bool isDisplayingMessage;
    private SkipButton skipButton_Ref;
    

    private bool hasShownMessage;

   public PlayState(GameStateMachine gameStateMachine_Ref, bool showMessage)
    {

        gameStateMachine = gameStateMachine_Ref;
        debugMode = gameStateMachine.debugMode;
        if (debugMode)
        {
            Debug.Log("------------Running PlayState Constructor-------------");
        }
        if (!gameStateMachine.IntroIsDone)
        {
            gameStateMachine_Ref.introTauntScreen_Ref.SetActive(true);
        }
            canvas_ref = gameStateMachine.Canvas_Ref;


       

        isDisplayingMessage = showMessage;
        if (!showMessage)
        {
            gameStateMachine.BubbleManager_Ref.TurnOn();
            gameStateMachine.ToggleSpawners(true);
        }
        if (debugMode)
        {
            Debug.Log("------------PlayState Constructor Done-------------");
        }
    }

    public void StateUpdate()
    {
        if(GameObject.Find("Skip") != null && skipButton_Ref == null)
        {
            skipButton_Ref = GameObject.Find("Skip").GetComponent<SkipButton>();
        }
        TimeTracker();

        if(!isDisplayingMessage) PlayerInput();

        if (Input.GetKeyDown(gameStateMachine.playerOneInput_Ref.upThruster)
           && !gameStateMachine.IntroIsDone)
        {
          
            skipButton_Ref.IsButtonPressed(true);
        }
        else if (Input.GetKeyUp(gameStateMachine.playerOneInput_Ref.upThruster)
            && !gameStateMachine.IntroIsDone)
        {
            skipButton_Ref.IsButtonPressed(false);
        }

    }
    public void ShowIt(){
        gameStateMachine.TurnOnCanvasSection(2);
    }
    public void PlayerInput(){
        if(Input.GetKeyDown(KeyCode.Escape) && gameStateMachine.IntroIsDone)
        {
            gameStateMachine.ChangeState(
                new PauseState(gameStateMachine));
           gameStateMachine.TurnOnCanvasSection(3);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            gameStateMachine.FastWin();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            gameStateMachine.bubbleManager_Ref.AddPeneltyPoint(true);
        }
       




    }
    private void TimeTracker()
    {
        if (gameStateMachine.IntroIsDone) timeBeingActive += Time.deltaTime;

        if (timeBeingActive > 2.0f && !hasShownMessage)
        {
            isDisplayingMessage = false;

            gameStateMachine.BubbleManager_Ref.TurnOn();

            if (gameStateMachine.canvas_Ref.transform.GetChild(2).GetChild(0).gameObject != null)
            {

                gameStateMachine.canvas_Ref.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            }

            if (!gameStateMachine.isFirstGame)
            {
                gameStateMachine.playerOneSpawner_Ref.GetComponent<Spawner>().SpawnNext();
                gameStateMachine.playerTwoSpawner_Ref.GetComponent<Spawner>().SpawnNext();
            }

            gameStateMachine.ToggleSpawners(true);

            Gameboard.GetInstance().PlayerOneSpawner =
           gameStateMachine.playerOneSpawner_Ref.GetComponent<Spawner>();

            Gameboard.GetInstance().PlayerTwoSpawner =
                gameStateMachine.playerTwoSpawner_Ref.GetComponent<Spawner>();

            Gameboard.GetInstance().GetAndSortLanders();
            Gameboard.GetInstance().SetFullDistances();

            Gameboard.GetInstance().SpawnerOneSpawnPosition = 
                gameStateMachine.playerOneSpawnPosition;

            Gameboard.GetInstance().SpawnerTwoSpawnPosition =
                gameStateMachine.playerTwoSpawnPosition;
            

            hasShownMessage = true;
        }
    }
    
}
