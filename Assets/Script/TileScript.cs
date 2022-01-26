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
    public int amount;
    public bool isFilled = false;
    public Sprite[] IconImages;
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
        icon.sprite = IconImages[image];
        isFilled = true;
        tileImage.color = tileColor;


    }

    public void OnPointerDown(PointerEventData eventData)
	{
        if (GameBoard.Instance.GameMode == GAME_MODE.SCAN_MODE)
        {
            
            GameBoard.Instance.ShowTilesScanMode(coordinate);
        }
	}

    public void ToggleTileActivation(bool toggle)
	{
        icon.gameObject.SetActive(toggle);
        CoverImage.gameObject.SetActive(!toggle);

    }
}
