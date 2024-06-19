using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLevel : MonoBehaviour
{
    [SerializeField]
    private PuzzleSlot radioButtonSlot;
    [SerializeField]
    private GameObject hiddenRadioButton;
    [SerializeField]
    private GameObject draggaleImg;

    // Start is called before the first frame update
    void Start()
    {
        radioButtonSlot.OnPuzzlePieceRight += (puzzlePiece) =>
        {
            radioButtonSlot.gameObject.SetActive(false);
            hiddenRadioButton.SetActive(true);
            draggaleImg.SetActive(false);
        };
    }

}
