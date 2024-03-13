using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriebaBalaMortero : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
