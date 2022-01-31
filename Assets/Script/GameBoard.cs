// There's a Reveal Game Board in the game to see the whole board for two seconds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// singleton class
public class GameBoard : MonoBehaviour
{
    private static GameBoard instance;
    public static GameBoard Instance { get { return instance; } }

    private System.Random rand = new System.Random();
    // Game Obejct refernces
    public GameObject TilePrefab;
    public Text MessageText;
    // 2D array that is filled with tiles
    public GameObject[,] grid = new GameObject[32, 32];
    // list of tiles' coordinates that has resources
    public List<Vector2> filledTileList = new List<Vector2>();

    
    public Color MaxColor;
    public Color HalfColor;
    public Color QuarterColor;
    public Color defaultColor;

    // Maximum amount for resources is 200
    public int MaxResourceAmount;
    // the number of max that are placed randomly in the grid
    public int numberOfMaxResources;

    // Extract mode or scan mode
    public GAME_MODE GameMode;

    //Number of clicks per in scan mode
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

    //generating grid 
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

    //generating random tiles with max amount and 2 rings of tile around it
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
    //generating random coordinates for maximum tile and check if it and 2 rings 
    //around it have already been filled. 
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
    // Showing clicked tile and its 8 neighbour tiles in scan mode
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
    // Hiding tiles when the mode changes
    public void ChangeMode(bool show)
	{
        for (int r = 0; r < 32; r++)
        {
            for (int c = 0; c < 32; c++)
            {
                grid[r, c].GetComponent<TileScript>().ToggleTileActivation(show);
            }
        }
    }
    // shows all the board for two second
    public IEnumerator showGameBoard()
    {
        for (int r = 0; r < 32; r++)
        {
            for (int c = 0; c < 32; c++)
            {
                grid[r, c].GetComponent<TileScript>().ToggleTileActivation(true);
            }
        }
        yield return new WaitForSeconds(2.0f);
        for (int r = 0; r < 32; r++)
        {
            for (int c = 0; c < 32; c++)
            {
                grid[r, c].GetComponent<TileScript>().ToggleTileActivation(false);
            }
        }
    }
    // extract resources from clicked tiles and its 2 ring neighbours
    public void extractTiles(Vector2 vec)
	{
        if (extractNumbers > 0)
        {
            extractNumbers--;
            int r = (int)vec.x;
            int c = (int)vec.y;
            for (int i = r - 2; i < r + 3; i++)
            {
                for (int j = c - 2; j < c + 3; j++)
                {
                    if ((j <= 31 && j >= 0) && (i <= 31 && i >= 0))
                    {
                        if(i== r && j==c)
						{
                            resourcesAmount[(int)grid[i, j].GetComponent<TileScript>().resource] += grid[i, j].GetComponent<TileScript>().amount;
                            if (grid[i, j].GetComponent<TileScript>().isFilled)
                            {
                                GameBoard.Instance.MessageText.text = "Extracted " + grid[i, j].GetComponent<TileScript>().amount + " " + resourceNames[(int)grid[i, j].GetComponent<TileScript>().resource];

                                grid[i, j].GetComponent<TileScript>().fillTile(defaultColor, -1, 0);

                            }
                            else
							{
                                GameBoard.Instance.MessageText.text = "Extracted empty tile" ;

                            }

                        }
                       else if (grid[i, j].GetComponent<TileScript>().amount == MaxResourceAmount)
                        {
                            resourcesAmount[(int)grid[i, j].GetComponent<TileScript>().resource] += MaxResourceAmount / 2;
                            grid[i, j].GetComponent<TileScript>().fillTile(HalfColor, (int)grid[i, j].GetComponent<TileScript>().resource, MaxResourceAmount / 2);

                        }
                        else if (grid[i, j].GetComponent<TileScript>().amount == MaxResourceAmount / 2)
                        {
                            grid[i, j].GetComponent<TileScript>().fillTile(QuarterColor, (int)grid[i, j].GetComponent<TileScript>().resource, MaxResourceAmount / 4);
                            resourcesAmount[(int)grid[i, j].GetComponent<TileScript>().resource] += MaxResourceAmount / 4;

                        }
                        else if (grid[i, j].GetComponent<TileScript>().amount == MaxResourceAmount / 4)
                        {
                            resourcesAmount[(int)grid[i, j].GetComponent<TileScript>().resource] += MaxResourceAmount / 4;
                            grid[i, j].GetComponent<TileScript>().fillTile(defaultColor, -1, 0);
                        }
                        else
                        {
                            grid[i, j].GetComponent<TileScript>().fillTile(defaultColor, -1, 0);

                          
                        }

                    }

                }
            }

            for (int i = r - 2; i < r + 3; i++)
            {
                for (int j = c - 2; j < c + 3; j++)
                {
                    if ((j <= 31 && j >= 0) && (i <= 31 && i >= 0))
                        grid[i, j].GetComponent<TileScript>().ToggleTileActivation(true);
                }
            }

         //   grid[r, c].GetComponent<TileScript>().ToggleTileActivation(true);
           
        }
        else
		{
            GameBoard.Instance.MessageText.text = "Number of extraction is 0";

        }
    }
}
