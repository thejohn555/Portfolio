using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granada : MonoBehaviour
{
    private float Impulso;
    private Collider[] Da�os;
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
        Da�os = Physics.OverlapSphere(transform.position, 2.5f);
        if (Da�os.Length > 0)
        {
            for (int i = 0; i < Da�os.Length; i++)
            {
                if (Da�os[i].GetComponent<Rigidbody>() != null)
                {
                    if (Da�os[i].transform.GetComponent<Vida>() != null)
                    {
                        Da�os[i].transform.GetComponent<Vida>().Da�o(2);
                    }
                    Direccion = Da�os[i].transform.position - transform.position;
                    Da�os[i].transform.GetComponent<Rigidbody>().AddForce(Direccion * 10, ForceMode.Impulse);
                }
            }
        }
        Destroy(this.gameObject);
    }
}
