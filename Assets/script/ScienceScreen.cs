using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScienceScreen : MonoBehaviour
{
    public List<ScienceButton> Buttons = new List<ScienceButton>();
    public Civilisation owner;
    public ScienceButton currentScience;

    public TextMeshProUGUI ScienceName;
    public TextMeshProUGUI NumScience;
    public TextMeshProUGUI Description;
    public Image ColorType;
    public Image Icon;
    private Sprite imageBase;
    public Sprite Checked;
    public void StartScreen()
    {
        imageBase = Icon.sprite;
        foreach (ScienceButton b in Buttons)
        {
            b.science = GetScience(b.NameScience);
            b.icon = Resources.Load<Sprite>(b.science.icon);
            b.iconImage.sprite = b.icon;
            b.Name.text = b.science.nameScience;
            b.buttonColor.color = b.science.Type;
            foreach (ScienceButton S in b.Sons)
            {
                S.GetComponent<Button>().interactable = false;
            }
        }

        currentScience = null;
    }

    public Science GetScience(string nameScience)
    {
        switch (nameScience)
        {
            case "Stockage Avance":
                return new StockageAvance();
            case "Mare Nostrum":
                return new MareNostrum();
            case "Entrainement":
                return new Entrainement();
            case "Monnaie":
                return new Monnaie();
            case "Astronomie":
                return new Astronomie();
            case "Waaagh":
                return new Waaagh();
            case "Bouclier":
                return new Bouclier();
            case "Equilibre":
                return new Equilibre();
            case "Cavalerie":
                return new Cavalerie();
        }

        return null;
    }

    public void EndTurn()
    {
        if (currentScience != null)
        {
            currentScience.science.cost -= owner.Science;
            NumScience.text = Mathf.Ceil(currentScience.science.cost / owner.Science)+"";
            if (currentScience.science.cost <= 0)
            {
                currentScience.science.Action(owner);
                currentScience.iconImage.sprite = Checked;
                foreach (ScienceButton E in currentScience.Excludes)
                {
                    E.GetComponent<Button>().interactable = false;
                }
                foreach (ScienceButton S in currentScience.Sons)
                {
                    S.GetComponent<Button>().interactable = true;
                }
                currentScience.GetComponent<Button>().interactable = false;
                Description.text = "Il est temps de commencer la recherche";
                ScienceName.text = "Pas de Recherche";
                ColorType.color = Color.white;
                Icon.sprite = imageBase;
                NumScience.text = 0+"";
                currentScience = null;
            }
        }       
    }
    
}

[Serializable]
public class Science
{
    public string nameScience;
    public string Description;
    public string icon;
    public Color Type;
    public float cost;
    public virtual void Action(Civilisation c){}
}

//BASE 4
public class StockageAvance : Science
{
    public StockageAvance()
    {
        nameScience = "Stockage Avance";
        Description = "Vous avez apris a ne plus laisse votre nouriture pourrire au soleil.\n\n(+5 de production de nourriture par villes)";
        icon = "Stockage";
        Type=Color.green;
        cost = 50;
    }

    public override void Action(Civilisation c)
    {
        c.bonusFood = 5;
        foreach (City City in c.Cities)
        {
            City.food += 5;
        }
    }
}
public class Entrainement : Science
{
    public Entrainement()
    {
        nameScience = "Entrainement";
        Description = "Si tu veux la paix prepare la guerre.\n\n(+1 dmg +1PV pour toutes les unites)";
        icon = "Warrior";
        Type=Color.red;
        cost = 50;
    }

    public override void Action(Civilisation c)
    {
        c.bonusDamage += 1;
        c.bonusHp += 1;
        foreach (Unit u in c.Units)
        {
            u.Damage += 1;
            u.MAXHP += 1;
            u.HP += 1;
        }
    }
}
public class Monnaie : Science
{
    public Monnaie()
    {
        nameScience = "Monnaie";
        Description = "Les échanges vont plus vite.\n\n(+5 de production d'or par villes)";
        icon = "Monnaie";
        Type=Color.yellow;
        cost = 50;
    }

    public override void Action(Civilisation c)
    {
        c.bonusGold = 5;
        foreach (City City in c.Cities)
        {
            City.gold += 5;
        }
    }
}
public class Astronomie : Science
{
    public Astronomie()
    {
        nameScience = "Astronomie";
        Description = "Le ciel regorge de secret.\n\n(+5 de production scientifique par villes)";
        icon = "Astronomie";
        Type=Color.blue;
        cost = 50;
    }

    public override void Action(Civilisation c)
    {
        c.bonusScience = 5;
        foreach (City City in c.Cities)
        {
            City.science += 5;
        }
    }
}
public class MareNostrum : Science
{
    public MareNostrum()
    {
        nameScience = "Mare Nostrum";
        Description = "Les oceans vous appelle.\n\n(Debloque les ports)";
        icon = "Port";
        Type = Color.yellow;
        cost = 80;
    }

    public override void Action(Civilisation c)
    {
        c.AuthorizedBuildings.Add("Port");
    }
}

// WAR1
public class Waaagh : Science
{
    public Waaagh()
    {
        nameScience = "Waaagh";
        Description = "Waaaaaaaaagh.\n\n(+5 dmg -5PV pour toutes les unites)";
        icon = "Waaagh";
        Type=Color.red;
        cost = 80;
    }

    public override void Action(Civilisation c)
    {
        c.bonusDamage += 5;
        c.bonusHp -= 5;
        foreach (Unit u in c.Units)
        {
            u.Damage += 5;
            u.MAXHP -= 5;
            if (u.HP <= 5)
            {
                u.HP = 1;
            }
            else
            {
                u.HP -= 5;
            }
            
        }
    }
}
public class Bouclier : Science
{
    public Bouclier()
    {
        nameScience = "Bouclier";
        Description = "Protege de 25% des fleches.\n\n(+5 PV -5dmg pour toutes les unites)";
        icon = "Bouclier";
        Type=Color.red;
        cost = 80;
    }

    public override void Action(Civilisation c)
    {
        c.bonusDamage -= 5;
        c.bonusHp += 5;
        foreach (Unit u in c.Units)
        {
            u.Damage -= 5;
            if (u.Damage <= 0)
            {
                u.Damage = 1;
            }
            u.HP += 5;
            u.MAXHP += 5;
            
        }
    }
}
public class Equilibre : Science
{
    public Equilibre()
    {
        nameScience = "Equilibre";
        Description = "Une bonne attaque commence par une bonne defense, ou l'inverse?\n\n(+2 PV +2dmg pour toutes les unites)";
        icon = "Equilibre";
        Type=Color.red;
        cost = 80;
    }

    public override void Action(Civilisation c)
    {
        c.bonusDamage += 2;

        foreach (Unit u in c.Units)
        {
            u.Damage += 2;
            u.MAXHP += 2;
            u.HP += 2;
        }
    }
}

public class Cavalerie : Science
{
    public Cavalerie()
    {
        nameScience = "Cavalerie";
        Description = "Horse go zoom zoom bad guy go dead dead?\n\n(Debloque les cavaliers)";
        icon = "Cavalerie";
        Type=Color.red;
        cost = 90;
    }

    public override void Action(Civilisation c)
    {
        c.AuthorizedUnits.Add("Riders0");
    }
}