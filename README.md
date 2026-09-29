# FIELD / TEST — Unity 6 URP First-Person Prototype

第一人稱空地原型。場景使用 Unity 基本幾何體建立草地、玩家 placeholder 與頭部相機，沒有使用第三方遊戲資產。

## 開啟方式

1. 以 **Unity 6000.3 LTS** 開啟本資料夾。
2. 開啟 `Assets/Scenes/FishingDemo.unity`，按 Play。
3. 主選單按「開始遊戲」或 `Enter`。

## Demo 操作流程

- `WASD`：第一人稱移動。
- 滑鼠：轉動視角。
- 左 Shift + W：跑步。
- 空白鍵：站立且在地面時跳躍。
- 按住左 Ctrl 或 C：蹲下；頭頂有障礙時不會強行站起。
- `Esc`：返回主選單並釋放滑鼠。

`UIManager` 管理主選單與場景啟動，`FirstPersonController` 接上 `PlayerMovement` 與 `PlayerCamera`。這是本專案的獨立單機版本，沒有直接匯入原遊戲的連線程式。使用 Unity Input System 和 CharacterController，包含加減速、重力、跳躍、蹲下碰撞高度及輕微視角晃動。Play 時可選取 First Person Player 調整速度、靈敏度或停用 Head Bob。相機仍跟隨頭部錨點。Canvas 採 Screen Space Overlay，Canvas Scaler 參考 1920×1080 並以寬高折衷縮放。
