using UnityEngine;

public class Bug : FSprite
{
    public AI ai;
    GridManager gm;
    public int tileInFront = 0;
    public int tileToLeft = 0;
    public int tileToRight = 0;
    public Bug bugInFront;
    public Bug bugToLeft;
    public Bug bugToRight;
    public Facing facing;
    public int gridX;
    public int gridY;
    public int energy = 50;
    public int maximumStomach = 200;
    public int age = 0;
    public int iff = 0;
    

    public Bug (GridManager gm) : base("bug")
    {
        ai = new NeuralAI();
        ai.parent = this;
        this.gm = gm;

        this.facing = (Facing)Random.Range(0, 6);
        iff = Random.Range(0, 1000);
        gridX = 0;
        gridY = 0;
        updateRotation();
    }

    public Bug (Facing facing, int x, int y, GridManager gm) : base("bug")
    {
        ai = new NeuralAI();
        ai.parent = this;
        this.gm = gm;
        this.facing = facing;
        gridX = x;
        gridY = y;
        updateRotation();
    }

    public Bug(Facing facing, int x, int y, GridManager gm, NeuralAI ai) : base("bug")
    {
        this.ai = new NeuralAI(ai.nodeList, ai.connectList);
        this.ai.parent = this;
        this.gm = gm;
        this.facing = facing;
        gridX = x;
        gridY = y;
        updateRotation();
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

    void updateRotation()
    {
        switch (facing)
        {
            case Facing.UL:
                rotation = 240f;
                break;
            case Facing.UR:
                rotation = 300f;
                break;
            case Facing.L:
                rotation = 180f;
                break;
            case Facing.R:
                rotation = 0f;
                break;
            case Facing.DL:
                rotation = 120f;
                break;
            case Facing.DR:
                rotation = 60f;
                break;
        }
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
        int tileFrontX = getTileXFromFacing(facing);
        int tileFrontY = getTileYFromFacing(facing);
        int tileLeftX = getTileXFromFacing(leftFace(facing));
        int tileLeftY = getTileYFromFacing(leftFace(facing));
        int tileRightX = getTileXFromFacing(rightFace(facing));
        int tileRightY = getTileYFromFacing(rightFace(facing));


        tileInFront = visionReturn(tileFrontX, tileFrontY);
        tileToLeft = visionReturn(tileLeftX, tileLeftY);
        tileToRight = visionReturn(tileRightX, tileRightY);
        if (tileInFront == 2) bugInFront = gm.getBugAt(tileFrontX, tileFrontY);
    }


    int visionReturn (int x, int y)
    {
        int o = 0;
        if (!gm.canMove(x, y))
        {
            o = 1; //I SEE the an unwalkable tile
        }
        else if (gm.isBugAt(x, y))
        {
            o = 2; //I SEE A BUG
        }
        else if (gm.isPlantAt(x, y))
        {
            o = 3; //I SEE TASTY FOOD
        }

        return o;
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
        energy-=2;
    }

    public void rotateRight()
    {
        facing = rightFace(facing);
        this.rotation += 60f;
        energy-=2;
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
                energy -= 2; //Walking forward is good
            } else
            {
                energy -= 5; //Don't walk into other bugs
            }
        } else
        {
            energy -= 10; //REALLY don't walk into walls
        }
    }

    public void doNothing()
    {
        energy--;
    }

    public void birth()
    {
        Facing behind = facing;

        behind = rightFace(behind);
        behind = rightFace(behind);
        behind = rightFace(behind);

        int behindX = getTileXFromFacing(behind);
        int behindY = getTileYFromFacing(behind);
        if ((energy > 100) && (gm.canMove(behindX, behindY)) && (!gm.isBugAt(behindX, behindY)))
        {
            energy -= 50;
            gm.makeBug(behindX, behindY, (NeuralAI)ai);
        } else
        {
            doNothing();
        }
    }

    public void Update(float dt)
    {
        
        if (energy <= 0)
        {
            gm.remove(this);
        }
        if (energy > 200)
        {
            energy = 200;
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