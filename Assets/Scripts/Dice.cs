using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Paxn.Toolkit {

    /// <summary>
    /// An Sprite Dice with a roll animation. It can be used with
    /// <c>UnityEngine.UI.Image</c> or <c>UnityEngine.SpriteRenderer</c>
    /// </summary>
    /// 
    /// * Author: Paxn (jfriffoa@gmail.com)
    /// * Last time modified: 2020-11-17
    public class Dice : MonoBehaviour {

        #region Parameters
        [Header("Animation")]
        /// <summary>
        /// Set of sprites to use to roll.
        /// </summary>
        [Tooltip("Set of sprites to use to roll.")]
        public Sprite[] diceSides;

        /// <summary>
        /// Sprite changes before the final result.
        /// </summary>
        /// <remarks>
        /// Play with <c>timeBetweenSpins</c> to determine how long will
        /// be the animation.
        /// </remarks>
        [Tooltip("Sprite changes before the final result.")]
        public int spins = 20;

        /// <summary>
        /// Time to change from sprite to sprite.
        /// </summary>
        /// <remarks>
        /// Play with <c>spins</c> to determine how long will
        /// be the animation.
        /// </remarks>
        [Tooltip("Time to change from sprite to sprite.")]
        public float timeBetweenSpins = 0.05f;

        [Tooltip("What component use to render the dice.")]
        [SerializeField] UseCase _useRender;

        [Tooltip("UI.Image to render the dice.")]
        [SerializeField] Image _imageRender;

        [Tooltip("SpriteRenderer to render the dice.")]
        [SerializeField] SpriteRenderer _spriteRender;
        bool _isRolling;
        bool _locked;

        [Header("Callbacks")]
        /// <summary>
        /// Event to invoke once the dice has a result.
        /// </summary>
        [Tooltip("Event to invoke once the dice has a result.")]
        public IntEvent onRoll;
        #endregion

        #region Methods
        void Start() {
            _isRolling = false;

            // Initialize if we have a render on us
            if (_imageRender == null) {
                var render = GetComponent<Image>();
                if (render != null) {
                    _imageRender = render;
                    _useRender = UseCase.UI;
                }
            }

            if (_spriteRender == null) {
                var render = GetComponent<SpriteRenderer>();
                if (render != null) {
                    _spriteRender = render;
                    _useRender = (_imageRender != null) ? UseCase.Both : UseCase.SpriteRenderer;
                }
            }

            // Initialize the renders
            SetSprite(0);
        }

        /// <summary>
        /// Roll the Dice
        /// </summary>
        public void Roll() {
            if (!_isRolling && !_locked)
                StartCoroutine(RollTheDice());
        }

        /// <summary>
        /// Lock the dice and avoid rolling.
        /// </summary>
        public void Lock()   => _locked = true;

        /// <summary>
        /// Unlock the dice and make it available once again.
        /// </summary>
        public void Unlock() => _locked = false;

        /// <summary>
        /// Set the <c>SpriteRenderer</c> for this dice.
        /// </summary>
        /// <param name="render"><c>SpriteRenderer</c> to assign.</param>
        public void SetRender(SpriteRenderer render) {
            _spriteRender = render;
            _useRender = (_imageRender != null) ? UseCase.Both : UseCase.SpriteRenderer;
        }

        /// <summary>
        /// Set the <c>UI.Image</c> for this dice.
        /// </summary>
        /// <param name="render"><c>UI.Image</c> to assign</param>
        public void SetRender(Image render) {
            _imageRender = render;
            _useRender = (_spriteRender != null) ? UseCase.Both : UseCase.UI;
        }

        IEnumerator RollTheDice() {
            _isRolling = true;

            // Roll the dice
            int value = 0;
            Sprite sprite;
            for (int i = 0; i < spins; i++) {
                (sprite, value) = RandomUtil.RandomPick(diceSides, value);
                SetSprite(sprite);
                yield return new WaitForSeconds(timeBetweenSpins);
            }
            _isRolling = false;

            // Callback
            onRoll.Invoke(value);
        }

        void SetSprite(int spriteIndex) => SetSprite(diceSides[spriteIndex]);
        void SetSprite(Sprite sprite) {
            switch(_useRender) {
                case UseCase.UI:
                    _imageRender.sprite = sprite;
                    break;
                case UseCase.SpriteRenderer:
                    _spriteRender.sprite = sprite;
                    break;
                case UseCase.Both:
                    _imageRender.sprite = sprite;
                    _spriteRender.sprite = sprite;
                    break;
            }
        }
        #endregion

        #region Utils
        /// <summary>
        /// Enum to define which render use.
        /// </summary>
        public enum UseCase { UI, SpriteRenderer, Both }
        [System.Serializable] public class IntEvent : UnityEvent<int> { }
        #endregion
    }

    #region Custom Unity Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(Dice))]
    public class DiceEditor : Editor {
        // Properties
        SerializedProperty _diceSides;
        SerializedProperty _spins;
        SerializedProperty _timeBetweenSpins;
        SerializedProperty _useRender;
        SerializedProperty _imRender;
        SerializedProperty _spRender;
        SerializedProperty _onRoll;

        void OnEnable() {
            _diceSides = serializedObject.FindProperty("diceSides");
            _spins = serializedObject.FindProperty("spins");
            _timeBetweenSpins = serializedObject.FindProperty("timeBetweenSpins");
            _useRender = serializedObject.FindProperty("_useRender");
            _imRender = serializedObject.FindProperty("_imageRender");
            _spRender = serializedObject.FindProperty("_spriteRender");
            _onRoll = serializedObject.FindProperty("onRoll");
        }

        public override void OnInspectorGUI() {
            //base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.PropertyField(_diceSides);
            EditorGUILayout.PropertyField(_spins);
            EditorGUILayout.PropertyField(_timeBetweenSpins);
            EditorGUILayout.PropertyField(_useRender);

            switch ((Dice.UseCase)_useRender.enumValueIndex) {
                case Dice.UseCase.Both:
                    EditorGUILayout.PropertyField(_imRender);
                    EditorGUILayout.PropertyField(_spRender);
                    break;
                case Dice.UseCase.SpriteRenderer:
                    EditorGUILayout.PropertyField(_spRender);
                    break;
                case Dice.UseCase.UI:
                    EditorGUILayout.PropertyField(_imRender);
                    break;
            }

            EditorGUILayout.PropertyField(_onRoll);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}