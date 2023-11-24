using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrulla : Vida
{
    public enum IAState
    {
        Idle,
        Patrol,
        Move,
        Attack,
        Return
    }
    private IAState estadoActual;
    private Vector3 distancia;
    private GameObject jugador;
    private NavMeshAgent navAgent;

    private GameObject pRuta;
    private GameObject[] psRutas;
    private int i;
    public float velocidad;
    public float rangoAtaque;
    public float rangoVision;
    public float tiempoEspera;
    private bool enemigo;
    private bool atacando;
    private bool Active;
    private float cadencia;
    private float tiempoAcumulado;

    // Start is called before the first frame update
    void Start()
    {
        estadoActual = IAState.Idle;
        tiempoEspera = 2f;
        i = 0;
        vida = 10;
        jugador = GameObject.FindGameObjectWithTag("Jugador");
        navAgent = GetComponent<NavMeshAgent>();
        pRuta = psRutas[i];
        enemigo = false;
        navAgent.speed = velocidad;
    }

    // Update is called once per frame
    void Update()
    {
        if (jugador != null)
        {
            Movimiento();
            Ataque();
            Cadencia();
        }
        switch (estadoActual)
        {
            case IAState.Idle:
                if (Active == false)
                {
                    StartCoroutine(Espera(tiempoEspera));
                }

                break;

            case IAState.Patrol:

                StopCoroutine("Espera");

                navAgent.SetDestination(psRutas[i].transform.position);

                if (Vector3.Distance(transform.position, psRutas[i].transform.position) < 2f)
                {
                    i++;
                    if (i >= psRutas.Length)
                    {
                        i = 0;
                    }
                }

                if (Vector3.Distance(transform.position, jugador.transform.position) < rangoVision)
                {
                    estadoActual = IAState.Move;
                }

                break;

            case IAState.Move:

                navAgent.speed = 3;
                navAgent.SetDestination(jugador.transform.position);

                if (Vector3.Distance(transform.position, jugador.transform.position) < rangoAtaque)
                {
                    estadoActual = IAState.Attack;
                }

                if (Vector3.Distance(transform.position, jugador.transform.position) > rangoVision)
                {
                    estadoActual = IAState.Return;
                }

                break;

            case IAState.Attack:





                if (Active == false)
                {

                    StartCoroutine("Cadencia");

                }

                if (Vector3.Distance(transform.position, jugador.transform.position) > rangoAtaque)
                {
                    navAgent.isStopped = false;
                    StopCoroutine("Cadencia");
                    Active = false;
                    estadoActual = IAState.Move;

                }

                break;

            case IAState.Return:

                navAgent.speed = 6;
                estadoActual = IAState.Idle;

                break;
        }
    }
    IEnumerator Espera(float Espera)
    {
        Active = true;

        navAgent.SetDestination(transform.position);

        yield return new WaitForSeconds(Espera);

        estadoActual = IAState.Patrol;
        Active = false;

    }
    private void Movimiento()
    {
        navAgent.SetDestination(pRuta.transform.position);

        if (enemigo == false)
        {
            pRuta = psRutas[i];
            if (Vector3.Distance(transform.position, pRuta.transform.position) < 2f)
            {
                i++;
                if (i >= psRutas.Length)
                {
                    i = 0;
                }
            }
        }
        else
        {
            pRuta = jugador;
        }
    }
    private void Ataque()
    {
        if (Vector3.Distance(transform.position, jugador.transform.position) < rangoVision)
        {
            distancia = jugador.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, distancia) < 180f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, distancia, out hit, rangoVision))
                {

                    if (hit.transform.tag == "Jugador")
                    {
                        navAgent.speed = 5;
                        enemigo = true;

                        if (atacando == false)
                        {
                            if (Vector3.Distance(transform.position, jugador.transform.position) < rangoAtaque)
                            {
                                if (Physics.Raycast(transform.position, transform.forward, out hit, 2))
                                {
                                    Debug.Log("ataco");
                                    hit.transform.gameObject.GetComponent<Vida>().Daño(3);
                                    atacando = true;
                                    cadencia = 1F;
                                    navAgent.isStopped = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        navAgent.speed = velocidad;

                        pRuta = psRutas[i];

                        enemigo = false;
                    }
                }
            }
        }
    }
    private void Cadencia()
    {
        if (atacando == true)
        {

            tiempoAcumulado += Time.deltaTime;

            if (tiempoAcumulado > cadencia)
            {

                atacando = false;

                tiempoAcumulado = 0;

                navAgent.isStopped = false;
            }
        }
    }
}
