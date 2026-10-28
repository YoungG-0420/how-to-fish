# How to Fish — Blender 素材

這個資料夾是從此專題使用的 Unity 匯入資產轉出的 Blender 可讀版本。

## 如何開啟

- **看人物組合**：下載整個專案並解壓縮，在 Blender 使用「檔案 → 開啟」，選擇 `Prefabs/Resources__other__PlayerHolder.blend`。這個檔案保存 10 個網格物件與 3 組骨架。
- **看單獨模型**：在 Blender 使用「檔案 → 匯入 → glTF 2.0 (.glb/.gltf)」，選擇 `Models` 中的 `.glb`。模型的貼圖已嵌入檔案，不需要另外指定圖片。
- **看有骨架的魚、道具或人物組合**：匯入 `Prefabs` 中的 `.glb`。這些比 `Models` 中的獨立網格保留更多原本的結構。
- **看動畫**：用「檔案 → 開啟」選擇 `Animations` 中的 `.blend`，切換到 Animation 工作區並播放時間軸。這些檔案保存獨立的動畫變換層級；要看已套在物件上的動畫，請使用有動畫的 Prefab。動畫與任意模型之間仍需要正確綁定，不能直接互換。
- **看材質**：直接開啟 `Materials/MaterialLibrary.blend`。切換到「材質預覽」可以查看材質展示格；圖片已打包進檔案。
- **看貼圖**：`Textures` 保存 PNG 等 Blender 可讀圖片，也可在 Blender 的圖片編輯器中開啟。

GitHub 網頁上的 GLB / BLEND 檔案需要先下載到電腦。下載整份專案可使用儲存庫首頁的 **Code → Download ZIP**。進入 Blender 後，可選取物件並按數字鍵盤 `.` 聚焦，按 `Home` 查看全部物件。

## 內容與驗證

| 類別 | 數量 | 格式 |
| --- | ---: | --- |
| 原始模型對應檔 | 448 | GLB |
| 實際含頂點的模型 | 437 | 上述 GLB 的子集 |
| 原本就是空網格的佔位物件 | 11 | 空 GLB 物件，保留來源名稱 |
| 組合物件 | 108 | GLB |
| 含骨架的組合物件 | 77 | 上述組合物件的子集 |
| 有路徑相符動畫的組合物件 | 17 | 上述組合物件的子集 |
| 獨立動畫 | 50 | GLB、BLEND |
| 材質 | 169 | GLB / BLEND 材質庫與 JSON 原始參數 |
| 圖片與解碼貼圖 | 187 | PNG 等 |

607 個 GLB 已逐一用 Blender 5.0.1 匯入檢查，沒有匯入失敗。`blender_verification.json` 記錄各檔案匯入後的網格、骨架、材質與動畫數量。`conversion_report.json` 記錄來源與輸出檔案的對應。

## 保留程度

- 模型保存可取得的頂點、法線、第一組 UV、頂點色與子網格。377 個模型從原始場景或 Prefab 找到材質指派，其餘模型使用預設材質，來源未提供足夠連結。
- 有完整骨架連結的 Prefab 保存骨架、綁定矩陣與可取得的頂點權重。單獨 `Models` 網格不包含骨架，請從 `Prefabs` 選擇帶骨架版本。
- 材質轉為通用 PBR 近似，保存找到的主貼圖、顏色、金屬度、粗糙度和透明設定。所有原始材質參數另存在 `materials_manifest.json`。Unity 自訂 Shader、海水波動、草木擺動、特殊光照與法線處理不會自動變成完全相同的 Blender 效果。
- 動畫保存可以讀取的位移、旋轉、縮放關鍵影格；轉出檔使用線性插值，曲線緩動效果可能與 Unity 有差異。只有路徑完全相符的動畫才加入 Prefab，這仍不等同於人工確認動作語意相符。
- 獨立動畫檔案包含可播放的變換階層，並不代表它們已綁定某個人物。遊戲事件、腳本、材質屬性動畫、動畫控制器狀態機、程序式動作與 Blend Shape 動畫沒有轉成 Blender 動作。
- BC6H 光照貼圖另提供 8-bit PNG 預覽，預覽未保留完整 HDR 動態範圍；原始 Unity `.texture2D` 已另存於 `Assets/HowToFishImported/Scenes`。
- 原始 Unity 材質、貼圖、動畫與必要 Prefab 資料位於 `Assets/HowToFishImported`，可供後續調整或重新轉換。

這些資產來自既有遊戲，轉檔不改變原資產的權利歸屬。
