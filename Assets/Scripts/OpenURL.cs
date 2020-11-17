using UnityEngine;

namespace Paxn.Toolkit {

    /// <summary>
    /// Component to open an URL. It is a component
    /// so you can call it from UnityEvents in the Inspector.
    /// </summary>
    /// 
    /// * Author: Paxn (jfriffoa@gmail.com)
    /// * Last time modified: 2020-11-17
    public class OpenURL : MonoBehaviour {

        #region Methods
        /// <summary>
        /// Ask the system to launch an URL.
        /// </summary>
        /// <param name="url">URL to be launched.</param>
        public void Open(string url) => Application.OpenURL(url);

        /// <summary>
        /// Ask the system to launch and URL.
        /// </summary>
        /// <param name="url">URL to be launched.</param>
        public static void OpenWebpage(string url) => Application.OpenURL(url);
        #endregion
    }
}