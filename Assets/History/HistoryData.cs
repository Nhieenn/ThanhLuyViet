using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct HistoryInfo
{
    public string battleName;
    public string forceComparison;
    public string battleMeaning;
}

public class HistoryData : MonoBehaviour
{
    public static HistoryData Instance;

    // Gán dữ liệu cho từng scene (màn chơi)
    private Dictionary<string, HistoryInfo> historyDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitHistoryData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitHistoryData()
    {
        historyDict = new Dictionary<string, HistoryInfo>();
        // Ví dụ: tên scene là "Lv1", "Lv2", ...
        historyDict["Lv1"] = new HistoryInfo
        {
            battleName = "Trận Ấp Bắc (2/1/1963)",
            forceComparison = "Ta: Tiểu đoàn 514 QGP, du kích địa phương.\r\n\r\nĐịch: VNCH (~2.000 người) có cố vấn Mỹ, xe M113, trực thăng.\r\n\r\nThương vong: Địch ~83 chết (3 Mỹ), 22 xe M113 hỏng; Ta ~18 hy sinh.",
            battleMeaning = "Lần đầu lực lượng du kích thắng thế quân ngụy, dù chúng mạnh hơn gấp nhiều lần, mở ra giai đoạn kháng chiến vững chắc bằng thế trận du kích."
        };
        historyDict["Lv2"] = new HistoryInfo
        {
            battleName = "Trận Đồng Xoài (6/1965)",
            forceComparison = "Ta: QGP chủ lực miền Đông Nam Bộ.\r\n\r\nĐịch: ngụy Biệt Động Quân, pháo và trực thăng yểm trợ từ Mỹ.\r\n\r\nThương vong: Địch hàng trăm chết; Ta ~100 hy sinh.",
            battleMeaning = "Gây sốc tinh thần đối phương, buộc Mỹ tăng cường tham chiến trực tiếp."
        };
        historyDict["Lv3"] = new HistoryInfo
        {
            battleName = "Trận Đức Cơ (8–10/1965)",
            forceComparison = "Ta: Trung đoàn 320 QGP.\r\n\r\nĐịch: quân ngụy đồn đóng tại trại Đức Cơ; có lính Mỹ hỗ trợ tại chốt.\r\n\r\nThương vong: Không rõ chính xác, nhưng địch thiệt hại nặng.",
            battleMeaning = "Bài học về địa hình cao nguyên — ta cố thủ xen kẽ địa đạo, hạn chế ưu thế Mỹ"
        };
        historyDict["Lv4"] = new HistoryInfo
        {
            battleName = "Trận Plei Me (10/1965)",
            forceComparison = "Ta: QGP Tây Nguyên (Trung đoàn 32, 33, có lực lượng PAVN hỗ trợ).\r\n\r\nĐịch: Đặc nhiệm Mỹ, CIDG, ARVN phản công.\r\n\r\nThương vong: Địch ~600–1.000 thiệt hại; Ta ~500 hy sinh.",
            battleMeaning = "Mở màn lục quân Mỹ đánh lớn tại Việt Nam — quyết định có đưa quân trực tiếp."
        };
        historyDict["Lv5"] = new HistoryInfo
        {
            battleName = "Trận Bến Củi (9/1966)",
            forceComparison = "Ta: Trung đoàn 1, Sư đoàn 9 QGP.\r\n\r\nĐịch: Sư đoàn Mỹ 25, quân số khoảng 22.000.\r\n\r\nThương vong: Mỹ ~155 chết, 200+ mất tích; ta cũng thiệt hại nặng.",
            battleMeaning = "Mỹ thất bại chiến thuật “Tìm-diệt” – ta khẳng định hiệu quả tác chiến rừng sâu."
        };
        historyDict["Lv6"] = new HistoryInfo
        {
            battleName = "Trận Khe Sanh (1–7/1968)",
            forceComparison = "Ta: Sư đoàn 304, 325 QĐNDVN.\r\n\r\nĐịch: 6.000 TQLC Mỹ + ARVN cố thủ tại Khe Sanh.\r\n\r\nThương vong: Địch ~205 chết, ~1.600 thương; ta có thể mất tới ~15.000.",
            battleMeaning = "Phòng thủ ngoạn mục kết hợp không quân – pháo binh mạnh; áp lực chiến lược tạo “hình mẫu Điện Biên Phủ hiện đại”."
        };
        historyDict["Lv7"] = new HistoryInfo
        {
            battleName = "Trận Tà Cơn (1968)",
            forceComparison = "Ta: Bộ đội QĐNDVN tấn công và phá boong-ke Mỹ tại căn cứ Tà Cơn gần Khe Sanh.\r\n\r\nĐịch: Quân Mỹ và ngụy giữ căn cứ Tà Cơn.\r\n\r\nThương vong: Địch thiệt hại lớn; ta tấn công thành công, số liệu ước tính cao.",
            battleMeaning = "Phân li lịch sử Khe Sanh thành 2 giai đoạn: vây Khe Sanh và tiến công Tà Cơn — kết hợp pháo binh và bộ đội chủ lực để giải toả căn cứ địch.\r\n\r\n"
        };
        historyDict["Lv8"] = new HistoryInfo
        {
            battleName = "Trận Cầu Bông – Cầu Xáng (Tết Mậu Thân 1968)",
            forceComparison = "Ta: Biệt động, du kích QGP nội đô Sài Gòn.\r\n\r\nĐịch: Cảnh sát dã chiến nụy, biệt kích và lính dù; sau đó Mỹ tham gia hỗ trợ.\r\n\r\nThương vong: Dữ liệu không rõ, nhưng giao tranh đô thị căng thẳng.",
            battleMeaning = "Mở đầu chiến thuật “du kích đô thị”, thể hiện tinh thần quật cường giữa nội thành"
        };
        historyDict["Lv9"] = new HistoryInfo
        {
            battleName = "Trận An Lộc (4–7/1972)",
            forceComparison = "Ta: Quân Giải phóng miền Đông (Sư đoàn 7, 9...).\r\n\r\nĐịch: quân ngụy cố thủ An Lộc, Mỹ hỗ trợ bằng không quân và pháo binh.\r\n\r\nThương vong: PAVN ~10.000 chết; ARVN ~2.000 chết, ~5.000 thương.",
            battleMeaning = "Bài kiểm định sức mạnh không quân Mỹ hỗ trợ đô thị — ta vẫn kiên trì chiến đấu, làm chậm bước tiến vào Sài Gòn."
        };
        historyDict["Lv10"] = new HistoryInfo
        {
            battleName = "Điện Biên Phủ trên không (12/1972)",
            forceComparison = "Ta: QĐNDVN phòng không (SAM, MIG, pháo cao xạ) bảo vệ Hà Nội.\r\n\r\nĐịch: Không quân chiến lược Mỹ gồm 207 máy bay (B-52, F‑4, F‑111).",
            battleMeaning = "Phòng không Việt Nam buộc Mỹ ngừng ném bom, mở đường cho đàm phán Paris — chiến thắng chiến lược quan trọng."
        };
    }

    public HistoryInfo GetHistoryForScene(string sceneName)
    {
        if (historyDict != null && historyDict.ContainsKey(sceneName))
            return historyDict[sceneName];
        // Nếu không có, trả về rỗng
        return new HistoryInfo { battleName = "", forceComparison = "", battleMeaning = "" };
    }
}