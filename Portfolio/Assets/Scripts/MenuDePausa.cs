using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDePausa : MonoBehaviour
{
    private GameObject _canvas;
    private bool _enabled;
    private void Start()
    {
        _enabled = false;
        if (GameObject.Find("Pausa")!=null)
        {
            _canvas = GameObject.Find("Pausa");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameObject.Find("Pausa") != null)
            {
                _canvas.GetComponent<Canvas>().enabled = !_canvas.GetComponent<Canvas>().enabled;
                Raton();
            }
        }
    }
    public void Raton()
    {
        _enabled = !_enabled;
        if(_enabled==true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
        }
        if(_enabled==false)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void inicio()
    {
        SceneManager.LoadScene("Inicio");
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void Juego()
    {
        SceneManager.LoadScene("Nivel1");
    }
    public void Salir()
    {
        Application.Quit();
    }
}
