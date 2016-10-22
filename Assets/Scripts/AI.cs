using UnityEngine;

class AI
{
    public Bug parent;
    float moveCount = 0f;
    public virtual void Update(float dt)
    {
        if (moveCount >= .20f)
        {
            moveCount = 0f;
            int toDo = Random.Range(0, 3);
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
                default:
                    break;
            }
        } else
        {
            moveCount += dt;
        }
    }
}

class Player : AI
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
    }
}