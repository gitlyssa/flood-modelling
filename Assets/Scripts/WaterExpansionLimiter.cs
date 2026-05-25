using UnityEngine;

public class WaterExpansionLimiter : MonoBehaviour
{
    [SerializeField] private bool limitXScale = true;
    [SerializeField] private float maximumXScale = 20f;
    [SerializeField] private bool limitZScale = true;
    [SerializeField] private float maximumZScale = 20f;

    public Vector3 ClampScale(Vector3 scale)
    {
        if (limitXScale)
        {
            scale.x = Mathf.Min(scale.x, maximumXScale);
        }

        if (limitZScale)
        {
            scale.z = Mathf.Min(scale.z, maximumZScale);
        }

        return scale;
    }
}
