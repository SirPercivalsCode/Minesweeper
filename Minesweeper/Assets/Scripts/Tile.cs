using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum states { Closed, Opened }

    public states state = states.Closed;

    public Vector2 pos;
    public int proxMinecount;

    private bool flagged;

    public Color closedColor = new Color (0.7028302f, 0.7461675f, 1f, 1);

    public Color zeroColor = new Color (0.1556604f, 0.2993777f, 1f, 1);
    public Color oneColor = new Color (0.1933962f, 0.3306904f, 1f, 1);
    public Color colorDiff = new Color (0.0754717f, 0.0623989f, 0f, 0);
    

    public GameObject numberGO;
    public GameObject bgGO;

    public GameMaster gm;

    void Start()
    {
        numberGO.SetActive(false);
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        bgGO.GetComponent<SpriteRenderer>().color = closedColor;
    }

    public void OnMouseOver()
    {
        if(!gm.gameRunning || state == states.Opened)
        {
            return;
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(flagged)
            {
                flagged = false;
                numberGO.SetActive(false);
            }
            else
            {
                flagged = true;
                gm.AddFlagCount();
                numberGO.SetActive(true);
                numberGO.GetComponent<SpriteRenderer>().sprite = gm.flagSprite;
            }
        }
    }

    public void OnMouseUp()
    {
        OpenTile();
    }

    public void OpenTile()
    {
        if(!gm.gameRunning)
        {
            return;
        }

        if(state == states.Closed)
        {
            //Debug.Log("Tile Nr. " + pos.x + "|" + pos.y + " opened.");
            state = states.Opened;
            gm.AddClickCount();

            

            bool lost = gm.CheckForMine(pos);
            if(lost)
            {
                numberGO.SetActive(true);
                numberGO.GetComponent<SpriteRenderer>().sprite = gm.mineSprite;
            }
            else
            {
                proxMinecount = gm.CalculateProxMines(pos);
                if(proxMinecount == 0)
                {
                    bgGO.GetComponent<SpriteRenderer>().color = zeroColor;
                }
                else
                {
                    numberGO.SetActive(true);
                    numberGO.GetComponent<SpriteRenderer>().sprite = gm.numberSprites[proxMinecount];
                    bgGO.GetComponent<SpriteRenderer>().color = oneColor + (colorDiff * proxMinecount);
                }
            }

            //Change Color
            //bgGO.GetComponent<SpriteRenderer>().color = openColor;
            
            if(lost)
            {
                gm.GameOver(false);
            }
            else
            {
                if(proxMinecount == 0)
                {
                    //Check and Open nearby Tiles
                    //Still room for improvement

                    Tile temp = null;
                    if(pos.x - 1 >= 0)
                    {
                        temp = gm.SearchTile(new Vector2(pos.x-1, pos.y));
                        temp.OpenTile();
                    }
                    
                    if(pos.x + 1 <= gm.gridWidth - 1)
                    {
                        temp = gm.SearchTile(new Vector2(pos.x+1, pos.y));
                        temp.OpenTile();
                    }

                    if(pos.y - 1 >= 0)
                    {
                        temp = gm.SearchTile(new Vector2(pos.x, pos.y-1));
                        temp.OpenTile();
                    }
                    
                    if(pos.y + 1 <= gm.gridHeigth - 1)
                    {
                        temp = gm.SearchTile(new Vector2(pos.x, pos.y+1));
                        temp.OpenTile();
                    }

                    if(pos.x + 1 <= gm.gridWidth - 1)
                    {
                        if(pos.y + 1 <= gm.gridHeigth - 1)
                        {
                            temp = gm.SearchTile(new Vector2(pos.x+1, pos.y+1));
                            temp.OpenTile();
                        }

                        if(pos.y - 1 >= 0)
                        {
                            temp = gm.SearchTile(new Vector2(pos.x+1, pos.y-1));
                            temp.OpenTile();
                        }
                    }

                    if(pos.x - 1 >= 0)
                    {
                        if(pos.y + 1 <= gm.gridHeigth - 1)
                        {
                            temp = gm.SearchTile(new Vector2(pos.x-1, pos.y+1));
                            temp.OpenTile();
                        }

                        if(pos.y - 1 >= 0)
                        {
                            temp = gm.SearchTile(new Vector2(pos.x-1, pos.y-1));
                            temp.OpenTile();
                        }
                    }


                }
            }
        }
        else
        {
            //Tile already Opened
        }
    }
}
