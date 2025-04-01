using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTyping : MonoBehaviour
{
     [TextArea(3, 10)]
    [SerializeField]
    private string[] sentences;
    [SerializeField]
    private TMP_Text textDisplay;
    [SerializeField]
    private float typingRate;
    [SerializeField]
    private float sentencePauseTime;
    [SerializeField]
    private CanvasGroup footer;
    [SerializeField]
    private AudioClip typingBGM;

    private int _currentSentenceIndex = 0;
    //private int _currentSentenceCharIndex;
    private bool _nextSentenceReady = true;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.SetBGMPlayRate(1);
        AudioManager.Instance.PlayBGM(typingBGM);
    }

    // Update is called once per frame
    void Update()
    {
        //string currentSentence = sentences[_currentSentenceIndex];
        if (_nextSentenceReady)
        {
            if(_currentSentenceIndex >= sentences.Length)
            {
                if (!footer.gameObject.activeSelf)
                {
                    footer.gameObject.SetActive(true);
                    UIManager.Instance.UIFadeIn(footer, 0.5f, footer.GetComponent<RectTransform>().anchoredPosition, footer.GetComponent<RectTransform>().anchoredPosition);
                }
                return;
            }
            StartCoroutine(TypeSentence(sentences[_currentSentenceIndex]));
            _nextSentenceReady = false;
        }

    }

    IEnumerator TypeSentence(string sentence)
    {
      
            foreach (char letter in sentence.ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingRate);
            }

            yield return new WaitForSeconds(sentencePauseTime);
            _nextSentenceReady = true;
            _currentSentenceIndex++;
        }
    

}
