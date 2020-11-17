using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyriadeE 
{
	public class EnvironmentManager : MonoBehaviour
	{
		[HideInInspector]
		public bool objectSetupComplete = false;

        [Tooltip("Game object to be used as root tile")]
        public GameObject EnvSettingsObject;
        public GameObject MyriadeESettingsObject;
        private GameObject navigationObject;

        public GameObject currentRootTileObject;

        [Space(10)]

        private Vector3 currentQuantizedEnvPosition;
        private Vector3 previousQuantizedPosition;
        private int currentEnvType = 1;
        private int currentEnvMode = 2;
        private float mainUpdateDelay = 0.1F;
        private int tileCount;

        private float contentThresh = 50; 

		public int rootTileSizePositionOffset;

		private List<Vector3> rootTilePositionOffsetList = new List<Vector3>();

        private Dictionary<Vector3, GameObject> tileMapDict = new Dictionary<Vector3, GameObject>();
        private Dictionary<Vector3, Vector3> offsetDict = new Dictionary<Vector3, Vector3>();

		private HashSet<Vector3> newRootTilePositions = new HashSet<Vector3>();
		private List<GameObject> tilesToMoveList = new List<GameObject>();

		private GameObject tileToMove;

        public Vector3 currentQuantizedPosition;

        private float currentTileScale;
        private float tileSizeMultiplier;

        private float currentX;
        private float currentY;
        private float currentZ;


        void UpdateQuantizedPosition()
        {   
            tileSizeMultiplier = 0.1f / (float)currentTileScale;
            float tlScale = currentTileScale * 10f;

            if (navigationObject) {
                Vector3 navObjPos = navigationObject.transform.position;

                currentX = (float)Mathf.RoundToInt(navObjPos.x * tileSizeMultiplier) * (tlScale);
                currentY = (float)Mathf.RoundToInt(navObjPos.y * tileSizeMultiplier) * (tlScale);
                currentZ = (float)Mathf.RoundToInt(navObjPos.z * tileSizeMultiplier) * (tlScale);
                currentQuantizedEnvPosition = new Vector3(currentX, currentY, currentZ);    
            } else {
                currentX = (float)Mathf.RoundToInt(transform.position.x * tileSizeMultiplier) * (tlScale);
                currentY = (float)Mathf.RoundToInt(transform.position.y * tileSizeMultiplier) * (tlScale);
                currentZ = (float)Mathf.RoundToInt(transform.position.z * tileSizeMultiplier) * (tlScale);
                currentQuantizedEnvPosition = new Vector3(currentX, currentY, currentZ);    
            }

        }


        void UpdateRootTiles() {
            //Debug.Log("UpdateRootTiles");

            newRootTilePositions.Clear ();
			tilesToMoveList.Clear ();
			int count = 0;

			foreach (Vector3 ofPos in rootTilePositionOffsetList) {
				newRootTilePositions.Add (currentQuantizedEnvPosition + ofPos);
			}

			foreach (Transform rootTile in transform) {
                if (newRootTilePositions.Contains (rootTile.transform.position)) {
                    newRootTilePositions.Remove (rootTile.transform.position);
				} 
				else {
                    tilesToMoveList.Add (rootTile.gameObject);
                    rootTile.transform.gameObject.SetActive (false);
				}
			}

            foreach (Vector3 rootTilePos in newRootTilePositions) {
				tileToMove = tilesToMoveList [count];
                tileToMove.transform.position = rootTilePos;
				tileToMove.transform.gameObject.SetActive (true);
				count += 1;
			}

		}


		void NavigationUpdate() {
            
            if (currentEnvMode == 1) {
				if (currentQuantizedEnvPosition != previousQuantizedPosition) {
                    
					if (!(Input.anyKey) && Input.touches.Length < 1) {
						previousQuantizedPosition = currentQuantizedEnvPosition;
						Invoke ("UpdateRootTiles", mainUpdateDelay);
					}
				} 

            }  
            else if (currentEnvMode == 2) {
                if (currentQuantizedEnvPosition != previousQuantizedPosition) {
					previousQuantizedPosition = currentQuantizedEnvPosition;
					Invoke ("UpdateRootTiles", mainUpdateDelay);
				}
			}

        }


        void InitTileMap(){
            Debug.Log("InitTileMap");

            int nameCount = 0;

            for (int x = -(tileCount); x < tileCount; x++) {
                for (int z = -(tileCount); z < tileCount; z++) {

                    nameCount += 1;
                    GameObject tile = Instantiate(currentRootTileObject, transform.position, Quaternion.identity) as GameObject;

                    tile.name = "RootTile_" + Mathf.Abs(nameCount).ToString();
                    tile.GetComponent<MyriadeE.TileManager>().indexName = nameCount;
                    tile.GetComponent<MyriadeE.TileManager>().sessionObject = transform.gameObject;
                    tile.GetComponent<MyriadeE.TileManager>().settingsObject = MyriadeESettingsObject;

                    tile.SetActive(true);

                    tile.transform.localScale = new Vector3(
                        rootTileSizePositionOffset * 0.1f,
                        rootTileSizePositionOffset * 0.1f,
                        rootTileSizePositionOffset * 0.1f
                    );

                    tile.transform.SetParent(transform);

                    float quantizedX = (float)Mathf.RoundToInt((x * rootTileSizePositionOffset)) + (rootTileSizePositionOffset * 0.5f);
                    float quantizedZ = (float)Mathf.RoundToInt((z * rootTileSizePositionOffset)) + (rootTileSizePositionOffset * 0.5f);

                    Vector3 offsetPos = new Vector3(quantizedX, transform.position.y, quantizedZ);

                    tileMapDict.Add(offsetPos, tile);
                    offsetDict.Add(offsetPos, offsetPos);

                    rootTilePositionOffsetList.Add(offsetPos);
                }
            }
        }


        


        void InitEnvironment()
        {

            if (MyriadeESettingsObject.GetComponent<Settings>().rootTileObject != null)
            {
                currentRootTileObject = MyriadeESettingsObject.GetComponent<Settings>().rootTileObject;
            }

            if (MyriadeESettingsObject.GetComponent<Settings>().navigationObject != null)
            {
                navigationObject = MyriadeESettingsObject.GetComponent<Settings>().navigationObject;
            }

            currentTileScale = 5;
            tileCount = 10;

            Debug.Log(currentEnvMode);
            Debug.Log(currentEnvType);

            rootTileSizePositionOffset = Mathf.RoundToInt(5 * 10f);
            mainUpdateDelay = MyriadeESettingsObject.GetComponent<Settings>().mainUpdateInterval + 0.001F;
            currentQuantizedEnvPosition = transform.GetComponent<Session>().currentQuantizedPosition;

            InitTileMap();

            Invoke("UpdateRootTiles", mainUpdateDelay);

        }



        void setIndex() {
            int itmC = 0;

            foreach (Transform t_a in transform)
            {
                foreach (Transform t_b in t_a.transform)
                {
                    foreach (Transform t_c in t_b.transform)
                    {
                        itmC += 1;
                        if (t_c.GetComponent<MyriadeM.RndSfx>())
                        {
                            t_c.GetComponent<MyriadeM.RndSfx>().objNoiseIndex = itmC;
                        }
                    }
                }
            }
        }


        void Update () {

            if (!objectSetupComplete && MyriadeESettingsObject) {

                if (transform.GetComponent<MyriadeE.Session>().sessionSetupComplete) {
					InitEnvironment ();

                    if(currentEnvType == 1 || currentEnvType == 3) {
						InvokeRepeating ("NavigationUpdate", 1f, mainUpdateDelay);
					}
                    objectSetupComplete = true;
                }
            } else if (MyriadeESettingsObject) {
                UpdateQuantizedPosition();
            }

        }
		

	}
}