
using UnityEngine;
using UnityEngine.UI; // For UI Text
using UnityEngine.SceneManagement; // For Scene Management
using TMPro; // Uncomment this if using TextMeshPro

public class Crossword : MonoBehaviour
{
    private int currentButtonIndex = 0;
    private int secretButtonIndex = 0;
    public int[] correctOrder = { 1, 2, 3, 4 };
    public int[] secretOrder = { 1, 2, 3, 4 };
    public Button nextbutton;
    public GameObject buttons; 

    public void ButtonClicked(int buttonNumber)
    {
        bool isCorrect = false;

        if (currentButtonIndex < correctOrder.Length && buttonNumber == correctOrder[currentButtonIndex])
        {
            buttons.transform.GetChild(buttonNumber-1).GetChild(0).gameObject.SetActive(true); // Show text element
            currentButtonIndex++;

            if (currentButtonIndex == correctOrder.Length)
            {
                PuzzleSolved();
            }

            isCorrect = true;
        }

        if (secretButtonIndex < secretOrder.Length && buttonNumber == secretOrder[secretButtonIndex])
        {
            buttons.transform.GetChild(buttonNumber-1).GetChild(0).gameObject.SetActive(true); // Show text element
            secretButtonIndex++;

            if (secretButtonIndex == secretOrder.Length)
            {
                SceneManager.LoadScene("BIOS");
            }

            isCorrect = true;
        }
 
        if (!isCorrect) ResetPuzzle();
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
        secretButtonIndex = 0;

        TextMeshProUGUI[] alltext = FindObjectsOfType<TextMeshProUGUI>(); // Get all text elements
        foreach (var textElement in alltext)
        {
            if (textElement.transform.IsChildOf(buttons.transform))
            {
                textElement.gameObject.SetActive(false); // Hide all text elements
            }
        }
        // Additional reset actions
    }
}