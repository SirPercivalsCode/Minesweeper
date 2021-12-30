using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum states { Closed, Opened }

    public states state = states.Closed;

    public Vector2 pos;
    public int proxMinecount;



    public Color closedColor = new Color (0.5283019f, 0.5283019f, 0.5283019f, 1);
    public Color openColor = new Color (0.45f, 0.45f, 0.45f, 1);

    public GameObject numberGO;
    public GameObject bgGO;

    public GameMaster gm;

    void Start()
    {
        numberGO.SetActive(false);
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        bgGO.GetComponent<SpriteRenderer>().color = closedColor;
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

            numberGO.SetActive(true);

            bool lost = gm.CheckForMine(pos);
            if(lost)
            {
                numberGO.GetComponent<SpriteRenderer>().sprite = gm.mineSprite;
            }
            else
            {
                proxMinecount = gm.CalculateProxMines(pos);
                numberGO.GetComponent<SpriteRenderer>().sprite = gm.numberSprites[proxMinecount];
            }

            //Change Color
            bgGO.GetComponent<SpriteRenderer>().color = openColor;
            
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
