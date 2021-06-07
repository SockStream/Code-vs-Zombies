using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Save humans, destroy zombies!
 **/
public class People
{
    public int X;
    public int Y;
    public int Id;
    public double shortZombieDistance;
    public bool Fucked;
    public int delai;

    public People(int xValue, int yValue, int id)
    {
        X = xValue;
        Y = yValue;
        Id = id;
        shortZombieDistance = -1;
        Fucked = false;
    }

    public void calculateDistance(int xPos, int yPos)
    {
        double distance = (xPos - X) * (xPos - X) + (yPos - Y) * (yPos - Y);
        distance = Math.Sqrt(distance);
        if (shortZombieDistance == -1)
        {
            shortZombieDistance = distance;
        }
        else
        {
            if (shortZombieDistance > distance)
            {
                shortZombieDistance = distance;
            }
        }
        delai = (int)Math.Ceiling(shortZombieDistance / 400);
    }

    public void amIFucked(int myPosX, int myPosY)
    {
        double rescueDistance = (X - myPosX) * (X - myPosX) + (Y - myPosY) * (Y - myPosY);
        rescueDistance = Math.Sqrt(rescueDistance) - 2000;
        int rescueDelai = (int)Math.Ceiling(rescueDistance / 1000) + 1;
        if (rescueDelai > delai)
        {
            Fucked = true;
            Console.Error.WriteLine(" --> " + Id + " is fucked because " + rescueDelai + " < " + delai);
        }
    }
}

class Player
{
    static People getClosestPeopleInDanger(List<People> peopleList)
    {
        People danger = null;
        foreach (People people in peopleList)
        {
            if (people.Fucked)
            {
                continue;
            }
            if (danger == null)
            {
                danger = people;
            }
            else
            {
                if (people.delai < danger.delai && !people.Fucked)
                {
                    danger = people;
                    Console.Error.WriteLine("will head to " + danger.Id);
                }
            }
        }
        return danger;
    }

    static void Main(string[] args)
    {
        string[] inputs;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int x = int.Parse(inputs[0]);
            int y = int.Parse(inputs[1]);
            int humanCount = int.Parse(Console.ReadLine());
            List<People> peopleList = new List<People>();
            for (int i = 0; i < humanCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int humanId = int.Parse(inputs[0]);
                int humanX = int.Parse(inputs[1]);
                int humanY = int.Parse(inputs[2]);
                People people = new People(humanX, humanY, humanId);
                peopleList.Add(people);
            }

            int zombieCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < zombieCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int zombieId = int.Parse(inputs[0]);
                int zombieX = int.Parse(inputs[1]);
                int zombieY = int.Parse(inputs[2]);
                int zombieXNext = int.Parse(inputs[3]);
                int zombieYNext = int.Parse(inputs[4]);
                foreach (People people in peopleList)
                {
                    people.calculateDistance(zombieXNext, zombieYNext);
                }
            }
            foreach (People people in peopleList)
            {
                people.amIFucked(x, y);
            }

            // Write an action using Console.WriteLine()
            //déterminer l'humain le plus en danger

            // To debug: Console.Error.WriteLine("Debug messages...");
            People danger = getClosestPeopleInDanger(peopleList);
            if (danger == null)
            {
                danger = peopleList[0];
            }
            Console.Error.WriteLine("heading to : " + danger.Id);
            Console.WriteLine(danger.X + " " + danger.Y); // Your destination coordinates
        }
    }
}