using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameDataManager : MonoBehaviour
{
    public GameObject EnterGameButton;
    public GameObject GamePanel;
    public GameObject GameMode;

    private Text GameModeText;
    void Start()
    {
        EnterGameButton.GetComponent<Button>().onClick.AddListener(OnEnterGameButton);
        GameMode.GetComponent<Button>().onClick.AddListener(OnGameModeClicked);
        GameModeText = GameMode.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnterGameButton()
	{
        GamePanel.SetActive(true);
	}

    public void OnGameModeClicked()
	{
        if(GameBoard.Instance.GameMode == GAME_MODE.EXTRACT_MODE)
		{
            GameBoard.Instance.GameMode = GAME_MODE.SCAN_MODE;
            GameModeText.text = "Extract Mode";
		}
        else
		{
            GameBoard.Instance.GameMode = GAME_MODE.EXTRACT_MODE;
            GameModeText.text = "Scan Mode";
        }
	}
}

public enum GAME_MODE
{
    EXTRACT_MODE,
    SCAN_MODE
}