using UnityEngine;
using UnityEngine.UI;

public class HieuUngRankToanTap : MonoBehaviour
{
    [Header("CÀI ĐẶT CHUNG")]
    public bool laRankHienTai = false;

    // Màu Xanh Lơ đặc trưng của Rank Diamond, độ mờ nhẹ nhàng
    public Color mauCuaRank = new Color(0f, 1f, 1f, 0.25f);
    public float tiLePhongTo = 1.05f; // Phóng to nhẹ 5%

    [Header("ĐIỂM GẮN HÀO QUANG")]
    public Transform diemGanCup;
    public Transform diemGanRuong;

    [Header("HAI MŨI TÊN (Cũ)")]
    public RectTransform muiTenTrai;
    public RectTransform muiTenPhai;
    public float khoangCachThutTho = 10f;
    public float tocDoMuiTen = 4f;

    [Header("CẤU HÌNH HÀO QUANG (Tam giác mờ dần)")]
    public int soTiaHaoQuang = 8;     // 8 tam giác
    public float chieuDaiTia = 200f;  // Chiều dài
    public float doDayTia = 60f;      // Bề ngang tam giác (để to hơn chút)
    public float tocDoXoay = -15f;    // Xoay chậm rãi

    private GameObject cumCup;
    private GameObject cumRuong;
    private Vector2 viTriGocTrai;
    private Vector2 viTriGocPhai;

    void Start()
    {
        // 1. Sao lưu vị trí mũi tên
        if (muiTenTrai != null) viTriGocTrai = muiTenTrai.anchoredPosition;
        if (muiTenPhai != null) viTriGocPhai = muiTenPhai.anchoredPosition;

        // 2. Kiểm tra nếu không phải rank hiện tại
        if (!laRankHienTai)
        {
            if (muiTenTrai != null) muiTenTrai.gameObject.SetActive(false);
            if (muiTenPhai != null) muiTenPhai.gameObject.SetActive(false);
            return;
        }

        // 3. Hiệu ứng phóng to nhẹ
        transform.localScale = Vector3.one * tiLePhongTo;

        // 4. BỘ MÁY CHÍNH: Tự động vẽ tam giác mờ dần và lót xuống dưới đáy
        if (diemGanCup != null) cumCup = TuDongVeTamGiacMoDan(diemGanCup);
        if (diemGanRuong != null) cumRuong = TuDongVeTamGiacMoDan(diemGanRuong);

        // 5. Hiện mũi tên
        if (muiTenTrai != null) muiTenTrai.gameObject.SetActive(true);
        if (muiTenPhai != null) muiTenPhai.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!laRankHienTai) return;

        // Xoay hào quang chậm rãi
        if (cumCup != null) cumCup.transform.Rotate(0, 0, tocDoXoay * Time.deltaTime);
        if (cumRuong != null) cumRuong.transform.Rotate(0, 0, tocDoXoay * Time.deltaTime);

        // Mũi tên thụt thò (Cũ)
        if (muiTenTrai != null && muiTenPhai != null)
        {
            float nhip = Mathf.Sin(Time.time * tocDoMuiTen) * khoangCachThutTho;
            muiTenTrai.anchoredPosition = new Vector2(viTriGocTrai.x + nhip, viTriGocTrai.y);
            muiTenPhai.anchoredPosition = new Vector2(viTriGocPhai.x - nhip, viTriGocPhai.y);
        }
    }

    // BỘ MÁY CHÍNH ĐỂ VẼ TAM GIÁC MỜ DẦN VÀ LÓT XUỐNG ĐÁY
    GameObject TuDongVeTamGiacMoDan(Transform diemNeo)
    {
        // Tạo trục xoay trung tâm
        GameObject trucXoay = new GameObject("HaoQuang_TamGiac_Root");
        trucXoay.transform.SetParent(diemNeo, false);
        trucXoay.transform.localPosition = Vector3.zero;

        // Góc chia cho 8 tia (360 / 8 = 45 độ)
        float gocChia = 360f / soTiaHaoQuang;

        // BÍ KÍP VẼ TAM GIÁC: Dùng RawImage và thủ thuật 'Tiling' để biến hình chữ nhật thành tam giác
        for (int i = 0; i < soTiaHaoQuang; i++)
        {
            GameObject tia = new GameObject("Tia_" + i);
            tia.transform.SetParent(trucXoay.transform, false);

            // 1. Thêm RawImage
            RawImage img = tia.AddComponent<RawImage>();

            // THỦ THUẬT QUAN TRỌNG: Thiết lập Texture là 'Knob' (icon tròn mờ mặc định của Unity)
            // để tạo độ mờ dần từ tâm ra ngoài. Nếu không có Knob, bạn có thể tạo ảnh trắng tinh.
            img.texture = Resources.GetBuiltinResource<Texture>("Knob.psd");

            // Thiết lập màu sắc (mờ ảo lấy từ trên xuống)
            img.color = mauCuaRank;

            // 2. Chỉnh hình dạng Tam Giác và vị trí
            RectTransform rt = img.GetComponent<RectTransform>();

            // Kích thước (bề ngang to hơn để trông giống tam giác)
            rt.sizeDelta = new Vector2(doDayTia, chieuDaiTia);

            // Đặt đỉnh tại tâm xoay: Đặt Pivot là giữa cạnh dưới
            rt.pivot = new Vector2(0.5f, 0f);

            // Đặt vị trí chính xác tại tâm
            rt.anchoredPosition = Vector2.zero;

            // Xoay tia sáng
            rt.localEulerAngles = new Vector3(0, 0, i * gocChia);
        }

        // BÍ KÍP ĐÂY: Đẩy toàn bộ trục xoay hào quang xuống dưới đáy Cúp/Rương
        // Nhờ dòng này, hào quang sẽ không đè lên Cúp/Rương nữa.
        trucXoay.transform.SetAsFirstSibling();

        return trucXoay;
    }
}