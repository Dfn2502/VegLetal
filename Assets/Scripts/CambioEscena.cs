using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public Animator transitionAnimator;
    void Start()
    {
    }
    private void Update()
    {
        
    }
    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(SceneLoad(nombreEscena));
    }
    public IEnumerator SceneLoad(string nombreEscena)
    {
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(nombreEscena);
    }
}