using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TestPage : Page
{
    GridManager gm;

    public TestPage ()
    {
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
    List<Plant> plants = new List<Plant>();
    List<object> toRemove = new List<object>();
    int width = 42;
    int height = 38;
    public GridManager()
    {
        FillGrid();
        for (int i = 0; i < 30; i++)
        {
            makeBug();
        }
        
    }

    public Vector2 g2w (int gridX, int gridY)
    {
        int x = 0;
        int y = 0;
        //Grid to world!
        //Take a bug and get it's grid, then figure out where to put on the world
        x = 9; //Start Position
        x += gridX * 18 + ((gridY % 2 == 0) ? 9 : 0); //Offset per tile

        //If row is even, offset a little more


        y = 9; //Start Position
        y += 15 * gridY; //Offset per row



        return new Vector2(x, y);
    }

    void makePlayer()
    {
        Boolean bugPlaced = false;
        while (!bugPlaced)
        {
            int x = UnityEngine.Random.Range(0, width);
            int y = UnityEngine.Random.Range(0, height);
            if (!isBugAt(x, y))
            {
                Bug b = new Bug(Facing.R, x, y, this);
                b.makePlayer();
                b.SetPosition(g2w(b.gridX, b.gridY));
                bugs.Add(b);
                AddChild(b);
                bugPlaced = true;
            }
        }
    }

    void makeBug()
    {
        Boolean bugPlaced = false;
        while (!bugPlaced)
        {
            int x = UnityEngine.Random.Range(0, width);
            int y = UnityEngine.Random.Range(0, height);
            if (!isBugAt(x, y))
            {
                makeBug(x, y);
                bugPlaced = true;
            }
        }
    }

    public void makeBug(int x, int y)
    {
        makeBug(Facing.R, x, y);
    }

    public void makeBug(Facing fac, int x, int y)
    {
        if (!isBugAt(x, y))
        {
            Bug b = new Bug(fac, x, y, this);
            b.SetPosition(g2w(b.gridX, b.gridY));
            bugs.Add(b);
            AddChild(b);
        }
    }

    void makePlant()
    {
        Boolean plantPlaced = false;
        while (!plantPlaced)
        {
            int x = UnityEngine.Random.Range(0, width);
            int y = UnityEngine.Random.Range(0, height);
            if ((!isBugAt(x, y)) && (!isPlantAt(x,y)))
            {
                makePlant(x, y);
                plantPlaced = true;
            }
        }
    }

    void makePlant(int x, int y)
    {
        if ((!isPlantAt(x,y)) && (!isBugAt(x,y)))
        {
            Plant p = new Plant(x, y);
            p.SetPosition(g2w(p.gridX, p.gridY));
            plants.Add(p);
            AddChild(p);
        }
    }
    
    public void remove(object o)
    {
        toRemove.Add(o);
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

    public Boolean isPlantAt(int x, int y)
    {
        foreach (Plant p in plants)
        {
            if ((p.gridX == x) && (p.gridY == y)) return true;
        }
        return false;
    }

    public Plant getPlantAt(int x, int y)
    {
        foreach (Plant p in plants)
        {
            if ((p.gridX == x) && (p.gridY == y)) return p;
        }
        return new Plant(0,0);
    }

    public void Update(float dt)
    {

        for (int i = bugs.Count-1; i>=0;i--)
        {
            bugs[i].Update(dt);
        }
        
        /*if (bugs.Count < 10)
        {
            makeBug();
        }*/

        while (plants.Count < 100)
        {
            makePlant();
        }

        foreach (object o in toRemove)
        {
            if (o is Bug)
            {
                bugs.Remove((Bug)o);
                RemoveChild((Bug)o);
            } else if (o is Plant)
            {
                plants.Remove((Plant)o);
                RemoveChild((Plant)o);
            }
            
        }
    }
}