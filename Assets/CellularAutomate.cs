using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellularAutomate : MonoBehaviour
{
    int[] cells;
    int[] ruleSet;
    int width = 200;
    int currentGeneration = 0;
    List<GameObject> allGO = new List<GameObject>();
    List<SpriteRenderer> allSR = new List<SpriteRenderer>();

    public GameObject blackCell = null;
    public GameObject whiteCell = null;
    public int generationNumber = 20;

    private void Start()
    {
        cells = new int[width];
        ruleSet = new int[8];

        //normal pattern 
        for (int i = 0; i < cells.Length; i++) { cells[i] = 0; }
        cells[cells.Length / 2] = 1;

        StartCoroutine("Proceed");

        int w = 10; //cell size

        for (int g = 0; g < generationNumber; g++)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                GameObject spawn = null;
                if (cells[i] == 1) spawn = Instantiate(blackCell);
                else spawn = Instantiate(whiteCell);

                spawn.transform.position = new Vector3(i * w, g * -w);
                allGO.Add(spawn);
                allSR.Add(spawn.GetComponent<SpriteRenderer>());
            }
        }

    }

    IEnumerator Proceed()
    {
        int index = 30;

        while(index < 255)
        {
            Debug.Log("index:" + index);

            for (int i = 0; i < cells.Length; i++) { cells[i] = 0; }
            cells[cells.Length / 2] = 1;

            DefineNumber(index);

            Render();
            currentGeneration++;

            for (int i = 0; i < generationNumber; i++)
            {
                Generate();
            }

            yield return new WaitForSeconds(1.0f);
            index++;
            currentGeneration = 0;
        }
    }

    private void Generate()
    {
        int[] newcells = new int[cells.Length];

        // For left case
        int left = cells[cells.Length - 1];
        int middle = cells[0];
        int right = cells[1];
        int newState = Rules(left, middle, right);
        newcells[0] = newState;

        for (int i = 1; i < cells.Length - 1; i++)
        {
            left = cells[i - 1];
            middle = cells[i];
            right = cells[i + 1];
            newState = Rules(left, middle, right);
            newcells[i] = newState;
        }

        // For right case
        left = cells[cells.Length - 2];
        middle = cells[cells.Length - 1];
        right = cells[0];
        newState = Rules(left, middle, right);
        newcells[cells.Length - 1] = newState;

        cells = newcells;

        Render();

        currentGeneration++;
    }

    private void Render()
    {
        int index = currentGeneration * width;
        if (index == width * generationNumber) return;
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] == 1) allSR[index + i].color = Color.black;
            else allSR[index + i].color = Color.white;
        }
    }

    int Rules(int a, int b, int c)
    {
        string s = "" + a + b + c;

        int index = System.Convert.ToInt32(s, 2);

        return ruleSet[index];
    }

    void DefineNumber(int number)
    {
        int temp = number;
        int[] bin = new int[8];
        int index = 0;
        while(temp > 0)
        {
            bin[index] = temp % 2;
            temp /= 2;
            index++;
        }

        int[] final = new int[8];

        index = 0;
        for (int i = 7; i != 0; i--)
        {
            final[index] = bin[i];
            index++;
        }

        for (int i = 0; i < 8; i++)
        {
            ruleSet[i] = final[i];
        }
    }

    void ChooseRuleSet(int number)
    {
        if (number == 99)
        {
            ruleSet[0] = 0;
            ruleSet[1] = 1;
            ruleSet[2] = 0;
            ruleSet[3] = 1;
            ruleSet[4] = 1;
            ruleSet[5] = 0;
            ruleSet[6] = 1;
            ruleSet[7] = 0;
        }

        else if (number == 222)
        {
            ruleSet[0] = 0;
            ruleSet[1] = 1;
            ruleSet[2] = 1;
            ruleSet[3] = 1;
            ruleSet[4] = 1;
            ruleSet[5] = 0;
            ruleSet[6] = 1;
            ruleSet[7] = 1;
        }

        else if (number == 190)
        {
            ruleSet[0] = 0;
            ruleSet[1] = 1;
            ruleSet[2] = 1;
            ruleSet[3] = 1;
            ruleSet[4] = 1;
            ruleSet[5] = 1;
            ruleSet[6] = 0;
            ruleSet[7] = 1;
        }

        //0001 1110
        else if (number == 30)
        {
            ruleSet[0] = 0;
            ruleSet[1] = 1;
            ruleSet[2] = 1;
            ruleSet[3] = 1;
            ruleSet[4] = 1;
            ruleSet[5] = 0;
            ruleSet[6] = 0;
            ruleSet[7] = 0;
        }

        //0110 1110 
        else if (number == 110)
        {
            ruleSet[0] = 0;
            ruleSet[1] = 1;
            ruleSet[2] = 1;
            ruleSet[3] = 1;
            ruleSet[4] = 0;
            ruleSet[5] = 1;
            ruleSet[6] = 1;
            ruleSet[7] = 0;
        }
    }
}
