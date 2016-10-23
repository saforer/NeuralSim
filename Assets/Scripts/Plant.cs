public class Plant : FSprite
{
    public int gridX;
    public int gridY;

    public Plant(int gridX, int gridY) : base("plant")
    {
        this.gridX = gridX;
        this.gridY = gridY;
    }
}