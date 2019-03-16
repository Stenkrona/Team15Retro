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
    

   
    void Start()
    {
        gameStateMachine_Ref = GameStateMachine.GetInstance();

        myText = GetComponent<Text>();
      
        charArrayIndex = 0;

        if (amIPlayerOne || gameStateMachine_Ref.GameOver) canIPrint = true;
     
      
        inGameUIManager_Ref = gameStateMachine_Ref.inGameUIManager_Ref;

        if (amIPlayerOne)
        {
            inGameUIManager_Ref.PlayerOnePrintText_Ref = this;
        }
        else
        {
            inGameUIManager_Ref.PlayerTwoPrintText_Ref = this;
        }

     
        isFullMessageShown = false;
       
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
                        Invoke("ImDone", .7f);
                    }
                    else
                    {
                        Invoke("ImDone", 1.4f);
                    }
                }
                else
                {
                    Invoke("ImDone", 3.0f);
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
                inGameUIManager_Ref.PlayerTwoPrintText_Ref.canIPrint = true;
            }
            else
            {
                gameStateMachine_Ref.IntroIsDone = true;
                gameStateMachine_Ref.TurnOffIntroTauntScreen();
            }
        }
        else
        {
            gameStateMachine_Ref.ChangeState(new BeginState(gameStateMachine_Ref));
        }
    }
  

    public bool IsFullMessageShown { get { return isFullMessageShown; } }
}
