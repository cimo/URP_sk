using UnityEngine;

namespace Player {
    public class PlayerWeapon : MonoBehaviour {
        [SerializeField]
        private float speedMovement = 250.0f;
        [SerializeField]
        private float speedRotation = -1500.0f;
        [SerializeField]
        private PlayerMovement playerMovement = null;
        [SerializeField]
        private GameObject playerWeaponHand = null;
        [SerializeField]
        private GameObject playerReturnPoint = null;
        
        public bool statusIdle { get; private set; }
        public bool statusTargeting { get; private set; }
        public bool statusThrow { get; private set; }
        public bool statusHit { get; private set; }
        public bool statusPull { get; private set; }

        private Rigidbody componentRigidbody;
        private TrailRenderer componentTrailRenderer;

        private Vector3 startPositon;
        private Vector3 startRotation;

        private Vector3 statusPullPosition;
        private float statusPullTime;

        // Properties
        
        // Functions public
        public void attackThrow() {
            statusIdle = false;
            statusTargeting = false;
            statusThrow = true;
            statusHit = false;
            statusPull = false;

            transform.parent = null;
            transform.position += playerMovement.gameObject.transform.right / 5.0f;
            transform.eulerAngles = new Vector3(0.0f, -90.0f + playerMovement.gameObject.transform.eulerAngles.y, 0.0f);
            
            componentRigidbody.isKinematic = false;
            componentRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            componentRigidbody.AddForce((Camera.main.transform.forward * speedMovement) + (playerMovement.gameObject.transform.up * 2), ForceMode.Impulse);

            componentTrailRenderer.emitting = true;
        }

        public void attackPull() {
            statusIdle = false;
            statusTargeting = false;
            statusThrow = false;
            statusHit = false;
            statusPull = true;

            statusPullPosition = transform.position;
            
            transform.parent = null;

            componentRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            componentRigidbody.isKinematic = true;
        }

        // Functions private
        private void Awake() {
            statusIdle = true;
            statusTargeting = false;
            statusThrow = false;
            statusHit = false;
            statusPull = false;

            componentRigidbody = GetComponent<Rigidbody>();
            componentTrailRenderer = GameObject.Find("Trail").GetComponent<TrailRenderer>();

            startPositon = transform.localPosition;
            startRotation = transform.localEulerAngles;

            statusPullPosition = Vector3.zero;
            statusPullTime = 0.0f;
        }

        private void Update() {
            if (statusThrow == true || statusPull == true)
                _rotate();

            if (statusPull == true) {
                if (statusPullTime < 1.0f)
                    _returnBack();
                else
                    _attackCatch();
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.layer == 8 & statusThrow == true) {
                statusIdle = false;
                statusTargeting = false;
                statusThrow = false;
                statusHit = true;
                statusPull = false;
                
                if (collision.gameObject.CompareTag("Target") == true) {
                    transform.parent = collision.gameObject.transform;
                    transform.position = collision.gameObject.transform.position;
                    transform.localEulerAngles = collision.gameObject.transform.localEulerAngles;
                }

                componentRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                componentRigidbody.isKinematic = true;
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Target") == true) {
                //...
            }
        }

        private void _rotate() {
            transform.localEulerAngles += Vector3.forward * speedRotation * Time.deltaTime;
        }

        private void _returnBack() {
            transform.position = _quadraticCurvePoint(statusPullTime, statusPullPosition, playerReturnPoint.transform.position, playerWeaponHand.transform.position);
            
            statusPullTime += 1.5f * Time.deltaTime;
        }

        public void _attackCatch() {
            statusIdle = true;
            statusTargeting = false;
            statusThrow = false;
            statusHit = false;
            statusPull = false;

            statusPullTime = 0;

            transform.parent = playerWeaponHand.transform;
            transform.localPosition = startPositon;
            transform.localEulerAngles = startRotation;

            componentTrailRenderer.emitting = false;
        }

        private Vector3 _quadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            return (uu * p0) + (2 * u * t * p1) + (tt * p2);
        }
    }
}