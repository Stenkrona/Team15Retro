using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [HideInInspector] public bool isMovingLeft;
    [HideInInspector] public float movementSpeed;
    [HideInInspector] public bool isOnPlayerOneSide;
    [HideInInspector] public float myDeSpawnValue;
    [HideInInspector] public bool isPaused;

    [HideInInspector] public BubbleManager bubbleManager_Ref;
    [HideInInspector] public float elevationSpeed;
    [HideInInspector] public GameStateMachine gameStateMachine_Ref;
    [HideInInspector] public GameObject bubblePopPrefab_Ref;

    private bool hasCaughtTetrisBlock;
    private bool hasChangedSprite;
    private GameObject myCaugtBlock;
    private float bobSpeed = 1;
    private float bobTimeTracker;
    private float difficultyMultiplier;
    private SpriteRenderer mySpriteRenderer;

    private float spriteSwitchTimeInterval;
    private float spriteSwitchTracker;

    private Sprite floatBubbleSprite_Ref;
    private Sprite movingBubbleSprite01_Ref;
    private Sprite movingBubbleSprite02_Ref;

    void Start()
    {
        if (movementSpeed == 0) movementSpeed = 5;
        if (elevationSpeed == 0) elevationSpeed = 3;
        if (spriteSwitchTimeInterval == 0) spriteSwitchTimeInterval = 0.3f;
        if (gameStateMachine_Ref == null) gameStateMachine_Ref = GameStateMachine.GetInstance();

        mySpriteRenderer = GetComponent<SpriteRenderer>();

        mySpriteRenderer.sprite = movingBubbleSprite01_Ref;

        FlipSpriteRenderer();
    }

    void Update()
    {
        Move();
        CheckDeSpawn();
    }

    void Move()
    {
        if (!isPaused)
        {
            if (!hasCaughtTetrisBlock)
            {

                ChangeSpriteAnimation();
                    transform.Translate(GetDirection() * Time.deltaTime * movementSpeed * 
                        difficultyMultiplier);
              
            }
            else
            {
                if (!hasChangedSprite)
                {
                    GetComponent<SpriteRenderer>().sprite = floatBubbleSprite_Ref;
                    hasChangedSprite = true;
                }


                transform.Translate(Vector2.up * Time.deltaTime * elevationSpeed);

                if(transform.position.y >= bubbleManager_Ref.p1_LeftTop.y - 2.0f)
                {
                    RemoveMeFromBubbleManagerList();
                    SetBlockParent();
                    SetBlockToDynamic(myCaugtBlock);
                    SpawnBubblePop();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void CheckDeSpawn()
    {
        if (isMovingLeft && transform.position.x - .5 < myDeSpawnValue)
        {
            RemoveMeFromBubbleManagerList();
            Destroy(gameObject);
        }
        else if(!isMovingLeft && transform.position.x + .5 > myDeSpawnValue)
        {
            RemoveMeFromBubbleManagerList();
            Destroy(gameObject);
        }
    }
    private void RemoveMeFromBubbleManagerList()
    {
        if (isOnPlayerOneSide)
        {
            bubbleManager_Ref.BubblesOnPlayerOnesSide.Remove(gameObject);
        }
        else
        {
            bubbleManager_Ref.BubblesOnPlayerTwosSide.Remove(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.GetComponent<PlayerController>() != null)
        {
            SetBlockToKinematic(coll.gameObject);

            gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            myCaugtBlock = coll.gameObject;

            hasCaughtTetrisBlock = true;

            coll.transform.position = transform.position;

            coll.transform.SetParent(gameObject.transform);
        }
        
    }
    private void SetBlockParent()
    {
        if (isOnPlayerOneSide)
        {
            myCaugtBlock.transform.SetParent(gameStateMachine_Ref.playerOneParent_Ref.transform);
        }
        else
        {
            myCaugtBlock.transform.SetParent(gameStateMachine_Ref.playerTwoParent_Ref.transform);
        }
    }
    private void SetBlockToKinematic(GameObject block)
    {
        block.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
    private void SetBlockToDynamic(GameObject block)
    {
        block.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
    private void BubbleBobbing()
    {

       
        float Yvalue = Mathf.Cos(PercentageOfBob()) + 1;

        transform.localPosition = new Vector3(transform.position.x, Yvalue + transform.position.y, 0);
    }
    private float PercentageOfBob()
    {
        if (bobTimeTracker >= bobSpeed)
            bobTimeTracker = 0;




        bobTimeTracker += Time.deltaTime;

        return bobTimeTracker / bobSpeed;
        
        


    }
    public void DestroyMyself()
    {
        RemoveMeFromBubbleManagerList();
        Destroy(gameObject);
    }
    private void SpawnBubblePop()
    {
        Instantiate(bubblePopPrefab_Ref, transform.position, Quaternion.identity);
    }
    private Vector2 GetDirection()
    {
        if (isMovingLeft)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }
    private bool SpriteSwitchTracker()
    {
       
        if(spriteSwitchTracker >= spriteSwitchTimeInterval)
        {
            spriteSwitchTracker = 0;
            return true;
        }
        else
        {
            spriteSwitchTracker += Time.deltaTime;
            return false;
        }
    }
    private void ChangeSpriteAnimation()
    {
        if (SpriteSwitchTracker())
        {
            if(mySpriteRenderer.sprite == movingBubbleSprite01_Ref)
            {
                mySpriteRenderer.sprite = movingBubbleSprite02_Ref;
            }
            else
            {
                mySpriteRenderer.sprite = movingBubbleSprite01_Ref;
            }

            FlipSpriteRenderer();
        }
    }
    private void FlipSpriteRenderer()
    {
        if (!isMovingLeft)
        {
            mySpriteRenderer.flipX = true;
        }
    }

    //properties
    public float DifficultyMultiplier { set { difficultyMultiplier = value; } }
    public Sprite FloatBubbleSprite_Ref { set { floatBubbleSprite_Ref = value; } }
    public Sprite MovingBubbleSprite01_Ref { set { movingBubbleSprite01_Ref = value; } }
    public Sprite MovingBubbleSprite02_Ref { set { movingBubbleSprite02_Ref = value; } }
}
