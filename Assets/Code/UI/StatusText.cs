using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{

    public bool amIPlayerOne;
    
    void Start()
    {
        if (InGameUIManager.GetInstance() != null)
        {
            if (amIPlayerOne)
            {
                if (InGameUIManager.GetInstance().PlayerOneInfo_Ref != null)
                {
                    InGameUIManager.GetInstance().PlayerOneInfo_Ref.playerStatusDisplay = GetComponent<Text>();
                }
                else
                {
                    Debug.Log("InGameUIManager is missing a ref to PlayerOneInfo_Ref");
                }
            }
            else
            {
                InGameUIManager.GetInstance().PlayerTwoInfo_Ref.playerStatusDisplay = GetComponent<Text>();
            }
        }
    }

   
}
