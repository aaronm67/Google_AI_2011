public class Planet
{
    // Initializes a planet.
    public Planet(int planetID,
                  int owner,
          int numShips,
          int growthRate,
          double x,
          double y)
    {
        this.PlanetID = planetID;
        this.Owner = owner;
        this.NumShips = numShips;
        this.GrowthRate = growthRate;
        this.X = x;
        this.Y = y;
    }

    // Accessors and simple modification functions. These should be mostly
    // self-explanatory.
    public int PlanetID { get; set; }

    public int Owner { get; set; }

    public int NumShips { get; set; }

    public int GrowthRate { get; set; }

    public double X { get; set; }

    public double Y { get; set; }

    public void SetOwner(int newOwner)
    {
        this.Owner = newOwner;
    }

    public void SetShips(int newNumShips)
    {
        this.NumShips = newNumShips;
    }

    public void AddShips(int amount)
    {
        NumShips += amount;
    }

    public void RemoveShips(int amount)
    {
        NumShips -= amount;
    }

    private Planet(Planet _p)
    {
        PlanetID = _p.PlanetID;
        Owner = _p.Owner;
        NumShips = _p.NumShips;
        GrowthRate = _p.GrowthRate;
        X = _p.X;
        Y = _p.Y;
    }
}
