using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace MyriadeE 
{
    public class Noise: MonoBehaviour 
    {

        public float noiseScale = 0.987654321f;
        public int randomSeed = 1234567;


        public float BasicPerlin(float pos_x, float pos_y, float pos_z, float nScale)
        {
            float prl_a = (pos_x) * nScale;
            float prl_b = (pos_y) * nScale;
            float prl_c = (pos_z) * nScale;

            return Mathf.PerlinNoise((prl_a + prl_c), (prl_a + prl_b));
        }


        public float PerlinSin_B(float pos_x, float pos_y, float pos_z, float nScale)
        {
            float prl_a = (Mathf.Sin(pos_x)) * nScale;
            float prl_b = (Mathf.Sin(pos_y)) * nScale;
            float prl_c = (Mathf.Sin(pos_z)) * nScale;

            return Mathf.PerlinNoise((prl_a + prl_c), (prl_a + prl_b));
        }


        public float PerlinSin_C(float pos_x, float pos_y, float pos_z, float nScale)
        {
            float prl_a = (Mathf.Sin(pos_x * nScale));
            float prl_b = (Mathf.Sin(pos_y * nScale));
            float prl_c = (Mathf.Sin(pos_z * nScale));

            return Mathf.PerlinNoise((prl_a + prl_c), (prl_a + prl_b));
        }
			

        public float PositionRandom(float pos_x, float pos_y, float pos_z, float nScale)
        {
            float prl_a = ((pos_x) + pos_z) * nScale;
            float prl_b = ((pos_y) + pos_x) * nScale;
            float prl_c = ((pos_z) + pos_x) * nScale;

            float maxVal = Mathf.Abs(Mathf.Sin(prl_a + prl_b + prl_c));
            float randomVal = Random.Range(0.0F, maxVal);

            return randomVal;
        }


        public float PositionNoise(float pos_x, float pos_y, float pos_z, int noiseType = 1)
		{

            if (noiseType == 1) 
			{
                return BasicPerlin(pos_x, pos_y, pos_z, noiseScale);
			} 
            else if (noiseType == 2) 
			{
                return PerlinSin_B (pos_x, pos_y, pos_z, noiseScale);
			} 
            else if (noiseType == 3) 
			{
                return PerlinSin_C (pos_x, pos_y, pos_z, noiseScale);
			} 
            else if (noiseType == 4) 
            {
                return PositionRandom (pos_x, pos_y, pos_z, noiseScale);
            } 
			else 
			{
                return BasicPerlin(pos_x, pos_y, pos_z, noiseScale);
			}
		}


        public void SetRandomSeed(int rSeed)
        {
            if (rSeed != 0) {
                Random.InitState(rSeed);    
            } else {
                Random.InitState(randomSeed);
            }
		}


	}
}