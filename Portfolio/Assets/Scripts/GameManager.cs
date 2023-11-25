using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _Reference;

    public static GameManager giveMeReference
    {
        get
        {


            if (_Reference == null)
            {
                _Reference = FindObjectOfType<GameManager>();
                if (_Reference == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _Reference = go.AddComponent<GameManager>();
                }
            }
            return _Reference;
        }
    }
    [SerializeField] private float _minutos;
    [SerializeField] private float _segundos;
    [SerializeField] private TMP_Text _tMinutos;
    [SerializeField] private TMP_Text _tSegundos;
    private bool _tiempoSigue;

    // Start is called before the first frame update
    void Start()
    {
        _minutos = 10;
        StartCoroutine("ContraRelog");
    }

    // Update is called once per frame
    void Update()
    {
        Relog();
    }
    void Relog()
    {
        _tMinutos.text=_minutos.ToString();
        _tSegundos.text=_segundos.ToString();
    }
    public void Loss()
    {
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("Derrota");
    }
    public void Win()
    {
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("Victoria");
    }
    IEnumerator ContraRelog()
    {
        while (_tiempoSigue == false)
        {
            _segundos -= 1;
            if (_segundos < 0)
            {
                _minutos -= 1;
                _segundos = 59;
                if (_minutos < 0)
                {
                    _tiempoSigue = true;
                    Loss();
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
