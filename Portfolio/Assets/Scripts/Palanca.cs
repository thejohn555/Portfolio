using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Palanca : MonoBehaviour
{
    public GameObject Plataforma;
    public bool On;
    // Start is called before the first frame update
    void Start()
    {
        On = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (On == false)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            Plataforma.GetComponent<Plataformas>().enabled = false;
        }
        if (On == true)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            Plataforma.GetComponent<Plataformas>().enabled = true;
        }
    }
}
