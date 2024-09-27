using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public Text[] menuText;
    private int seleckedIndex = 0; //���õ� ��ư �ε���

    private void Start()
    {
        HighlightButton(seleckedIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveUP();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveDown();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (seleckedIndex == 0)
            {
                StartNewGame();
            }
            else if (seleckedIndex == 1)
            {
                LoadGame();
            }
        }
    }
    private void MoveUP()
    {
        seleckedIndex = (seleckedIndex - 1 + menuText.Length) % menuText.Length;
        HighlightButton(seleckedIndex);
    }

    private void MoveDown()
    { 
    seleckedIndex = (seleckedIndex + 1 + menuText.Length) % menuText.Length;
        HighlightButton(seleckedIndex);
    }
    private void HighlightButton(int index)
    {
        // ���� �⺻ ����
        foreach (Text text in menuText)
        {

            text.color = Color.white;
        }

        //���� ��ư ����
        //menuText[index].color = Color.yellow;
        menuText[index].color = new Color(1.0f, 0.5f, 0.0f);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("World 1");   
    }

    public void LoadGame()
    {
        SaveSystem.Instance.LoadGame();
        StateManager.instance.LoadBoxData();
        InventoryManager.instance.LoadInventory();
    }
}
