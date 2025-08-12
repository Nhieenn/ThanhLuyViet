# 🏗️ Tower Action Menu - Hướng dẫn sử dụng

## 📋 Tổng quan
TowerActionMenu là menu tương tác với tháp trong game Tower Defense, bao gồm:
- **Upgrade Button:** Nâng cấp tháp
- **Destroy Button:** Phá hủy tháp
- **Close Button:** Đóng menu (nút thoát mới)
- **Sell Value Text:** Hiển thị giá bán tháp

## 🚀 Cài đặt nhanh

### Bước 1: Setup tự động
1. Tạo GameObject mới trong scene
2. Thêm script `TowerActionMenuSetup`
3. Sử dụng Context Menu: **Create Tower Action Menu Prefab**
4. Gán prefab vào Tower script

### Bước 2: Setup thủ công
1. Tạo UI prefab với 3 buttons và 1 text
2. Gán các components vào TowerActionMenu script
3. Setup event listeners cho từng button

## 📁 Các Script chính

### 1. TowerActionMenu.cs
**Chức năng:** Xử lý logic menu tương tác với tháp
- **3 Buttons:** Upgrade, Destroy, Close
- **Sell Value:** Hiển thị giá bán tháp
- **Camera Control:** Tự động tắt/bật camera movement
- **Menu Management:** Tự động đóng menu khi cần

**Các components:**
```csharp
[Header("Action Buttons")]
public Button upgradeButton;    // Nút nâng cấp
public Button destroyButton;    // Nút phá hủy
public Button closeButton;      // Nút thoát (mới)

[Header("UI Elements")]
public Text sellValueText;      // Text hiển thị giá bán
```

### 2. TowerActionMenuSetup.cs
**Chức năng:** Tạo prefab menu tự động
- **Auto Creation:** Tạo menu với đầy đủ components
- **Customizable:** Có thể tùy chỉnh màu sắc, kích thước
- **TextMeshPro Support:** Sử dụng TextMeshPro cho text đẹp hơn

## 🎮 Cách sử dụng

### Setup cơ bản:
```csharp
// Trong Tower.cs, khi click vào tháp:
if (currentMenuInstance == null)
{
    currentMenuInstance = Instantiate(actionMenuPrefab, canvas.transform);
    currentMenuInstance.GetComponent<TowerActionMenu>().SetTargetTower(this);
}
```

### Tùy chỉnh menu:
```csharp
// Trong TowerActionMenuSetup Inspector
upgradeButtonColor = Color.green;    // Màu nút nâng cấp
destroyButtonColor = Color.red;      // Màu nút phá hủy
closeButtonColor = Color.gray;       // Màu nút thoát
fontSize = 18;                       // Kích thước chữ
```

### Đóng menu từ code:
```csharp
// Đóng menu từ bên ngoài
TowerActionMenu menu = FindObjectOfType<TowerActionMenu>();
if (menu != null)
{
    menu.CloseMenu();
}
```

## 🎨 Tùy chỉnh

### Thay đổi màu sắc buttons:
```csharp
// Trong TowerActionMenuSetup Inspector
upgradeButtonColor = new Color(0.2f, 0.8f, 0.2f, 1f); // Xanh lá
destroyButtonColor = new Color(0.8f, 0.2f, 0.2f, 1f); // Đỏ
closeButtonColor = new Color(0.5f, 0.5f, 0.5f, 1f);   // Xám
```

### Thay đổi kích thước menu:
```csharp
// Trong TowerActionMenuSetup.cs
rectTransform.sizeDelta = new Vector2(250, 180); // Rộng hơn, cao hơn
```

### Thay đổi vị trí buttons:
```csharp
// Trong CreateButton method
new Vector2(0, 50);   // Upgrade button cao hơn
new Vector2(0, 0);    // Destroy button ở giữa
new Vector2(0, -50);  // Close button thấp hơn
```

## 🧪 Testing

### Sử dụng Context Menu:
- **Create Tower Action Menu Prefab:** Tạo prefab menu
- **Test Menu Creation:** Test menu trong scene

### Test thủ công:
```csharp
// Tạo menu test
GameObject testMenu = Instantiate(towerActionMenuPrefab, canvas.transform);
testMenu.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
```

## 🔧 Troubleshooting

### Menu không hiển thị:
- Kiểm tra Canvas có trong scene không
- Kiểm tra TowerActionMenu script có được gán không
- Kiểm tra actionMenuPrefab có được gán vào Tower không

### Buttons không hoạt động:
- Kiểm tra các Button components có được gán không
- Kiểm tra event listeners có được setup không
- Kiểm tra targetTower có được set không

### Camera không di chuyển được:
- Kiểm tra Camera_move script có trong scene không
- Kiểm tra canMoveCamera có được set đúng không
- Kiểm tra EnableCameraMovement() có được gọi khi đóng menu không

### Text không hiển thị:
- Kiểm tra TextMeshProUGUI component có được gán không
- Kiểm tra font asset có được gán không
- Kiểm tra text có được set đúng không

## 📊 Performance Tips

1. **Menu Pooling:** Sử dụng object pooling cho menu nếu có nhiều tháp
2. **Event Cleanup:** Đảm bảo remove event listeners khi destroy menu
3. **UI Update:** Chỉ update sell value khi cần thiết
4. **Camera Control:** Sử dụng static methods để control camera

## 🎯 Tính năng mới

### ✅ **Nút Close (Thoát):**
- **Vị trí:** Dưới cùng của menu
- **Màu sắc:** Xám (có thể tùy chỉnh)
- **Chức năng:** Đóng menu và bật lại camera movement
- **Tương tác:** Có thể click hoặc gọi từ code

### ✅ **Camera Integration:**
- **Tự động tắt:** Camera movement bị tắt khi mở menu
- **Tự động bật:** Camera movement được bật khi đóng menu
- **Static Control:** Sử dụng Camera_move.EnableCameraMovement()

### ✅ **Menu Management:**
- **Auto Close:** Menu tự động đóng khi thực hiện action
- **Force Close:** Có thể đóng menu từ bên ngoài
- **Cleanup:** Tự động cleanup khi destroy

## 🎉 Kết luận

TowerActionMenu với nút Close cung cấp:
- ✅ **UX tốt hơn:** Người chơi có thể thoát menu dễ dàng
- ✅ **Camera Control:** Tự động quản lý camera movement
- ✅ **Setup tự động:** Tạo menu với TowerActionMenuSetup
- ✅ **Tùy chỉnh:** Có thể thay đổi màu sắc, kích thước
- ✅ **TextMeshPro Support:** Text đẹp và responsive
- ✅ **Performance:** Tối ưu cho Tower Defense game

Chỉ cần sử dụng `TowerActionMenuSetup` và gán prefab vào Tower là có ngay menu hoàn chỉnh với nút thoát! 🏰
