using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//���� �޴�â
//������ �޼��ߴٸ� �̹����� ���������� ������ ������ �ٲ�
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
