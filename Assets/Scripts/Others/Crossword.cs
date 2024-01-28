using UnityEngine;
using UnityEngine.UI; // For UI Text
using TMPro; // Uncomment this if using TextMeshPro

public class ButtonPuzzleScript : MonoBehaviour
{
    private int currentButtonIndex = 0;
    private int[] correctOrder = { 1, 2, 3, 4 };
    public Button nextbutton;
    public Button[] buttons; // Array to hold the button references
    public GameObject[] textElements; // Array to hold the text element references

    public void ButtonClicked(int buttonNumber)
    {
        if (currentButtonIndex < correctOrder.Length && buttonNumber == correctOrder[currentButtonIndex])
        {
            textElements[currentButtonIndex].SetActive(true); // Show the text
            currentButtonIndex++;

            if (currentButtonIndex == correctOrder.Length)
            {
                PuzzleSolved();
            }
        }
        else
        {
            ResetPuzzle();
        }
    }

    public void BackgroundClicked()
    {
        ResetPuzzle();
    }

    private void PuzzleSolved()
    {
        nextbutton.gameObject.SetActive(true);
        // Additional actions on puzzle solved
    }

    private void ResetPuzzle()
    {
        currentButtonIndex = 0;
        foreach (var textElement in textElements)
        {
            textElement.SetActive(false); // Hide all text elements
        }
        // Additional reset actions
    }
}