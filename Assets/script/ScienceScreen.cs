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
    
    public Image ScienceGem;
    public Image WarGem;
    public Image FoodGem;
    public Image GoldGem;
    public TextMeshProUGUI ScienceCiv;
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

    public void load()
    {
        ScienceCiv.text = owner.Science+"";
        if (owner.ScienceWin)
            ScienceGem.color = new Color(0,0,255,255);
        if (owner.FoodWin)
            FoodGem.color = new Color(0,255,0,255);
        if (owner.WarWin)
            WarGem.color = new Color(255,0,0,255);
        if (owner.GoldWin)
            GoldGem.color = new Color(0,255,255,255);
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
            case "Mine de fer":
                return new IronMine();
            case "Mine d'or":
                return new GoldMine();
            case "Plateforme Maritime":
                return new Plateforme();
            case "Recolte de corail":
                return new Corail();
            case "Sagesse":
                return new Sagesse();
            case "Arme Imperial":
                return new ArmeImperial();
            case "Culture en Terrasse":
                return new CultureTerrasse();
            case "Tresorerie":
                return new Tresorerie();
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

//ECO1
public class MareNostrum : Science
{
    public MareNostrum()
    {
        nameScience = "Mare Nostrum";
        Description = "Les oceans vous appelle.\n\n(Debloque les ports)";
        icon = "Port";
        Type = Color.yellow;
        cost = 100;
    }

    public override void Action(Civilisation c)
    {
        c.AuthorizedBuildings.Add("Port");
    }
}
public class GoldMine : Science
{
    public GoldMine()
    {
        nameScience = "Mine d'or";
        Description = "Make it rain.\n\n(Debloque les mines d'or)";
        icon = "Mine(Or)";
        Type = Color.yellow;
        cost = 80;
    }

    public override void Action(Civilisation c)
    {
        c.AuthorizedBuildings.Add("Mine(Or)");
    }
}
public class IronMine : Science
{
    public IronMine()
    {
        nameScience = "Mine de fer";
        Description = "Vous decouvrez les pioche en cobble quoi.\n\n(Debloque les mines de fer)";
        icon = "Mine(Fer)";
        Type = new Color(255,166,0);
        cost = 80;
    }

    public override void Action(Civilisation c)
    {
        c.AuthorizedBuildings.Add("Mine(Fer)");
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

// WAR2
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

// FOOD

public class Plateforme : Science
{
    public Plateforme()
    {
        nameScience = "Plateforme Maritime";
        Description = "Qui vit dans ananas dans la mer?.\n\n(Debloque les plateforme maritime)";
        icon = "Plateforme Maritime";
        Type = Color.green;
        cost = 90;
    }

    public override void Action(Civilisation c)
    {
        c.AuthorizedBuildings.Add("Plateforme Maritime");
    }
}

public class Corail : Science
{
    public Corail()
    {
        nameScience = "Recolte de corail";
        Description = "Passe le bonjour a nemo.\n\n(Debloque les plateforme de Corail)";
        icon = "Plateforme Corail";
        Type = Color.green;
        cost = 120;
    }

    public override void Action(Civilisation c)
    {
        c.AuthorizedBuildings.Add("Plateforme Corail");
    }
}


// WIN
public class CultureTerrasse : Science
{
    public CultureTerrasse()
    {
        nameScience = "Culture en Terrasse";
        Description = "Un pas vers la victoire.\n\n(Vous octroie un point de victoire scientifique)";
        icon = "Culture";
        Type = Color.green;
        cost = 1;
    }

    public override void Action(Civilisation c)
    {      
        c.FoodWin = true;
        foreach (City City in c.Cities)
        {
            City.food += 20;
        }
    }
}
public class ArmeImperial : Science
{
    public ArmeImperial()
    {
        nameScience = "Arme Imperial";
        Description = "You must not follow but you will witness.\n\n(Vous octroie un point de victoire scientifique)";
        icon = "Arme";
        Type = Color.red;
        cost = 1;
    }

    public override void Action(Civilisation c)
    {
        c.WarWin = true;
        c.bonusDamage += 8;

        foreach (Unit u in c.Units)
        {
            u.Damage += 8;
            u.MAXHP += 8;
            u.HP += 8;
        }
    }
}

public class Tresorerie : Science
{
    public Tresorerie()
    {
        nameScience = "Tresorerie";
        Description = "Cash-For-Days.\n\n(Vous octroie un point de victoire scientifique)";
        icon = "Tresorerie";
        Type = Color.yellow;
        cost = 1;
    }

    public override void Action(Civilisation c)
    {
        c.GoldWin = true;
        c.Gold = (int)c.Gold * 1.3f;

    }
}

public class Sagesse : Science
{
    public Sagesse()
    {
        nameScience = "Sagesse universel";
        Description = "Ctulhu existe.\n\n(Vous octroie un point de victoire scientifique)";
        icon = "Sagesse";
        Type = Color.blue;
        cost = 1;
    }

    public override void Action(Civilisation c)
    {
        c.ScienceWin = true;
        c.bonusScience = 20;
        foreach (City City in c.Cities)
        {
            City.science += 20;
        }

    }
}