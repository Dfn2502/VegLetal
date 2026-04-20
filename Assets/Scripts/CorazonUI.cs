using UnityEngine;

public class CorazonUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     Animator animator;
    public void Update()
    {
        animator = GameObject.Find("vida").GetComponent(Animator);
    }
    public void PerderVida()
    {
        animator.Play("Vida_Rompiendose");
    }
}
