using UnityEngine;

namespace InputController {
    public class InputControllerKeyboard : MonoBehaviour {
        // Vars

        // Properties
        public float p_horizontal { get; private set; }
        public float p_vertical { get; private set; }
        public bool p_targeting { get; private set; }
        public bool p_throw { get; private set; }
        public bool p_run { get; private set; }

        // Functions public

        // Functions private
        private void Awake() {
            p_horizontal = 0.0f;
            p_vertical = 0.0f;
            p_targeting = false;
            p_throw = false;
            p_run = false;
        }

        private void Update() {
            p_horizontal = Input.GetAxis("Horizontal");
            p_vertical = Input.GetAxis("Vertical");
            p_targeting = Input.GetMouseButton(1);
            p_throw = Input.GetKeyDown(KeyCode.Z);
            p_run = Input.GetKey(KeyCode.LeftShift);
        }
    }
}