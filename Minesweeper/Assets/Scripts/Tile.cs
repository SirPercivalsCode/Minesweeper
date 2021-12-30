using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum states { Closed, Opened }

    public states state = states.Closed;

    public Vector2 pos;
    public int proxMinecount;

    public GameMaster gm;

    void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    public void OnMouseUp()
    {
        if(state == states.Closed)
        {
            Debug.Log("Tile Nr. " + pos.x + "|" + pos.y + " opened.");
            state = states.Opened;
            gm.OpenTile(pos);
            proxMinecount = gm.CalculateProxMines(pos);
            
        }

    }
}
