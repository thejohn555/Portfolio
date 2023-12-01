using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Torreta : Vida
{
    GameObject jugador;
    float tiempoAcumulado;
    float cadencia;
    Vector3 direccion;
    Vector3 distancia;
    Quaternion rotacion;
    int giro;
    int rangoVision;
    int rangoAtaque;
    bool atacando;
    public GameObject bala;
    public GameObject salidabala;
    GameObject bala1;
    // Start is called before the first frame update
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");
        tiempoAcumulado = 0;
        cadencia = 0.5f;
        giro = 8;
        rangoVision = 15;
        rangoAtaque = 10;
        atacando = false;
        vida = 10;
        vidamax = vida;
    }

    // Update is called once per frame
    void Update()
    {
        if (jugador != null)
        {
            Mira();
            Ataque();
        }

    }
    private void Mira()
    {

        direccion = jugador.transform.position - transform.GetChild(0).position;

        if (Vector3.Distance(transform.position, jugador.transform.position) < rangoVision)
        {
            distancia = jugador.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, distancia) < 45)
            {
                
                        rotacion = Quaternion.LookRotation(direccion.normalized, Vector3.up);
                        transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, rotacion, giro * Time.deltaTime);
                        direccion.y = 0;
                        rotacion = Quaternion.LookRotation(direccion.normalized, Vector3.up);
                        transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, rotacion, giro * Time.deltaTime);
                
                
            }
        }
    }
    private void Ataque()
    {
        if (Vector3.Distance(transform.position, jugador.transform.position) < rangoAtaque && atacando == false)
        {
            distancia = jugador.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, distancia) < 45)
            {
                if (Physics.Raycast(transform.position, direccion, out RaycastHit hit, rangoVision))
                {
                    if (hit.transform.CompareTag("Jugador"))
                    {
                        bala1 = GameObject.Instantiate(bala, salidabala.transform.position, salidabala.transform.rotation);
                        bala1.gameObject.GetComponent<Bala>().velocidad = 20;
                        bala1.gameObject.GetComponent<Bala>().daño = 1;
                        atacando = true;
                        cadencia = 1;

                    }
                }
            }
        }
        if (atacando == true)
        {

            tiempoAcumulado += Time.deltaTime;

            if (tiempoAcumulado > cadencia)
            {
                atacando = false;

                tiempoAcumulado = 0;
            }
        }
    }
}
