using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed = 1f;
    private Vector3 scrollVec;

    public GameObject WrapPos;
    private Vector3 startPos;
    private float xWrap;

    // Start is called before the first frame update
    void Start()
    {
        startPos = WrapPos.transform.localPosition;
        xWrap = WrapPos.transform.localPosition.x;
        scrollVec = new Vector3(scrollSpeed * Time.deltaTime, 0f, 0f);
    }

    
    // Update is called once per frame
    void Update()
    {
        transform.localPosition += scrollVec;
        if (transform.localPosition.x > -xWrap)
        {
            this.transform.localPosition = startPos;
        }
    }
}
