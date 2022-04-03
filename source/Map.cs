using System.Collections.Generic;
using Steel;

namespace SteelCustom
{
    public class Map : ScriptComponent
    {
        public void Generate()
        {
            Sprite ground = ResourcesManager.LoadImage("ground2.png");
            List<Sprite> groundDetails = new List<Sprite>();
            groundDetails.Add(ResourcesManager.LoadImage("ground.png"));
            groundDetails.Add(ResourcesManager.LoadImage("ground3.png"));
            groundDetails.Add(ResourcesManager.LoadImage("ground4.png"));

            const float SIZE = 32;
            const float DETAIL_CHANCE = 0.1f;

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    Entity entity = new Entity($"Tile {i} {j}", Entity);
                    entity.AddComponent<SpriteRenderer>().Sprite = 
                        Random.NextFloat(0.0f, 1.0f) >= DETAIL_CHANCE
                        ? ground
                        : groundDetails[Random.NextInt(0, groundDetails.Count - 1)];
                    
                    entity.Transformation.Position = new Vector3(
                        (i - SIZE / 2) * 2,
                        (j - SIZE / 2) * 2,
                        -1.0f
                    );
                }
            }
        }
    }
}