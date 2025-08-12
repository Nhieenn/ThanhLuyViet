# 🪙 Coin System - Tower Defense

**Chức năng:** Quản lý tiền tệ chính của game Tower Defense

## 🎯 **Tính năng chính:**

### **💰 Quản lý Coin**
- **Reset mỗi lần Play/Restart:** Luôn bắt đầu với `startingCoins` (mặc định: 500)
- **Hiển thị số coin trên UI với TextMeshPro**
- **Lưu trữ persistent với PlayerPrefs**
- **Direct Coin Addition:** Thêm coin trực tiếp khi enemy chết (không cần nhặt)

### **🛡️ Validation & Protection**
- **CanAfford():** Kiểm tra có đủ tiền để mua/nâng cấp không
- **GetMissingAmount():** Tính số tiền còn thiếu
- **TrySpendCoins():** Chi tiêu an toàn với validation
- **Thông báo rõ ràng:** Log warning khi không đủ tiền

### **🎮 Game Integration**
- **Thêm coin khi enemy chết:** `AddCoinsFromEnemy()`
- **Mua tháp:** Validation trong `BuildableArea.BuildTower()`
- **Nâng cấp tháp:** Validation trong `Tower.Upgrade()`
- **Bán tháp:** Hoàn lại tiền qua `Tower.DestroyTower()`

## 📁 **Scripts chính:**

### **CoinManager.cs**
- **Singleton pattern:** Một instance duy nhất
- **DontDestroyOnLoad:** Hoạt động across scenes
- **Reset mỗi lần Play:** Luôn bắt đầu với `startingCoins`
- **TextMeshPro Support:** Tương thích với TMP UI
- **Context Menu:** Tools để testing và debug

### **CoinUI.cs**
- **TextMeshPro Support:** Hiển thị số coin với TMP
- **Animation Trigger:** Hiệu ứng khi thêm/chi tiêu coin
- **Auto-find TMP:** Tự động tìm TextMeshProUGUI component

### **CoinSystemSetup.cs**
- **Auto Setup:** Tạo UI tự động cho Tower Defense
- **TextMeshPro Ready:** Tạo TMP components
- **Context Menu:** Quick setup trong Editor

## 🔧 **Setup Instructions:**

### **1. Cơ bản:**
```csharp
// Thêm CoinManager vào scene đầu tiên
// Set Starting Coins = 100 (hoặc giá trị mong muốn)
// Assign TextMeshProUGUI component cho coinText
```

### **2. Tự động:**
```csharp
// Thêm CoinSystemSetup vào scene
// Right-click → "Setup Coin System"
// Script sẽ tạo UI tự động
```

### **3. Manual:**
```csharp
// Tạo UI với TextMeshProUGUI
// Gọi CoinManager.Instance.SetCoinUI(coinText, coinIcon)
```

## 🎮 **Usage Examples:**

### **Thêm coin khi enemy chết:**
```csharp
CoinManager.Instance.AddCoinsFromEnemy(25);
```

### **Kiểm tra đủ tiền:**
```csharp
if (CoinManager.Instance.CanAfford(100))
{
    // Có thể mua tháp
}
else
{
    int missing = CoinManager.Instance.GetMissingAmount(100);
    Debug.Log($"Thiếu {missing} coins!");
}
```

### **Chi tiêu an toàn:**
```csharp
if (CoinManager.Instance.TrySpendCoins(50))
{
    // Mua thành công
}
else
{
    // Không đủ tiền
}
```

## 🧪 **Testing Tools:**

### **CoinSystemTester.cs**
- **Test Keys:** 1,2,3,4,R,C,F,A
- **Context Menu:** Test từng chức năng
- **OnGUI Display:** Hiển thị status real-time

### **CoinDebugger.cs**
- **Auto Check:** Kiểm tra khi Start
- **Fix Issue:** Sửa vấn đề startingCoins
- **Test Key:** Phím X để sửa nhanh

## ⚙️ **Settings:**

### **CoinManager Inspector:**
- **Starting Coins:** Số coin bắt đầu (mặc định: 500)
- **Coin Text:** TextMeshProUGUI component
- **Coin Icon:** GameObject icon (optional)
- **Coin Collect Sound:** AudioClip (optional)

### **Reset Behavior:**
- **Mỗi lần Play:** Luôn reset về `startingCoins`
- **Không lưu progress:** Phù hợp cho Tower Defense
- **Clean slate:** Mỗi game session mới

## 🐛 **Troubleshooting:**

### **Coin không được thêm khi enemy chết:**
- Kiểm tra `Enemy.cs` có gọi `AddCoinsFromEnemy()`
- Đảm bảo `CoinManager.Instance` tồn tại

### **TextMeshPro không hiển thị:**
- Kiểm tra `CoinUI.cs` có assign `TextMeshProUGUI`
- Đảm bảo TMP component được setup đúng

### **Không đủ tiền mua/nâng cấp:**
- Sử dụng `CanAfford()` để kiểm tra trước
- Xem log để biết số tiền còn thiếu

### **Starting coins không đúng:**
- Kiểm tra `Starting Coins` trong Inspector
- Sử dụng "Check and Fix Starting Coins" Context Menu

## 🎯 **Đặc điểm Tower Defense:**
- **Reset mỗi lần Play:** Phù hợp với game TD
- **Validation chặt chẽ:** Không cho mua khi thiếu tiền
- **Performance tốt:** Không cần object pooling
- **UI responsive:** Cập nhật real-time

## 📝 **Conclusion:**
Hệ thống coin được tối ưu cho Tower Defense với TextMeshPro support, validation chặt chẽ, và reset behavior phù hợp. Tất cả logic đã được đồng bộ và test đầy đủ.
