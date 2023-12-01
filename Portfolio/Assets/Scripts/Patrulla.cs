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
    public IAState estadoActual;
    private Vector3 distancia;
    private GameObject jugador;
    private NavMeshAgent navAgent;

    private GameObject pRuta;
    [SerializeField] private GameObject[] psRutas;
    private int i;
    public float velocidad;
    public float rangoAtaque;
    public float rangoVision;
    public float tiempoEspera;
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
        vidamax = vida;
        jugador = GameObject.FindGameObjectWithTag("Jugador");
        navAgent = GetComponent<NavMeshAgent>();
        pRuta = psRutas[i];
        navAgent.speed = velocidad;

    }

    // Update is called once per frame
    void Update()
    {
        Mira();
        if (jugador != null)
        {

        
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
                        distancia = jugador.transform.position - transform.position;
                        if (Vector3.Angle(transform.forward, distancia) < 45)
                        {
                            if(Physics.Raycast(transform.position,distancia,out RaycastHit hit , rangoVision))
                            {
                                if (hit.transform.CompareTag("Jugador"))
                                {
                                    estadoActual = IAState.Move;
                                }
                            }
                        }
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
    }
    void Mira()
    {

        BarraVida.transform.parent.LookAt(jugador.transform.position);
        
    }
    
    private void Ataque()
    {
        if (Vector3.Distance(transform.position, jugador.transform.position) < rangoVision)
        {
            distancia = jugador.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, distancia) < 45)
            {

                if (Physics.Raycast(transform.position, distancia, out RaycastHit hit, rangoVision))
                {

                    if (hit.transform.tag == "Jugador")
                    {
                        navAgent.speed = 5;
                        if (atacando == false)
                        {
                            if (Vector3.Distance(transform.position, jugador.transform.position) < rangoAtaque)
                            {
                                if (Physics.Raycast(transform.position, distancia, out hit, 2))
                                {
                                    hit.transform.gameObject.GetComponent<Vida>().Daño(1);
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
                    }
                }
            }
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
    IEnumerator Cadencia()
    {
        Active = true;
        Ataque();
        yield return new WaitForSeconds(cadencia);
        Active = false;
        yield return null;
    }
}
