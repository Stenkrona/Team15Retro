using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    private GameStateMachine gameStateMachine_Ref;


    public Text playerNameDisplay;
  
    public Image playerPortraitDisplay;

    public bool amIPlayerOne;

    void Awake()
    {
        if(playerNameDisplay == null)
        {
            if(transform.GetChild(0) != null && transform.GetChild(0).GetComponent<Text>() != null)
            {
                playerNameDisplay = transform.GetChild(0).GetComponent<Text>();
            }
            else
            {
                Debug.Log("PlayerInfo, player one: " + amIPlayerOne + " , can't get a" +
                    " reference to playerNameDisplay Text");
            }
        }

        if (playerPortraitDisplay == null)
        {
            if(transform.GetChild(2) != null && transform.GetChild(2).GetComponent<Image>() != null)
            {
                playerPortraitDisplay = transform.GetChild(2).GetComponent<Image>();
            }
            else
            {
                Debug.Log("PlayerInfo, player one: " + amIPlayerOne + " , can't get a" +
                   " reference to playerPortrait Image");
            }
        }
          
        

    }
    void Start()
    {
        SetReferences();
    }
    void OnEnable()
    {
        SetReferences();
    }

    private void SetReferences()
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

        if (playerNameDisplay == null)
        {
            if (transform.GetChild(0) != null && transform.GetChild(0).GetComponent<Text>() != null)
            {
                playerNameDisplay = transform.GetChild(0).GetComponent<Text>();
            }
            else
            {
                Debug.Log("PlayerInfo, player one: " + amIPlayerOne + " , can't get a" +
                    " reference to playerNameDisplay Text");
            }
        }

        if (playerPortraitDisplay == null)
        {
            if (transform.GetChild(2) != null && transform.GetChild(2).GetComponent<Image>() != null)
            {
                playerPortraitDisplay = transform.GetChild(2).GetComponent<Image>();
            }
            else
            {
                Debug.Log("PlayerInfo, player one: " + amIPlayerOne + " , can't get a" +
                   " reference to playerPortrait Image");
            }
        }
    }
}
