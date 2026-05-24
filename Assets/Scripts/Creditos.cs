using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CambioEscena.Instance.CambiarEscena("EscenaInicio");
        }
    }
}
