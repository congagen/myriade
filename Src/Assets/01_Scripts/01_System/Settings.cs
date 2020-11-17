using UnityEngine;
using System.Collections.Generic;

namespace MyriadeE
{
    public class Settings : MonoBehaviour
    {

        public enum environmentTypeValues
        {
            DynamicXZ = 1, StaticXZ = 2, DynamicXYZ = 3, StaticXYZ = 4
        }

        public enum updateModeValues
        {
            Static = 1, Instant = 2
        }

        public enum cameraProjectionValues
        {
            Orthographic = 1, Perspective = 2
        }

        public enum rootTileCountVaules
        {
            _4x4 = 2, _6x6 = 3, _8x8 = 4, _10x10 = 5, _12x12 = 6, _14x14 = 7, _16x16 = 8, _18x18 = 9, _20x20 = 10,
            _22x22 = 11, _24x24 = 12, _26x26 = 13, _28x28 = 14, _30x30 = 15, _40x40 = 20, _50x50 = 25
        }

        public enum tileContentVaules
        {
            _1x1 = 1, _2x2 = 2, _3x3 = 3, _4x4 = 4
        }

        public enum NoiseTypes
        {
            BasicPerlin = 1, PerlinSine_A = 2, PerlinSine_B = 3, Random = 4
        }


        //------------------------------------------------------------------------------------
        [Header("System")]
        //------------------------------------------------------------------------------------

        [Space(5)]

        public GameObject omniSettingsObject;

        [Space(15)]

        public GameObject navigationObject;

        [Space(10)]

        //[Tooltip("Continuously updated or static environment")]
        //public environmentTypeValues environmentType = environmentTypeValues.DynamicXZ;

        //[Tooltip("Update environment continuously or when no user input is detected")]
        //public updateModeValues updateMethod = updateModeValues.Instant;

        [Tooltip("Update frequency for user input and position")]
        [Range(0f, 1f)] public float mainUpdateInterval = 0.1f;

        [Tooltip("Minimum delay before updating tiles, max will depend on tile/content count")]
        [Range(0f, 1f)] public float tileUpdateDelay = 0.01f;

        [Space(10)]

        //------------------------------------------------------------------------------------
        [Header("Content")]
        //------------------------------------------------------------------------------------

        [Space(5)]

        [Tooltip(("Prefab paths (relative to any project directory named Resources)"))]
        public List<string> prefabPaths = new List<string>() { "" };

        [Space(5)]

        [Tooltip("Game object to be used as root tile")]
        public GameObject rootTileObject;

        [Tooltip("Game object to be used as content selector")]
        public GameObject contentSelector;

        [Space(5)]
                       

        //------------------------------------------------------------------------------------
        [Header("Dynamics")]
        //------------------------------------------------------------------------------------

        [Space(5)]

        [Tooltip("Scale content using the chosen noise method")]
        public bool scale = true;

        [Space(10)]

        //------------------------------------------------------------------------------------
        [Header("Noise")]
        //------------------------------------------------------------------------------------

        [Space(5)]

        [Tooltip("Noise scale multiplier")]
        public float noiseScale = 0.987654321f;

        [Tooltip("Main noise method, may affect content activation and customization")]
        public NoiseTypes noiseType = NoiseTypes.BasicPerlin;

        [Tooltip("Random seed for random noise types")]
        public int randomSeed = 0;


    }

}