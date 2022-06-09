using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurn : MonoBehaviour
{
    public bool canWalk;
    public bool canAct;


    private void Start()
    {
        GrantTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrantTurn()
    {
        //턴 넘겼을 떄 행동권을 준다.
        canWalk = true;
        canAct = true;

    }
}
