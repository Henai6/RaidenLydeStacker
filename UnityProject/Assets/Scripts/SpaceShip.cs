using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class SpaceShip : MonoBehaviour
{
    public static SpaceShip instance;

    public GameObject ChildHolder;
    public GameObject ChildHolderHolder;
    public GameObject MiddleBlock;

    [DllImport("__Internal")]
    private static extern void Shake();

    public void OnStartShip()
    {
        ResetStage();
        instance = this;
        SpaceShipActive = false;
        Stage = Instantiate(SpaceStagePrefab);

        foreach (Transform child in  ChildHolder.transform)
        {
            Rigidbody2D rb = child.gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            child.gameObject.AddComponent<BoxCollider2D>();
            //ADD colision Script
            //MIGHT NOT NEED IF COLLISION OBJECTS HAVE
        }
        Shake(); //UNCOMMENT WHEN BUILDING
        ChildHolder.GetComponent<Animator>().speed = 0.6f;
        ChildHolder.GetComponent<Animator>().Play("Shake");
    }

    bool SpaceShipActive = false;
    float startX;
    void Update()
    {
        if (SpaceShipActive)
        {
            float inp = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(inp) > 0.1)
            {
                Vector3 movVec = Vector3.right * inp * Time.deltaTime;
                if (Mathf.Abs((ChildHolderHolder.transform.position + movVec).x - startX) < 2.2f)
                {
                    ChildHolderHolder.transform.position += movVec;
                }
                
            }
        }
    }

    public void MoveAnim() { 
        StartCoroutine(MoveToCenter());
    }

    public float AnimMoveSpeed;
    IEnumerator MoveToCenter()
    {
        Vector3 StartPos = ChildHolderHolder.transform.position;
        Vector3 EndBlockPos = MiddleBlock.transform.position;
        Vector3 StartBlockPos = ChildHolder.transform.GetChild(1).transform.position;
        Vector3 EndPos = StartPos + (EndBlockPos - StartBlockPos);

        float timerMax = AnimMoveSpeed;
        float curTimer = AnimMoveSpeed;
        while (curTimer > 0)
        {
            curTimer -= Time.deltaTime;
            ChildHolderHolder.transform.position = Vector3.Lerp(StartPos, EndPos, 1 - curTimer / timerMax);

            yield return null;
        }
        ChildHolderHolder.transform.position = EndPos;
        startX = EndPos.x;
        movingStage = StartCoroutine(MoveStage());
        SpaceShipActive = true;
    }


    [SerializeField, Tooltip("Game Over")]
    private UnityEvent GameOver;

    [SerializeField, Tooltip("On Damage")]
    private UnityEvent OnDmg;
    public void CheckDeath()
    {
        if (deathChecker != null)
        {
            StopCoroutine(deathChecker);
        }
        deathChecker = StartCoroutine(CheckDeathSoon());

        OnDmg?.Invoke();
    }

    Coroutine deathChecker = null;
    IEnumerator CheckDeathSoon()
    {
        yield return null;
        if (ChildHolder.transform.childCount == 1) //Thruster is a child
        {
            GameOver?.Invoke();
        }

    }

    public static void TookDmg()
    {
        instance.CheckDeath();
    }

    Coroutine movingStage = null;
    GameObject Stage = null;
    public float stageSpeed;
    public GameObject Thruster;
    IEnumerator MoveStage()
    {
        //Start Thruster effects
        Thruster.transform.position = ChildHolder.transform.GetChild(1).transform.position;
        Thruster.SetActive(true);
        Thruster.transform.parent = ChildHolder.transform;


        Vector3 movVec = Vector3.up * stageSpeed;
        while (true)
        {
            Stage.transform.position -= movVec * Time.deltaTime;
            yield return null;
        }
    }

    public void EndStage()
    {
        if (movingStage != null)
        {
            StopCoroutine(movingStage);
        }
        if (Stage != null)
        {
            Destroy(Stage);
        }
    }


    public GameObject SpaceStagePrefab;
    public void ResetStage()
    {
        EndStage();
        Thruster.SetActive(false);
        SpaceShipActive = false;
        ChildHolderHolder.transform.position = Vector3.zero;
    }


    public UnityEvent GameWin;
    public void StartWin()
    {
        GameWin?.Invoke();
        GetWinPos();
        ChildHolder.GetComponent<Animator>().speed = 1.0f;
        ChildHolder.GetComponent<Animator>().Play("Leave");
    }

    private void GetWinPos()
    {
        List<float> blockXes = new List<float>();
        float highestY = -10;

        foreach (Transform child in ChildHolder.transform)
        {

            if (blockXes.All(x =>
                   (!(Mathf.Abs(child.position.x - x) < 0.5f))
                ))
            {
                blockXes.Add(child.position.x);
            }

            if (child.position.y > highestY)
            {
                highestY = child.position.y;
            }
        }
        float blockX = blockXes.Average();

        Vector3 CurPos = ChildHolderHolder.transform.position;
        Vector3 EndBlockPos = WinPos.transform.position;
        Vector3 StartBlockPos = new Vector3(blockX, highestY, 0);
        Vector3 EndPos = CurPos + (EndBlockPos - StartBlockPos);
        EndEndPos = EndPos;
    }
    private Vector3 EndEndPos;

    
    public GameObject WinPos;
    public void Win()
    {
        ChildHolderHolder.transform.position = EndEndPos;
        ChildHolderHolder.transform.RotateAround(WinPos.transform.position, Vector3.forward, 20f);
        ChildHolderHolder.transform.parent = Mira;
        Thruster.SetActive(false);
        StartCoroutine(MoveMira());
    }

    public Transform Mira;
    public Transform MiraEndPos;
    public float MiraSpeed;
    IEnumerator MoveMira()
    {
        Vector3 startPos = Mira.transform.position;
        float MaxMiraSpeed = MiraSpeed;
        while (MiraSpeed > 0)
        {
            MiraSpeed -= Time.deltaTime;

            Mira.position = Vector3.Lerp(startPos, MiraEndPos.position, 1 - (MiraSpeed / MaxMiraSpeed));
            yield return null;
        }
        Mira.position = MiraEndPos.position;

        GG.SetActive(true);
    }

    public GameObject GG;
    
}
