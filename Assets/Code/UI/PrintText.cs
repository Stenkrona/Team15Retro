using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintText : MonoBehaviour
{
    private Text myText;

    private string currentStringToDisplay;

    public bool amIPlayerOne;
    public string myString;
    public bool canIPrint;

    public float printSpeed;

    private float printSpeedTracker;

    private string[] stringArray;

    private char[] charArray;

    private int charArrayIndex;
    private int stringArrayIndex;
    private InGameUIManager inGameUIManager_Ref;
    private GameStateMachine gameStateMachine_Ref;
    private bool isFullMessageShown;
    private bool inGameUIManagerRefOfMeIsSet;
    
    private void SetReferencesAndStartValues()
    {
        gameStateMachine_Ref = GameStateMachine.GetInstance();
        if (GetComponent<Text>() != null)
        {
            myText = GetComponent<Text>();
        }
        else
        {
            Debug.Log("PrintText can't get a Text reference for the variable myText");
        }
        if (inGameUIManager_Ref == null)
            inGameUIManager_Ref = InGameUIManager.GetInstance();

        //Check if I can print right away, if I have to wait for 
        //player one taunt text to print first or if the game is
        //over and I can print at once.
        if (amIPlayerOne || gameStateMachine_Ref.GameOver)
        {
            canIPrint = true;
            if(inGameUIManager_Ref.playerOneIntroUIAnim_Ref != null)
                inGameUIManager_Ref.playerOneIntroUIAnim_Ref.isTalking = true;
        }
            

        if (amIPlayerOne)
        {
            if (inGameUIManager_Ref.PlayerOnePrintText_Ref == null)
            {
                inGameUIManager_Ref.PlayerOnePrintText_Ref = this;
                inGameUIManagerRefOfMeIsSet = true;
               
            }
        }
        else
        {
           


                if (inGameUIManager_Ref != null)
                {
                    inGameUIManager_Ref.PlayerTwoPrintText_Ref = this;
                    inGameUIManagerRefOfMeIsSet = true;
                }
                else
                {
                    inGameUIManager_Ref = InGameUIManager.GetInstance();

                    if (inGameUIManager_Ref != null)
                    {
                        inGameUIManager_Ref.PlayerTwoPrintText_Ref = this;
                        inGameUIManagerRefOfMeIsSet = true;
                    }
                    else
                    {
                        Debug.Log("PrintText on: " + gameObject.name + ", is missing a value for inGameUIManager_Ref");
                    }

                }
            
        }

        charArrayIndex = 0;
        isFullMessageShown = false;
        currentStringToDisplay = "";
    }
   
   
    void Start()
    {
        SetReferencesAndStartValues();
    }
    void OnEnable()
    {
        if(!GameStateMachine.GetInstance().isFirstGame)
       SetReferencesAndStartValues();
    }

    void Update()
    {
        PrintLetters();
    }
  
    public void MakeCharArray()
    {
        charArray = myString.ToCharArray();
        charArrayIndex = 0;
    }
    private bool PrintSpeedGate()
    {
        printSpeedTracker += Time.deltaTime;

        if(printSpeedTracker >= printSpeed)
        {
            printSpeedTracker = 0;
            return true;

        }
        else
        {
            return false;
        }
    }
    private void PrintLetters()
    {
        if (canIPrint)
        {
            if (charArray != null && PrintSpeedGate() && charArrayIndex < charArray.Length)
            {


                currentStringToDisplay = currentStringToDisplay + charArray[charArrayIndex];

                myText.text = currentStringToDisplay;
                charArrayIndex++;
                isFullMessageShown = false;
            }
            else if (charArray != null && charArrayIndex == charArray.Length)
            {
                isFullMessageShown = true;
                canIPrint = false;

                if (!gameStateMachine_Ref.GameOver)
                { 

                    if (amIPlayerOne)
                    {
                        inGameUIManager_Ref.playerOneIntroUIAnim_Ref.isTalking = false;
                        Invoke("ImDone", .7f);
                    }
                    else
                    {
                        inGameUIManager_Ref.playerTwoIntroUIAnim_Ref.isTalking = false;
                        Invoke("ImDone", 1.4f);
                    }
                }
                else
                {
                    gameStateMachine_Ref.WaitAndChangeToBeginState();
                }

            }
        }

    }
    private void ImDone()
    {
        if (!gameStateMachine_Ref.GameOver)
        {
            if (amIPlayerOne)
            {
                if (!gameStateMachine_Ref.GameOver)
                {
                    inGameUIManager_Ref.PlayerTwoPrintText_Ref.canIPrint = true;
                    inGameUIManager_Ref.playerTwoIntroUIAnim_Ref.isTalking = true;
                    myString = "";
                }
            }
            else
            {
                gameStateMachine_Ref.IntroIsDone = true;
                currentStringToDisplay = "";
                gameStateMachine_Ref.TurnOffIntroTauntScreen();
            }
        }
        else
        {
            gameStateMachine_Ref.ChangeState(new BeginState(gameStateMachine_Ref));
        }
    }
    private void SetMeAsReferenceToInGameUIManager()
    {
        if (!inGameUIManagerRefOfMeIsSet)
        {
            if (amIPlayerOne)
            {
                inGameUIManager_Ref.PlayerOnePrintText_Ref = this;
                inGameUIManagerRefOfMeIsSet = true;
            }
            else
            {
                if (inGameUIManager_Ref != null)
                {
                    inGameUIManager_Ref.PlayerTwoPrintText_Ref = this;

                    inGameUIManagerRefOfMeIsSet = true;
                }
                else
                {
                    inGameUIManager_Ref = InGameUIManager.GetInstance();

                    if (inGameUIManager_Ref != null)
                    {
                        inGameUIManager_Ref.PlayerTwoPrintText_Ref = this;

                        inGameUIManagerRefOfMeIsSet = true;
                    }
                    else
                    {
                        Debug.Log("PrintText on: " + gameObject.name + ", is missing a value for inGameUIManager_Ref");
                    }

                }

            }
        }
    }
    public void ClearMyString()
    {
        myText.text = "";
        
        currentStringToDisplay = "";
    }
  

    public bool IsFullMessageShown { get { return isFullMessageShown; } }
}
