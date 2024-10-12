using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStaticBadCode : MonoBehaviour
{
    public GameObject PhysBlockPrefab;

    void Start()
    {
        PhysStacker.blockPrefab = PhysBlockPrefab;
    }
}
