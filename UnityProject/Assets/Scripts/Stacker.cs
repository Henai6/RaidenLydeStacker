using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stacker : MonoBehaviour
{
    public GameObject cornerBlock;
    public static Grid grid;
    public static GameObject blockPrefab;
    public GameObject block;

    [SerializeField, Tooltip("On Place Success")]
    private UnityEvent OnPlaceSuccess;

    [SerializeField, Tooltip("On Place Fail")]
    private UnityEvent OnPlaceFail;

    [SerializeField, Tooltip("Per Tick")]
    private UnityEvent PerTick;

    [SerializeField, Tooltip("Game Over")]
    private UnityEvent GameOver;

    [SerializeField, Tooltip("Space Game Over")]
    private UnityEvent SpaceGameOver;

    public GameObject ChildHolder;

    // Start is called before the first frame update
    void Start()
    {
        blockPrefab = block;
        grid = new Grid(cornerBlock.transform.position, 12f, 8f);
        timer = updateSpeed;
        bigwinDefualtY = bigWin.transform.position.y;
    }

    private int stackSize = 3;
    private (int, int) curStack = (0, 0);
    private int? lastStackX = null;
    private bool running = false;

    List<Block> curBlocks;
    void SpawnBlocks()
    {
        curBlocks = new List<Block>();
        for (int i = 0; i < stackSize; i++)
        {
            curBlocks.Add(new Block(Instantiate(blockPrefab)));
        }

        int index = 0;
        foreach (Block block in curBlocks)
        {
            block.SetPos(curStack.Item1 + index, curStack.Item2);
            index++;
            block.block.transform.parent = ChildHolder.transform;
            block.block.GetComponent<Renderer>().material.SetFloat("ytiles", blockWinLevel);
        }
    }

    public GameObject bigWin;
    private float bigwinDefualtY;

    public void StartGame()
    {
        running = true;
        timer = updateSpeed;
        stackSize = 3;
        curStack = (0, 0);
        lastStackX = null;
        blockdir = DIR.RIGHT;
        spaceAvailable = false;
        updateSpeed = 0.4f;

        var temp = bigWin.transform.position;
        temp.y = bigwinDefualtY - (12 - blockWinLevel) * grid.celly;
        bigWin.transform.position = temp;

        foreach (Transform child in ChildHolder.transform)
        {
            Destroy(child.gameObject);
        }

        SpawnBlocks();
    }

    public void StopGame()
    {
        running = false;
    }

    public void KillChildren()
    {
        foreach (Transform child in ChildHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private float updateSpeed = 0.4f;
    private DIR blockdir = DIR.RIGHT;
    bool spaceAvailable = false;
    float timer;
    void Update()
    {
        if (running)
        {
            if (Input.GetKeyDown(KeyCode.Space) && spaceAvailable)
            {
                LevelUp();
                spaceAvailable = false;
            }

        
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = updateSpeed;
                MoveBlocks();
                spaceAvailable = true;
            }
        }
    }

    void LevelUp()
    {
        if (lastStackX != null)
        {
            int? lastStackChanged = null;

            foreach (Block block in curBlocks)
            {
                if (block.x < lastStackX + stackSize && block.x >= lastStackX)
                {
                    //keep block
                    if (lastStackChanged == null)
                    {
                        lastStackChanged = block.x;
                    }
                }
                else
                {
                    Destroy(block.block);
                    stackSize -= 1;
                }
            }

            if (lastStackX != lastStackChanged)
            {
                if (stackSize < 1)
                {
                    //Death sound
                    if (!spaceMode)
                    {
                        GameOver?.Invoke();
                    }
                    else
                    {
                        SpaceGameOver?.Invoke();
                    }

                    return;
                } else
                {
                    OnPlaceFail.Invoke();
                }
            }
            
            lastStackX = lastStackChanged;
        }
        else
        {
            lastStackX = curStack.Item1;
        }

        OnPlaceSuccess.Invoke();

        curStack = (0, curStack.Item2 + 1);
        blockdir = DIR.RIGHT;
        if (curStack.Item2 < 7)
        {
            updateSpeed *= 0.8f;
        }
        else
        {
            updateSpeed *= 0.9f;
        }
        
        if (curStack.Item2 >= blockWinLevel)
        {
            if (!spaceMode)
            {
                OnWin?.Invoke();
            }
            else
            {
                OnSpaceWin?.Invoke();
            }

        } else
        {
            SpawnBlocks();
        }
    }

    public UnityEvent OnWin;
    public UnityEvent OnSpaceWin;
    private int blockWinLevel = 12;
    private int level1Pity = 0;

    public void Pity()
    {
        if (level1Pity >= 3) {
            blockWinLevel = Math.Clamp(blockWinLevel - 1, 6, 12);
        } else
        {
            level1Pity++;
        }
    }


    void MoveBlocks()
    {
        PerTick.Invoke();
        foreach (Block block in curBlocks)
        {
            if (block.block == null)
            {
                Debug.LogError("ERR");
                GameOver?.Invoke();
            }
            block.Move(blockdir);
        }

        curStack.Item1 += 1 * Math.Sign((int)blockdir);
        if (curStack.Item1 == 8 - stackSize || curStack.Item1 == 0)
        {
            blockdir = (DIR)((int)blockdir * -1);
        }
    }


    private bool spaceMode = false;
    public void EnableSpaceMode()
    {
        blockWinLevel = 6;
        spaceMode = true;
    }

    public void DisableSpaceMode()
    {
        blockWinLevel = 12;
        spaceMode = false;
    }
}
