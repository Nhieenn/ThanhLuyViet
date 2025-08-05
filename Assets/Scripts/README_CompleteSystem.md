# Hệ thống Tower Defense Game - Hướng dẫn hoàn chỉnh

## 🎯 Tổng quan
Hệ thống này bao gồm tất cả các thành phần cần thiết cho một game Tower Defense hoàn chỉnh:
- **Path System**: Đường đi cho enemy
- **Wave System**: Hệ thống wave quái
- **Enemy System**: Các loại enemy đa dạng
- **Tower System**: Hệ thống tháp phòng thủ
- **Game Management**: Quản lý level, wave, và gameplay

## 🚀 Setup nhanh

### Bước 1: Setup tự động (Khuyến nghị)
1. Tạo một GameObject mới trong scene
2. Thêm script `GameSystemSetup`
3. Chạy game → Hệ thống sẽ tự động setup mọi thứ

### Bước 2: Setup thủ công
Nếu muốn setup từng phần riêng biệt:

#### Setup Path System:
```csharp
// Thêm PathSystemSetup script
// Hoặc sử dụng PathManager trực tiếp
```

#### Setup Enemy Data:
```csharp
// Thêm EnemyDataCreator script
// Sẽ tạo 8 loại enemy khác nhau
```

#### Setup Wave System:
```csharp
// Thêm WaveCreator script
// Sẽ tạo 10 levels với 5 waves mỗi level
```

## 📁 Các Script chính

### 1. GameSystemSetup.cs
**Chức năng:** Setup toàn bộ hệ thống game
- Tự động tạo Path, Wave, Enemy systems
- Cấu hình mặc định cho game
- Test và debug toàn bộ hệ thống

### 2. PathManager.cs
**Chức năng:** Quản lý đường đi cho enemy
- 5 loại path: Straight, Curved, ZigZag, Spiral, Random
- Tự động tạo waypoints và visual path
- Tích hợp với EnemyPathfinding

### 3. EnemyPathfinding.cs
**Chức năng:** Điều khiển enemy di chuyển
- Smooth movement và rotation
- Tự động tìm PathManager
- Debug visualization

### 4. WaveCreator.cs
**Chức năng:** Tạo wave quái tự động
- 5 wave templates: Infantry Rush, Mixed Infantry, Tank Introduction, Air Attack, Boss Wave
- Scaling difficulty theo level
- Tích hợp với LevelWaveManager

### 5. EnemyDataCreator.cs
**Chức năng:** Tạo EnemyData ScriptableObjects
- 8 loại enemy: Infantry, Tank, Aircraft, Boss, Elite Infantry, Heavy Tank, Fighter Jet, Mega Boss
- Tự động set properties theo loại
- Lưu thành asset files

### 6. LevelWaveManager.cs
**Chức năng:** Quản lý level và wave
- Spawn enemy theo pattern
- Quản lý wave progression
- Enemy modifiers và scaling

## 🎮 Các loại Enemy

### 1. Infantry (Bộ binh)
- **Health:** 100
- **Speed:** 2.0
- **Damage:** 10
- **Reward:** 20
- **Đặc điểm:** Nhanh nhưng yếu

### 2. Tank (Xe tăng)
- **Health:** 300
- **Speed:** 1.0
- **Damage:** 25
- **Reward:** 50
- **Đặc điểm:** Chậm nhưng mạnh, có armor

### 3. Aircraft (Máy bay)
- **Health:** 150
- **Speed:** 3.0
- **Damage:** 15
- **Reward:** 35
- **Đặc điểm:** Rất nhanh, có thể bay

### 4. Boss
- **Health:** 1000
- **Speed:** 1.5
- **Damage:** 50
- **Reward:** 200
- **Đặc điểm:** Mạnh, có thể tự hồi máu

### 5. Elite Infantry
- **Health:** 200
- **Speed:** 2.5
- **Damage:** 20
- **Reward:** 40
- **Đặc điểm:** Mạnh hơn Infantry thường

### 6. Heavy Tank
- **Health:** 500
- **Speed:** 0.8
- **Damage:** 40
- **Reward:** 80
- **Đặc điểm:** Rất chậm nhưng cực kỳ mạnh

### 7. Fighter Jet
- **Health:** 250
- **Speed:** 4.0
- **Damage:** 30
- **Reward:** 60
- **Đặc điểm:** Cực kỳ nhanh và nguy hiểm

### 8. Mega Boss
- **Health:** 2000
- **Speed:** 1.0
- **Damage:** 100
- **Reward:** 500
- **Đặc điểm:** Boss cuối cùng, gần như bất tử

## 🌊 Wave Templates

### 1. Infantry Rush
- **Enemy:** 8 Infantry
- **Pattern:** Sequential
- **Difficulty:** 1.0x

