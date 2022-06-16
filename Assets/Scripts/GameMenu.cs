using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    SelectCharacter selectCharacter;

    // Start is called before the first frame update
    private void Awake()
    {
        selectCharacter = GetComponent<SelectCharacter>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(selectCharacter.enabled == false)
        {
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            panel.SetActive(true);
        }
    }
}
