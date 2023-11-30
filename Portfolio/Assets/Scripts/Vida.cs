using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vida : MonoBehaviour
{
    public GameObject Salud;
    public GameObject MuniArma;
    public GameObject MunniGranada;
    public GameObject BarraVida;
    public float vida;
    public float vidamax;
    private int _suerte;
    [SerializeField] private float _menosVida;
    // Start is called before the first frame update
    void Start()
    {
        Salud = GameObject.Find("Vida");
    }

    // Update is called once per frame
    void Update()
    {
        _menosVida = ((0.8f / vidamax) * vida) + 0.2f;

        Debug.Log(vidamax + "  " + vida);
    }
    public void Daño(int daño)
    {

        vida -= daño;
        BarraVida.GetComponent<Image>().fillAmount = ((0.8f / vidamax) * vida) + 0.2f;

        if (vida <= 0)
        {
            if (this.gameObject.CompareTag("Enemigo"))
            {
                _suerte = Random.Range(0, 5);
                switch (_suerte)
                {
                    case 0:
                        GameObject.Instantiate(Salud, transform.position, transform.rotation);
                        
                        break;
                        case 1:
                        GameObject.Instantiate(MuniArma, transform.position, transform.rotation);
                        break;
                        case 2:
                        GameObject.Instantiate(MunniGranada, transform.position, transform.rotation);
                        break;

                }
            }
            if (this.gameObject.CompareTag("Jugador"))
            {
                GameManager.giveMeReference.Loss();
            }
                Destroy(this.gameObject);
        }

    }
}
