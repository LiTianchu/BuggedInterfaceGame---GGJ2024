using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GravityController2D))]
[RequireComponent(typeof(ShakeAnim))]
public class DroppingObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private float gravityScale = 100f;
    [SerializeField]
    private int numOfClicksBeforeDrop = 3;
    [SerializeField]
    private AudioClip dropSound;


    private GravityController2D _gravityController2D;
    private ShakeAnim _shakeAnim;

    // Start is called before the first frame update
    void Start()
    {
        _gravityController2D = GetComponent<GravityController2D>();
        _shakeAnim = GetComponent<ShakeAnim>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        numOfClicksBeforeDrop--;
        Debug.Log("Clicked once, " + numOfClicksBeforeDrop + " more clicks to go");

        if (numOfClicksBeforeDrop == 0) //drop the object
        {
            _shakeAnim.StopShake();
            _gravityController2D.EnableSimulation();
            _gravityController2D.SetGravity(gravityScale);
            _gravityController2D.UseGravity();
            if(dropSound != null){
                AudioManager.Instance.PlaySFX(dropSound);
            }

        }else{ //if num of click not reached, shake the object
            _shakeAnim.PlayOneShot(1f);
        }
    }



}
