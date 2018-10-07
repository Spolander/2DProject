using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundScaler : MonoBehaviour {

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Camera cam = Camera.main;
        float cameraHeight = cam.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(cam.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }

        transform.localScale = scale;
    }
}
