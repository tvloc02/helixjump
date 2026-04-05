using UnityEngine;
using UnityEngine.UI;

public class HieuUngZoom : MonoBehaviour
{
    public Transform tamZoom;
    public ScrollRect cuonMap;

    public float toNhat = 1.5f;
    public float nhoNhat = 1.0f;
    public float vungAnhHuong = 400f;
    public float tocDoHut = 15f;

    // Phải dùng LateUpdate để chạy SAU KHI Scroll View đã tính toán xong
    void LateUpdate()
    {
        if (tamZoom == null || cuonMap == null) return;

        float khoangCachNhoNhat = Mathf.Infinity;
        Transform levelGanNhat = null;

        // 1. Phóng to cục ở gần giữa (Giữ nguyên cấu trúc zic-zắc trục X của bạn)
        foreach (Transform level in transform)
        {
            float dist = Mathf.Abs(level.position.y - tamZoom.position.y);
            float tiLe = Mathf.Clamp01(1f - (dist / vungAnhHuong));
            float kichThuoc = Mathf.Lerp(nhoNhat, toNhat, tiLe);
            level.localScale = new Vector3(kichThuoc, kichThuoc, 1f);

            if (dist < khoangCachNhoNhat)
            {
                khoangCachNhoNhat = dist;
                levelGanNhat = level;
            }
        }

        // 2. Nhận diện thao tác NGUYÊN THỦY (Bỏ qua EventSystem rườm rà)
        // Chỉ cần ngón tay chạm vào màn hình (chuột trái hoặc cảm ứng) là đang kéo
        bool dangKeo = Input.GetMouseButton(0) || Input.touchCount > 0;

        // BẮT ĐẦU HÚT khi thỏa mãn 3 điều kiện: Thả tay ra + Có cục gần đó + Tốc độ trôi đã chậm lại
        if (!dangKeo && levelGanNhat != null && Mathf.Abs(cuonMap.velocity.y) < 300f)
        {
            cuonMap.StopMovement(); // Khóa cứng quán tính của Scroll View

            RectTransform contentRect = cuonMap.content;

            // Tính toán độ lệch an toàn tuyệt đối
            Vector3 tamLocal = contentRect.InverseTransformPoint(tamZoom.position);
            Vector3 levelLocal = contentRect.InverseTransformPoint(levelGanNhat.position);

            float lechY = tamLocal.y - levelLocal.y;

            // Dịch chuyển túi Content thẳng vào điểm giữa (Chỉ dịch chuyển trục Y, giữ nguyên trục X zic-zắc)
            if (Mathf.Abs(lechY) > 1f)
            {
                Vector2 posHienTai = contentRect.anchoredPosition;
                Vector2 posMucTieu = new Vector2(posHienTai.x, posHienTai.y + lechY);
                contentRect.anchoredPosition = Vector2.Lerp(posHienTai, posMucTieu, Time.deltaTime * tocDoHut);
            }
        }
    }
}