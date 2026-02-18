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
        //첫번째 인수 : 자신의 위치정보, 두번째 인수 : 공격범위 정보, 세번째는 말 그대로
        //없을때만 새로 만든다 있으면 클리어시킴
        if (attackPosition == null)
        {
            attackPosition = new List<Vector2Int>();
        }
        else
        {
            attackPosition.Clear();
        }

        attackPosition = new List<Vector2Int>();

        //자신의 위치에서 범위 수색
        for (int x = - attackRange; x <= attackRange; x++)
        {
            for (int y = - attackRange; y <= attackRange; y++)
            {
                //타일과 거리 재기 -- 거리 내에 있을때만 아래로(대각선 취급 안함)
                if(Mathf.Abs(x) + Mathf.Abs(y) > attackRange)
                {
                    continue;//거리 밖일 시 스킵
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
                    //모든 체크 통과. 이 좌표를 저장 이 좌표는 공격범위를 뜻함
                    attackPosition.Add(new Vector2Int(characterpositionOnGrid.x + x, characterpositionOnGrid.y + y));
                }
            }
        }
        //이 포지션들을 하이라이트 시킴
        highlight.Highlight(attackPosition);
    }

    internal GridObject getAttackTarget(Vector2Int positionOnGrid)
    {
        //해당 좌표의 캐릭터를 (있으면) 노드에게서 정보 받아옴
        GridObject target = targetGrid.GetPlacedObject(positionOnGrid);
        return target;
    }

    internal bool Check(Vector2Int positionOnGrid)
    {
        //좌표에 그리드 오브젝트가 실존하는지 확인
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
