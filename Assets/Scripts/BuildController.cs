using UnityEngine;
using UnityEngine.UI;

public class BuildController : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;
    private AllTiles allTiles;
    private PlaceController _placeController;
    
    public Category actualCategory;
    public enum Category { Blocks, Back, Enemies}

    void Start()
    {
        allTiles = GetComponent<AllTiles>();
        _placeController = GetComponent<PlaceController>();
        
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Image>().sprite = allTiles.tiles[i].sprite;
        }
    }

    public void SelectElement(Button button)
    {
        var buttonImg = button.GetComponent<Image>().sprite;
        if (actualCategory == Category.Blocks)
        {
            for (int i = 0; i < allTiles.tiles.Length; i++)
            {
                if (buttonImg == allTiles.tiles[i].sprite)
                {
                    _placeController.tile = allTiles.tiles[i];
                    break;
                }
            }
        }
        else if (actualCategory == Category.Back)
        {
            for (int i = 0; i < allTiles.back.Length; i++)
            {
                if (buttonImg == allTiles.back[i].sprite)
                {
                    _placeController.tile = allTiles.back[i];
                    break;
                }
            }
        }
        else if (actualCategory == Category.Enemies)
        {
            for (int i = 0; i < allTiles.enemies.Length; i++)
            {
                if (buttonImg == allTiles.enemiesPlaceholders[i].GetComponent<SpriteRenderer>().sprite)
                {
                    _placeController.enemy = allTiles.enemiesPlaceholders[i];
                    break;
                }
            }
        }
    }

    public void ChangeCategory(Button button)
    {
        CheckCategory(button);
        if (actualCategory == Category.Blocks || actualCategory == Category.Back)
        {
            _placeController.actualMode = PlaceController.Mode.Place;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (actualCategory == Category.Blocks)
                    buttons[i].GetComponent<Image>().sprite = allTiles.tiles[i].sprite;
                else if (actualCategory == Category.Back)
                    buttons[i].GetComponent<Image>().sprite = allTiles.back[i].sprite;
            }
        }

        if (actualCategory == Category.Enemies)
        {
            _placeController.actualMode = PlaceController.Mode.Enemy;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Image>().sprite = allTiles.enemies[i].GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    private void CheckCategory(Button button)
    {
        if (button.name == "Blocks")
        {
            actualCategory = Category.Blocks;
        }
        else if (button.name == "Back")
        {
            actualCategory = Category.Back;
        }
        else if(button.name == "Enemies")
        {
            actualCategory = Category.Enemies;
        }
    }

    public void ChangeElementUp()
    {
        if (actualCategory == Category.Blocks)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].GetComponent<Image>().sprite == allTiles.tiles[allTiles.tiles.Length - 1].sprite)
                {
                    buttons[i].GetComponent<Image>().sprite = allTiles.tiles[0].sprite;
                }
                else
                {
                    for (int j = 0; j < allTiles.tiles.Length; j++)
                    {
                        if (buttons[i].GetComponent<Image>().sprite == allTiles.tiles[j].sprite)
                        {
                            buttons[i].GetComponent<Image>().sprite = allTiles.tiles[j + 1].sprite;
                            break;
                        }
                    }
                }
            }
        }
        
        else if (actualCategory == Category.Back)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].GetComponent<Image>().sprite == allTiles.back[allTiles.back.Length - 1].sprite)
                {
                    buttons[i].GetComponent<Image>().sprite = allTiles.back[0].sprite;
                }
                else
                {
                    for (int j = 0; j < allTiles.back.Length; j++)
                    {
                        if (buttons[i].GetComponent<Image>().sprite == allTiles.back[j].sprite)
                        {
                            buttons[i].GetComponent<Image>().sprite = allTiles.back[j + 1].sprite;
                            break;
                        }
                    }
                }
            }
        }

        else if(actualCategory == Category.Enemies)
        {   
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].GetComponent<Image>().sprite == allTiles.enemies[allTiles.enemies.Length - 1].GetComponent<SpriteRenderer>().sprite)
                {
                    buttons[i].GetComponent<Image>().sprite = allTiles.enemies[0].GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    for (int j = 0; j < allTiles.enemies.Length; j++)
                    {
                        if (buttons[i].GetComponent<Image>().sprite == allTiles.enemies[j].GetComponent<SpriteRenderer>().sprite)
                        {
                            buttons[i].GetComponent<Image>().sprite = allTiles.enemies[j + 1].GetComponent<SpriteRenderer>().sprite;
                            break;
                        }
                    }
                }
            }
        }
        
    }

    public void ChangeElementDown()
    {
        if (actualCategory == Category.Blocks)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].GetComponent<Image>().sprite == allTiles.tiles[0].sprite)
                {
                    buttons[i].GetComponent<Image>().sprite = allTiles.tiles[allTiles.tiles.Length - 1].sprite;
                }
                else
                {
                    for (int j = 0; j < allTiles.tiles.Length; j++)
                    {
                        if (buttons[i].GetComponent<Image>().sprite == allTiles.tiles[j].sprite)
                        {
                            buttons[i].GetComponent<Image>().sprite = allTiles.tiles[j - 1].sprite;
                            break;
                        }
                    }
                }
            }
        }
        
        if (actualCategory == Category.Back)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].GetComponent<Image>().sprite == allTiles.back[0].sprite)
                {
                    buttons[i].GetComponent<Image>().sprite = allTiles.back[allTiles.back.Length - 1].sprite;
                }
                else
                {
                    for (int j = 0; j < allTiles.back.Length; j++)
                    {
                        if (buttons[i].GetComponent<Image>().sprite == allTiles.back[j].sprite)
                        {
                            buttons[i].GetComponent<Image>().sprite = allTiles.back[j - 1].sprite;
                            break;
                        }
                    }
                }
            }
        }
        
        if (actualCategory == Category.Enemies)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].GetComponent<Image>().sprite == allTiles.enemies[0].GetComponent<SpriteRenderer>().sprite)
                {
                    buttons[i].GetComponent<Image>().sprite = allTiles.enemies[allTiles.enemies.Length - 1].GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    for (int j = 0; j < allTiles.enemies.Length; j++)
                    {
                        if (buttons[i].GetComponent<Image>().sprite == allTiles.enemies[j].GetComponent<SpriteRenderer>().sprite)
                        {
                            buttons[i].GetComponent<Image>().sprite = allTiles.enemies[j - 1].GetComponent<SpriteRenderer>().sprite;
                            break;
                        }
                    }
                }
            }
        }
    }
}
