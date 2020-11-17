using UnityEngine;
using UnityEngine.Events;

namespace Paxn.Toolkit {

    /// <summary>
    /// Component to make delayed callbacks. Useful when you want to 
    /// set up things in the Inspector instead of coding a 'Delay' method
    /// each time. It uses the <c>Invoke</c> call from Unity.
    /// </summary>
    /// 
    /// * Author: Paxn (jfriffoa@gmail.com)
    /// * Last time modified: 2020-11-17
    public class DelayEvent : MonoBehaviour {

        #region Parameters
        [Tooltip("Should the event execute at Start")]
        [SerializeField] bool _onStart;

        /// <summary>
        /// Seconds of delay before invoking the event.
        /// </summary>
        [Tooltip("Seconds of delay before invoking the event.")]
        public float delay;

        /// <summary>
        /// The event to invoke once the time passes.
        /// </summary>
        [Tooltip("The event to invoke once the time passes.")]
        public UnityEvent @event;
        #endregion

        #region Methods
        void Start() {
            if (_onStart) {
                Invoke();
            }
        }

        /// <summary>
        /// Invoke the delayed event.
        /// </summary>
        public void Invoke() {
            Invoke("DoEvent", delay);
        }

        /// <summary>
        /// Invoke the event without any delay.
        /// </summary>
        public void InvokeImmediatly() {
            DoEvent();
        }

        void DoEvent() {
            @event.Invoke();
        }
        #endregion
    }
}