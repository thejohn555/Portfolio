using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell : MonoBehaviour
{

    public bool Generado;
    public GameObject[] opcionesValidas;

    public void GenerarCelda(bool EstadoCelda,GameObject[] Opciones)
    {
        Generado = EstadoCelda;
        opcionesValidas = Opciones;
    }
    public void ReGenerarCelda(GameObject[] Opciones)
    {

        opcionesValidas = Opciones;

    }
}
