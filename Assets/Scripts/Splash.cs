using System.Collections;
using UnityEngine;

public class Splash : MonoBehaviour
{
    private IEnumerator Start()
    {
        // Bỏ kiểm tra consent vì không dùng quảng cáo
        yield return null;
        GameManager.LoadScene("Main");
    }
}