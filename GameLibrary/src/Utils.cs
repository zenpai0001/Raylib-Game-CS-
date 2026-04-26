namespace GameLibrary;

// Designated dumping ground for extension methods and math functions
public static class Utils
{
    public static float MoveTowards(this float start, float target, float maxDistanceDelta)
    {
        if (Math.Abs(target - start) < maxDistanceDelta)
        {
            return target;
        } 
        return (start > target) ? (start - maxDistanceDelta) : (start + maxDistanceDelta);
    }
}