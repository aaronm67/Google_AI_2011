public class Fleet
{
    // Initializes a fleet.
    public Fleet(int owner, int numShips, int sourcePlanet, int destinationPlanet, int totalTripLength, int turnsRemaining)
    {
        this.Owner = owner;
        this.NumShips = numShips;
        this.SourcePlanet = sourcePlanet;
        this.DestinationPlanet = destinationPlanet;
        this.TotalTripLength = totalTripLength;
        this.TurnsRemaining = turnsRemaining;
    }

    // Initializes a fleet.
    public Fleet(int owner, int numShips)
    {
        this.Owner = owner;
        this.NumShips = numShips;
        this.SourcePlanet = -1;
        this.DestinationPlanet = -1;
        this.TotalTripLength = -1;
        this.TurnsRemaining = -1;
    }

    // Accessors and simple modification functions. These should be mostly
    // self-explanatory.
    public int Owner { get; set; }

    public int NumShips { get; set; }

    public int SourcePlanet { get; set; }

    public int DestinationPlanet { get; set; }

    public int TotalTripLength { get; set; }

    public int TurnsRemaining { get; set; }

    public void RemoveShips(int amount)
    {
        NumShips -= amount;
    }

    // Subtracts one turn remaining. Call this function to make the fleet get
    // one turn closer to its destination.
    public void TimeStep()
    {
        if (TurnsRemaining > 0)
        {
            --TurnsRemaining;
        }
        else
        {
            TurnsRemaining = 0;
        }
    }

    //private int owner;
    //private int numShips;
    //private int sourcePlanet;
    //private int destinationPlanet;
    //private int totalTripLength;
    //private int turnsRemaining;

    private Fleet(Fleet _f)
    {
        Owner = _f.Owner;
        NumShips = _f.NumShips;
        SourcePlanet = _f.SourcePlanet;
        DestinationPlanet = _f.DestinationPlanet;
        TotalTripLength = _f.TotalTripLength;
        TurnsRemaining = _f.TurnsRemaining;
    }
}
