using UnityEngine;

public class UIShopManager : MonoBehaviour
{
    [Header("DANH SÁCH CÁC PANEL")]
    public GameObject trangChu;    // Kéo cái Scroll View hoặc Menu chính vào đây
    public GameObject trangLeague;  // Kéo cái Panel League bạn đã làm vào đây

    // Hàm này để gọi khi bấm nút League
    public void MoTrangLeague()
    {
        trangChu.SetActive(false);   // Ẩn trang chủ
        trangLeague.SetActive(true); // Hiện trang league
    }

    // Hàm này để gọi khi bấm nút X (Đóng) ở trang League
    public void VeTrangChu()
    {
        trangChu.SetActive(true);    // Hiện lại trang chủ
        trangLeague.SetActive(false); // Ẩn trang league
    }
}