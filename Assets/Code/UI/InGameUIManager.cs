using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    private static InGameUIManager inGameUIManager_Ref;
    private GameStateMachine gameStateMachine_Ref;

    public Image playerOneIntroTauntImage_Ref;
    public Image playerTwoIntroTauntImage_Ref;
    public GameObject playerOneCollectedBlocks_Ref;
    public GameObject playerTwoCollectedBlocks_Ref;

    private PlayerInfoUI playerOneInfo_Ref;
    private PlayerInfoUI playerTwoInfo_Ref;

    private PrintText playerOnePrintText_Ref;
    private PrintText playerTwoPrintText_Ref;


    private int playerOneScore;
    private int playerTwoScore;

    public static InGameUIManager GetInstance()
    {
        return inGameUIManager_Ref;
    }

    void Awake()
    {
        if(inGameUIManager_Ref == null)
        {
            inGameUIManager_Ref = this;
        }
        else
        {
            Destroy(this);
        }
    }
    

    void Start()
    {
        gameStateMachine_Ref = GameStateMachine.GetInstance();
        gameStateMachine_Ref.inGameUIManager_Ref = this;

        playerOneInfo_Ref = gameStateMachine_Ref.playerInfoOne_Ref;
        playerTwoInfo_Ref = gameStateMachine_Ref.playerInfoTwo_Ref;

        FirstUIUpdate();
    }
   
    void OnEnable()
    {
        UpdatePlayerDisplays();
    }
    private void FirstUIUpdate()
    {
        playerOneInfo_Ref.playerNameDisplay.text = "Player One";
        playerTwoInfo_Ref.playerNameDisplay.text = "Player Two";

        playerOneIntroTauntImage_Ref.sprite = gameStateMachine_Ref.PlayerCharacterArray[0].MyPicture;
        playerTwoIntroTauntImage_Ref.sprite = gameStateMachine_Ref.PlayerCharacterArray[1].MyPicture;

        playerOnePrintText_Ref.ClearMyString();
        playerOnePrintText_Ref.myString = gameStateMachine_Ref.PlayerCharacterArray[0].MyIntroPhrase;
        playerOnePrintText_Ref.MakeCharArray();

        PlayerTwoPrintText_Ref.ClearMyString();
        playerTwoPrintText_Ref.myString = gameStateMachine_Ref.PlayerCharacterArray[1].MyIntroPhrase;
        PlayerTwoPrintText_Ref.MakeCharArray();
       

    }
    public void ActivateImage(bool playerOne, int blockNumber)
    {
        if(playerOne)
        {
            playerOneCollectedBlocks_Ref.transform.GetChild(blockNumber).gameObject.SetActive(true);
        }
        else
        {
            playerTwoCollectedBlocks_Ref.transform.GetChild(blockNumber).gameObject.SetActive(true);
        }
        
    }

    public void ResetCollectedBlocks()
    {
        for (int i = 0; i < 7; i++)
        {
            playerOneCollectedBlocks_Ref.transform.GetChild(i).gameObject.SetActive(false);
            playerTwoCollectedBlocks_Ref.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void UpdatePlayerDisplays()
    {

        if (gameStateMachine_Ref != null)
        {
            playerOneIntroTauntImage_Ref.sprite = gameStateMachine_Ref.PlayerCharacterArray[0].MyPicture;
            playerTwoIntroTauntImage_Ref.sprite = gameStateMachine_Ref.PlayerCharacterArray[1].MyPicture;
        }

        if (playerOnePrintText_Ref != null)
        {
            playerOnePrintText_Ref.myString = gameStateMachine_Ref.PlayerCharacterArray[0].MyIntroPhrase;
            playerOnePrintText_Ref.MakeCharArray();
            playerOnePrintText_Ref.ClearMyString();
        }

        if (playerTwoPrintText_Ref != null)
        {
            playerTwoPrintText_Ref.myString = gameStateMachine_Ref.PlayerCharacterArray[1].MyIntroPhrase;
            PlayerTwoPrintText_Ref.MakeCharArray();
            PlayerTwoPrintText_Ref.ClearMyString();
        }
    }

    
    //properties
    public int PlayerOneScore { set { playerOneScore = value; } }
    public int PlayerTwoScore { set { playerTwoScore = value; } }
    public PrintText PlayerOnePrintText_Ref { get { return playerOnePrintText_Ref; }
        set { playerOnePrintText_Ref = value; } }
    public PrintText PlayerTwoPrintText_Ref { get { return playerTwoPrintText_Ref; }
        set { playerTwoPrintText_Ref = value; }
    }
}
