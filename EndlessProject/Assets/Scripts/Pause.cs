using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pause : MonoBehaviour {

    public static Pause instance;


    [SerializeField]
    private RectTransform cursor;

    [SerializeField]
    private Vector3[] selectionPositions;

    int currentSelection = 0;

    [SerializeField]
    RectTransform[] selections;

    [SerializeField]
    private AudioSource pauseMusic;

    [SerializeField]
    private AudioSource[] otherMusic;

    private bool[] otherMusicState;

    bool directionPressed = false;
    private bool gamePaused = false;


    [SerializeField]
    private Canvas c;
    private void Awake()
    {
        instance = this;
        cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
        otherMusicState = new bool[otherMusic.Length];

        for (int i = 0; i < otherMusicState.Length; i++)
            otherMusicState[i] = otherMusic[i].isPlaying;
    }


    public void PauseGame()
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            Time.timeScale = 0;
            pauseMusic.Play();
            for (int i = 0; i < otherMusic.Length; i++)
            {
                otherMusicState[i] = otherMusic[i].isPlaying;
                otherMusic[i].Pause();
            }
               
        }
        else
        {
            Time.timeScale = 1;
            pauseMusic.Stop();
            for (int i = 0; i < otherMusic.Length; i++)
            {
                if (otherMusicState[i])
                    otherMusic[i].Play();
            }
        }

        c.enabled = gamePaused;
    }

    private void Update()
    {
        if (gamePaused == false)
            return;

        if (Input.GetAxisRaw("Vertical") < -0.5f && directionPressed == false)
        {
            directionPressed = true;
            currentSelection--;
            if (currentSelection < 0)
                currentSelection = selections.Length - 1;

            cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
        }

        else if (Input.GetAxisRaw("Vertical") > 0.5f && directionPressed == false)
        {
            directionPressed = true;
            currentSelection++;
            if (currentSelection > selections.Length - 1)
                currentSelection = 0;

            cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.5f)
            directionPressed = false;

        if (Input.GetButtonDown("Jump"))
        {
            handleSelection();
        }
    }



    void handleSelection()
    {
        if (currentSelection == 0)
            PauseGame();
        else
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
           
    }
}
