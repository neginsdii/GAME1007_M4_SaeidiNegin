using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TileScript : MonoBehaviour, IPointerDownHandler
{
    public Image icon;
    public Image CoverImage;
   
    public Vector2 coordinate;
    public Image tileImage;
    public Color tileColor;
    public Sprite[] IconImages;
    public int amount;
    public RESOURCES resource;
    public bool isFilled = false;
    void Start()
    {
		icon.gameObject.SetActive(false);

		CoverImage.gameObject.SetActive(true);
	}

    void Update()
    {
        
    }

    public void fillTile(Color color,int image , int amt)
	{
        amount = amt;
        tileColor = color;
        tileImage.color = tileColor;

        isFilled = true;
        if (image >= 0)
        {
            resource = (RESOURCES)image;
            icon.sprite = IconImages[image];
        }
        else
		{
            icon.sprite = null;
		}
    }

    public void OnPointerDown(PointerEventData eventData)
	{
        if (GameBoard.Instance.GameMode == GAME_MODE.SCAN_MODE)
        {
            
            GameBoard.Instance.ShowTilesScanMode(coordinate);
        }
        else
		{
            GameBoard.Instance.extractTiles(coordinate);
        }
	}

    public void ToggleTileActivation(bool toggle)
	{
        icon.gameObject.SetActive(toggle);
        CoverImage.gameObject.SetActive(!toggle);

    }
}
