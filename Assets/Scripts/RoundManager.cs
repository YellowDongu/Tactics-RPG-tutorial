using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //싱글턴 패턴
    //static - 실행 후 해당 클래스가 처음으로 사용될 때 한번 초기화, 계속 동일한 메모리를 사용
    public static RoundManager instance;

    private void Awake()
    {
        //인스턴스를 자신으로 지정
        instance = this;
    }

    //싱글턴 패턴 끝

    List<CharacterTurn> characters;
    //시작시 1번째 턴
    int round = 1;
    //턴을 표시할 텍스트(TMP) 지정
    [SerializeField] TMPro.TextMeshProUGUI turnCountText;

    private void Start()
    {
        //시작할떄 텍스트 수정
        UpdateTextOnScreen();
    }

    public void AddMe(CharacterTurn character)
    {
        //턴 넘길 시 리스트에 있는 캐릭터들의 행동력을 반환하기 위해 케릭터들 리스트에 추가하고 사용
        if (characters == null)
        {
            characters = new List<CharacterTurn>();
        }
        characters.Add(character);
    }

    public void NextRound()
    {
        //턴 넘기기
        //이 변수로 몇번째 턴인지 저장
        round += 1;
        //텍스트 수정(TMP TEXT도 포함)
        UpdateTextOnScreen();
        //한명씩 행동력 반환 -- 위에서 추가했던 리스트 사용
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].GrantTurn();
        }
    }

    void UpdateTextOnScreen()
    {
        //텍스트 표시 -- 턴 표시
        turnCountText.text = "Turn : " + round.ToString();
    }
}
