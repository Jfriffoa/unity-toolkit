using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//using TMPro;

namespace Paxn.Toolkit {
    public class Popup : MonoBehaviour {
        // Singleton
        static Popup _instance;
        public static Popup Instance { get => _instance; }
        static PopupLayout _layout;


        [Header("Textos Default")]
        public string exitMessage = "¿Estás seguro que deseas salir?";
        public string defaultCancelName = "Cancelar";
        public string defaultButtonName = "Ok";
        public string defaultTitle = "Alerta";

        [Header("Componentes del Popup")]
        public GameObject layout;
        //public TextMeshProUGUI title;
        //public TextMeshProUGUI description;
        public Text title;
        public Text description;
        public Image background;
        public Button[] buttons;

        void Awake() {
            _instance = this;
            layout.SetActive(false);
        }

        void Start() {
            _layout = new PopupLayout {
                root = layout,
                title = title,
                description = description,
                background = background,
                buttons = buttons
            };
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ToExit();
            }
        }

        void ToExit() {
            ShowAlert(new Alert {
                title = "Cerrar Aplicacion",
                message = exitMessage,
                buttonName = new string[] { "Si", "No" },
                call = new UnityAction[] { Application.Quit, Instance.ClosePopup }
            });
        }

        public static PopupLayout ShowAlert(string message) {
            return ShowAlert(new Alert {
                title = Instance.defaultTitle,
                message = message,
                buttonName = new string[] { Instance.defaultButtonName },
                call = new UnityAction[] { Instance.ClosePopup }
            });
        }

        public static PopupLayout ShowAlert(string message, string[] btnName) {
            return ShowAlert(new Alert {
                title = Instance.defaultTitle,
                message = message,
                buttonName = btnName,
                call = new UnityAction[] { Instance.ClosePopup }
            });
        }

        public static PopupLayout ShowAlert(string message, UnityAction[] call) {
            return ShowAlert(new Alert {
                title = Instance.defaultTitle,
                message = message,
                buttonName = new string[] { Instance.defaultButtonName },
                call = call
            });
        }

        public static PopupLayout ShowAlert(string message, string title, string[] btnName, UnityAction[] call) {
            return ShowAlert(new Alert {
                title = title,
                message = message,
                buttonName = btnName,
                call = call
            });
        }

        public static PopupLayout ShowAlert(Alert alert) {
            if (alert.buttonName == null)
                alert.buttonName = new string[] { };
            if (alert.call == null)
                alert.call = new UnityAction[] { };

            Instance.layout.SetActive(true);

            // Mensajes
            Instance.title.text = alert.title;
            Instance.description.text = alert.message;

            for (int i = 0; i < Instance.buttons.Length; i++) {
                var btn = Instance.buttons[i];

                // Texto
                if (i < alert.buttonName.Length)
                    btn.transform.GetChild(0).GetComponent<Text>().text = alert.buttonName[i];
                //btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = alert.buttonName[i];

                // Listeners
                btn.onClick.RemoveAllListeners();
                if (i < alert.call.Length)
                    btn.onClick.AddListener(alert.call[i]);
                if (!alert.overrideDefaultCall)
                    btn.onClick.AddListener(Instance.ClosePopup);
            }

            return _layout;
        }

        public void ClosePopup() {

            var fade = layout.GetComponentsInChildren<Fade>();
            if (fade != null) {
                foreach (var child in fade) {
                    child.FadeOut(() => { layout.SetActive(false); });
                }
            } else {
                layout.SetActive(false);
            }
        }

        public struct PopupLayout {
            public GameObject root;
            public Text title;
            public Text description;
            public Image background;
            public Button[] buttons;
        }

        public struct Alert {
            public string title;
            public string message;
            public string[] buttonName;
            public UnityAction[] call;
            public bool overrideDefaultCall;
        }

    }
}