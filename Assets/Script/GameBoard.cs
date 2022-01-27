using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameBoard : MonoBehaviour
{
    private static GameBoard instance;
    public static GameBoard Instance { get { return instance; } }
    private System.Random rand = new System.Random();
    public GameObject TilePrefab;
    public Text MessageText;

    public GameObject[,] grid = new GameObject[32, 32];
    public List<Vector2> filledTileList = new List<Vector2>();

    public Color MaxColor;
    public Color HalfColor;
    public Color QuarterColor;
    public Color defaultColor;

    public int MaxResourceAmount;
    public int numberOfMaxResources;

    public GAME_MODE GameMode;
    public int scanModeClickNumbers;
    public int MaxScanClickNumbers;
    public int extractNumbers;
    public int ScanNumbrs = 6;
    public int[] resourcesAmount=new int[12];
    public string[] resourceNames = new string[12];
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        GameMode = GAME_MODE.EXTRACT_MODE;
        for (int r = 0; r < 32; r++)
        {
            for (int c = 0; c < 32; c++)
            {
                grid[r, c] = Instantiate(TilePrefab, this.transform);
                grid[r, c].GetComponent<TileScript>().coordinate = new Vector2(r, c);
            }
        }

        SetupTiles();
    }

	private void Update()
	{
		
	}
	private void SetupTiles()
	{
        Vector2 vec = Vector2.zero;
        int image = 0;
        for (int b = 0; b < numberOfMaxResources; b++)
        {
            Vector2 tileIndex = FindAvailableTile(vec);
      
            if (tileIndex.x!=-1)
		    {
              
                int r = (int)tileIndex.x;
                int c = (int)tileIndex.y;
                Debug.Log( r + "," + c);
                for (int i = r - 2; i < r + 3; i++)
                {
                    for (int j = c - 2; j < c + 3; j++)
                    {
                         if(i==r && j==c)
						{
                            grid[i, j].GetComponent<TileScript>().fillTile(MaxColor,image,MaxResourceAmount);
						}
                         else if((i<= r+1 && i>=r-1) && (j <= c + 1 && j >= c - 1))
                        {
                            grid[i, j].GetComponent<TileScript>().fillTile(HalfColor, image, MaxResourceAmount/2);
                        }
                         else
						{
                            grid[i, j].GetComponent<TileScript>().fillTile(QuarterColor,image, MaxResourceAmount / 4);
                        }
                        filledTileList.Add( new Vector2(i,j)); 
                    }
                }
                image++;
                if (image >= 12)
                    image = 0;
            }
        }
	}

    private Vector2 FindAvailableTile(Vector2 vec)
	{
        int r =0, c=0;
        bool isfilled = true;

        while (isfilled)
        {

            r = rand.Next(2, 29);
            c = rand.Next(2, 29);
            isfilled = false;
            for (int i = r - 2; i < r + 3; i++)
            {
                for (int j = c - 2; j < c + 3; j++)
                {
                    if (filledTileList.Contains(new Vector2(i, j)))
                    {
                        isfilled = true;
                        break;

                    }

                }
            }
        }
        if(isfilled)
           return new Vector2(-1, -1);
        else
          return new Vector2(r, c);
      
    }

    public void ShowTilesScanMode(Vector2 vec)
	{
       
        if (scanModeClickNumbers > 0)
        {
            scanModeClickNumbers--;
            int r = (int)vec.x;
            int c = (int)vec.y;
            for (int i = r - 1; i < r + 2; i++)
            {
                for (int j = c - 1; j < c + 2; j++)
                {
                    if ((j <= 31 && j >= 0) && (i <= 31 && i >= 0))
                        grid[i, j].GetComponent<TileScript>().ToggleTileActivation(true);
                }
            }
        }
        else
        {
            GameBoard.Instance.MessageText.text = "Number of Scan clicks is 0";

        }
    }

    public void ChangeMode()
	{
        for (int r = 0; r < 32; r++)
        {
            for (int c = 0; c < 32; c++)
            {
                grid[r, c].GetComponent<TileScript>().ToggleTileActivation(false);
            }
        }
    }

    public void extractTiles(Vector2 vec)
	{
        if (extractNumbers > 0)
        {
            extractNumbers--;
            int r = (int)vec.x;
            int c = (int)vec.y;
            for (int i = r - 2; i < r + 2; i++)
            {
                for (int j = c - 2; j < c + 2; j++)
                {
                    if ((j <= 31 && j >= 0) && (i <= 31 && i >= 0))
                    {

                        if (grid[i, j].GetComponent<TileScript>().amount == MaxResourceAmount)
                        {
                            resourcesAmount[(int)grid[i, j].GetComponent<TileScript>().resource] += MaxResourceAmount / 2;
                            grid[i, j].GetComponent<TileScript>().fillTile(HalfColor, (int)grid[i, j].GetComponent<TileScript>().resource, MaxResourceAmount / 2);
                            GameBoard.Instance.MessageText.text = "Extracted " + MaxResourceAmount / 2 + " " + resourceNames[(int)grid[i, j].GetComponent<TileScript>().resource];

                        }
                        else if (grid[i, j].GetComponent<TileScript>().amount == MaxResourceAmount / 2)
                        {
                            grid[i, j].GetComponent<TileScript>().fillTile(QuarterColor, (int)grid[i, j].GetComponent<TileScript>().resource, MaxResourceAmount / 4);
                            resourcesAmount[(int)grid[i, j].GetComponent<TileScript>().resource] += MaxResourceAmount / 4;
                            GameBoard.Instance.MessageText.text = "Extracted " + MaxResourceAmount / 4 + " " + resourceNames[(int)grid[i, j].GetComponent<TileScript>().resource];

                        }
                        else if (grid[i, j].GetComponent<TileScript>().amount == MaxResourceAmount / 4)
                        {
                            resourcesAmount[(int)grid[i, j].GetComponent<TileScript>().resource] += MaxResourceAmount / 4;
                            grid[i, j].GetComponent<TileScript>().fillTile(defaultColor, -1, 0);
                            GameBoard.Instance.MessageText.text = "Extracted " + MaxResourceAmount / 4 + " " + resourceNames[(int)grid[i, j].GetComponent<TileScript>().resource];
                        }
                        else
                        {
                            grid[i, j].GetComponent<TileScript>().fillTile(defaultColor, -1, 0);
                            GameBoard.Instance.MessageText.text = "Extracted empty tile";
                        }

                    }

                }
            }
            for (int i = r - 2; i < r + 2; i++)
            {
                for (int j = c - 2; j < c + 2; j++)
                {
                    if ((j <= 31 && j >= 0) && (i <= 31 && i >= 0))
                        grid[i, j].GetComponent<TileScript>().ToggleTileActivation(true);
                }
            }
        }
        else
		{
            GameBoard.Instance.MessageText.text = "Number of extraction is 0";

        }
    }
}
