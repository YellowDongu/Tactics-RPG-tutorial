using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //캐릭터들의 움직임을 관장한다. 패스파인딩이랑 그리드등 여러 애들을 중개하는 애
    GridObject gridObject;
    CharacterAnimator characterAnimator;

    List<Vector3> pathWorldPositions;//경로 좌표

    public bool IS_MOVING
    {
        get
        {
            if(pathWorldPositions == null)//만약 경로가 없으면 false 출력
            {
                return false;
            }
            return pathWorldPositions.Count > 0;//있으면 true
        }
    }

    [SerializeField] float moveSpeed = 1f;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
    }

    public void Move(List<PathNode> path)
    {
        //움직일때는 스킵시켜서 목적지까지 옮긴 후 다음 목적지로 움직임 실시 --안할거임
        if (IS_MOVING)
        {
            SkipAnimation();
        }

        //경로에 있는 노드 좌표들을 모두 월드 좌표로 변환
        pathWorldPositions =  gridObject.targetGrid.ConvertPathNodesToWorldPositions(path);
        //움직일 애를 기존 좌표 노드에서 등록 해제
        gridObject.targetGrid.RemoveObject(gridObject.positionOnGrid, gridObject);

        //노드에 도달할때마다 노드의 좌표를 불러와 캐릭터의 좌표를 노드 좌표로 업데이트
        gridObject.positionOnGrid.x = path[path.Count - 1].pos_x;
        gridObject.positionOnGrid.y = path[path.Count - 1].pos_y;
        //등록도 함
        gridObject.targetGrid.PlaceObject(gridObject.positionOnGrid, gridObject);
        //다음 좌표로 몸 틀기
        RotateCharacter(transform.position, pathWorldPositions[0]);
        //애니메이션도 재생
        characterAnimator.StartMoving();
    }


    private void RotateCharacter(Vector3 originPosition, Vector3 destinationPosition)
    {
        //고개 돌려봐
        //출발지점과 도착지점 빼고 노멀라이즈로 값을 1 아래로 내린다
        Vector3 direction = (destinationPosition - originPosition).normalized;
        //물론 하늘보지 말게 하자
        direction.y = 0;
        //돌려!
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Update()
    {
        //만약 등록된 경로가 없다 = 그럼 아무것도 안한다
        if(pathWorldPositions == null)
        {
            return;
        }
        if(pathWorldPositions.Count == 0)//나도
        {
            return;
        }
        //캐릭터를 실질적으로 움직이게하는 아이
        transform.position = Vector3.MoveTowards(transform.position, pathWorldPositions[0], moveSpeed * Time.deltaTime);
        //경로 노드에 다다름(체크포인트에 근접함)
        if(Vector3.Distance(transform.position, pathWorldPositions[0]) < 0.05f)
        {
            //체크포인트 패스 -- 다다른 노드 제거 후 다음 노드로 출발
            pathWorldPositions.RemoveAt(0);
            if(pathWorldPositions.Count == 0)//더이상 경로 노드가 없다
            {
                characterAnimator.StopMoving();//그럼 움직이지마
            }
            else//경로가 남았네? 그럼 그 경로로 방향 틀어봐바
            {
                RotateCharacter(transform.position, pathWorldPositions[0]);
            }
        }
    }

    public void SkipAnimation()
    {
        if(pathWorldPositions.Count < 2)//이미 도착해있네? 그럼 무시해
        {
            return;
        }
        //경로의 마지막 노드의 위치로 텔레포트!
        transform.position = pathWorldPositions[pathWorldPositions.Count - 1];
        //몸 방향좀 정하게 도착지점 한칸 전 노드를 불러와
        Vector3 originPosition = pathWorldPositions[pathWorldPositions.Count - 2];
        //경로의 마지막 노드 불러와
        Vector3 destinationPosition = pathWorldPositions[pathWorldPositions.Count - 1];
        //몸 틀어
        RotateCharacter(originPosition, destinationPosition);
        //받아온 경로 전부다 삭제해버려 필요없다
        pathWorldPositions.Clear();
        //멈춰!
        characterAnimator.StopMoving();
    }
}
