using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public int minGridDiameter;
    public int maxGridDiameter;

    public int minecount;
    public int gridWidth;
    public int gridHeigth;

    public Vector2[] mines;


    public Vector2 offset;

    public GameObject gridGO;
    public GameObject gridBG;
    public Transform tileParent;
    public GameObject tilePrefab;
    public Transform mineParent;
    public GameObject minePrefab;

    public GameObject userInput;
    public Text inputCount, inputWidth, inputHeight;

    public void SetGrid()
    {
        int.TryParse(inputCount.text, out minecount);
        int.TryParse(inputWidth.text, out gridWidth);
        int.TryParse(inputHeight.text, out gridHeigth);


        if (gridHeigth > maxGridDiameter)
        {
            gridHeigth = maxGridDiameter;
        }
        else if(gridHeigth < minGridDiameter)
        {
            gridHeigth = minGridDiameter;
        }


        if (gridWidth > maxGridDiameter)
        {
            gridWidth = maxGridDiameter;
        }
        else if (gridWidth < minGridDiameter)
        {
            gridWidth = minGridDiameter;
        }


        if (minecount < 1)
        {
            minecount = 1;
        }
        else if (minecount < gridHeigth * gridWidth / 20)
        {
            minecount = (int)gridHeigth * gridWidth / 20;
        }
        else if (minecount > gridHeigth * gridWidth / 5)
        {
            minecount = (int)gridHeigth * gridWidth / 5;
        }

        SetMines();
        CreateGrid();
    }

    void Start()
    {
        userInput.SetActive(true);
        gridGO.SetActive(false);
    }

    public void CreateGrid()
    {
        //Change UI
        userInput.SetActive(false);
        gridGO.SetActive(true);

        
        //Calculate Positional Offset
        offset = new Vector2(-gridWidth/2, -gridHeigth/2);
        if (gridWidth % 2 == 0f)
        {
            offset.x += 0.5f;
        }
        if (gridHeigth % 2 == 0f)
        {
            offset.y += 0.5f;
        }

        gridGO.transform.position = gridGO.transform.position + new Vector3(offset.x, offset.y, 0f);
        gridBG.transform.position = gridGO.transform.position - new Vector3(offset.x, offset.y, 0f);
        gridBG.transform.localScale = new Vector3(gridWidth * 1.05f, gridHeigth * 1.05f, 1);


        //Calculate Position
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeigth; j++)
            {
                Vector2 pos = new Vector2(i, j) + offset;
                Tile tile = Instantiate(tilePrefab, pos, Quaternion.identity, tileParent).GetComponent<Tile>();
                tile.pos = new Vector2(i, j);
                //tile.proxMinecount = CalculateProxMines(new Vector2(i, j));
            }
        }
    }

    public void SetMines()
    {
        bool correct = true;

        mines = new Vector2[minecount];

        for (int i = 0; i < minecount; i++)
        {
            do
            {
                correct = true;
                mines[i] = new Vector2(Random.Range(0, gridWidth), Random.Range(0, gridHeigth));

                for (int j = 0; j < i; j++)
                {
                    if (mines[i] == mines[j])
                    {
                        correct = false;
                    }
                }
            }
            while (!correct);

            //Debug.Log("Mine Nr." + (i+1) + " at " + mines[i].x + ", " + mines[i].y);

            Vector2 pos = new Vector2(mines[i].x + offset.x, mines[i].y + offset.y);
            Instantiate(minePrefab, pos, Quaternion.identity, mineParent);
        }
    }

    public void OpenTile(Vector2 position)
    {
        for(int i = 0; i < minecount; i++)
        {
            if(mines[i] == position)
            {
                ClearedTile(true);
                return;
            }
        }

        ClearedTile(false);
    }

    public void ClearedTile(bool mine)
    {
        if (mine)
        {
            //Game Over
            
        }
        else
        {
            //Show Tile Number

        }
    }

    public int CalculateProxMines(Vector2 pos)
    {
        int count = 0;


        for(int i = 0; i < minecount; i++)
        {
            Vector2 relative = pos - mines[i];
            Debug.Log("relative position to Mine " + i + " is " + relative);

            if(relative.x < 2 && relative.y < 2 && relative.x > -2 && relative.y > -2)
            {
                count++;
            }
        }

        return count;
    }
}
