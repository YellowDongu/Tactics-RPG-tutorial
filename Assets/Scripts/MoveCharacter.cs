using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    [SerializeField] Grid targetGrid;
    [SerializeField] GridHighlight gridHighlight;

    Pathfinding pathfinding;


    private void Start()
    {
        pathfinding = targetGrid.GetComponent<Pathfinding>();
    }

    public void CheckWalkableTerrain(Character targetCharacter)
    {
        //ĳ���Ͱ� �� �� �ִ� ���� ����ϰ� ǥ�����ش�.

        GridObject gridObject = targetCharacter.GetComponent<GridObject>();
        //���� ������ ���� ��
        List<PathNode> walkableNodes = new List<PathNode>();
        //�ʱ�ȭ�ؼ� ���� ���� �ʰ�
        pathfinding.Clear();
        //�� �� �ִ� ���� ������ִ� ��
        pathfinding.CalculateWalkableNodes(gridObject.positionOnGrid.x, gridObject.positionOnGrid.y, targetCharacter.movementPoints, ref walkableNodes);
        //������ �ִ� �ֵ��� �Ⱥ��̰� �Ѵ�
        gridHighlight.Hide();
        //���� ����� �޾Ƽ� �׷������� �����ִ� ��
        gridHighlight.Highlight(walkableNodes);
    }

    public List<PathNode> GetPath(Vector2Int from)
    {
        //������ �Ϸ�� ����Ʈ�� ����ٰ� �����Ѵ�.
        List<PathNode> path = pathfinding.TraceBackPath(from.x, from.y);
        //��� ���� �� �ƹ��͵� �ȹ���
        if (path == null)
        {
            return null;
        }
        if (path.Count == 0)
        {
            return null;
        }
        //�޼ҵ忡�� ������ �Ƚ��������� ���⼭ ������
        path.Reverse();

        return path;//��� �޾ƶ�
    }
}
