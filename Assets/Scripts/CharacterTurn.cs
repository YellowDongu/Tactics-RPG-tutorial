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
        //�� �Ѱ��� �� �ൿ���� �ش�.
        canWalk = true;
        canAct = true;

    }
}
