using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambioEscena : MonoBehaviour
{
    public static CambioEscena Instance;

    public Animator transitionAnimator;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(SceneLoad(nombreEscena));
    }

    IEnumerator SceneLoad(string nombreEscena)
    {
        Time.timeScale = 1f;
        transitionAnimator.SetTrigger("StartTransition");

        yield return new WaitForSeconds(1.1f);

        SceneManager.LoadScene(nombreEscena);

        yield return null;

        ReasignarAnimator();

        transitionAnimator.SetTrigger("EndTransition");
    }

    void ReasignarAnimator()
    {
        var obj = GameObject.Find("TransicionCanvas");
        if (obj != null)
            transitionAnimator = obj.GetComponent<Animator>();
    }
}