using UnityEngine;
using TMPro;

public class ScramblingText : MonoBehaviour
{
    [SerializeField] private TextMeshPro textComponent;
    [SerializeField] private string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    [SerializeField] private float scrambleInterval = 0.1f;

    private string currentText = "SCRAMBLE";
    private float timer;

    private void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshPro>();
        }
        SetText(currentText);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= scrambleInterval)
        {
            ScrambleText();
            timer = 0f;
        }
    }

    public void SetText(string newText)
    {
        currentText = newText;
        textComponent.text = newText;
    }

    private void ScrambleText()
    {
        char[] scrambled = currentText.ToCharArray();
        for (int i = 0; i < scrambled.Length; i++)
        {
            scrambled[i] = characters[Random.Range(0, characters.Length)];
        }
        textComponent.text = new string(scrambled);
    }
}
