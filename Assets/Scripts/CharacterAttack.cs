using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] Grid targetGrid;
    [SerializeField] GridHighlight highlight;

    List<Vector2Int> attackPosition;

    /*private void Start()
    {
        CalculateAttackArea();
    }*/

    public void CalculateAttackArea(Vector2Int characterpositionOnGrid, int attackRange, bool selfTargetable = false)
    {
        if (attackPosition == null)
        {
            attackPosition = new List<Vector2Int>();
        }
        else
        {
            attackPosition.Clear();
        }

        attackPosition = new List<Vector2Int>();

        //�ڽ��� ��ġ���� ���� �� Ÿ�� ����
        for (int x = - attackRange; x <= attackRange; x++)
        {
            for (int y = - attackRange; y <= attackRange; y++)
            {
                //Ÿ�� �O���� �� �ڽŰ� Ÿ���� �Ÿ� ��� -- �Ÿ� ���� �������� �Ʒ���
                if(Mathf.Abs(x) + Mathf.Abs(y) > attackRange)
                {
                    continue;//�Ÿ� ���� �� for������ ���ư�
                }
                //�ڽ��� Ÿ�꿡�� ��
                if(selfTargetable == false)
                {
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }
                }
                //Ÿ���� ��ǥ ���� �����ϴ��� üũ -- CheckBoundry
                if(targetGrid.CheckBoundry(characterpositionOnGrid.x + x, characterpositionOnGrid.y + y) == true)
                {
                    attackPosition.Add(new Vector2Int(characterpositionOnGrid.x + x, characterpositionOnGrid.y + y));
                }
            }
        }
        highlight.Highlight(attackPosition);
    }

    internal GridObject getAttackTarget(Vector2Int positionOnGrid)
    {
        GridObject target = targetGrid.GetPlacedObject(positionOnGrid);
        return target;
    }

    internal bool Check(Vector2Int positionOnGrid)
    {
        return attackPosition.Contains(positionOnGrid);
    }

    /*
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
    }*/
}
