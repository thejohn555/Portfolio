using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victoria : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Inicio()
    {
        SceneManager.LoadScene("Inicio");
    }
    public void VolveraJugar()
    {
        SceneManager.LoadScene("Juego");
    }
}
