using UnityEngine;

using InputController;

namespace Player {
    public class PlayerMovement : MonoBehaviour {
        // Vars
        [SerializeField]
        private float speedWalk = 6.0f;
        [SerializeField]
        private float speedRotation = 6.0f;
        [SerializeField]
        private PlayerWeapon playerWeapon = null;

        private CharacterController componentCharacterController;
        private Animator componentAnimator;

        private float inputX;
        private float inputZ;

        private float magnitude;
        private Vector3 forward;
        private Vector3 right;
        private Vector3 moveDirection;
        private Vector3 rotationOffset;

        private InputControllerKeyboard inputControllerKeyboard;

        // Properties
        
        // Functions public
        public void attackThrowWeapon() {
            playerWeapon.attackThrow();
        }

        // Functions private
        private void Awake() {
            componentCharacterController = GetComponent<CharacterController>();
            componentAnimator = GetComponent<Animator>();

            inputX = 0.0f;
		    inputZ = 0.0f;

            magnitude = 0.0f;
            forward = Vector3.zero;
		    right = Vector3.zero;
            moveDirection = Vector3.zero;
            rotationOffset = Vector3.zero;

            inputControllerKeyboard = GameObject.Find("Script container").GetComponent<InputControllerKeyboard>();
        }

        private void Update() {
            if (inputControllerKeyboard.p_targeting == true && playerWeapon.statusIdle == true) {
                _targeting(true);

                _movement(false);

                if (inputControllerKeyboard.p_throw == true)
                    componentAnimator.SetBool("attackThrowWeapon", true);
            }
            else {
                componentAnimator.SetBool("attackThrowWeapon", false);

                _targeting(false);

                _movement(true);

                if (inputControllerKeyboard.p_throw == true && playerWeapon.statusIdle == false)
                    playerWeapon.attackPull();
            }
        }

        private void _movement(bool type) {
            float running = 1.0f;
            
            if (inputControllerKeyboard.p_run == true)
                running = 2.0f;

            if (type == false) {
                inputX = 0.0f;
                inputZ = 0.0f;
            }
            else {
                inputX = inputControllerKeyboard.p_horizontal;
		        inputZ = inputControllerKeyboard.p_vertical;
            }

            forward = Camera.main.transform.forward;
		    right = Camera.main.transform.right;

		    forward.y = 0.0f;
		    right.y = 0.0f;

		    forward.Normalize();
		    right.Normalize();

            moveDirection = (forward * inputZ) + (right * inputX);

            magnitude = new Vector2(inputX, inputZ).sqrMagnitude;

            if (magnitude > 0.1f) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), speedRotation);

                componentCharacterController.Move(moveDirection * speedWalk * running * Time.deltaTime);
            }

            componentAnimator.SetFloat("horizontal", inputX);
            componentAnimator.SetFloat("vertical", inputZ);
            componentAnimator.SetFloat("movement", Vector3.ClampMagnitude(moveDirection, 1.0f).magnitude * running);
        }

        private void _targeting(bool type) {
            componentAnimator.SetBool("targeting", type);

            if (type == true) {
                forward = Camera.main.transform.forward;
                
                forward.y = 0.0f;
		        
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward), speedRotation);
            }
        }
    }
}