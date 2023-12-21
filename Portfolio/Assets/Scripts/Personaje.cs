using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Personaje : Vida
{
    private LineRenderer _lr;
    private SpringJoint _joint;
    [SerializeField] private LayerMask _zonaEnganchable;
    private GameObject _camara;
    [SerializeField] private GameObject _granadaPrefab;
    [SerializeField] private GameObject _salidaGranada;
    [SerializeField] private GameObject _salidaGancho;
    [SerializeField] private GameObject _objetoEnganchado;
    private GameObject _Palanca;
    private GameObject Diana;
    private Vector3 _spawn;
    private Vector3 _vectorGanchoV1;
    private Vector3 _vectorGanchoV3;
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
    [SerializeField] private int _nShift;
    [SerializeField] private TMP_Text _tMunicionArma;
    [SerializeField] private TMP_Text _tMunicionGranada;
    private Rigidbody _rb;
    private bool _disparando;
    private bool _disparandoG;
    private bool _saltando;
    private bool _llave1;
    private bool _llave2;
    private bool _llave3;
    private bool _escalando;
    private bool _checksalto;
    private bool _gancho;
    private bool _gancho2;
    private bool _dash;
    private bool _checkDash;
    private bool _checkGanchoV3;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _dash = false;
        _checkDash=false;
        _nShift = 0;
        _checksalto = false;
        _escalando = false;
        Time.timeScale = 1f;
        _Palanca = GameObject.Find("Palanca");
        _municionGranada = 3;
        _municionArma = 20;
        _spawn = transform.position;
        _saltando = false;
        _coleccionables = 0;
        _camara = GameObject.Find("Main Camera");
        vida = 10;
        vidamax = vida;
        _disparando = false;
        _disparandoG = false;
        Cursor.lockState = CursorLockMode.Locked;
        _velocidadCaminar = 4;
        _fuerzaSalto = 10;
        _rb = GetComponent<Rigidbody>();
        _hSpeed = 1600;
    }

    // Update is called once per frame
    void Update()
    {
        dash();
        Gravedad();
        Mira();
        Movimiento();
        Escalada();
        GanchoV1();
        GanchoV2();
        Ataque();
        Granada();
        Salto();
        Agacharse();
        Correr();
        UI();
        Interactuar();
        GanchoV3();
    }
    private void LateUpdate()
    {
        DrawRope();
    }
    void dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)&&_checkDash==false)
        {
            _nShift += 1;
        }
        if(_nShift >= 2)
        {
            _nShift =0;
            _checkDash = true;
            _rb.AddRelativeForce(new Vector3(_movimientoR, 0, _movimientoF).normalized * 3000);
            StartCoroutine("Espera");
        }
        if (_nShift > 0)
        {
            if(_dash==false)
            {
                StartCoroutine("reinicio");
            }                
        }
    }
    void Gravedad()
    {
        if (_rb.velocity.y <= -0.9 && _gancho2 == false && _checkGanchoV3 == false && _checkDash == false)
        {
            transform.position += 7.5f * -Vector3.up*Time.deltaTime;
        }
    }
    void Mira()
    {
        _hMira =Input.GetAxis("Mouse X")*_hSpeed*Time.deltaTime;
        _hReal += _hMira;
        transform.rotation = Quaternion.Euler(0, _hReal, 0);
    }
    void Escalada()
    {
        if (Input.GetKey(KeyCode.W) && _escalando == true)
        {
            transform.Translate(new Vector3(0, 1, 0)*Time.deltaTime*_velocidadCaminar);
        }
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
        _rb.AddRelativeForce(new Vector3(_movimientoR,0,_movimientoF).normalized * _velocidadCaminar);

        if(_checksalto == true && _rb.velocity.y < 0)
        {
            _rb.AddRelativeForce(-Vector3.up * _fuerzaSalto / 10);
        }
    }
    void Salto()
    {
        if (_checksalto == true && _rb.velocity.y < 0)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 0.71f))
            {
                if (hit.transform.CompareTag("Suelo"))
                {
                    _saltando = false;
                    _checksalto = false;
                }
            }
        }
        if (Input.GetKey(KeyCode.Space) && _saltando==false)
        {
            _saltando = true;
            StartCoroutine(Retraso());
            _rb.AddRelativeForce(Vector3.up * _fuerzaSalto, ForceMode.Impulse);
        }
    }
    void Ataque()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _disparando == false && _municionArma > 0) 
        {
            StartCoroutine("Disparo");
            _municionArma -= 1;
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
            _velocidadCaminar = 8;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _velocidadCaminar = 4;
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
                if (hit.transform.name == "Cerradura1" && _llave1 == true)
                {
                    Destroy(GameObject.Find("Pared 1"));
                }
                if (hit.transform.name == "Cerradura2" && _llave2 == true)
                {
                    Destroy(GameObject.Find("Pared 2"));
                }
                if (hit.transform.name == "Cerradura3" && _llave3 == true)
                {
                    Destroy(GameObject.Find("Pared 3"));
                }
            }
        }
    }
    void GanchoV1()
    {
        if (Input.GetKeyDown(KeyCode.G) && _gancho == false)
        {
            if(Physics.Raycast(_camara.transform.position,_camara.transform.forward,out RaycastHit hit, 10))
            {
                if (hit.transform.CompareTag("GanchoV1"))
                {
                    Diana = hit.transform.gameObject;

                    StartCoroutine(Gancho(Diana));
                }
            }
        }
    }
    void GanchoV2()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Physics.Raycast(_camara.transform.position, _camara.transform.forward, out RaycastHit hit, 10))
            {
                if (hit.transform.CompareTag("GanchoV2"))
                {
                    _gancho2 = true;
                    Diana = hit.transform.gameObject;
                }
            }
        }
        if (_gancho2 == true)
        {
            _vectorGanchoV1 = Diana.transform.position - transform.position;
            transform.position += _vectorGanchoV1.normalized * Time.deltaTime * 10;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            _gancho2 = false;
        }
    }
    void GanchoV3()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(Physics.Raycast(_camara.transform.position,_camara.transform.forward,out RaycastHit hit, 10,_zonaEnganchable))
            {
                _objetoEnganchado=hit.transform.gameObject;
                _joint=this.gameObject.AddComponent<SpringJoint>();
                _joint.autoConfigureConnectedAnchor = false;
                
                _joint.spring = 4.5f;
                _joint.damper = 7f;
                _joint.massScale = 4.5f;
                _lr.positionCount = 2;
                _checkGanchoV3 = true;
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (!_joint) return;
            _joint.connectedAnchor = _objetoEnganchado.transform.position;
            float distanciaAlPunto = Vector3.Distance(this.transform.position, _objetoEnganchado.transform.position);
            _joint.maxDistance = distanciaAlPunto * 0.8f;
            _joint.minDistance = distanciaAlPunto * 0.25f;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            _lr.positionCount = 0;
            Destroy(_joint);
            _checkGanchoV3 = false;
        }
    }
    void DrawRope()
    {
        if (!_joint)
        {
            return;
        }
        else
        {
            _lr.SetPosition(0, _salidaGancho.transform.position);
            _lr.SetPosition(1, _objetoEnganchado.transform.position);
        }
    }
    IEnumerator Gancho(GameObject hit)
    {
        _gancho = true;
        _vectorGanchoV1 = hit.transform.position - transform.position;
        _rb.AddForce(_vectorGanchoV1.normalized * 40, ForceMode.Impulse);
        yield return new WaitForSeconds(1);
        _gancho = false;
        yield return null;
    }
    IEnumerator Retraso()
    {   
        yield return new WaitForSeconds(0.1f);
        _checksalto = true;
    }
    IEnumerator reinicio()
    {
        _dash = true;
        yield return new WaitForSeconds(0.5f);
        _nShift = 0;
        _dash = false;
        yield return null;
    }
    IEnumerator Espera()
    {
        yield return new WaitForSeconds(0.15f);
        _rb.velocity= Vector3.zero;
        yield return new WaitForSeconds(1);
        _checkDash = false;
    }
    IEnumerator Disparo()
    {
        _disparando = true;
        if (Physics.Raycast(_camara.transform.position,_camara.transform.forward , out RaycastHit hit, 10))
        {
            if(hit.transform.GetComponent<Vida>()!=null)
            {
                hit.transform.GetComponent<Vida>().Daño(2);
                
            }
            if (hit.transform.CompareTag("Boton"))
            {
                hit.transform.GetComponent<botonRampa>().Swithc();
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
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("MuniArma"))
        {
            _municionArma += 10;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("MuniGranada"))
        {
            _municionGranada+= 1;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Victoria"))
        {
            GameManager.giveMeReference.Win();
        }
        if (other.CompareTag("Escalable"))
        {
            _escalando = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Escalable"))
        {
            _escalando = false;
        }
    }
}
