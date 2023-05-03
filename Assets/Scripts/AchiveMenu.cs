using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//업적 메뉴창
//업적을 달성했다면 이미지가 검은색에서 기존의 색으로 바뀜
public class AchiveMenu : AchiveManager
{
    public Image[] images;
    [SerializeField] Achive type;
    void OnEnable()
    {
        if (PlayerPrefs.GetInt(type.ToString()) == 1)
        {
            ColorWhite();
        }

        void ColorWhite()
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = Color.white;
            }
        }
    }
}
