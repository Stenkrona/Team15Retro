using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    public float timePressedRequired;
    public string myString;

    private float buttonPressedTimeTracker;
    private Text myText;
    private char[] charArray;
    private int charIndex;
    private float printInterval;
    private float printIntervalTracker;
    private string currentStringToPrint;
    private bool areAllCharsPrinted;
    [HideInInspector] public bool buttonPressed;

   
    void Start()
    {
        if (timePressedRequired == 0)
            timePressedRequired = 1.5f;

        myText = GetComponent<Text>();
        charArray = myString.ToCharArray();

        printInterval = timePressedRequired / charArray.Length;


      
    }

    
    void Update()
    {
        CheckIfSkipIsValid();
        PrintMyText();
    }

    public void IsButtonPressed(bool b)
    {
        
        buttonPressed = b;
    }

    private float WindUp()
    {
        if (buttonPressed)
        {
            buttonPressedTimeTracker += Time.deltaTime;
            return buttonPressedTimeTracker;
        }
        else
        {
            buttonPressedTimeTracker = 0.0f;
            return 0;
        }
    }

    private void CheckIfSkipIsValid()
    {
        float t = WindUp();

     

        if(t > timePressedRequired)
        {
            GameStateMachine.GetInstance().IntroIsDone = true;
            GameStateMachine.GetInstance().introTauntScreen_Ref.SetActive(false);
                
        }
    }
    private void PrintMyText()
    {
        if (buttonPressed)
        {
          

            if (printIntervalTracker > printInterval && !areAllCharsPrinted)
            {
                
                myText.text = currentStringToPrint += charArray[charIndex];
                printIntervalTracker = 0;
                charIndex++;
                if (charIndex == charArray.Length)
                    areAllCharsPrinted = true;
            }
            printIntervalTracker += Time.deltaTime;
        }
        else
        {
            printIntervalTracker = 0;
            myText.text = "";
            currentStringToPrint = "";
            charIndex = 0;
            areAllCharsPrinted = false;
        }
    }
    
    
    

}
