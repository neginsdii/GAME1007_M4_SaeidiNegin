using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameDataManager : MonoBehaviour
{
    public GameObject EnterGameButton;
    public GameObject GamePanel;
    public GameObject GameMode;
    public Text GameStatsText;
    public Text MessageText;
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
        updateTextUI();
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
            GameBoard.Instance.ScanNumbrs--;

        }
        else
		{
            GameBoard.Instance.GameMode = GAME_MODE.EXTRACT_MODE;
            GameModeText.text = "Scan Mode";
        }
	}

    private void updateTextUI()
	{
        GameStatsText.text = "Extract Numbers: " + GameBoard.Instance.extractNumbers + "\n Scan Numbers: " + GameBoard.Instance.ScanNumbrs +
                        "\n Scan Mode Click Numbers: " + GameBoard.Instance.scanModeClickNumbers;
	}
}

public enum GAME_MODE
{
    EXTRACT_MODE,
    SCAN_MODE
}

public enum RESOURCES
{
    APPLE,
    GEM,
    HEALING_POTION,
    MAGIC_POTION,
    COINS,
    ARMOR,
    SHIELD,
    MEAT,
    INGOTS,
    SWORD,
    HELMETS,
    AXE

}