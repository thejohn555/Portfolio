using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plataformas : MonoBehaviour
{
    public float primerPunto;
    public float segundoPunto;
    private Vector3 direccion;
    // Start is called before the first frame update
    void Start()
    {
        direccion = Vector3.forward * 3;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direccion * Time.deltaTime;

        if (transform.position.z >= primerPunto)
        {
            direccion *= -1;
        }
        if (transform.position.z <= segundoPunto)
        {
            direccion *= -1;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Jugador")
        {
            collision.transform.parent = transform;
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Jugador")
        {
            collision.transform.parent = null;
        }
    }
}
