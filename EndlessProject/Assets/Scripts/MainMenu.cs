using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {



    [SerializeField]
    private RectTransform cursor;

    [SerializeField]
    private Vector3[] selectionPositions;

    int currentSelection = 0;

    [SerializeField]
    RectTransform[] selections;

    bool directionPressed = false;

    [SerializeField]
    private Transform controlRoot;

    [SerializeField]
    private RectTransform backSelection;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
    }


    private void Update()
    {

        if (Input.GetButtonDown("Jump") ||Input.GetButtonDown("Fire1"))
        {
            handleSelection();
        }

        if (currentSelection == -1)
            return;


        if (Input.GetAxisRaw("Vertical") > 0.5f && directionPressed == false)
        {
            directionPressed = true;
            currentSelection--;
            if (currentSelection < 0)
                currentSelection = selections.Length - 1;

            cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
        }

        else if (Input.GetAxisRaw("Vertical") < -0.5f && directionPressed == false)
        {
            directionPressed = true;
            currentSelection++;
            if (currentSelection > selections.Length - 1)
                currentSelection = 0;

            cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.5f)
            directionPressed = false;

       
    }


   
    void handleSelection()
    {
        if (currentSelection == 0)
            SceneManager.LoadScene(1);
        else if (currentSelection == 1)
        {
            currentSelection = -1;
            controlRoot.gameObject.SetActive(true);
            cursor.position = backSelection.TransformPoint(selectionPositions[3]);
        }
        else if (currentSelection == 2)
        {
            Application.Quit();
        }
        else if (currentSelection == -1)
        {
            currentSelection = 1;
            controlRoot.gameObject.SetActive(false);
            cursor.position = selections[currentSelection].TransformPoint(selectionPositions[currentSelection]);
        }
    }

 
}
