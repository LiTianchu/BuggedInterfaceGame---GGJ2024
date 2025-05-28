using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinePuzzleGrid1 : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] protected int mineNum = 0;
    [SerializeField] protected AudioClip incrementSound;
    [SerializeField] protected AudioClip decrementSound;
    protected virtual int MIN_NUM => 0;
    protected virtual int MAX_NUM => 3;
    protected TMP_Text _numText;
    public MineplanterLevel3 Mineplanter { get; set; }
    public int X { get; set; }
    public int Y { get; set; }


    public virtual void Start()
    {
        _numText = GetComponentInChildren<TMP_Text>();
        UpdateNumView();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            mineNum++;
            AudioManager.Instance.PlaySFX(incrementSound);
        }else if(eventData.button == PointerEventData.InputButton.Right)
        {
            mineNum--;
            AudioManager.Instance.PlaySFX(decrementSound);
        }
        mineNum = Mathf.Clamp(mineNum, MIN_NUM, MAX_NUM);
        Mineplanter.CurrentMineNums[X][Y] = mineNum;
        UpdateNumView();
        Mineplanter.ValidateInput(Mineplanter.CurrentMineNums);
    }

    public virtual void UpdateNumView()
    {
        if(mineNum == MIN_NUM){
            _numText.text = "";
            return;
        }

        _numText.text = mineNum.ToString();
    }
}
