using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour
{
    public int vida;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Da�o(int da�o)
    {

        vida -= da�o;

        if (vida <= 0)
        {
            Destroy(this.gameObject);
        }

    }
}
