using System;
using System.Collections.Generic;
using System.Linq;

public class Move
{
    public int SourcePlanet { get; set; }
    public int DestPlanet { get; set; }
    public int Turn { get; set; }
}

public class MyBot
{
    private static int CurrentTurn = 0;
    private static bool IsKamikazi = false;
    // The DoTurn function is where your code goes. The PlanetWars object
    // contains the state of the game, including information about all planets
    // and fleets that currently exist. Inside this function, you issue orders
    // using the pw.IssueOrder() function. For example, to send 10 ships from
    // planet 3 to planet 8, you would say pw.IssueOrder(3, 8, 10).
    //
    // There is already a basic strategy in place here. You can use it as a
    // starting point, or you can throw it out entirely and replace it with
    // your own. Check out the tutorials and articles on the contest website at
    // http://www.ai-contest.com/resources.

    private static double GetScore(Planet source, Planet dest, PlanetWars pw)
    {
        // on the first turn, don't attack the enemy -- take as many neutral planets as possible
        if (CurrentTurn == 0 && dest.Owner == 2)
            return double.MaxValue;

        double score = dest.NumShips / 1 + dest.GrowthRate;
        if (dest.Owner != 0)
            score = score * 1.1;

        int distance = pw.Distance(source.PlanetID, dest.PlanetID);
        score = score * (double)(distance / 2);
        return score;
    }

    private static bool CanSend(Planet source, Planet dest, PlanetWars pw)
    {
        int fleetsHeaded = pw.MyFleets().Where(f => f.DestinationPlanet == dest.PlanetID).Sum(f => f.NumShips);
        int destFleets = dest.NumShips;
        int sourceFleets = source.NumShips;
        return (destFleets < sourceFleets && fleetsHeaded < destFleets);
    }

    private static int AvailableFleets(Planet p, PlanetWars pw)
    {
        int available = p.NumShips - pw.EnemyFleets().Where(f => f.DestinationPlanet == p.PlanetID).Sum(f => f.NumShips);
        return available;
    }

    private static int GetNumFleetsToSend(Planet source, Planet dest, PlanetWars pw)
    {
        int distance = pw.Distance(source.PlanetID, dest.PlanetID);
        int inFlight = pw.MyFleets().Where(f => f.DestinationPlanet == dest.PlanetID && f.TurnsRemaining < distance)
                                    .Sum(f => f.NumShips);

        int shipsOnArrival = (dest.Owner == 0) ? dest.NumShips : dest.NumShips + (distance * dest.GrowthRate);
        return (shipsOnArrival  + 1) - inFlight;
    }

    public static void DoTurn(PlanetWars pw)
    {
        if (CurrentTurn == 0 && pw.Distance(pw.MyPlanets().First().PlanetID, pw.EnemyPlanets().First().PlanetID) <= 8)
            IsKamikazi = true;

        if (IsKamikazi)
        {
            foreach (var planet in pw.MyPlanets())
            {
                var enemy = pw.EnemyPlanets().FirstOrDefault();
                int shipsHeaded = pw.Fleets().Where(f => f.DestinationPlanet == planet.PlanetID).Sum(f => f.NumShips);
                if (enemy != null)
                {
                    if (planet.NumShips - shipsHeaded - 1 > 0)
                        pw.IssueOrder(planet, enemy, planet.NumShips - shipsHeaded - 1);
                }
                else
                {
                    foreach (int dest in pw.EnemyFleets().Select(f => f.DestinationPlanet).Distinct())
                    {
                        if (dest == planet.PlanetID)
                            break;

                        int headedTo = pw.EnemyFleets().Where(f => f.DestinationPlanet == dest).Sum(f => f.NumShips);
                        int numToSend = ( (planet.NumShips - shipsHeaded) > headedTo) ? headedTo + 1 : (planet.NumShips - shipsHeaded);

                        if (numToSend > 0)
                            pw.IssueOrder(planet, pw.GetPlanet(dest), numToSend);
                    }
                }
            }
        }
        else
        {
            foreach (var planet in pw.MyPlanets())
            {
                int available = AvailableFleets(planet, pw);
                var enemy = pw.NotMyPlanets().Where(p => CanSend(planet, p, pw));
                enemy = enemy.OrderBy(p => GetScore(planet, p, pw));
                int skip = 0;

                while (available > 0)
                {
                    var bestEnemy = enemy.Skip(skip).FirstOrDefault();
                    skip++;

                    if (bestEnemy != null)
                    {
                        int numToSend = GetNumFleetsToSend(planet, bestEnemy, pw);
                        if (available > numToSend)
                        {

                            pw.IssueOrder(planet, bestEnemy, numToSend);
                            available = available - numToSend;
                        }
                    }
                    else
                    {
                        available = 0;
                    }
                }
            }
        }
    }

    public static void Main()
    {
        string line = "";
        string message = "";
        int c;
        try
        {
            while ((c = Console.Read()) >= 0)
            {
                switch (c)
                {
                    case '\n':
                        if (line.Equals("go"))
                        {
                            PlanetWars pw = new PlanetWars(message);
                            DoTurn(pw);
                            pw.FinishTurn();
                            message = "";
                            CurrentTurn++;
                        }
                        else
                        {
                            message += line + "\n";
                        }
                        line = "";
                        break;
                    default:
                        line += (char)c;
                        break;
                }
            }
        }
        catch (Exception)
        {
            // Owned.
        }
    }
}

