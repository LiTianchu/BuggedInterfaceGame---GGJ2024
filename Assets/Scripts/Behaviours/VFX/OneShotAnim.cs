using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OneShotAnim : MonoBehaviour
{
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool destroyOnFinish = true;
    // Start is called before the first frame update
    void Start()
    {
        if(playOnStart)
        {
            StartCoroutine(PlayAnimation());
        }
    }


    private IEnumerator PlayAnimation()
    {
        Animator animator = GetComponent<Animator>();
        
        // Wait for the animation to finish
        yield return new WaitWhile(()=>AnimationUtils.AnimatorIsPlaying(animator));
        
        if(destroyOnFinish)
        {
            Destroy(gameObject);
        }
    }
}
