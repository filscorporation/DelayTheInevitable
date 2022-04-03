using System;
using System.Collections;
using Steel;
using Random = Steel.Random;

namespace SteelCustom
{
    public class CameraController : ScriptComponent
    {
        private Coroutine shakeRoutine;
        
        private const float MAX_OFFSET = 0.5F;
        private const float SHAKE_REDUCTION = 0.6F;
        
        public void Shake(float strength)
        {
            if (shakeRoutine != null)
                StopCoroutine(shakeRoutine);
            
            shakeRoutine = StartCoroutine(ShakeRoutine(strength));
        }

        private IEnumerator ShakeRoutine(float strength)
        {
            float seed = Random.NextFloat(0, 1);
            float timePassed = 0.0f;

            while (strength > 0.0f)
            {
                float offsetX = MAX_OFFSET * strength * Random.PerlinNoise(seed + 0.1F, timePassed * 1000f);
                float offsetY = MAX_OFFSET * strength * Random.PerlinNoise(seed + 0.2F, timePassed * 1000f);

                Transformation.Position = new Vector3(offsetX, offsetY, Transformation.Position.Z);
                
                timePassed += Time.DeltaTime;
                strength -= Time.DeltaTime * SHAKE_REDUCTION;

                yield return null;
            }
        }
    }
}