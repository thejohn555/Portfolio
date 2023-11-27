using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Camara : MonoBehaviour
{
    private GameObject Jugador;
    private float _vMira;
    private float _vReal;
    private float _hMira;
    private float _hReal;
    private int _vSpeed;
    private int _hSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Jugador = GameObject.FindGameObjectWithTag("Jugador");
        _vSpeed = 1400;
        _hSpeed = 1600;
    }

    // Update is called once per frame
    void Update()
    {
        if (Jugador != null)
        {
            transform.position = Jugador.transform.position + new Vector3(0, 0.5f, 0);
        }
        
        //transform.rotation= Jugador.transform.rotation;
        Mira();
    }
    void Mira()
    {
        _hMira = Input.GetAxis("Mouse X") * _hSpeed * Time.deltaTime;
        _hReal += _hMira;
        _vMira = Input.GetAxis("Mouse Y") * _vSpeed * Time.deltaTime;
        _vReal -= _vMira;
        _vReal = Mathf.Clamp(_vReal, -90, 90);
        transform.rotation = Quaternion.Euler(_vReal, _hReal,0);
    }
}
