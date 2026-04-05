using UnityEngine;
using UnityEngine.SceneManagement; // Phải có dòng này mới gọi Scene được

public class ChuyenTrang : MonoBehaviour
{
    // Hàm này sẽ được gọi khi bấm nút
    public void VaoGame(string tenTrang)
    {
        // Chuyển sang trang có tên được nhập vào
        SceneManager.LoadScene(tenTrang);
    }
}