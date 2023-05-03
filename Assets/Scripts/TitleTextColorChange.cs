using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//���ӽ��۽� ��ư�� �ִ� �۾� ��¦�̰���
public class TitleTextColorChange : MonoBehaviour
{
    TextMeshProUGUI text;
    Color tmpColor;
    float timer;
    [SerializeField][Range(0f, 400f)] float speed;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        tmpColor = text.color;
    }

    void Update()
    {
        timer += Time.deltaTime;
        tmpColor.r = Mathf.Sin(timer* speed * Mathf.Deg2Rad);
        text.color = tmpColor;
    }
}
