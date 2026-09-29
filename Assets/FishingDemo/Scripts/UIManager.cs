using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Lakeside.Fishing
{
 public class UIManager:MonoBehaviour
 {
   Canvas canvas; GameObject menu, game; MoneyUI money;
   void Awake(){ BuildEnvironment(); BuildUI(); ShowMenu(); }
   void Update(){ if(Keyboard.current==null)return; if(menu!=null && menu.activeSelf && Keyboard.current.enterKey.wasPressedThisFrame){StartGame();return;} if(Keyboard.current.escapeKey.wasPressedThisFrame && game.activeSelf)ShowMenu(); }
   void BuildUI(){ var root=new GameObject("PrototypeUI",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));canvas=root.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;var scaler=root.GetComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1920,1080);scaler.matchWidthOrHeight=.5f;
     if(FindFirstObjectByType<EventSystem>()==null){var es=new GameObject("EventSystem",typeof(EventSystem),typeof(InputSystemUIInputModule));}
     game=new GameObject("Gameplay State"); game.SetActive(false); money=root.AddComponent<MoneyUI>(); money.Build(root.transform); BuildMenu(root.transform);
   }
   void BuildMenu(Transform c){menu=UIFactory.Panel("MainMenu",c,new Color(.005f,.025f,.035f,.43f),Vector2.zero,Vector2.one,Vector2.zero,Vector2.zero).gameObject;var title=UIFactory.Text("FIELD / TEST",menu.transform,65,Color.white,TextAlignmentOptions.Center);UIFactory.SetRect(title.rectTransform,new(.5f,.5f),new(.5f,.5f),new(-420,115),new(420,200));var sub=UIFactory.Text("第一人稱場景原型",menu.transform,20,UIFactory.Teal,TextAlignmentOptions.Center);UIFactory.SetRect(sub.rectTransform,new(.5f,.5f),new(.5f,.5f),new(-360,75),new(360,105));UIFactory.Button("開始遊戲",menu.transform,new(0,-5),new(250,52),StartGame);var note=UIFactory.Text("Enter 開始  ·  Esc 返回選單",menu.transform,16,new Color(1,1,1,.75f),TextAlignmentOptions.Center);UIFactory.SetRect(note.rectTransform,new(.5f,0),new(.5f,0),new(-300,36),new(300,64));}
   void StartGame(){menu.SetActive(false);game.SetActive(true);money.SetAmount(0);money.SetVisible(true);Cursor.lockState=CursorLockMode.Locked;Cursor.visible=false;}
   void ShowMenu(){if(game!=null)game.SetActive(false);if(money!=null)money.SetVisible(false);if(menu!=null)menu.SetActive(true);Cursor.lockState=CursorLockMode.None;Cursor.visible=true;Time.timeScale=1;}
   void BuildEnvironment(){RenderSettings.skybox=null;RenderSettings.ambientLight=new Color(.48f,.52f,.44f);var player=new GameObject("First Person Player",typeof(CharacterController),typeof(FirstPersonController));player.transform.position=new Vector3(0,0,-8f);var controller=player.GetComponent<CharacterController>();controller.height=1.8f;controller.radius=.35f;controller.center=new Vector3(0,.9f,0);var headAnchor=BuildSimpleFisher(player.transform);var cam=new GameObject("First Person Camera",typeof(Camera));cam.transform.SetParent(headAnchor,false);cam.transform.localPosition=Vector3.zero;cam.GetComponent<Camera>().nearClipPlane=.03f;cam.GetComponent<Camera>().backgroundColor=new Color(.43f,.68f,.85f);cam.GetComponent<Camera>().farClipPlane=250;player.GetComponent<FirstPersonController>().Initialize(cam.transform);CreateLight();Primitive(PrimitiveType.Plane,"Open Clearing",Vector3.zero,new Vector3(42,1,42),new Color(.25f,.55f,.22f));}
   Transform BuildSimpleFisher(Transform player){var root=new GameObject("Player Model").transform;root.SetParent(player,false);var clay=new Color(.66f,.60f,.48f);var body=Primitive(PrimitiveType.Capsule,"Rounded Capsule Body",player.position+new Vector3(0,1.18f,0),new Vector3(1.72f,1.38f,1.18f),clay);body.transform.SetParent(root);body.GetComponent<Collider>().enabled=false;var head=Primitive(PrimitiveType.Sphere,"Faceted Round Head",player.position+new Vector3(0,2.92f,0),new Vector3(.98f,.94f,.94f),clay);MakeFlatShaded(head);head.transform.SetParent(root);head.GetComponent<Collider>().enabled=false;var anchor=new GameObject("Head Camera Anchor").transform;anchor.SetParent(root,false);anchor.localPosition=new Vector3(0,2.92f,0);return anchor;}
   void MakeFlatShaded(GameObject o){var source=o.GetComponent<MeshFilter>().sharedMesh;var srcV=source.vertices;var srcUv=source.uv;var srcT=source.triangles;var v=new Vector3[srcT.Length];var uv=new Vector2[srcT.Length];var t=new int[srcT.Length];for(int i=0;i<srcT.Length;i++){v[i]=srcV[srcT[i]];if(srcUv.Length>srcT[i])uv[i]=srcUv[srcT[i]];t[i]=i;}var flat=new Mesh{name="Flat Shaded Head"};flat.vertices=v;flat.uv=uv;flat.triangles=t;flat.RecalculateNormals();o.GetComponent<MeshFilter>().mesh=flat;}
   void CreateLight(){var l=new GameObject("Sun",typeof(Light)).GetComponent<Light>();l.type=LightType.Directional;l.transform.rotation=Quaternion.Euler(40,-35,0);l.color=new Color(1,.84f,.65f);l.intensity=1.15f;}
   GameObject Primitive(PrimitiveType t,string n,Vector3 pos,Vector3 scale,Color color){var o=GameObject.CreatePrimitive(t);o.name=n;o.transform.position=pos;o.transform.localScale=scale;var r=o.GetComponent<Renderer>();r.material=new Material(Shader.Find("Universal Render Pipeline/Lit"));r.material.color=color;return o;}
 }
}
