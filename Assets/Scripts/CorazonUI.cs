using UnityEngine;

public class CorazonUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     Animator animator;
    public void Update()
    {

    }
    public void PerderVida()
    {
        animator.Play("Vida_Rompiendose");
    }
}
