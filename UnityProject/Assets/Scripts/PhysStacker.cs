using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Unity.Collections.AllocatorManager;

public class PhysStacker : MonoBehaviour
{
    public static PhysStacker Instance;
    public static GameObject blockPrefab;
    public float placeInterval;
    public float WrapSpeed;
    public GameObject EdgeBlock;
    float placeTimer;
    float wrapTimer;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI maxlevelText;

    [SerializeField, Tooltip("Game Over")]
    private UnityEvent GameOver;
    [SerializeField, Tooltip("On Place")]
    private UnityEvent OnPlace;
    [SerializeField, Tooltip("On Take Damage")]
    private UnityEvent OnTakeDamage;

    public GameObject ChildHolder;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        placeTimer = placeInterval;
        wrapTimer = WrapSpeed;
        endPos = EdgeBlock.transform.position.x;
        healthArr = healthContainer.GetComponentsInChildren<SpriteRenderer>();
    }
    public void StartGame()
    {
        blockLevel = 0f;
        health = 5;
        CountDown.SetActive(false);
        maxlevelText.text = (blockWinLevel - 1).ToString();

        if (healthArr != null) { 
            foreach (SpriteRenderer health in healthArr)
            {
                health.color = Color.white;
            }
        }

        SpawnBlocks();
        running = true;
    }

    public void KillChildren()
    {
        foreach (Transform child in ChildHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    bool spaceAvailable = false;
    bool running = false;

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            if (Input.GetKeyDown(KeyCode.Space) && spaceAvailable)
            {
                OnPlace.Invoke();
                PlaceBlocks();
                if (blockLevel < blockWinLevel - 1)
                {
                    SpawnBlocks();
                }
                else
                {
                    Instance.DoTimer();
                }
                placeTimer = placeInterval;
                spaceAvailable = false;
            }

            placeTimer -= Time.deltaTime;
            if (placeTimer < 0 && !spaceAvailable)
            {
                spaceAvailable = true;
            }

            MoveBlocks();
        }
    }

    private float blockLevel = 0f;
    List<GameObject> curBlocks;
    List<Material> materials;
    float endPos;
    DIR dir = DIR.RIGHT;
    void SpawnBlocks()
    {
        curBlocks = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            curBlocks.Add(Instantiate(blockPrefab));
        }

        int index = 0;
        materials = new List<Material>();
        foreach (GameObject block in curBlocks)
        {
            block.transform.parent = ChildHolder.transform;
            block.transform.position = EdgeBlock.transform.position + new Vector3(Stacker.grid.cellx, 0f, 0f) * index;
            index++;
            materials.Add(block.GetComponent<Renderer>().material);
        }
        foreach (Material mat in materials)
        {
            mat.SetFloat("offsety", blockLevel);
            mat.SetFloat("ytiles", blockWinLevel);
        }
        blockLevel = Math.Clamp(blockLevel + 1f, 0f, Stacker.grid.rows -1f);
        levelText.text = (blockLevel).ToString();
    }

    void PlaceBlocks()
    {
        if (curBlocks != null)
        {
            foreach (GameObject block in curBlocks)
            {
                block.GetComponent<Rigidbody2D>().simulated = true;
            }
            curBlocks = new List<GameObject>();
            materials = new List<Material>();
        }
    }

    //1 = right, 0 = left
    private void SetBlockPoses(float movdir)
    {
        int index = 0;
        foreach (GameObject block in curBlocks)
        {
            //CHANGE TO LINEAR INTERPOLATION
            block.transform.position =
                new Vector3(
                    Mathf.Lerp(endPos, -endPos - 2 * Stacker.grid.cellx, (movdir - wrapTimer / WrapSpeed) * Math.Sign((int) dir)),
                    EdgeBlock.transform.position.y,
                    EdgeBlock.transform.position.z) + 
                new Vector3(Stacker.grid.cellx, 0f, 0f) * index;
            index++;
        }
        index = 0;
        foreach (Material mat in materials)
        {
            mat.SetFloat("offsetx", (movdir - wrapTimer / WrapSpeed) * Math.Sign((int)dir) * Stacker.grid.columns - 2f + index);
            //mat.
            //material.SetInt("offsetx", x);
            //material.SetInt("offsety", y);
            //12 by 8, also, cellsize
            index++;
        }
    }

    private void MoveBlocks()
    {
        wrapTimer -= Time.deltaTime;
        if (dir == DIR.RIGHT)
        {
            SetBlockPoses(1f);
        }
        else
        {
            SetBlockPoses(0f);
        }

        if (wrapTimer < 0f)
        {
            wrapTimer = WrapSpeed;
            dir = (DIR)(-(int)dir);
        }
    }


    public GameObject healthContainer;
    private static SpriteRenderer[] healthArr;

    private static int health = 5;
    public static void TakeDamage()
    {
        if (health == 0)
        {
            Instance.GameOver?.Invoke();
            Instance.callInInstance();
            return;
        }
        else
        {
            Instance.OnTakeDamage.Invoke();
        }
        healthArr[5 - health].color = new Color32(0x71, 0x71, 0x71, 0xFF);
        health--;
        Debug.Log(health);
    }

    //Needed after gameover
    private void callInInstance() {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
    }


    public void DoTimer()
    {
        if (timer ==  null)
        {
            timer = StartCoroutine(EndCounter());
        }
    }

    public GameObject CountDown;
    public TMPro.TMP_Text CountDown1;
    public TMPro.TMP_Text CountDown2;

    public UnityEvent OnSuccess;

    Coroutine timer = null;
    IEnumerator EndCounter()
    {
        CountDown.SetActive(true);
        float time = 3f;
        while (time > 0)
        {
            float nonFractPart = MathF.Truncate(time);
            CountDown1.text = ((int)nonFractPart).ToString();
            CountDown2.text = (time - nonFractPart).ToString("F2").Substring(1);
            time -= Time.deltaTime;
            yield return null;
        }
        OnSuccess?.Invoke();
    }


    private int blockWinLevel = 12;
    private int pity = 0;
    public UnityEvent onPity;

    public void Pity()
    {
        pity++;
        if (pity == 3)
        {
            onPity?.Invoke();
        }
    }
}
