using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaMortero : MonoBehaviour
{
    [SerializeField] private GameObject _bala;
    [SerializeField] private GameObject _salidaBala;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
    public void Fuego()
    {
        GameObject.Instantiate(_bala, _salidaBala.transform.position, _salidaBala.transform.rotation);
    }
}
