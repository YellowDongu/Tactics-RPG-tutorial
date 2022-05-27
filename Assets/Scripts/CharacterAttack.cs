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

        //자신의 위치에서 범위 내 타깃 수색
        for (int x = - attackRange; x <= attackRange; x++)
        {
            for (int y = - attackRange; y <= attackRange; y++)
            {
                //타깃 찿았을 때 자신과 타깃의 거리 재기 -- 거리 내에 있을때만 아래로
                if(Mathf.Abs(x) + Mathf.Abs(y) > attackRange)
                {
                    continue;//거리 밖일 시 for문으로 돌아감
                }
                //자신은 타깃에서 뺌
                if(selfTargetable == false)
                {
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }
                }
                //타깃이 좌표 내에 실존하는지 체크 -- CheckBoundry
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
