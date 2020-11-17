using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyriadeE 
{
	public class TileContentManager : MonoBehaviour
    {

		private bool objectSetupComplete = false;
		public GameObject settingsObject;
        public GameObject sessionObject;
        public bool midpointPivot = false;
        public int indexName = 0;

        private GameObject currentTileObject;

		private List<GameObject> objectList = new List<GameObject>();
		private List<GameObject> objectPool = new List<GameObject>();

        private bool rotateContent = false;
        private bool resizeContent = false;

		private float minSize;
		private float maxSize;

		private float rootTileSize;
		private float positionNoiseValue;
		private float contentSizeLimit;

		private float cur_obj_size = 0f;
		private float minUpdateDelay = 0.01f;
		private float maxUpdateDelay = 0.1f;

		private GameObject currentContentObject;
		private Vector3 absolutePosition;

        private int noiseType = 1;
        Noise nvGens;


		void InitContentSelectorObjectPool()
		{
            objectList = sessionObject.GetComponent<MyriadeE.ResourceManager> ().projectPrefabObjecstList;

			if(objectList.Count > 0)
			{
				for (int i = 0; i < objectList.Count; i++) 
				{
					GameObject inst_obj = objectList[i];
					GameObject obj = Instantiate (inst_obj, transform.position, Quaternion.identity) as GameObject;
					obj.transform.SetParent (transform);

                    objectPool.Add (obj);
					obj.transform.gameObject.SetActive (false);
				}
			}
		}

		void ScaleContent(GameObject cr_ob)
		{
            cur_obj_size = Mathf.Clamp(Mathf.Abs(Mathf.Sin(positionNoiseValue * 23415612.4284f)) * maxSize, minSize, maxSize);
            cr_ob.transform.localScale = new Vector3(cur_obj_size, cur_obj_size, cur_obj_size);
		}


		void UpdateContent()
		{
			absolutePosition = transform.parent.position + transform.position;
            positionNoiseValue = nvGens.PositionNoise (
                absolutePosition.x, absolutePosition.y, 
                absolutePosition.z, noiseType
            );

			foreach(Transform tr in transform)
			{
				tr.gameObject.SetActive (false);
			}

			if (objectPool.Count > 0 && transform.parent != null) 
			{
				int obindx = Mathf.Clamp (
                    Mathf.RoundToInt (objectPool.Count * positionNoiseValue), 
                    0, 
                    Mathf.Abs(objectPool.Count - 1)
                );

				currentContentObject = objectPool[obindx];
				currentContentObject.gameObject.SetActive (true);

				if (resizeContent) 
				{
					ScaleContent (currentContentObject);
                } 

                if (midpointPivot) {
                    float currentObjectHeight = currentContentObject.GetComponent<Renderer>().bounds.size.y;

                    currentContentObject.transform.position = new Vector3(
                        transform.position.x, 
                        transform.parent.position.y + (currentObjectHeight * 0.5f), 
                        transform.position.z);
                } else {
                    currentContentObject.transform.position = new Vector3(
                        transform.position.x, 
                        transform.parent.position.y, 
                        transform.position.z);
                }

			}
		}


		void InitContentManager()
		{
            noiseType = (int)settingsObject.GetComponent<MyriadeE.Settings>().noiseType;

			minUpdateDelay = settingsObject.GetComponent<MyriadeE.Settings>().tileUpdateDelay;
			maxUpdateDelay = settingsObject.GetComponent<MyriadeE.Settings>().tileUpdateDelay * 2f;

            currentTileObject = transform.parent.gameObject;

            indexName = currentTileObject.GetComponent<MyriadeE.TileManager>().indexName;

            rotateContent = false;
            resizeContent = settingsObject.GetComponent<MyriadeE.Settings>().scale;

            int contentCount = 1;

			float tileBounds = currentTileObject.GetComponent<MeshRenderer> ().bounds.size.x;
			rootTileSize = tileBounds / currentTileObject.transform.localScale.x;

			if (contentCount > 1) 
			{
				contentSizeLimit = (rootTileSize / (contentCount));
			} 
			else 
			{
				contentSizeLimit = ((rootTileSize));
			}

            minSize = ((200 * 0.01f) * (contentSizeLimit));
            maxSize = ((200 * 0.01f) * (contentSizeLimit));
		}


		void OnEnable()
		{
			if (objectSetupComplete) {
				Invoke ("UpdateContent", Random.Range (minUpdateDelay, maxUpdateDelay));
			} 
			else 
            {
                nvGens = sessionObject.GetComponent<MyriadeE.Noise>();
                InitContentManager();
                InitContentSelectorObjectPool();
				objectSetupComplete = true;
                UpdateContent();
			}
		}


	}
}