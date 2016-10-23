using UnityEngine;

public class AI
{
    public Bug parent;
    public float moveCount = 0f;
    public virtual void Update(float dt)
    {
        if (moveCount >= .20f)
        {
            move();
            moveCount = 0f;
        } else
        {
            moveCount += dt;
        }
    }

    public virtual void move() { }
}

/*public class RandomAI : AI
{
    override
    public void move()
    {
        int toDo = Random.Range(0, 5);
        switch (toDo)
        {
            case 0:
                parent.moveForward();
                break;
            case 1:
                parent.rotateLeft();
                break;
            case 2:
                parent.rotateRight();
                break;
            case 3:
                parent.birth();
                break;
            default:
                break;
        }
    }
}*/

public class Player : AI
{
    override
    public void Update(float dt)
    {
        //rotation
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            parent.rotateLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            parent.rotateRight();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            parent.moveForward();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            parent.birth();
        }
    }
}