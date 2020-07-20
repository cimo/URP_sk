using UnityEngine;

namespace Setting {
    public class SettingGeneral : MonoBehaviour {
        // Vars

        // Properties

        // Functions public

        // Functions private
        private void Awake() {
        }

        private void Update() {
            if (Input.GetKey("escape") == true)
                Application.Quit();

            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}