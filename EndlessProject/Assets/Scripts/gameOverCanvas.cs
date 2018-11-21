using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class gameOverCanvas : MonoBehaviour {

    [SerializeField]
    TMPro.TMP_Text youAreDead;

    string deadText = "You are dead";

    public static gameOverCanvas m_gameOverCanvas;

    Coroutine textAppear;

    private bool isGameOver = false;

    [SerializeField]
    private RectTransform cursor;

    [SerializeField]
    private Vector3[] selectionPositions;

    int currentSelection = 0;

    [SerializeField]
    RectTransform[] selections;
    private void Awake()
    {
        m_gameOverCanvas = this;

        cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
    }


    private void Update()
    {
        if (isGameOver == false)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentSelection--;
            if (currentSelection < 0)
                currentSelection = selections.Length - 1;

            cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentSelection++;
            if (currentSelection > selections.Length - 1)
                currentSelection = 0;

            cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
        }

        if (Input.GetButtonDown("Jump"))
        {
            handleSelection();
        }
    }

    void handleSelection()
    {
        if (currentSelection == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else if (currentSelection == 1)
            SceneManager.LoadScene("Menu");
    }

    public void gameOver()
    {
        if (isGameOver)
            return;

        Time.timeScale = 1;
        cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
        GetComponent<Canvas>().enabled = true;
        isGameOver = true;

        StartCoroutine(textAppearAnimation());
    }

    IEnumerator textAppearAnimation()
    {
        youAreDead.text = "";
       
        for (int i = 0; i < deadText.Length; i++)
        {
            string s = youAreDead.text;
            s = s + deadText[i];
            youAreDead.text = s;
            yield return new WaitForSecondsRealtime(0.08f);
        }
    }
}
