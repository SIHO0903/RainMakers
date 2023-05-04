using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    private void OnEnable()
    {
        for (int i = 0; i < GameManager.Instance.poolManager.transform.childCount; i++)
        {
            GameManager.Instance.poolManager.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
