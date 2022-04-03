using Steel;

namespace SteelCustom
{
    public abstract class Spell : ScriptComponent
    {
        public abstract void Cast(Vector3 position);
    }
}