using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public float x;
    public float y;
    public float width;
    public float height;

    public float rows;
    public float columns;

    public float cellx;
    public float celly;

    public Grid(Vector3 bottomPivot, float inrows, float incolumns)
    {
        x = bottomPivot.x;
        y = bottomPivot.y;
        width = Mathf.Abs(bottomPivot.x) * 2;
        height = Mathf.Abs(bottomPivot.y) * 2;
        rows = inrows;
        columns = incolumns;

        cellx = width / (columns - 1);
        celly = height / (rows - 1);
    }
}