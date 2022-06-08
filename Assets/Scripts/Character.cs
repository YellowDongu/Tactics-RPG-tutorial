using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string Name = "Nameless";
    //이동거리, A* 알고리즘을 이용하기 때문에 한칸 거리로 따지는거다.
    //한칸에 10포인트 사용한다(수평기준) 대각선은 15포인트
    //대각선을 고려한 움직임이지만 직관적이지 않다 -- 바꿔야 할 대상
    public float movementPoints = 50f;
    public int hp = 100;
    public int damage = 20;
    public int attackRange = 1;



}
