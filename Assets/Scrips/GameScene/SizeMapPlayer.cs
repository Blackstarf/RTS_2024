using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SizeMapPlayer : MonoBehaviour
{
    public TMP_Text TMPPlayer;
    private float planeWidth = 0f;
    private float planeLength = 0f;
    private string NamePlayer= "����� 1";
    // Start is called before the first frame update
    void Start()
    {
        planeWidth = PlayerPrefs.GetFloat("SizeMap");
        planeLength = PlayerPrefs.GetFloat("SizeMap");
        // ������������� �����
        TMPPlayer.text = NamePlayer;
        // ��������� ������� � ���������
        Vector3 planeScale = new Vector3(planeWidth, 1, planeLength);
        transform.localScale = planeScale;
        if (TMPPlayer == null)
        {
            Debug.LogError("TMPPlayer �� ����������!");
        }
    }
}
