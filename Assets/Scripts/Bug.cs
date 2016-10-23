using UnityEngine;

public class Bug : FSprite
{
    AI ai;
    GridManager gm;
    public Facing facing;
    public int gridX;
    public int gridY;
    public int energy = 10;
    

    public Bug (GridManager gm) : base("bug")
    {
        ai = new AI();
        ai.parent = this;
        this.gm = gm;
        this.facing = Facing.R;
        gridX = 0;
        gridY = 0;
    }

    public Bug (Facing facing, int x, int y, GridManager gm) : base("bug")
    {
        ai = new AI();
        ai.parent = this;
        this.gm = gm;
        this.facing = facing;
        gridX = x;
        gridY = y;
    }

    public void makePlayer()
    {
        ai = new Player();
        ai.parent = this;
    }

    public void moveForward()
    {
        int gridXTo = gridX;
        int gridYTo = gridY;
        switch (facing)
        {
            case Facing.R:
                gridXTo++;
                break;
            case Facing.L:
                gridXTo--;
                break;
            case Facing.UR:
                gridXTo += (y % 2 == 0) ? 0 : 1;
                gridYTo++;
                break;
            case Facing.DR:
                gridXTo += (y % 2 == 0) ? 0 : 1;
                gridYTo--;
                break;
            case Facing.UL:
                gridXTo -= (y % 2 == 0) ? 1 : 0;
                gridYTo++;
                break;
            case Facing.DL:
                gridXTo -= (y % 2 == 0) ? 1 : 0;
                gridYTo--;
                break;
            default:
                break;
        }

        //is the tile in the grid?
        if (gm.canMove(gridXTo, gridYTo))
        {
            //Is the tile we wanna go to empty?
            if (!gm.isBugAt(gridXTo, gridYTo))
            {
                gridX = gridXTo;
                gridY = gridYTo;
                updatePosition();
                energy--;
            }
        }
    }

    public void updatePosition()
    {
        this.SetPosition(gm.g2w(gridX, gridY));
    }

    public void rotateLeft()
    {
        
        if (facing == Facing.DR)
        {
            facing = Facing.R;
        } else
        {
            facing += 1;
        }
        this.rotation -= 60f;
        energy--;
    }

    public void rotateRight()
    {
        if (facing == Facing.R)
        {
            facing = Facing.DR;
        }
        else
        {
            facing -= 1;
        }
        this.rotation += 60f;
        energy--;
    }

    public void Update(float dt)
    {
        if (energy <= 0)
        {
            gm.remove(this);
        }
        ai.Update(dt);
        if (gm.isPlantAt(gridX, gridY))
        {
            gm.remove(gm.getPlantAt(gridX, gridY));
            energy += 50;
        }
    }
}


public enum Facing
{
    R,
    UR,
    UL,
    L,
    DL,
    DR
}