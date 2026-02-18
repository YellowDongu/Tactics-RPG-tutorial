using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearUtility : MonoBehaviour
{
    [SerializeField] Pathfinding targetPF;
    [SerializeField] GridHighlight attackHighlight;
    [SerializeField] GridHighlight moveHighlight;

    public void ClearPathfinding()
    {
        targetPF.Clear();
    }

    public void ClearGridHighlightAttack()
    {
        attackHighlight.Hide();
    }

    public void CleaGridHighlightMove()
    {
        moveHighlight.Hide();
    }
}
