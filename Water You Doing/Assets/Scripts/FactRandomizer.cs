using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactRandomizer : MonoBehaviour
{
    List<string> facts;
    List<string> factsStart;
    string fact;
    int index;

    void Start()
    {
        factsStart = new List<string>()
        {
            "2.5 percent of all the water on the planet is freshwater that it is drinkable.",
            "Only 1 percent of all freshwater is easily accessible in rivers, lakes and streams. The rest of it is stuck in glaciers and snowfields.",
            "Out of around 7.8 billion people in the world, only about 6 billion of them have access to clean water.",
            "A person can survive about a month without food, but only 5 to 7 days without water.",
            "Showering and bathing are the largest indoor uses (27%) of water domestically.",
            "If every household in America had a faucet that dripped once each second, 928 million gallons of water a day would leak away.",
            "One flush of the toilet uses 10 liters of water on average.",
            "You use about 20 liters of water if you leave the water running while brushing your teeth.",
            "An average bath requires 60 liters of water.",
            "An automatic dishwasher uses less than half of the water used while washing by hand."
        };
        FillFactList();
    }

    void FillFactList()
    {
        facts = new List<string>(factsStart);
    }

    public string GetFact()
    {
        index = Random.Range(0, facts.Count);
        fact = facts[index];
        facts.RemoveAt(index);

        if (facts.Count == 0)
            FillFactList();

        return fact;
    }
}
