using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageCompleteAnimation : MonoBehaviour {

    [SerializeField]
    private RectTransform stageCompleteText;

    public void ActivateAnimation()
    {
        stageCompleteText.gameObject.SetActive(true);
        StartCoroutine(appear());
    }

    IEnumerator appear()
    {
     
        float lerp = 0;

        Vector3 startPos = new Vector3(-550, 100, 0);
        Vector3 endPos = new Vector3(0, 100, 0);

        while(lerp < 1)
        {
            lerp += Time.deltaTime;

            Vector3 v = Vector3.Lerp(startPos, endPos, lerp);
            stageCompleteText.localPosition = v;
            yield return null;

        }
    }
}
