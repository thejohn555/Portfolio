using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateNode : MonoBehaviour
{
    private int dimensions;
    int porcentajeFinal;
    public GameObject[] opciones;
    public List<cell> grid;
    public cell node;

    private int _repeticiones;
    public List<int> porcentajes = new List<int>();

    // Start is called before the first frame update
    void Awake()
    {
        _repeticiones = 0;

        dimensions = 10;
        grid = new List<cell>();
        GeneracionDeNodos();
    }

    private void GeneracionDeNodos()
    {
        for (float z = 0; z < dimensions; z++) 
        {
            for (float x = 0; x < dimensions; x++) 
            {
                
                cell newCell = Instantiate(node, new Vector3(x, 0, z), Quaternion.identity);
                newCell.GetComponent<cell>().Generado = false;
                grid.Add(newCell);

            }
        }
        StartCoroutine(RevisarOpciones());
    }
    IEnumerator RevisarOpciones()
    {
        List<cell> TempGrid = new List<cell>(grid);

        TempGrid.RemoveAll(c => c.Generado);

        TempGrid.Sort((a, b) => { return a.opcionesValidas.Length - b.opcionesValidas.Length; });

        int nOpciones = TempGrid[0].opcionesValidas.Length;
        int stopIndex = default;

        for (int i = 1; i < TempGrid.Count; i++)
        {
            if (TempGrid[i].opcionesValidas.Length > nOpciones)
            {
                stopIndex = i;
                break;
            }
        }

        if (stopIndex > 0)
        {

            TempGrid.RemoveRange(stopIndex, TempGrid.Count - stopIndex);

        }
        yield return new WaitForSeconds(0.01f);

        CollapseCell(TempGrid);
    }
    void CollapseCell(List<cell> tempGrid)
    {
        //tempGrid.Sort((a, b) => { return a.GetComponent<ValidsPrefabs>().porcentaje - b.GetComponent<ValidsPrefabs>().porcentaje; });
        porcentajes = new List<int>();
        porcentajeFinal = 0;
        //Debug.Log(tempGrid.Count);
        int randIndex = UnityEngine.Random.Range(0, tempGrid.Count);

        cell cellToCollapse = tempGrid[randIndex];
        cellToCollapse.Generado = true;
        if (cellToCollapse.GetComponent<cell>().opcionesValidas.Length == 0)
        {

        }
        else
        {
            for (int i = 0; i < cellToCollapse.opcionesValidas.Length; i++)
            {
                if (i == 0)
                {
                    porcentajes.Add(cellToCollapse.opcionesValidas[i].GetComponent<ValidsPrefabs>().porcentaje);
                }
                else
                {
                    porcentajes.Add(cellToCollapse.opcionesValidas[i].GetComponent<ValidsPrefabs>().porcentaje + porcentajes[i - 1]);
                }
                porcentajeFinal += cellToCollapse.opcionesValidas[i].GetComponent<ValidsPrefabs>().porcentaje;

            }

            randIndex = UnityEngine.Random.Range(0, porcentajeFinal);
            /*
            foreach (int i in porcentajes)
            {
                Debug.Log(i);
                Debug.Log(i+" "+porcentajes[i]+" "+randIndex);
                if (randIndex < porcentajes[i])
                {
                    Debug.Log(i);
                    randIndex = i;

                    break;
                }
            }
            */
            for (int i = 0; i <= porcentajes.Count; i++)
            {

                if (randIndex <= porcentajes[i])
                {

                    randIndex = i;

                    break;
                }
            }

            GameObject selectedTile = cellToCollapse.opcionesValidas[randIndex];
            cellToCollapse.opcionesValidas = new GameObject[] { selectedTile };

        }

        GameObject foundTile = cellToCollapse.opcionesValidas[0];
        Instantiate(foundTile, cellToCollapse.transform.position, Quaternion.identity);
        
        UpdateGeneration();
    }

    void UpdateGeneration()
    {
        List<cell> newGenerationCell = new List<cell>(grid);

        for (int z = 0; z < dimensions; z++)
        {
            for (int x = 0; x < dimensions; x++)
            {
                var index = x + z * dimensions;
                if (grid[index].Generado)
                {

                    newGenerationCell[index] = grid[index];
                
                }
                else
                {
                    List<GameObject> options = new List<GameObject>();
                    foreach (GameObject t in opciones)
                    {
                        options.Add(t);
                    }

                    //update above
                    if (z > 0)
                    {
                        cell up = grid[x + (z - 1) * dimensions];
                        List<GameObject> validOptions = new List<GameObject>();

                        foreach (GameObject possibleOptions in up.opcionesValidas)
                        {
                            var valOption = Array.FindIndex(opciones, obj => obj == possibleOptions);
                            var valid = opciones[valOption].GetComponent<ValidsPrefabs>().PosiblesArriba;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    //update right
                    if (x < dimensions - 1)
                    {
                        cell right = grid[x + 1 + z * dimensions];
                        List<GameObject> validOptions = new List<GameObject>();

                        foreach (GameObject possibleOptions in right.opcionesValidas)
                        {
                            var valOption = Array.FindIndex(opciones, obj => obj == possibleOptions);
                            var valid = opciones[valOption].GetComponent<ValidsPrefabs>().PosiblesIzquierda;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    //look down
                    if (z < dimensions - 1)
                    {
                        cell down = grid[x + (z + 1) * dimensions];
                        List<GameObject> validOptions = new List<GameObject>();

                        foreach (GameObject possibleOptions in down.opcionesValidas)
                        {
                            var valOption = Array.FindIndex(opciones, obj => obj == possibleOptions);
                            var valid = opciones[valOption].GetComponent<ValidsPrefabs>().PosiblesAbajo;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    //look left
                    if (x > 0)
                    {
                        cell left = grid[x - 1 + z * dimensions];
                        List<GameObject> validOptions = new List<GameObject>();

                        foreach (GameObject possibleOptions in left.opcionesValidas)
                        {
                            var valOption = Array.FindIndex(opciones, obj => obj == possibleOptions);
                            var valid = opciones[valOption].GetComponent<ValidsPrefabs>().PosiblesDerecha;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    GameObject[] newTileList = new GameObject[options.Count];

                    for (int i = 0; i < options.Count; i++)
                    {
                        newTileList[i] = options[i];
                    }

                    newGenerationCell[index].ReGenerarCelda(newTileList);
                }
            }
        }

        grid = newGenerationCell;
        _repeticiones++;

        if (_repeticiones < dimensions * dimensions)
        {
            StartCoroutine(RevisarOpciones());
        }

    }

    void CheckValidity(List<GameObject> optionList, List<GameObject> validOption)
    {
        for (int x = optionList.Count - 1; x >= 0; x--)
        {
            var element = optionList[x];
            if (!validOption.Contains(element))
            {
                optionList.RemoveAt(x);
            }
        }
    }
}
