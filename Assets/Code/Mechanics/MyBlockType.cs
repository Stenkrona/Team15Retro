using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBlockType : MonoBehaviour
{
    public BlockType myBlockType;

    private Gameboard gameboard_Ref;

    void Start()
    {
        gameboard_Ref = Gameboard.GetInstance();

        if (PlayerOne())
        {
            gameboard_Ref.PairLanderWithObject(true, gameObject);
        }
        else
        {
            gameboard_Ref.PairLanderWithObject(false, gameObject);
        }
    }
    private bool PlayerOne()
    {
        if(transform.position.x < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
