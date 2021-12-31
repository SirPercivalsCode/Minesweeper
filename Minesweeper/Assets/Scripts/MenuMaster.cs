using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMaster : MonoBehaviour
{

    [Header("Variables")]
    public int deepness;

    [Header("References")]
    public GameObject mainGO;
    public GameObject gameModesGO;

    //public GameObject popupGO;
    //public Text popupText;
    //public GameObject yesButton;
    //public GameObject noButton;


    void Start()
    {
        mainGO.SetActive(true);
        gameModesGO.SetActive(false);
        //popupGO.SetActive(false);
    }

    public void LoadScene(int scenenr)
    {
        SceneManager.LoadScene(scenenr);
    }

    public void GoBack()
    {
        deepness--;
    }

    public void Quit()
    {
        Application.Quit();
    }

    /*
    public void PopupYesNo(string question = "Are you sure?")
    {
        if(StartCoroutine(Dialog(question)))
        {
            Application.Quit;
        }
    }

    IEnumerator Dialog(string question = "Are you sure?")
    {
        popupText.text = question;
        popupGO.SetActive(true);

        var waitForButton = new WaitForUIButtons(yesButton, noButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == yesButton)
        {
            // yes was pressed
            return true;
        }
        else
        {
            // no was pressed
            popupGO.SetActive(false);
            return false;
        }
    }
    */
}
