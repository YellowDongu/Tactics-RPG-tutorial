using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] GridObject selectedCharacter;
    [SerializeField] Grid targetGrid;
    [SerializeField] GridHighlight highlight;
    [SerializeField] LayerMask terrainLayerMask;

    List<Vector2Int> attackPosition;

    private void Start()
    {
        CalculateAttackArea();
    }


    public void CalculateAttackArea(bool selfTargetable = false)
    {
        Character character = selectedCharacter.GetComponent<Character>();
        int attackRange = character.attackRange;

        attackPosition = new List<Vector2Int>();

        for (int x = - attackRange; x <= attackRange; x++)
        {
            for (int y = - attackRange; y <= attackRange; y++)
            {
                if(Mathf.Abs(x) + Mathf.Abs(y) > attackRange)
                {
                    continue;
                }
                if(selfTargetable == false)
                {
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }
                }
                if(targetGrid.CheckBoundry(selectedCharacter.positionOnGrid.x + x, selectedCharacter.positionOnGrid.y + y) == true)
                {
                    attackPosition.Add(new Vector2Int(selectedCharacter.positionOnGrid.x + x, selectedCharacter.positionOnGrid.y + y));
                }
            }
        }
        highlight.Highlight(attackPosition);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
            {
                Vector2Int gridposition = targetGrid.GetGridPosition(hit.point);

                if (attackPosition.Contains(gridposition))
                {
                    GridObject gridObject = targetGrid.GetPlacedObject(gridposition);
                    if(gridObject == null)
                    {
                        return;
                    }
                    selectedCharacter.GetComponent<Attack>().AttackPosition(gridObject);
                }
            }
        }
    }
}
