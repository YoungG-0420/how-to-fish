using UnityEngine;
namespace Lakeside.Fishing { public class DemoBootstrap:MonoBehaviour { void Awake(){ if(FindFirstObjectByType<UIManager>()==null) gameObject.AddComponent<UIManager>(); } } }
