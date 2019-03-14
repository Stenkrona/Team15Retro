using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour
{
    public bool debugMode;
    public GameObject canvas_Ref;
    public GameObject bordersParent_Ref;

    private IStateBase gameState;
    private BubbleManager bubbleManager_Ref;
   
    private Text debugTxtReference;
    private string currentState;

    void Start()
    {

         if(canvas_Ref == null)
         {
            if (transform.GetChild(0).gameObject != null)
            {
                canvas_Ref = transform.GetChild(0).gameObject;
            }
            else
            {
                Debug.Log("GameStateMachine is missing a reference to the Canvas");
            }
        }
         

        if(gameState == null)
        {
            gameState = new BeginState(this);
        }
       
        if(bordersParent_Ref == null)
        {
            if(GameObject.Find("BordersParent") != null)
            {
                bordersParent_Ref = GameObject.Find("BordersParent");
            }
            else
            {
                Debug.Log("GameStateMachine is missing a reference to BordersParent");
            }
        }

       gameState.ShowIt();
      
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
        //the argument child will be the canvas child index and that child will be set active
        // all the other childs will be set inactive.

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
