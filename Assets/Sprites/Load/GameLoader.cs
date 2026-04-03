using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [Header("Kéo object FILL (cái ruột xanh) vào đây:")]
    public Image loadingBarFill; // Gọi thẳng vào Image thay vì Slider

    [Header("Gõ tên màn hình game vào đây:")]
    public string sceneToLoad;

    [Header("Thời gian load tối thiểu (giây):")]
    public float minLoadTime = 3f;

    void Start()
    {
        // Ép thanh xanh về 0 (trống trơn) lúc mới bật
        if (loadingBarFill != null) loadingBarFill.fillAmount = 0f;
        StartCoroutine(LoadSceneWithTimer());
    }

    IEnumerator LoadSceneWithTimer()
    {
        float timer = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (timer < minLoadTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / minLoadTime);

            if (loadingBarFill != null)
            {
                // Dùng fillAmount giúp thanh trượt mượt mà, không méo hình
                loadingBarFill.fillAmount = progress;
            }

            yield return null;
        }

        if (loadingBarFill != null) loadingBarFill.fillAmount = 1f;
        yield return new WaitForSeconds(0.2f);
        operation.allowSceneActivation = true;
    }
}