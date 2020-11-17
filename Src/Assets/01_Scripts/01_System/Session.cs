using UnityEngine;
using System.Collections;


namespace MyriadeE 
{
    public class Session : MonoBehaviour 
    {

		[HideInInspector]
		public bool sessionSetupComplete = false;
        public GameObject MyriadeESettingsObject;

		private GameObject navigationObject;
		public Vector3 currentQuantizedPosition;

		[HideInInspector]
		public Vector3 previousQuantizedPosition;


        private void Update()
        {
            transform.gameObject.GetComponent<MyriadeE.Noise>().noiseScale  = MyriadeESettingsObject.GetComponent<Settings>().noiseScale;
            transform.gameObject.GetComponent<MyriadeE.Noise>().randomSeed  = MyriadeESettingsObject.GetComponent<Settings>().randomSeed;

            if (!sessionSetupComplete) {
                transform.gameObject.GetComponent<MyriadeE.Noise>().SetRandomSeed(
                    MyriadeESettingsObject.GetComponent<Settings>().randomSeed
                );

                if (MyriadeESettingsObject.GetComponent<MyriadeE.Settings>().navigationObject)
                {
                    navigationObject = MyriadeESettingsObject.GetComponent<MyriadeE.Settings>().navigationObject;                
                }

                bool dataReady = transform.GetComponent<MyriadeE.ResourceManager>().sessionDataReady;
                bool tileObjectFound = MyriadeESettingsObject.GetComponent<MyriadeE.Settings>().rootTileObject != null;
                bool selectorObjectFound = MyriadeESettingsObject.GetComponent<MyriadeE.Settings>().contentSelector != null;

                if (dataReady && tileObjectFound)
                {
                    sessionSetupComplete = true;
                }
            }

        }


	}
}
