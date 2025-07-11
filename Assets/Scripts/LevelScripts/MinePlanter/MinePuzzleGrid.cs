using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinePuzzleGrid : MonoBehaviour,IPointerDownHandler
{
    [Range(MIN_NUM, MAX_NUM)]
    [SerializeField] private int mineNum = 0;
    [SerializeField] private AudioClip incrementSound;
    [SerializeField] private AudioClip decrementSound;
    private const int MIN_NUM = 0;
    private const int MAX_NUM = 8;
    private TMP_Text _numText;
    public MineplanterLevel1 Mineplanter { get; set; }
    public int X { get; set; }
    public int Y { get; set; }


    public void Start()
    {
        _numText = GetComponentInChildren<TMP_Text>();
        UpdateNumView();
    }

    public void OnPointerDown(PointerEventData eventData)
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

        //mineNum = Mathf.Clamp(mineNum, MIN_NUM, MAX_NUM);
        if(mineNum < MIN_NUM){
            mineNum = MAX_NUM;
        }else if(mineNum > MAX_NUM){
            mineNum = MIN_NUM;
        }

        Mineplanter.CurrentMineNums[X][Y] = mineNum;
        UpdateNumView();
        if(Mineplanter.CheckLevelClear()){
            Debug.Log("Level Clear");
        }
    }

    private void UpdateNumView()
    {
        if(mineNum == MIN_NUM){
            _numText.text = "";
            return;
        }

        _numText.text = mineNum.ToString();
    }
}
