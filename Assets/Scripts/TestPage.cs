using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TestPage : Page
{
    GridManager gm;

    public TestPage ()
    {
        shouldSortByZ = true;
        gm = new GridManager();
        AddChild(gm);
    }

    override
    public void Update(float dt)
    {
        gm.Update(dt);
    }
}


public class GridManager : FContainer
{
    FSprite[,] hexes;
    List<Bug> bugs = new List<Bug>();
    int width = 42;
    int height = 38;
    public GridManager()
    {
        FillGrid();
        for (int i = 0; i < 80; i++)
        {
            makeBug();
        }
    }

    public Vector2 g2w (Bug b)
    {
        int x = 0;
        int y = 0;
        //Grid to world!
        //Take a bug and get it's grid, then figure out where to put on the world
        x = 9; //Start Position
        x += b.gridX * 18 + ((b.gridY % 2 == 0) ? 9 : 0); //Offset per tile

        //If row is even, offset a little more


        y = 9; //Start Position
        y += 15 * b.gridY; //Offset per row



        return new Vector2(x, y);
    }

    void makeBug()
    {
        Boolean bugPlaced = false;
        int x = 0;
        int y = 0;
        while (!bugPlaced)
        {
            if (!isBugAt(x, y))
            {
                makeBug(x, y);
                bugPlaced = true;
            }
            else
            {
                if (x >= width - 1)
                {
                    y++;
                    x = 0;
                } else
                {
                    x++;
                }
            }
        }
    }
    void makeBug(int x, int y)
    {
        if (!isBugAt(x, y))
        {
            Bug b = new Bug(Facing.R, x, y, this);
            b.SetPosition(g2w(b));
            bugs.Add(b);
            AddChild(b);
        }
    }

    void FillGrid()
    {
        hexes = new FSprite[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                FSprite h = new FSprite("Hex");
                h.x = 9; //Start Position
                h.x += i * 18 + ((j%2==0)? 9 : 0) ; //Offset per tile

                //If row is even, offset a little more


                h.y = 9; //Start Position
                h.y += 15 * j; //Offset per row
                
                
                hexes[i, j] = h;
                AddChild(h);
            }
        }
    }

    public Boolean canMove(int x, int y)
    {
        if ((x < 0) || (x > width-1)) return false;
        if ((y < 0) || (y > height-1)) return false;
        return true;
    }

    public Boolean isBugAt(int x, int y)
    {
        foreach (Bug b in bugs) {
            if ((b.gridX == x) && (b.gridY == y)) return true;
        }
        return false;
    }


    public void Update(float dt)
    {
        foreach (Bug b in bugs)
        {
            b.Update(dt);
        }
    }
}