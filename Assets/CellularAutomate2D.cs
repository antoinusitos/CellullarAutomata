using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellularAutomate2D : MonoBehaviour
{
    int[,] board;
    int rows = 100;
    int columns = 200;
    List<GameObject> allGO = new List<GameObject>();
    List<SpriteRenderer> allSR = new List<SpriteRenderer>();

    public GameObject blackCell = null;
    public GameObject whiteCell = null;

    private void Start()
    {
        board = new int[columns, rows];

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                board[x, y] = Random.Range(0, 2);
            }
        }

        int w = 10; //cell size

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject spawn = null;
                if (board[x, y] == 1) spawn = Instantiate(blackCell);
                else spawn = Instantiate(whiteCell);

                spawn.transform.position = new Vector3(x * w, y * -w);
                allGO.Add(spawn);
                allSR.Add(spawn.GetComponent<SpriteRenderer>());
            }
        }

        //Generate();
        StartCoroutine("Proceed");
    }

    IEnumerator Proceed()
    {
        int index = 0;
        int iteration = 50;

        while (index < iteration)
        {
            Generate();
            yield return new WaitForSeconds(0.5f);
            index++;
        }
    }

    private void Generate()
    {
        int[,] next = new int[columns, rows];

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                int neighbors = 0;

                for(int i = -1; i <= 1; i++)
                {
                    for(int j = -1; j <= 1; j++)
                    {
                        neighbors += board[x + i, y + j];
                    }
                }

                neighbors -= board[x, y];

                if ((board[x,y] == 1) && (neighbors < 2)) next[x,y] = 0;
                else if ((board[x,y] == 1) && (neighbors > 3)) next[x,y] = 0;
                else if ((board[x,y] == 0) && (neighbors == 3)) next[x,y] = 1;
                else next[x,y] = board[x,y];

            }
        }

        board = next;

        Render();
    }

    private void Render()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (board[x, y] == 1) allSR[x * rows + y].color = Color.black;
                else allSR[x * rows + y].color = Color.white;
            }
        }
    }
}