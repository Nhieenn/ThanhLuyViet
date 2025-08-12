# 🎮 Hệ Thống Win/Lose Hoàn Chỉnh

## 📋 Tổng Quan

Hệ thống win/lose mới cung cấp logic hoàn chỉnh để xử lý các trạng thái thắng/thua trong game Tower Defense, bao gồm:

- ✅ **Win Conditions**: Hoàn thành wave, tiêu diệt enemy, thời gian
- ❌ **Lose Conditions**: Hết máu, quá thời gian
- ⏸️ **Pause System**: Tạm dừng game với UI đầy đủ
- 🎯 **Game State Management**: Quản lý trạng thái game tập trung
- 💾 **Progress Saving**: Tự động lưu tiến độ

## 🏗️ Cấu Trúc Hệ Thống

### 1. **GameWinManager.cs** - Quản lý Win/Lose chính
- Xử lý điều kiện thắng/thua
- Hiển thị màn hình Victory/Game Over
- Quản lý UI và button
- Tích hợp với hệ thống khác

### 2. **GameStateManager.cs** - Quản lý trạng thái game
- Quản lý các trạng thái: Playing, Paused, Won, Lost
- Xử lý input (Escape, R, N, M)
- Tính toán điểm số và thời gian
- Lưu tiến độ game

### 3. **SimpleHealthText.cs** - Hệ thống máu người chơi
- Quản lý máu và damage
- Tích hợp với GameWinManager
- Events cho thay đổi máu và cái chết

### 4. **EndPosDamage.cs** - Xử lý damage khi enemy đến đích
- Hiệu ứng damage popup
- Camera shake
- Âm thanh damage
- Tích hợp với hệ thống máu

### 5. **PauseMenuUI.cs** - Menu tạm dừng
- UI pause game hoàn chỉnh
- Settings panel
- Confirm dialogs
- Tích hợp với GameStateManager

## 🚀 Cách Sử Dụng

### **Bước 1: Setup cơ bản**

1. **Thêm GameStateManager vào scene:**
   ```csharp
   // Tự động tìm các component cần thiết
   // Hoặc gán thủ công trong Inspector
   ```

2. **Thêm GameWinManager vào scene:**
   ```csharp
   // Có thể để autoSetupUI = true để tự tạo UI
   ```

3. **Thêm PauseMenuUI vào scene:**
   ```csharp
   // Tự động tạo UI pause menu
   ```

### **Bước 2: Cấu hình Win Conditions**

Trong **GameWinManager**:
```csharp
[Header("Win Conditions")]
public bool checkWaveCompletion = true;    // Thắng khi hoàn thành wave
public bool checkEnemyElimination = true;  // Thắng khi tiêu diệt hết enemy
public bool checkTimeLimit = false;        // Thắng trong thời gian nhất định
public float timeLimit = 300f;            // 5 phút
```

### **Bước 3: Cấu hình Lose Conditions**

```csharp
[Header("Lose Conditions")]
public bool checkPlayerHealth = true;      // Thua khi hết máu
public bool checkTimeLimitLose = false;    // Thua khi quá thời gian
public float loseTimeLimit = 600f;        // 10 phút
```

### **Bước 4: Tích hợp với EnemySpawner**

```csharp
// Trong EnemySpawner, gọi khi wave hoàn thành:
GameWinManager gameWinManager = FindObjectOfType<GameWinManager>();
if (gameWinManager != null)
{
    gameWinManager.WaveCompleted();
}
```

### **Bước 5: Tích hợp với Enemy**

```csharp
// Trong Enemy, gọi khi enemy bị tiêu diệt:
GameWinManager gameWinManager = FindObjectOfType<GameWinManager>();
if (gameWinManager != null)
{
    gameWinManager.EnemyEliminated();
}
```

## 🎯 Các Điều Kiện Thắng

### **1. Wave Completion**
- Hoàn thành tất cả wave trong level
- Tự động kiểm tra khi gọi `WaveCompleted()`

### **2. Enemy Elimination**
- Tiêu diệt hết enemy trong scene
- Kiểm tra sau khi gọi `EnemyEliminated()`

### **3. Time Limit**
- Hoàn thành level trong thời gian nhất định
- Có thể bật/tắt trong Inspector

## 💀 Các Điều Kiện Thua

### **1. Player Health Depleted**
- Máu người chơi về 0
- Tự động kích hoạt khi enemy đến EndPos
- Tích hợp với `SimpleHealthText`

### **2. Time Limit Exceeded**
- Vượt quá thời gian cho phép
- Có thể bật/tắt trong Inspector

