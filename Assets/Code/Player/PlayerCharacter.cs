using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter 
{
    
    private Sprite myPicture;
    private Sprite[] mySprites;
    
    private string myGamerTag;
    private string myIntroPhrase;
    private string myVictoryPhrase;

    public PlayerCharacter(Object[] portraits, Object[] introPhrase, Object[] victoryPhrase, Object[] gamerTag)
    {
        
        if (GameStateMachine.GetInstance().devMode)
        {
            myPicture = SetMyPicture(portraits);
        }
        else
        {
            if(portraits[0] != null)
            {
                mySprites = new Sprite[4];
                myPicture = (Sprite)portraits[0];
                mySprites[0] = (Sprite)portraits[0];
                portraits[0] = null;

                mySprites[1] = (Sprite)portraits[1];
                mySprites[2] = (Sprite)portraits[2];
                mySprites[3] = (Sprite)portraits[3];
            }
            else
            {
                mySprites = new Sprite[4];

                myPicture = (Sprite)portraits[4];
                mySprites[0] = (Sprite)portraits[4];                
                portraits[4] = null;

                mySprites[1] = (Sprite)portraits[5];
                mySprites[2] = (Sprite)portraits[6];
                mySprites[3] = (Sprite)portraits[7];
            }
        }
        myIntroPhrase = SetMyPhrase(introPhrase);
        myVictoryPhrase = SetMyPhrase(victoryPhrase);
        myGamerTag = SetMyPhrase(gamerTag);
    }
    private Sprite SetMyPicture(Object[] portraits)
    {
        int index = Random.Range(0, portraits.Length);

        Sprite spriteToReturn;

        if(portraits[index] != null)
        {
            spriteToReturn = (Sprite)portraits[index];
            portraits[index] = null;
        }
        else
        {
            spriteToReturn = SetMyPicture(portraits);
        }

        return spriteToReturn;

    }
    private string SetMyPhrase(Object[] phrases)
    {
        int index = Random.Range(0, phrases.Length);


        string stringToReturn;

        if(phrases[index] != null)
        {
            TextAsset temp = (TextAsset)phrases[index];

            stringToReturn = temp.ToString();
            phrases[index] = null;
        }
        else
        {
            stringToReturn = SetMyPhrase(phrases);
        }

        return stringToReturn;
    }
   

    //properties
    public Sprite MyPicture { get { return myPicture; } }
    public string MyIntroPhrase { get { return myIntroPhrase; } }
    public string MyVictoryPhrase { get { return myVictoryPhrase; } }
    public string MyGamerTag { get { return myGamerTag; } }
    public Sprite[] MySprites { get { return mySprites; } }




}
