using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameDataManager : MonoBehaviour
{
    public GameObject EnterGameButton;
    public GameObject ShowButton;
    public GameObject GamePanel;
    public GameObject GameMode;
    public Text GameStatsText;
    public GameObject ExitButton;
    public GameObject exitPanel;
    public Text ResourceCounterText;
    private Text GameModeText;
    void Start()
    {
        EnterGameButton.GetComponent<Button>().onClick.AddListener(OnEnterGameButton);
        GameMode.GetComponent<Button>().onClick.AddListener(OnGameModeClicked);
        ShowButton.GetComponent<Button>().onClick.AddListener(OnShowGameBoardButtonClicked);
        ExitButton.GetComponent<Button>().onClick.AddListener(OnExitButtonClicked);
        GameModeText = GameMode.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        updateTextUI();
        if (GameBoard.Instance.extractNumbers <= 0)
        {
            exitPanel.SetActive(true);
        }
    }

    public void OnEnterGameButton()
	{
        GamePanel.SetActive(false);
       

    }

    public void OnGameModeClicked()
	{
            GameBoard.Instance.ChangeMode(false);
        if(GameBoard.Instance.GameMode == GAME_MODE.EXTRACT_MODE)
		{
            if (GameBoard.Instance.ScanNumbrs > 0)
            {

                GameBoard.Instance.MessageText.text = "You're in scan mode";
                GameBoard.Instance.GameMode = GAME_MODE.SCAN_MODE;
                GameModeText.text = "Extract Mode";

            }
            else
			{
                GameBoard.Instance.MessageText.text = "Number of Scans is 0";
            }
        }
        else
        {
            GameBoard.Instance.MessageText.text = "You're in extract mode";
            GameBoard.Instance.GameMode = GAME_MODE.EXTRACT_MODE;
            GameModeText.text = "Scan Mode";
        }
	}

    public void OnShowGameBoardButtonClicked()
	{
      StartCoroutine(  GameBoard.Instance.showGameBoard());
    }
    public void OnExitButtonClicked()
	{
        GamePanel.SetActive(true);
        Debug.Log("exit");
	}
    private void updateTextUI()
	{
        GameStatsText.text = "Extract Numbers: " + GameBoard.Instance.extractNumbers + "\n Scan Numbers: "+ GameBoard.Instance.scanModeClickNumbers;
        ResourceCounterText.text = "";
        for (int i = 0; i < 12; i++)
        {
            ResourceCounterText.text += GameBoard.Instance.resourceNames[i] + ": \t" + GameBoard.Instance.resourcesAmount[i]+"\n";

        }

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