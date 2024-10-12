using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject block;
    public GameObject pivotPos;


    // Start is called before the first frame update
    void Start()
    {
        Grid grid = new Grid(pivotPos.transform.position, 12f, 8f);
        float rows = 12f;
        float columns = 8f;

        float cellx = grid.width / (columns - 1);
        float celly = grid.height / (rows - 1);
        for (int i = 0; i < columns; i++) //Width
        {
            for (int j = 0; j < rows; j++) //Height
            {
                Vector3 position = new Vector3(grid.x + cellx * i, grid.y + celly * j, 0);
                Instantiate(block, position, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
