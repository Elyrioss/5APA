using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityMenu : MonoBehaviour
{
    Animator Anim;

    private List<Unit> Units = new List<Unit>();
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Construct()
    {
        Anim.SetBool("IN", false);
        Anim.SetBool("OUT", true);
    }

    public void CreateWarrior()
    {
        Anim.SetBool("OUT", false);
        Anim.SetBool("IN", true);
    }


}
