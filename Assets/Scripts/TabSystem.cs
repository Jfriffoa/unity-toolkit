using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TabSystem : MonoBehaviour
{
    public Button[] tabs;
    public GameObject[] screens;
    public Image screensBackground;
    
    // Start is called before the first frame update
    void Start()
    {
        ActiveTab(0);
    }

    public void ActiveTab(int tabIndex) {
        if (tabIndex >= tabs.Length) {
            Debug.LogError("DAME UN BUEN INDICE SACO WEA", gameObject);
            return;
        }

        // Ajustar Ancho
        for (int i = 0; i < tabs.Length; i++) {
            var layout = tabs[i].GetComponent<LayoutElement>();
            if (layout != null)
                layout.flexibleWidth = (i == tabIndex) ? 30 : 0;
        }

        // Ajustar fondo
        if (screensBackground != null) {
            screensBackground.color = tabs[tabIndex].GetComponent<Image>().color;
        }

        // Mostrar screen
        for (int i = 0; i < screens.Length; i++) {
            screens[i].SetActive(i == tabIndex);
        }
    }
}
