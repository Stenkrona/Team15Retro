using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter 
{
    
    private Sprite myPicture;
    private string myGamerTag;
    private string myIntroPhrase;
    private string myVictoryPhrase;

    public PlayerCharacter(Object[] portraits, Object[] introPhrase, Object[] victoryPhrase, Object[] gamerTag)
    {
        myPicture = SetMyPicture(portraits);
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




}
