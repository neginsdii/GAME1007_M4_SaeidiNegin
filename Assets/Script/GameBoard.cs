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

    public GameObject[,] grid = new GameObject[32, 32];
    public List<Vector2> filledTileList = new List<Vector2>();

    public Color MaxColor;
    public Color HalfColor;
    public Color QuarterColor;

    public int MaxResourceAmount;
    public int numberOfMaxResources;

    public GAME_MODE GameMode;
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
        int r = (int)vec.x;
        int c = (int)vec.y;
        for (int i = r - 1; i < r + 2; i++)
        {
            for (int j = c - 1; j < c + 2; j++)
            {
                if((j<=31 && j>=0) && (i <= 31 && i >= 0))
                grid[i, j].GetComponent<TileScript>().ToggleTileActivation(true);
            }
        }
    }
}
