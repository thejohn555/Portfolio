using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class botonRampa : MonoBehaviour
{
    private bool boton;
    [SerializeField] private Material Encendido;
    [SerializeField] private Material Apagado;
    private Renderer Rend;
    [SerializeField] private GameObject Rampa;
    void Start()
    {
        boton = false;
        Rend = GetComponent<Renderer>();
        Rampa = GameObject.Find("Rampa");
    }
    void Update()
    {
        if (boton == true)
        {
            Rend.material = Encendido;
            Rampa.gameObject.SetActive(true);
        }
        if (boton == false)
        {
            Rend.material = Apagado;
            Rampa.gameObject.SetActive(false);
        }
    }
    public void Swithc()
    {
        boton = !boton;
    }
}
