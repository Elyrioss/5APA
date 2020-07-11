using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceButton : MonoBehaviour
{
    
    public Science science;
    public string NameScience;

    public void Action()
    {
        science.Action();
    }

}
