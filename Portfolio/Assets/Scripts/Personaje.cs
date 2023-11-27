using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Personaje : Vida
{
    private GameObject _camara;
    [SerializeField] private GameObject _granadaPrefab;
    [SerializeField] private GameObject _salidaGranada;
    private GameObject _Palanca;
    private Vector3 _spawn;
    private float _movimientoF;
    private float _movimientoR;
    private float _hMira;
    private float _hReal;
    private int _hSpeed;
    private int _velocidadCaminar;
    private int _fuerzaSalto;
    private int _coleccionables;
    private int _municionArma;
    private int _municionGranada;
    [SerializeField] private TMP_Text _tMunicionArma;
    [SerializeField] private TMP_Text _tMunicionGranada;
    private Rigidbody _rb;
    private bool _disparando;
    private bool _disparandoG;
    private bool _saltando;
    private bool _llave1;
    private bool _llave2;
    private bool _llave3;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _Palanca = GameObject.Find("Palanca");
        _municionGranada = 3;
        _municionArma = 20;
        _spawn = transform.position;
        _saltando = false;
        _coleccionables = 0;
        _camara = GameObject.Find("Main Camera");
        vida = 10;
        _disparando = false;
        _disparandoG = false;
        Cursor.lockState = CursorLockMode.Locked;
        _velocidadCaminar = 800;
        _fuerzaSalto = 200000;
        _rb = GetComponent<Rigidbody>();
        _hSpeed = 1600;
    }

    // Update is called once per frame
    void Update()
    {
        Mira();
        Movimiento();
        Ataque();
        Granada();
        Salto();
        Agacharse();
        Correr();
        UI();
        Interactuar();

    }
    void Mira()
    {
        _hMira =Input.GetAxis("Mouse X")*_hSpeed*Time.deltaTime;
        _hReal += _hMira;
        transform.rotation = Quaternion.Euler(0, _hReal, 0);
    }
    void Movimiento ()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
           _movimientoF = 1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            _movimientoF = 0;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _movimientoR = 1;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _movimientoR = 0;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _movimientoF = -1;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            _movimientoF = 0;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _movimientoR =  -1;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _movimientoR = 0;
        }
        _rb.AddRelativeForce(new Vector3(_movimientoR,0,_movimientoF).normalized * _velocidadCaminar * Time.deltaTime);
    }
    void Salto()
    {
        if (_saltando == true&&_rb.velocity.y<0)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 0.71f))
            {
                if (hit.transform.CompareTag("Suelo"))
                {
                    _saltando = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)&&_saltando==false)
        {
            _saltando = true;
            _rb.AddRelativeForce(Vector3.up * _fuerzaSalto * Time.deltaTime);
        }
    }
    void Ataque()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _disparando == false && _municionArma > 0) 
        {
            StartCoroutine("Disparo");
        }
    }
    void Granada()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2) && _disparandoG == false && _municionGranada > 0) 
        {
            _municionGranada -= 1;
            StartCoroutine("DisparoG");
        }
    }
    void Agacharse()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3 (transform.localScale.x, 0.2f,transform.localScale.z);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(0.71f, 0.71f, 0.71f);
        }
    }
    void Correr()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _velocidadCaminar = 1500;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _velocidadCaminar = 800;
        }
    }
    void UI()
    {
        _tMunicionArma.text = _municionArma.ToString();
        _tMunicionGranada.text = _municionGranada.ToString();
    }
    void Interactuar()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(Physics.Raycast(_camara.transform.position,_camara.transform.forward,out  RaycastHit hit, 2))
            {
                if (hit.transform.CompareTag("Palanca"))
                {
                    _Palanca.GetComponent<Palanca>().On = !_Palanca.GetComponent<Palanca>().On;
                }
            }
        }
    }
    IEnumerator Disparo()
    {
        _disparando = true;
        if (Physics.Raycast(_camara.transform.position,_camara.transform.forward , out RaycastHit hit, 10))
        {
            if(hit.transform.GetComponent<Vida>()!=null)
            {
                hit.transform.GetComponent<Vida>().Daño(2);
                _municionArma -= 1;
            }
        }
        yield return new WaitForSeconds(1);
        _disparando = false;
        yield return null;
    }
    IEnumerator DisparoG()
    {
        _disparando = true;
        GameObject.Instantiate(_granadaPrefab, _salidaGranada.transform.position, _salidaGranada.transform.rotation);
        yield return new WaitForSeconds(1);
        _disparando = false;
        yield return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Llave1"))
        {
            _llave1 = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Llave2"))
        {
            _llave2 = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Llave3"))
        {
            _llave3 = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Coleccionable"))
        {
            _coleccionables++;
            Destroy(other.gameObject);
            if (_coleccionables > 3)
            {
                Destroy(GameObject.Find("Pared ?"));
            }
        }
        if (other.gameObject.CompareTag("Spawn"))
        {
            _spawn = transform.position;
        }
        if (other.gameObject.CompareTag("Kill"))
        {
            transform.position = _spawn;
        }
        if (other.gameObject.CompareTag("Vida"))
        {
            vida += 5;
        }
        if (other.gameObject.CompareTag("MuniArma"))
        {
            _municionArma += 10;
        }
        if (other.gameObject.CompareTag("MuniGranada"))
        {
            _municionGranada+= 1;
        }
        if (other.gameObject.CompareTag("Victoria"))
        {
            GameManager.giveMeReference.Win();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Puerta1"))
        {
            if (Input.GetKeyDown(KeyCode.F) && _llave1 == true)
            {
                Destroy(GameObject.Find("Pared 1"));
            }
        }
        if (other.gameObject.CompareTag("Puerta2"))
        {
            if (Input.GetKeyDown(KeyCode.F) && _llave2 == true)
            {
                Destroy(GameObject.Find("Pared 2"));
            }
        }
        if (other.gameObject.CompareTag("Puerta3"))
        {
            if (Input.GetKeyDown(KeyCode.F) && _llave3 == true)
            {
                Destroy(GameObject.Find("Pared 3"));
            }
        }
    }

}
