using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlight : MonoBehaviour
{
    //Ÿ�Ͽ� ���������� �༮, �����̳ʿ� ��ũ��Ʈ�� ž����Ѿ� ��
    //�׸��忡 ��ũ��Ű��
    Grid grid;

    //�����̳ʶ� �������� �����Ѵ�
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
        //������ ���̶���Ʈ Ÿ�� ������ �ν��Ͻ� ����
        GameObject go = Instantiate(highlightPoint);
        //���̶��Ʈ ��Ͽ� �߰�
        highlightPointsGOs.Add(go);
        go.transform.SetParent(Container.transform);
        return go;
    }

    public void Highlight(List<Vector2Int> positions)
    {
        //����Ʈ(����2��, �׸��� ��ǥ) �ϳ��ϳ� ���� �����ؼ� �־��ش�
        for (int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].x, positions[i].y, GetHighlightPointGO(i));
        }
    }

    internal void Hide()
    {
        //���̶���Ʈ ���ִ� �ֵ� ��Ȱ��ȭ�� ������·� ����
        for (int i = 0; i < highlightPointsGOs.Count; i++)
        {
            highlightPointsGOs[i].SetActive(false);
        }
    }

    public void Highlight(List<PathNode> positions)
    {
        //����Ʈ(��� ��� ����) �ϳ��� ���� ��带 �����ؼ� �־��ش�.
        for (int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].pos_x, positions[i].pos_y, GetHighlightPointGO(i));
        }
    }

    private GameObject GetHighlightPointGO(int i)
    {
        //���� �̹� ������ ���̶���Ʈ �� ����Ʈ�� �����Ѵ�
        if(highlightPointsGOs.Count > i)
        {
            return highlightPointsGOs[i];//�׷� �� ����Ʈ(�� �ش��׸�) �佺�Ѵ�
        }
        //������ �����
        GameObject newHighlightObject = CreateHighlightObject();
        //�����(�� ����Ʈ ���� �ְ�)�� �ش�
        return newHighlightObject;
    }

    public void Highlight(int posX, int posY, GameObject highlightObject)
    {
        //���̶���Ʈ ������ ������ȭ(��Ƽ��)
        highlightObject.SetActive(true);
        //�׸��� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 position = grid.GetWorldPosition(posX, posY, true);
        //�ڸ����� ���� ���� ����
        position += Vector3.up * 0.2f;
        //���� ��ǥ ����(�ڸ� ��ġ)
        highlightObject.transform.position = position;
    }
}
