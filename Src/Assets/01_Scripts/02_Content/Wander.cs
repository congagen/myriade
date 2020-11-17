using UnityEngine;
using System.Collections;

namespace MyriadeM
{
    public class Wander : MonoBehaviour
    {
        public GameObject marketObj;
        public GameObject originObj;

        public bool randomDestination = false;

        //private bool objectSetupComplete = false;
        public float maxWanderDistance = 10;
        public float retrigDistance = 0.01f;

        public bool wanderY = false;
        public float maxSpeed = 100;

        public bool rotate = true;

        public Vector3 wanderOffset = new Vector3(0, 0, 0);
        private Vector3 desitnation;

        [Range(0f, 100f)] public float xRotateSpeed;
        [Range(0f, 100f)] public float yRotateSpeed;
        [Range(0f, 100f)] public float zRotateSpeed;

        private float startRotateOffset;
        public bool initRotateOffset;


        void WanderCircle() {
            float oX = transform.position.x * 1.4f;
            float oZ = transform.position.z * 1.4f;

            float dist = Vector3.Distance(transform.position, desitnation);
            float destval_x = Mathf.Sin((Time.timeSinceLevelLoad * 0.0001f)) * 10000;
            float destval_z = Mathf.Cos((Time.timeSinceLevelLoad * 0.0001f)) * 10000;

            desitnation = new Vector3(
                transform.position.x + destval_x,
                transform.position.y,
                transform.position.z + destval_z
            );

            dist = Vector3.Distance(transform.position, desitnation);

            if (dist > maxSpeed) 
            {
                dist = maxSpeed;
            }

            marketObj.transform.position = desitnation;

            transform.position = Vector3.MoveTowards(
                transform.position, desitnation,
                (Time.deltaTime * (dist + 1f))
                );
        }


        void WanderRandom()
        {
            float dist = Vector3.Distance(transform.position, desitnation);

            if (dist > maxSpeed)
            {
                dist = maxSpeed;
            }

            if (dist < retrigDistance)
            {
                float destval_x = Random.Range(-maxWanderDistance, maxWanderDistance);
                float destval_z = Random.Range(-maxWanderDistance, maxWanderDistance);

                if (wanderY) {
                    float destval_y = Random.Range(-maxWanderDistance, maxWanderDistance);

                    desitnation = new Vector3(
                        wanderOffset.x + destval_x,
                        wanderOffset.y + destval_y,
                        wanderOffset.z + destval_z
                    );

                }
                else {
                    desitnation = new Vector3(
                        wanderOffset.x + destval_x,
                        transform.position.y,
                        wanderOffset.z + destval_z
                    );
                }

            }
            else
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, desitnation,
                    (Time.deltaTime * (dist + 1f))
                );
            }
        }


        void Update() {
            maxSpeed = PlayerPrefs.GetFloat("slider_h");
            if (PlayerPrefs.GetInt("wander") == 1 && PlayerPrefs.GetInt("play_audio") == 1) {
                if (randomDestination) {
                    WanderRandom();
                } else {
                    WanderCircle();
                }
            }
        }


        void OnEnable()
        {
            float destval_x = Random.Range(-maxWanderDistance, maxWanderDistance);
            float destval_z = Random.Range(-maxWanderDistance, maxWanderDistance);

            desitnation = new Vector3(destval_x, 0, destval_z);
        }

    }
}