using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GravityController2D))]
[RequireComponent(typeof(ShakeAnim))]
public class BrokenObject : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float gravityScale = 100f;
    [Range(0, 10)]
    [SerializeField] private int numOfClicksBeforeDrop = 3;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private BreakType breakType;
    [SerializeField] private bool shakeWhenClicked = true;
    [SerializeField] private float shakeDurationPerClick = 0.5f;
    [SerializeField] private Transform parentWhenBroken;
    [SerializeField] private UnityEvent onDrop;

    private bool _isDropped = false;

    private GravityController2D _gravityController2D;
    private ShakeAnim _shakeAnim;
    private IEnumerator _dropCoroutine;

    public event System.Action OnBroken;

    // Start is called before the first frame update
    void Start()
    {
        _gravityController2D = GetComponent<GravityController2D>();
        _shakeAnim = GetComponent<ShakeAnim>();
        Initialize();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HandleClick();
    }

    public void HandleClick()
    {
        if (_isDropped) return;
        numOfClicksBeforeDrop--;
        Debug.Log("Clicked once, " + numOfClicksBeforeDrop + " more clicks to go");

        if (numOfClicksBeforeDrop <= 0 && gameObject.activeSelf && _dropCoroutine==null) //drop the object
        {
            _dropCoroutine = Drop();
            StartCoroutine(_dropCoroutine);
        }
        else if (shakeWhenClicked)
        {
            _shakeAnim.PlayOneShot(shakeDurationPerClick);
        }

    }



    private IEnumerator Drop()
    {
        _shakeAnim.StopShake();

        if (shakeWhenClicked)
        {
            _shakeAnim.PlayOneShot(shakeDurationPerClick);
            yield return new WaitForSeconds(shakeDurationPerClick);
        }

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

        if (parentWhenBroken != null)
        {
            transform.SetParent(parentWhenBroken);
        }
        OnBroken?.Invoke();
        onDrop?.Invoke();
        HandleBroken();

        _isDropped = true;
        if (breakSound != null)
        {
            AudioManager.Instance.PlaySFX(breakSound);
        }
    }

    private enum BreakType
    {
        Drop, DetachHinge
    }

    protected virtual void HandleBroken()
    {

    }
    protected virtual void Initialize()
    {

    }


}
