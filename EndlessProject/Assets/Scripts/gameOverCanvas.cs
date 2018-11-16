using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class gameOverCanvas : MonoBehaviour {

    [SerializeField]
    TMPro.TMP_Text youAreDead;

    string deadText = "You are dead";

    public static gameOverCanvas m_gameOverCanvas;

    Coroutine textAppear;

    private bool isGameOver = false;
    private void Awake()
    {
        m_gameOverCanvas = this;

        gameOver();
    }

    public void gameOver()
    {
        if (isGameOver)
            return;

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
