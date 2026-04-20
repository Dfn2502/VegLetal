using UnityEngine;

public class CorazonUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator animator;

    public void PerderVida()
    {
        animator.SetTrigger("Hit");
    }
}
