using System;
using Steel;
using Math = Steel.Math;

namespace SteelCustom
{
    public class Projectile : ScriptComponent
    {
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float timer;
        private float duration;
        private Action onFinishAction;
        
        public void Init(Vector3 target, float speed, Action onFinish)
        {
            startPosition = Transformation.Position;
            targetPosition = target;
            duration = Vector3.Distance(startPosition, targetPosition) / speed;
            onFinishAction = onFinish;
        }

        public override void OnUpdate()
        {
            timer += Time.DeltaTime;
            Transformation.Position = Math.Lerp(startPosition, targetPosition, timer / duration);

            if (timer > duration)
            {
                Entity.Destroy();
                onFinishAction?.Invoke();
            }
        }
    }
}