using UnityEngine;

public class Bug : FSprite
{
    AI ai;
    GridManager gm;
    int tileInFront = 0;
    public Facing facing;
    public int gridX;
    public int gridY;
    public int energy = 50;
    

    public Bug (GridManager gm) : base("bug")
    {
        ai = new RandomAI();
        ai.parent = this;
        this.gm = gm;
        this.facing = Facing.R;
        gridX = 0;
        gridY = 0;
    }

    public Bug (Facing facing, int x, int y, GridManager gm) : base("bug")
    {
        ai = new RandomAI();
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
    
    public void updatePosition()
    {
        this.SetPosition(gm.g2w(gridX, gridY));
    }

    public int getTileXFromFacing(Facing direction)
    {
        int facingX = gridX;
        int facingY = gridY;
        switch (direction)
        {
            case Facing.R:
                facingX++;
                break;
            case Facing.L:
                facingX--;
                break;
            case Facing.UR:
                facingX += (y % 2 == 0) ? 0 : 1;
                break;
            case Facing.DR:
                facingX += (y % 2 == 0) ? 0 : 1;
                break;
            case Facing.UL:
                facingX -= (y % 2 == 0) ? 1 : 0;
                break;
            case Facing.DL:
                facingX -= (y % 2 == 0) ? 1 : 0;
                break;
            default:
                break;
        }

        return facingX;
    }

    public int getTileYFromFacing(Facing direction)
    {
        int facingX = gridX;
        int facingY = gridY;
        switch (direction)
        {
            case Facing.R:
                break;
            case Facing.L:
                break;
            case Facing.UR:
                facingY++;
                break;
            case Facing.DR:
                facingY--;
                break;
            case Facing.UL:
                facingY++;
                break;
            case Facing.DL:
                facingY--;
                break;
            default:
                break;
        }

        return facingY;
    }

    public void see()
    {
        tileInFront = 0;
        int tileFrontX = getTileXFromFacing(facing);
        int tileFrontY = getTileYFromFacing(facing);

        if (!gm.canMove(tileFrontX, tileFrontY))
        {
            tileInFront = 1; //I SEE the an unwalkable tile
        }   else if (gm.isBugAt(tileFrontX, tileFrontY))
        {
            tileInFront = 2; //I SEE A BUG
        }   else if (gm.isPlantAt(tileFrontX, tileFrontY)) 
        {
            tileInFront = 3; //I SEE TASTY FOOD
        }
    }

    public Facing leftFace(Facing d)
    {
        if (d == Facing.DR)
        {
            d = Facing.R;
        }
        else
        {
            d += 1;
        }

        return d;
    }

    public Facing rightFace(Facing d)
    {
        if (d == Facing.R)
        {
            d = Facing.DR;
        }
        else
        {
            d -= 1;
        }

        return d;
    }

    public void rotateLeft()
    {
        facing = leftFace(facing);
        this.rotation -= 60f;
        energy--;
    }

    public void rotateRight()
    {
        facing = rightFace(facing);
        this.rotation += 60f;
        energy--;
    }
    
    public void moveForward()
    {
        int gridXTo = getTileXFromFacing(facing);
        int gridYTo = getTileYFromFacing(facing);

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

    public void birth()
    {
        Facing behind = facing;

        behind = rightFace(behind);
        behind = rightFace(behind);
        behind = rightFace(behind);

        int behindX = getTileXFromFacing(behind);
        int behindY = getTileYFromFacing(behind);
        if ((energy > 50) && (gm.canMove(behindX, behindY)) && (!gm.isBugAt(behindX, behindY)))
        {
            energy -= 50;
            gm.makeBug(behindX, behindY);
        }
    }

    public void Update(float dt)
    {
        
        if (energy <= 0)
        {
            gm.remove(this);
        }
        see();
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