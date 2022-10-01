using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceContainer : MonoBehaviour
{
    public Allegiance allegiance;
    public List<CharacterTurn> force;

    public void AddMe(CharacterTurn character)
    {
        if(force == null)
        {
            force = new List<CharacterTurn>();
        }
        force.Add(character);
        character.transform.parent = transform;
    }

    public void GrantTurn()
    {
        for (int i = 0; i < force.Count; i++)
        {
            force[i].GrantTurn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
