using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public void IrAJugar()
    {
        SceneManager.LoadScene("EscenaJuego");
    }
}