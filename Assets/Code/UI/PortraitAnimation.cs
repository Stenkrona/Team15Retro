using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitAnimation : MonoBehaviour
{
    private GameStateMachine gameStateMachine_Ref;

    public Sprite[] mySprites;
    public bool playerOne;
    public bool introText;
    public bool victoryScreen;

    [HideInInspector] public bool isTalking;

    private bool playAnimation;
    


    private Image myImage;
    private float frameInterval;
    private int amountOfLoops;
    private int loopsTracker;
    private float frameTracker;
    private int frameIndex;
    private PrintText myPrintText;


    private bool addToFrameIndex;

    
    void Start()
    {
        gameStateMachine_Ref = GameStateMachine.GetInstance();

        myImage = GetComponent<Image>();
        addToFrameIndex = true;

        if (amountOfLoops == 0) amountOfLoops = 1;
        if (frameInterval == 0) frameInterval = 0.1f;

        if (gameStateMachine_Ref.devMode)
        {
            Destroy(this);
        }
        if (playerOne && !victoryScreen)
        {
            mySprites = gameStateMachine_Ref.PlayerCharacterArray[0].MySprites;

            if (!introText)
            {
                InGameUIManager.GetInstance().playerOneUIAnim_Ref = this;
            }
            else
            {
                InGameUIManager.GetInstance().playerOneIntroUIAnim_Ref = this;
            }

        }
        else
        {
            
            mySprites = gameStateMachine_Ref.PlayerCharacterArray[1].MySprites;
            if (!introText)
            {
                InGameUIManager.GetInstance().playerTwoUIAnim_Ref = this;
            }
            else
            {
                InGameUIManager.GetInstance().playerTwoIntroUIAnim_Ref = this;
            }
            
        }

        if(victoryScreen)
        {
            if (gameStateMachine_Ref.PlayerOneWon)
            {
                 mySprites = gameStateMachine_Ref.PlayerCharacterArray[0].MySprites;
            }
            else
            {
                mySprites = gameStateMachine_Ref.PlayerCharacterArray[1].MySprites;
            }
            myPrintText = transform.parent.GetComponentInChildren<PrintText>();
        }

    }

   
    void Update()
    {
        if (playAnimation && !introText || introText && isTalking || victoryScreen && myPrintText.canIPrint)
        {
            Play();
        }   
    }
    private void Play()
    {
        if(UpdateFrameIntervalTracker() >= frameInterval)
        {
            frameTracker = 0;

            myImage.sprite = mySprites[frameIndex];

            if (addToFrameIndex)
            {
                frameIndex++;
                if(frameIndex == 4)
                {
                    frameIndex = 3;
                    addToFrameIndex = false;
                }
                
            }
            else
            {
                frameIndex--;
                    if(frameIndex == -1)
                {
                    frameIndex = 0;
                    addToFrameIndex = true;

                    loopsTracker++;
                }
            }

        }

        if(loopsTracker >= amountOfLoops)
        {
            playAnimation = false;
        }
    }
    private float UpdateFrameIntervalTracker()
    {
        return frameTracker += Time.deltaTime;
    }
    public void TurnOnAnimation()
    {
        playAnimation = true;
        loopsTracker = 0;
    }

    
}
