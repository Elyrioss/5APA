using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceScreen : MonoBehaviour
{
    public List<ScienceButton> Buttons = new List<ScienceButton>();
    void Start()
    {
        foreach (ScienceButton b in Buttons)
        {
            b.science = GetScience(b.NameScience);
        }
    }

    public Science GetScience(string nameScience)
    {
        switch (nameScience)
        {
            case "Manger":
                return new Manger();
        }

        return null;
    }
}

[Serializable]
public class Science
{
    public string nameScience;
    public virtual void Action(){}
}

public class Manger : Science
{
    public Manger()
    {
        nameScience = "Manger";
    }

    public override void Action()
    {
        Debug.Log("AAAAAAAAA");
    }
}