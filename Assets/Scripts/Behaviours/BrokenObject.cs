using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GravityController2D))]
[RequireComponent(typeof(ShakeAnim))]
public class BrokenObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private float gravityScale = 100f;
    [Range(0, 10)]
    [SerializeField]
    private int numOfClicksBeforeDrop = 3;
    [SerializeField]
    private AudioClip breakSound;
    [SerializeField]
    private BreakType breakType;

    private bool _isDropped = false;

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
        if (_isDropped) return;
        numOfClicksBeforeDrop--;
        Debug.Log("Clicked once, " + numOfClicksBeforeDrop + " more clicks to go");

        if (numOfClicksBeforeDrop <= 0) //drop the object
        {
            _shakeAnim.StopShake();
            _gravityController2D.Unfreeze();

            switch (breakType)
            {
                case BreakType.Drop:
                    _gravityController2D.SetGravity(gravityScale);
                    _gravityController2D.UseGravity();
                    break;
                case BreakType.DetachHinge:
                    _gravityController2D.SetGravity(gravityScale);
                    _gravityController2D.UseGravity();
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    
                    rb.GetComponentInChildren<HingeJoint2D>().enabled = false;
                    break;
            }

            _isDropped = true;
            if (breakSound != null)
            {
                AudioManager.Instance.PlaySFX(breakSound);
            }

        }
        else
        { //if num of click not reached, shake the object
            _shakeAnim.PlayOneShot(1f);
        }
    }

    private enum BreakType
    {
        Drop, DetachHinge
    }


}
