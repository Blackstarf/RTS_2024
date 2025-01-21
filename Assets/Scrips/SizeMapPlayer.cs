using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeMapPlayer : MonoBehaviour
{
    private float planeWidth = 0f;
    private float planeLength = 0f;
    // Start is called before the first frame update
    void Start()
    {
        planeWidth = PlayerPrefs.GetFloat("SizeMap");
        planeLength = PlayerPrefs.GetFloat("SizeMap");
        // Примените размеры к плоскости
        Vector3 planeScale = new Vector3(planeWidth, 1, planeLength);
        transform.localScale = planeScale;
    }
}
