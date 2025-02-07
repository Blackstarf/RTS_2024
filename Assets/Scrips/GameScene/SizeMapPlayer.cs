using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SizeMapPlayer : MonoBehaviour
{
    public TMP_Text TMPPlayer;
    public GameObject Vrags; 
    private float planeWidth = 0f;
    private float planeLength = 0f;
    private string NamePlayer = "Игрок 1";

    void Start()
    {
        // Load the saved data from PlayerPrefs
        planeWidth = PlayerPrefs.GetFloat("SizeMap");
        planeWidth = Mathf.FloorToInt(Mathf.Sqrt(planeWidth));
        NamePlayer = PlayerPrefs.GetString("NamePlay");
        planeLength = planeWidth;

        if (string.IsNullOrWhiteSpace(NamePlayer))
        {
            NamePlayer = "Игрок 1";
        }

        // Устанавливаем текст для игрока
        TMPPlayer.text = NamePlayer;

        // Примените размеры к плоскости
        Vector3 planeScale = new Vector3(planeWidth, 1, planeLength);
        transform.localScale = planeScale;

        // Start the coroutine to check for new enemies
        StartCoroutine(CheckForNewEnemies());
    }

    private IEnumerator CheckForNewEnemies()
    {
        while (true)
        {
            // Load and set the names for enemies
            for (int i = 0; i < Vrags.transform.childCount; i++)
            {
                Transform vragTransform = Vrags.transform.GetChild(i);
                string enemyNameKey = "Name_" + vragTransform.name.Substring(5); // Adjusted to substring(5) to match "Vrag_"
                string enemyName = PlayerPrefs.GetString(enemyNameKey);

                // Find the TMP_Text component named "Player"
                TMP_Text enemyText = vragTransform.GetComponentInChildren<TMP_Text>(true);
                if (string.IsNullOrWhiteSpace(enemyText.text))
                {
                    enemyText.text = "Противник " + (i + 1);                    
                }
                else
                {
                    enemyText.text = enemyName;
                }
            }

            // Wait for a short period before checking again
            yield return new WaitForSeconds(1f);
        }
    }
}
