using UnityEngine;

public enum DIR : int {
    UP = 1,
    DOWN = -1,
    RIGHT = 2,
    LEFT = -2,
}

public class Block
{
    public GameObject block;
    public int x;
    public int y;
    Material material;

    public Block(GameObject inblock)
    {
        block = inblock;
        material = inblock.GetComponent<Renderer>().material;
    }

    public void SetPos(int x, int y)
    {
        this.x = x;
        this.y = y;

        Vector3 position = new Vector3(Stacker.grid.x + Stacker.grid.cellx * x, Stacker.grid.y + Stacker.grid.celly * y, 0);
        this.block.transform.position = position;

        material.SetFloat("offsetx", x);
        material.SetFloat("offsety", y);
    }

    public void Move(DIR dir)
    {
        switch (dir)
        {
            case DIR.UP:
                SetPos(x, y + 1);
                break; 
            case DIR.DOWN:
                SetPos(x, y - 1);
                break;
            case DIR.RIGHT:
                SetPos(x + 1, y);
                break;
            default: //LEFT
                SetPos(x - 1, y);
                break;
        }
    }
}
