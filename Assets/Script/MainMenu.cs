using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField PlayerOneNameInput;
    public TMP_InputField PlayerTwoNameInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
       
        string playerOneName = PlayerOneNameInput.text;
        string playerTwoName = PlayerTwoNameInput.text;

        PlayerPrefs.SetString("PlayerOneName", playerOneName);
        PlayerPrefs.SetString("PlayerTwoName", playerTwoName);

        SceneManager.LoadScene("SampleScene");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
