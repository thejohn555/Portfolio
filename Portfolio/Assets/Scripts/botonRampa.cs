using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botonRampa : MonoBehaviour
{
    private bool boton;
    [SerializeField] private Material Encendido;
    [SerializeField] private Material Apagado;
    private Renderer Rend;
    private GameObject Rampa;
    void Start()
    {
        boton = false;
        Rend = GetComponent<Renderer>();
    }
    void Update()
    {
        if (boton == true)
        {
            Rend.material = Encendido;
        }
        if (boton == false)
        {
            Rend.material = Apagado;
        }
    }
    public void Swithc()
    {
        boton = !boton;
    }
}
