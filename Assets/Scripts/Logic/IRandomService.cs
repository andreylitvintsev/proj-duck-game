namespace Logic
{
    public interface IRandomService
    {
        public int Range(int minInclude, int maxExclude);
        public float Range(float minInclude, float maxInclude);
        public bool Boolean();
    }
}