## ⏸️ Hệ Thống Pause

### **Phím tắt:**
- **Escape**: Pause/Resume game
- **R**: Restart level
- **N**: Next level (chỉ khi thắng)
- **M**: Main menu

### **UI Pause Menu:**
- Resume: Tiếp tục game
- Settings: Cài đặt âm thanh, fullscreen
- Restart: Khởi động lại level
- Main Menu: Về menu chính
- Quit: Thoát game

## 🎮 Game States

### **Playing**
- Game đang chạy bình thường
- Time.timeScale = 1

### **Paused**
- Game tạm dừng
- Time.timeScale = 0
- Hiển thị pause menu

### **Won**
- Level hoàn thành
- Hiển thị victory screen
- Tự động lưu tiến độ

### **Lost**
- Level thất bại
- Hiển thị game over screen
- Có thể retry hoặc về main menu

## 💾 Lưu Tiến Độ

### **Tự động lưu:**
- Level cao nhất đã hoàn thành
- Trạng thái hoàn thành từng level
- Cài đặt game (âm thanh, fullscreen)

### **PlayerPrefs keys:**
```csharp
"HighestLevel"           // Level cao nhất đã hoàn thành
"Level{X}Completed"      // Level X đã hoàn thành
"MusicVolume"           // Âm lượng nhạc
"SFXVolume"             // Âm lượng hiệu ứng
"Fullscreen"            // Chế độ fullscreen
```

## 🔧 Tùy Chỉnh

### **Victory Screen:**
- Thay đổi text, màu sắc
- Thêm particle effects
- Tùy chỉnh button layout

### **Game Over Screen:**
- Thay đổi thông báo
- Thêm hiệu ứng âm thanh
- Tùy chỉnh button actions

### **Pause Menu:**
- Thêm/bớt button
- Tùy chỉnh layout
- Thêm animation

## 🧪 Testing

### **Context Menu Tests:**
```csharp
[ContextMenu("Test Victory")]      // Test thắng
[ContextMenu("Test Game Over")]    // Test thua
[ContextMenu("Test Pause")]        // Test pause
[ContextMenu("Test Resume")]       // Test resume
```

### **Debug UI:**
- Hiển thị thông tin game state
- Button test các chức năng
- Thông tin level, thời gian, điểm số

## 📱 Tích Hợp Mobile

### **Touch Controls:**
- UI button responsive
- Touch-friendly button size
- Swipe gestures (có thể thêm)

### **Mobile Optimization:**
- UI scale phù hợp với resolution
- Performance optimization
- Battery-friendly

## 🎨 UI Customization

### **Victory Panel:**
```csharp
public GameObject victoryPanel;
public TextMeshProUGUI victoryText;
public TextMeshProUGUI statsText;
public Button nextLevelButton;
public Button mainMenuButton;
public Button restartButton;
```

### **Game Over Panel:**
```csharp
public GameObject gameOverPanel;
public TextMeshProUGUI gameOverText;
public Button retryButton;
public Button mainMenuButton;
public Button restartButton;
```

### **Pause Panel:**
```csharp
public GameObject pausePanel;
public Button resumeButton;
public Button settingsButton;
public Button restartButton;
public Button mainMenuButton;
public Button quitButton;
```

## 🚨 Troubleshooting

### **Common Issues:**

1. **UI không hiển thị:**
   - Kiểm tra Canvas trong scene
   - Đảm bảo autoSetupUI = true
   - Kiểm tra Camera.main

2. **Game không pause:**
   - Kiểm tra Time.timeScale
   - Đảm bảo GameStateManager được gán
   - Kiểm tra input handling

3. **Win/Lose không kích hoạt:**
   - Kiểm tra điều kiện trong Inspector
   - Đảm bảo gọi đúng method
   - Kiểm tra Debug.Log

### **Debug Tips:**
- Sử dụng OnGUI debug panels
- Kiểm tra Console logs
- Test với Context Menu
- Verify component references

## 🔮 Tính Năng Tương Lai

### **Có thể thêm:**
- Achievement system
- Leaderboard
- Multiple difficulty levels
- Custom win/lose conditions
- Tutorial system
- Analytics tracking

---

## 📞 Hỗ Trợ

Nếu gặp vấn đề hoặc cần hỗ trợ:
1. Kiểm tra Console logs
2. Verify component setup
3. Test với Context Menu
4. Kiểm tra Inspector settings

**Hệ thống này được thiết kế để dễ dàng tích hợp và tùy chỉnh cho game Tower Defense của bạn! 🎮✨**


