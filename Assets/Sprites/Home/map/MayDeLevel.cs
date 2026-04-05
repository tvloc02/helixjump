using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MayDeLevel : MonoBehaviour
{
    [Header("--- 1. Cấu hình Level ---")]
    public GameObject levelPrefab;
    public int tongSoLevel = 100;

    // Đã đổi mặc định về 1 cho bạn dễ ghép game thực tế
    public int levelDangChoi = 1;
    public float khoangCachY = 800f;
    public float doNgoanNgoeo = 180f;

    [Header("--- 2. Tùy chỉnh vị trí Chữ ---")]
    public float doXechCuaChu = 60f;

    [Header("--- 3. Hình ảnh Trạng thái ---")]
    public Sprite anhDaChoi;
    public Sprite anhHienTai;
    public Sprite anhChuaChoi;

    [Header("--- 4. Nút bấm thông minh ---")]
    public GameObject nutPlay;
    // BẠN CÓ THỂ ĐỔI SỐ NÀY TRONG INSPECTOR (Ví dụ: 3 tức là cách 3 level sẽ cất nút Play)
    public float lechDeAnPlay = 3f;

    public GameObject nutQuayVe;
    // BẠN CÓ THỂ ĐỔI SỐ NÀY TRONG INSPECTOR (Ví dụ: 6 tức là cách 6 level sẽ hiện mũi tên)
    public float lechDeHienMuiTen = 6f;
    public RectTransform muiTenIcon;

    [Header("--- 5. Tham chiếu hệ thống UI ---")]
    public ScrollRect cuonMap;
    public Transform tamZoom;

    private GameObject cucHienTai;
    private RectTransform contentRect;

    void Start()
    {
        if (levelPrefab == null) return;

        // Cầu chì an toàn: Lỡ bạn gõ nhầm số âm hoặc quá 100 thì nó tự sửa
        levelDangChoi = Mathf.Clamp(levelDangChoi, 1, tongSoLevel);

        contentRect = GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, (tongSoLevel + 1) * khoangCachY);

        for (int i = 1; i <= tongSoLevel; i++)
        {
            GameObject cucMoi = Instantiate(levelPrefab, transform);
            cucMoi.name = "Level_" + i;

            RectTransform rt = cucMoi.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 0f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(Mathf.Sin(i * 0.8f) * doNgoanNgoeo, i * khoangCachY);

            if (i == levelDangChoi) cucHienTai = cucMoi;

            TextMeshProUGUI chuSo = cucMoi.GetComponentInChildren<TextMeshProUGUI>();
            Image hinhAnh = cucMoi.GetComponent<Image>();
            Transform oKhoaTransform = cucMoi.transform.Find("O_Khoa");
            GameObject oKhoa = (oKhoaTransform != null) ? oKhoaTransform.gameObject : null;

            if (chuSo != null) chuSo.text = i.ToString();

            float yGoc = (chuSo != null) ? chuSo.rectTransform.anchoredPosition.y : 0;

            if (hinhAnh != null)
            {
                if (i < levelDangChoi)
                {
                    hinhAnh.sprite = anhDaChoi;
                    if (oKhoa != null) oKhoa.SetActive(false);
                    if (chuSo != null)
                    {
                        chuSo.color = Color.white;
                        chuSo.rectTransform.anchoredPosition = new Vector2(chuSo.rectTransform.anchoredPosition.x, yGoc);
                    }
                }
                else if (i == levelDangChoi)
                {
                    hinhAnh.sprite = anhHienTai;
                    if (oKhoa != null) oKhoa.SetActive(false);
                    if (chuSo != null)
                    {
                        chuSo.color = Color.black;
                        chuSo.rectTransform.anchoredPosition = new Vector2(chuSo.rectTransform.anchoredPosition.x, yGoc);
                    }
                }
                else
                {
                    hinhAnh.sprite = anhChuaChoi;
                    if (oKhoa != null) oKhoa.SetActive(true);
                    if (chuSo != null)
                    {
                        chuSo.color = new Color(1, 1, 1, 0.4f);
                        chuSo.rectTransform.anchoredPosition = new Vector2(chuSo.rectTransform.anchoredPosition.x, yGoc + doXechCuaChu);
                    }
                }
            }
        }

        FocusToCurrentLevel(true);
    }

    void Update()
    {
        if (cucHienTai == null || tamZoom == null || contentRect == null) return;

        Vector3 tamLocal = contentRect.InverseTransformPoint(tamZoom.position);
        Vector3 levelLocal = contentRect.InverseTransformPoint(cucHienTai.transform.position);
        float lechY = tamLocal.y - levelLocal.y;

        float nguongAnPlay = khoangCachY * lechDeAnPlay;
        float nguongHienMuiTen = khoangCachY * lechDeHienMuiTen;

        if (nutPlay != null)
        {
            if (Mathf.Abs(lechY) > nguongAnPlay) nutPlay.SetActive(false);
            else nutPlay.SetActive(true);
        }

        if (nutQuayVe != null)
        {
            if (Mathf.Abs(lechY) > nguongHienMuiTen)
            {
                // Bật mũi tên (Đã xóa dòng code gây lỗi tàng hình)
                nutQuayVe.SetActive(true);

                if (muiTenIcon != null)
                {
                    // Dùng localEulerAngles xoay 180 độ là an toàn nhất
                    // lechY > 0 (Màn đang chơi ở dưới) -> Chỉ xuống (0 độ)
                    // lechY < 0 (Màn đang chơi ở trên) -> Chỉ lên (180 độ)
                    muiTenIcon.localEulerAngles = new Vector3(0, 0, lechY > 0 ? 0 : 180);
                }
            }
            else
            {
                nutQuayVe.SetActive(false);
            }
        }
    }

    public void FocusToCurrentLevel(bool instant = false)
    {
        if (cucHienTai == null || contentRect == null) return;

        Vector3 tamLocal = contentRect.InverseTransformPoint(tamZoom.position);
        Vector3 levelLocal = contentRect.InverseTransformPoint(cucHienTai.transform.position);
        float lechY = tamLocal.y - levelLocal.y;

        Vector2 posMoi = new Vector2(contentRect.anchoredPosition.x, contentRect.anchoredPosition.y + lechY);

        if (instant) contentRect.anchoredPosition = posMoi;
        else StartCoroutine(SmoothScroll(posMoi));
    }

    System.Collections.IEnumerator SmoothScroll(Vector2 target)
    {
        float t = 0;
        Vector2 startPos = contentRect.anchoredPosition;
        if (cuonMap != null) cuonMap.StopMovement();

        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            contentRect.anchoredPosition = Vector2.Lerp(startPos, target, t);
            yield return null;
        }
    }
}