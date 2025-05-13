using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GlobalSingleton<GameManager>
{
    public float TimePassed { get; set; }
    public int Difficulty { get; set; }

    public void Start()
    {
        TimePassed = 0;
        Difficulty = 0;
    }

    public void Update()
    {
        TimePassed += Time.deltaTime;
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator LoadScene(string sceneName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator QuitGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.Quit();
    }

    public string GenerateCode()
    {
        string code = string.Empty;
        //generate a random code of 10 characters
        for (int i = 0; i < 10; i++)
        {
            int randomNumber = Random.Range(10, 36);
            code += ((char)(randomNumber + 87)).ToString(); // a=10, b=11, ..., z=35
        }
        return code;
    }

}
