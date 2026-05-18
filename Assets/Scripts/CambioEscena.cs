using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    private Animator transitionAnimator;
    public Transicion transicion;
    public float transitionTime = 1f;

    void Start()
    {
        transitionAnimator = GetComponentInChildren<Animator>();
    }

    public void IrAJugar()
    {
        int indiceSiguienteEscena = SceneManager.GetActiveScene().buildIndex + 1;  
        StartCoroutine(transicion.CargarEscena(indiceSiguienteEscena));

    }

} 