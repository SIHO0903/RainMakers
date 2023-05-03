using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//게임시작시 버튼에 있는 글씨 반짝이게함
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
