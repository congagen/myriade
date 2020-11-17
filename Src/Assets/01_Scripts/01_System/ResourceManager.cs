using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace MyriadeE
{
    public class ResourceManager : MonoBehaviour 
    {

        public GameObject MyriadeESettingsObject;

        [HideInInspector]

        public bool sessionDataReady = false;
		public List<GameObject> projectPrefabObjecstList;
		public List<Texture> projectTextureList;


		void LoadGameObjects(List<string> gameObjectPaths) 
		{
			foreach (string path in gameObjectPaths) 
			{
				if (path != "") 
				{
					GameObject[] prefabFiles = Resources.LoadAll<GameObject> (path);
					foreach (GameObject ob in prefabFiles) 
					{
						if(ob.gameObject.GetComponent<Renderer>() != null)
						{
							projectPrefabObjecstList.Add (ob);
						}
					}
				} 
			}

		}
			

			
		void Start() 
		{
			List <string>  prefabPaths = MyriadeESettingsObject.GetComponent<MyriadeE.Settings> ().prefabPaths;
			LoadGameObjects (prefabPaths);		

			Debug.Log ("GameObjects loaded: " + projectPrefabObjecstList.Count.ToString());

			sessionDataReady = true;
		}

	}
}