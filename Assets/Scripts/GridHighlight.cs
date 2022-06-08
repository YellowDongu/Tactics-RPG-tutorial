using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlight : MonoBehaviour
{
    //타일에 색깔입히는 녀석, 컨테이너에 스크립트를 탑재시켜야 함
    //그리드에 링크시키고
    Grid grid;

    //컨테이너랑 프리팹을 연결한다
    [SerializeField] GameObject highlightPoint;
    [SerializeField] GameObject Container;

    List<GameObject> highlightPointsGOs;



    void Awake()
    {
        grid = GetComponentInParent<Grid>();
        highlightPointsGOs = new List<GameObject>();
        //Highlight(testTargetPosition);
    }

    private GameObject CreateHighlightObject()
    {
        //만들어둔 하이라이트 타일 프리팹 인스턴스 생성
        GameObject go = Instantiate(highlightPoint);
        //하이라아트 목록에 추가
        highlightPointsGOs.Add(go);
        go.transform.SetParent(Container.transform);
        return go;
    }

    public void Highlight(List<Vector2Int> positions)
    {
        //리스트(벡터2용, 그리드 좌표) 하나하나 빼고 분해해서 넣어준다
        for (int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].x, positions[i].y, GetHighlightPointGO(i));
        }
    }

    internal void Hide()
    {
        //하이라이트 해주는 애들 비활성화해 투명상태로 만듬
        for (int i = 0; i < highlightPointsGOs.Count; i++)
        {
            highlightPointsGOs[i].SetActive(false);
        }
    }

    public void Highlight(List<PathNode> positions)
    {
        //리스트(경로 노드 전용) 하나씩 빼고 노드를 분해해서 넣어준다.
        for (int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].pos_x, positions[i].pos_y, GetHighlightPointGO(i));
        }
    }

    private GameObject GetHighlightPointGO(int i)
    {
        //만약 이미 생성된 하이라이트 할 리스트가 존재한다
        if(highlightPointsGOs.Count > i)
        {
            return highlightPointsGOs[i];//그럼 그 리스트(의 해당항목) 토스한다
        }
        //없으면 만든다
        GameObject newHighlightObject = CreateHighlightObject();
        //만든거(를 리스트 끝에 넣고)를 준다
        return newHighlightObject;
    }

    public void Highlight(int posX, int posY, GameObject highlightObject)
    {
        //하이라이트 프리팹 불투명화(액티브)
        highlightObject.SetActive(true);
        //그리드 좌표를 월드 좌표로 변환
        Vector3 position = grid.GetWorldPosition(posX, posY, true);
        //자리에서 조금 위로 띄운다
        position += Vector3.up * 0.2f;
        //월드 좌표 적용(자리 배치)
        highlightObject.transform.position = position;
    }
}
