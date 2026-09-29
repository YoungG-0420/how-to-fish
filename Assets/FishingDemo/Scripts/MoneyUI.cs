using TMPro;
using UnityEngine;

namespace Lakeside.Fishing
{
    public class MoneyUI : MonoBehaviour
    {
        GameObject root;
        TextMeshProUGUI amountText;
        public int Amount { get; private set; }

        public void Build(Transform canvas)
        {
            root = new GameObject("Money HUD", typeof(RectTransform));
            root.transform.SetParent(canvas, false);
            var rootRect = root.GetComponent<RectTransform>();
            UIFactory.SetRect(rootRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            amountText = UIFactory.Text("$0", root.transform, 44, Color.white, TextAlignmentOptions.Center);
            UIFactory.SetRect(amountText.rectTransform, new(.5f,1), new(.5f,1), new(-140,-78), new(140,-16));
            SetVisible(false);
        }

        public void SetAmount(int amount) { Amount = Mathf.Max(0, amount); amountText.text = "$" + Amount; }
        public void SetVisible(bool visible) => root.SetActive(visible);
    }
}
