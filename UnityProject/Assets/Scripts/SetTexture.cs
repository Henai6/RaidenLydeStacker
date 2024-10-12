using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTexture : MonoBehaviour
{
    public int tilex;
    public int tiley;

    void Awake()
    {
        Material material = this.GetComponent<Renderer>().material;
        
        material.SetFloat("offsetx", tilex);
        material.SetFloat("offsety", tiley);
    }
}
