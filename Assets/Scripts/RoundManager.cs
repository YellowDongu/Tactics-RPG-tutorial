using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //�̱��� ����
    //static - ���� �� �ش� Ŭ������ ó������ ���� �� �ѹ� �ʱ�ȭ, ��� ������ �޸𸮸� ���
    public static RoundManager instance;

    private void Awake()
    {
        //�ν��Ͻ��� �ڽ����� ����
        instance = this;
    }

    //�̱��� ���� ��

    List<CharacterTurn> characters;
    //���۽� 1��° ��
    int round = 1;
    //���� ǥ���� �ؽ�Ʈ(TMP) ����
    [SerializeField] TMPro.TextMeshProUGUI turnCountText;

    private void Start()
    {
        //�����ҋ� �ؽ�Ʈ ����
        UpdateTextOnScreen();
    }

    public void AddMe(CharacterTurn character)
    {
        //�� �ѱ� �� ����Ʈ�� �ִ� ĳ���͵��� �ൿ���� ��ȯ�ϱ� ���� �ɸ��͵� ����Ʈ�� �߰��ϰ� ���
        if (characters == null)
        {
            characters = new List<CharacterTurn>();
        }
        characters.Add(character);
    }

    public void NextRound()
    {
        //�� �ѱ��
        //�� ������ ���° ������ ����
        round += 1;
        //�ؽ�Ʈ ����(TMP TEXT�� ����)
        UpdateTextOnScreen();
        //�Ѹ� �ൿ�� ��ȯ -- ������ �߰��ߴ� ����Ʈ ���
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].GrantTurn();
        }
    }

    void UpdateTextOnScreen()
    {
        //�ؽ�Ʈ ǥ�� -- �� ǥ��
        turnCountText.text = "Turn : " + round.ToString();
    }
}
