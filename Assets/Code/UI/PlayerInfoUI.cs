using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    private GameStateMachine gameStateMachine_Ref;


    public Text playerNameDisplay;
    public Text scoreDisplay;
    public Image playerPortraitDisplay;

    public bool amIPlayerOne;

    void Awake()
    {
        if(playerNameDisplay == null)
            playerNameDisplay = transform.GetChild(0).GetComponent<Text>();

        if (scoreDisplay == null)
            scoreDisplay = transform.GetChild(1).GetComponent<Text>();

        if (playerPortraitDisplay == null)
            playerPortraitDisplay = transform.GetChild(2).GetComponent<Image>();
        

    }
    void Start()
    {
        gameStateMachine_Ref = GameStateMachine.GetInstance();

        if (amIPlayerOne)
        {
            playerPortraitDisplay.sprite = gameStateMachine_Ref.PlayerCharacterArray[0].MyPicture;
        }
        else
        {
            playerPortraitDisplay.sprite = gameStateMachine_Ref.PlayerCharacterArray[1].MyPicture;
        }
           
    }
}
