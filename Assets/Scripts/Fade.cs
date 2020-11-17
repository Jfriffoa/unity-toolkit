using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Paxn.Toolkit {
    /// <summary>
    /// Component to made a Fade animation, using <c>Curves</c> you
    /// can specify the smoothness of the animation.
    /// </summary>
    /// 
    /// * Author: Paxn (jfriffoa@gmail.com)
    /// * Last time modified: 2020-11-17
    public class Fade : MonoBehaviour {
        
        #region Parameters
        [Header("Fade parameters")]
        /// <summary>
        /// Limits of the alpha component.
        /// </summary>
        [Tooltip("Limits of the alpha component.")]
        public Vector2 alphaLimits = new Vector2(0, 1);

        /// <summary>
        /// Time to Fade.
        /// </summary>
        [Tooltip("Time to Fade.")]
        public float fadeTime = 1f;

        /// <summary>
        /// Smoothness to apply.
        /// </summary>
        [Tooltip("Smoothness to apply.")]
        public Curves.CurveType smoothness;

        /// <summary>
        /// Behaviour each time this component is activated or re-activated.
        /// </summary>
        [Tooltip("Behaviour each time this component is activated or re-activated.")]
        public FadeStartType fadeOnEnable;

        /// <summary>
        /// Fade only this object or its childrens as well.
        /// </summary>
        [Tooltip("Fade only this object or its childrens as well.")]
        public bool fadeChildrens;

        [Header("Callbacks")]
        /// <summary>
        /// Event to invoke once the fade ends.
        /// </summary>
        [Tooltip("Event to invoke once the fade ends.")]
        public UnityEvent onFinish;
        #endregion

        #region Methods
        void OnEnable() {
            switch (fadeOnEnable) {
                case FadeStartType.Fade_In:
                    StartCoroutine(DoFade(true));
                    break;
                case FadeStartType.Fade_Out:
                    StartCoroutine(DoFade(false));
                    break;
            }
        }

        /// <summary>
        /// Make a Fade-In using the default parameters.
        /// </summary>
        public void FadeIn() => StartCoroutine(DoFade(true));

        /// <summary>
        /// Make a Fade-In using a custom time.
        /// </summary>
        /// <param name="time">Time to Fade.</param>
        public void FadeIn(float time) => StartCoroutine(DoFade(true, time));

        /// <summary>
        /// Make a Fade-In with a callback.
        /// </summary>
        /// <param name="onFinish">Event to be called at the end of the Fade.</param>
        public void FadeIn(System.Action onFinish) => StartCoroutine(DoFade(true, fadeTime, onFinish));

        /// <summary>
        /// Make a Fade-In with a custom time and a callback.
        /// </summary>
        /// <param name="time">Time to Fade.</param>
        /// <param name="onFinish">Event to be called at the end of the Fade.</param>
        public void FadeIn(float time, System.Action onFinish) => StartCoroutine(DoFade(true, time, onFinish));

        /// <summary>
        /// Make a Fade-Out using the default parameters.
        /// </summary>
        public void FadeOut() => StartCoroutine(DoFade(false));

        /// <summary>
        /// Make a Fade-Out using a custom time.
        /// </summary>
        /// <param name="time">Time to Fade.</param>
        public void FadeOut(float time) => StartCoroutine(DoFade(false, time));

        /// <summary>
        /// Make a Fade-Out with a callback.
        /// </summary>
        /// <param name="onFinish">Event to be called at the end of the Fade.</param>
        public void FadeOut(System.Action onFinish) => StartCoroutine(DoFade(false, fadeTime, onFinish));

        /// <summary>
        /// Make a Fade-Out with a custom time and a callback.
        /// </summary>
        /// <param name="time">Time to Fade.</param>
        /// <param name="onFinish">Event to be called at the end of the Fade.</param>
        public void FadeOut(float time, System.Action onFinish) => StartCoroutine(DoFade(false, time, onFinish));

        // Do the fade
        IEnumerator DoFade(bool _in, float forceTime = -1f, System.Action onFinishFade = null) {
            // Initialize parameters
            var graphics = (fadeChildrens) ? GetComponentsInChildren<Graphic>() : GetComponents<Graphic>();
            var origin = (_in) ? alphaLimits.x : alphaLimits.y;
            var target = (_in) ? alphaLimits.y : alphaLimits.x;

            var time = (forceTime < 0) ? fadeTime : forceTime;

            // fade
            float counter = 0f;
            while (counter <= time) {
                var t = counter / time;
                for (int i = 0; i < graphics.Length; i++) {
                    var color = graphics[i].color;
                    
                    color.a = Mathf.Lerp(origin, target, Curves.ApplyCurve(t, smoothness));
                    graphics[i].color = color;
                }
                yield return new WaitForEndOfFrame();
                counter += Time.deltaTime;
            }

            // Callbacks
            if (onFinishFade != null)
                onFinishFade.Invoke();
            onFinish.Invoke();
        }
        #endregion

        #region Utils
        public enum FadeStartType { None, Fade_In, Fade_Out }
        #endregion
    }
}