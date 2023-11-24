using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granada : MonoBehaviour
{
    private float Impulso;
    private Collider[] Daños;
    private Vector3 Direccion;
    // Start is called before the first frame update
    void Start()
    {
        Impulso = 10;
        GetComponent<Rigidbody>().AddForce(transform.forward * Impulso, ForceMode.Impulse);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        Daños = Physics.OverlapSphere(transform.position, 2.5f);
        if (Daños.Length > 0)
        {
            for (int i = 0; i < Daños.Length; i++)
            {
                if (Daños[i].GetComponent<Rigidbody>() != null)
                {
                    if (Daños[i].transform.GetComponent<Vida>() != null)
                    {
                        Daños[i].transform.GetComponent<Vida>().Daño(2);
                    }
                    Direccion = Daños[i].transform.position - transform.position;
                    Daños[i].transform.GetComponent<Rigidbody>().AddForce(Direccion * 10, ForceMode.Impulse);
                }
            }
        }
        Destroy(this.gameObject);
    }
}
