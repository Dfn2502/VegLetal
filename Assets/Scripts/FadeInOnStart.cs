using UnityEngine;

public class FadeInOnStart : MonoBehaviour
{
    public Animator transitionAnimator;

    void Start()
    {
        transitionAnimator.SetTrigger("FadeIn");
    }
}
