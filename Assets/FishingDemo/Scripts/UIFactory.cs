using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lakeside.Fishing
{
    public static class UIFactory
    {
        public static readonly Color Ink = new Color(0.035f, .09f, .12f, .94f);
        public static readonly Color Teal = new Color(.12f, .78f, .72f, 1f);
        public static readonly Color Gold = new Color(1f, .72f, .24f, 1f);
        // A bundled Noto Sans TC font makes TMP text reliable even in a new Unity project
        // that has not imported TMP Essential Resources yet.
        static TMP_FontAsset cachedFont;
        public static TMP_FontAsset DefaultFont
        {
            get
            {
                if (cachedFont != null) return cachedFont;
                var source = Resources.Load<Font>("PrototypeUIFont");
                if (source == null)
                {
                    Debug.LogError("Prototype UI font is missing: Assets/Resources/PrototypeUIFont.ttf");
                    return null;
                }
                cachedFont = TMP_FontAsset.CreateFontAsset(source);
                return cachedFont;
            }
        }
        public static RectTransform Panel(string name, Transform parent, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            var o = new GameObject(name, typeof(RectTransform), typeof(Image)); o.transform.SetParent(parent, false);
            var r = o.GetComponent<RectTransform>(); r.anchorMin = anchorMin; r.anchorMax = anchorMax; r.offsetMin = offsetMin; r.offsetMax = offsetMax; o.GetComponent<Image>().color = color; return r;
        }
        public static TextMeshProUGUI Text(string text, Transform parent, int size, Color color, TextAlignmentOptions align = TextAlignmentOptions.Left)
        {
            var o = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI)); o.transform.SetParent(parent, false);
            var t = o.GetComponent<TextMeshProUGUI>(); t.font = DefaultFont; t.text = text; t.fontSize = size; t.color = color; t.alignment = align; t.textWrappingMode = TextWrappingModes.NoWrap;
            var r = t.rectTransform; r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = r.offsetMax = Vector2.zero; return t;
        }
        public static Button Button(string label, Transform parent, Vector2 pos, Vector2 size, System.Action action)
        {
            var r = Panel("Button_" + label, parent, new Color(.07f,.20f,.24f,.96f), new Vector2(.5f,.5f), new Vector2(.5f,.5f), pos - size*.5f, pos + size*.5f);
            var b = r.gameObject.AddComponent<Button>(); var c = b.colors; c.normalColor = Color.white; c.highlightedColor = new Color(.7f,1f,.94f); b.colors=c;
            Text(label, r, 20, Color.white, TextAlignmentOptions.Center); b.onClick.AddListener(() => action()); return b;
        }
        public static Image Bar(Transform parent, Vector2 pos, Vector2 size, Color background, Color fill, out Image fillImage)
        {
            var back = Panel("Bar", parent, background, new Vector2(.5f,.5f), new Vector2(.5f,.5f), pos-size*.5f, pos+size*.5f);
            var f = Panel("Fill", back, fill, Vector2.zero, new Vector2(1,.5f), Vector2.zero, Vector2.zero).GetComponent<Image>();
            fillImage=f; return back.GetComponent<Image>();
        }
        public static void SetRect(RectTransform r, Vector2 min, Vector2 max, Vector2 offMin, Vector2 offMax) { r.anchorMin=min; r.anchorMax=max; r.offsetMin=offMin; r.offsetMax=offMax; }
    }
}
