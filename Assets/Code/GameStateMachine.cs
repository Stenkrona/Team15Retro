using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour
{
    public bool debugMode;
    public GameObject canvas_Ref;

    private IStateBase gameState;
    private BubbleManager bubbleManager_Ref;
   
    private Text debugTxtReference;
    private string currentState;
    
    private int currentScore;
      
    void Start()
    {

        if(canvas_Ref == null) canvas_Ref = transform.GetChild(0).gameObject;

        if (gameState == null)
        {
            gameState = new BeginState(this);
        }
       

       
        gameState.ShowIt();
      

     
        currentScore = 0;
    }

    
    void Update()
    {
        gameState.StateUpdate();
    }

    public void ChangeState(IStateBase newState)
    {
        gameState = newState;
        gameState.ShowIt();
    }

    public void TurnOnCanvasSection(int child)
    {
        int canvasChildLength = canvas_Ref.transform.childCount;
       
        for (int i = 0; i < canvasChildLength; i++)
        {
            canvas_Ref.transform.GetChild(i).gameObject.SetActive(false);
        }


        canvas_Ref.transform.GetChild(child).gameObject.SetActive(true);
       
    }
   
  
   
    // properties
    public GameObject Canvas_Ref {get {return canvas_Ref;}}
    public BubbleManager BubbleManager_Ref { set { bubbleManager_Ref = value; } get { return bubbleManager_Ref; } }
  
   

}