### 2. Mixed Infantry
- **Enemy:** 6 Infantry (Group) + 4 Infantry (Sequential)
- **Pattern:** Mixed
- **Difficulty:** 1.2x

### 3. Tank Introduction
- **Enemy:** 4 Infantry + 2 Tank
- **Pattern:** Sequential
- **Difficulty:** 1.5x

### 4. Air Attack
- **Enemy:** 3 Aircraft (Group) + 6 Infantry
- **Pattern:** Mixed
- **Difficulty:** 1.8x

### 5. Boss Wave
- **Enemy:** 8 Infantry (Burst) + 3 Tank + 1 Boss
- **Pattern:** Mixed
- **Difficulty:** 2.5x

## 🛣️ Path Styles

### 1. Straight
- Đường thẳng từ start đến end
- Đơn giản, ít waypoints

### 2. Curved
- Đường cong mượt mà
- Sử dụng Bezier curve
- Có thể điều chỉnh độ cong

### 3. ZigZag
- Đường zic zac
- Tạo cảm giác phức tạp
- Tốt cho level khó

### 4. Spiral
- Đường xoắn ốc
- Tạo cảm giác mê cung
- Thú vị cho người chơi

### 5. Random
- Đường ngẫu nhiên
- Mỗi lần tạo sẽ khác nhau
- Tăng tính đa dạng

## 🎯 Cách sử dụng

### Setup cơ bản:
1. Thêm `GameSystemSetup` vào scene
2. Chạy game
3. Hệ thống sẽ tự động setup mọi thứ

### Tùy chỉnh:
1. **Path System:** Điều chỉnh trong PathManager
2. **Wave System:** Điều chỉnh trong WaveCreator
3. **Enemy Data:** Điều chỉnh trong EnemyDataCreator

### Test:
1. Sử dụng Context Menu trong Inspector
2. Hoặc dùng OnGUI buttons trong Play mode
3. Kiểm tra Console logs

## 🔧 Debug và Troubleshooting

### Enemy không di chuyển:
- Kiểm tra PathManager có tồn tại không
- Đảm bảo enemy có EnemyPathfinding script
- Kiểm tra StartPos/EndPos có tồn tại không

### Wave không spawn:
- Kiểm tra LevelWaveManager có tồn tại không
- Đảm bảo EnemySpawner có trong scene
- Kiểm tra EnemyData có được gán không

### Path không hiển thị:
- Kiểm tra showPathInGame trong PathManager
- Đảm bảo có LineRenderer component
- Kiểm tra material của LineRenderer

## 📊 Performance Tips

1. **Tối ưu Path System:**
   - Giảm số lượng waypoints nếu không cần thiết
   - Tắt debug visualization trong build

2. **Tối ưu Wave System:**
   - Giới hạn số lượng enemy đồng thời
   - Sử dụng object pooling cho enemy

3. **Tối ưu Enemy Movement:**
   - Sử dụng smooth movement thay vì direct movement
   - Giảm update frequency nếu cần

## 🎨 Customization

### Tạo Enemy mới:
1. Tạo EnemyData ScriptableObject
2. Thêm vào EnemyDataCreator templates
3. Tạo prefab cho enemy

### Tạo Wave mới:
1. Thêm template vào WaveCreator
2. Điều chỉnh difficulty multiplier
3. Test với Test Wave

### Tạo Path mới:
1. Thêm PathStyle mới vào PathManager
2. Implement calculation method
3. Test với Create Path

## 📝 Ví dụ sử dụng

```csharp
// Setup toàn bộ hệ thống
GameSystemSetup gameSetup = FindObjectOfType<GameSystemSetup>();
gameSetup.SetupGameSystem();

// Tạo path tùy chỉnh
PathManager pathManager = FindObjectOfType<PathManager>();
pathManager.pathStyle = PathManager.PathStyle.Spiral;
pathManager.CreatePath();

// Start level
LevelWaveManager waveManager = FindObjectOfType<LevelWaveManager>();
waveManager.StartLevel(1);

// Tạo enemy tùy chỉnh
EnemyData customEnemy = ScriptableObject.CreateInstance<EnemyData>();
customEnemy.enemyName = "Custom Enemy";
customEnemy.health = 500;
customEnemy.moveSpeed = 2.5f;
```

## 🎉 Kết luận

Hệ thống này cung cấp một nền tảng hoàn chỉnh cho Tower Defense game với:
- Setup tự động và dễ dàng
- Hệ thống wave đa dạng và thú vị
- Path system linh hoạt
- Enemy system phong phú
- Debug tools mạnh mẽ

Chỉ cần thêm `GameSystemSetup` script và chạy game là có ngay một Tower Defense game hoàn chỉnh! 