using UnityEngine;

public class RisingWater : MonoBehaviour
{
    [SerializeField] private float riseAmount = 0.25f;
    [SerializeField, HideInInspector] private float expansionAmount = 0.05f;
    [SerializeField] private float spreadPositiveX = 0.05f;
    [SerializeField] private float spreadNegativeX = 0.05f;
    [SerializeField] private float spreadPositiveZ = 0.05f;
    [SerializeField] private float spreadNegativeZ = 0.05f;
    [SerializeField] private float timeBetweenRises = 1f;
    [SerializeField] private bool startRisingOnPlay = true;
    [SerializeField] private bool useMaximumHeight;
    [SerializeField] private float maximumHeight = 10f;

    private float timer;
    private bool isRising;
    private WaterExpansionLimiter expansionLimiter;

    private void Awake()
    {
        expansionLimiter = GetComponent<WaterExpansionLimiter>();
    }

    private void Start()
    {
        isRising = startRisingOnPlay;
    }

    private void Update()
    {
        if (!isRising || timeBetweenRises <= 0f)
        {
            return;
        }

        timer += Time.deltaTime;

        while (timer >= timeBetweenRises)
        {
            timer -= timeBetweenRises;
            RaiseAndExpandWater();
        }
    }

    public void StartRising()
    {
        isRising = true;
    }

    public void StopRising()
    {
        isRising = false;
    }

    private void RaiseAndExpandWater()
    {
        RaiseWaterHeight();
        ExpandWater();
    }

    private void RaiseWaterHeight()
    {
        Vector3 scale = transform.localScale;
        float previousHeight = scale.y;
        scale.y += riseAmount;

        if (useMaximumHeight && scale.y > maximumHeight)
        {
            scale.y = maximumHeight;
            isRising = false;
        }

        transform.localScale = scale;
        transform.position += Vector3.up * ((scale.y - previousHeight) * 0.5f);
    }

    private void ExpandWater()
    {
        Vector3 scale = transform.localScale;
        Vector3 previousScale = scale;

        float positiveXSpread = Mathf.Max(0f, spreadPositiveX);
        float negativeXSpread = Mathf.Max(0f, spreadNegativeX);
        float positiveZSpread = Mathf.Max(0f, spreadPositiveZ);
        float negativeZSpread = Mathf.Max(0f, spreadNegativeZ);

        if (positiveXSpread + negativeXSpread + positiveZSpread + negativeZSpread <= 0f)
        {
            positiveXSpread = Mathf.Max(0f, expansionAmount);
            negativeXSpread = Mathf.Max(0f, expansionAmount);
            positiveZSpread = Mathf.Max(0f, expansionAmount);
            negativeZSpread = Mathf.Max(0f, expansionAmount);
        }

        scale.x += positiveXSpread + negativeXSpread;
        scale.z += positiveZSpread + negativeZSpread;

        if (expansionLimiter != null)
        {
            scale = expansionLimiter.ClampScale(scale);
        }

        transform.localScale = scale;

        float appliedXSpread = scale.x - previousScale.x;
        float appliedZSpread = scale.z - previousScale.z;
        float xDirectionOffset = GetDirectionalOffset(positiveXSpread, negativeXSpread, appliedXSpread);
        float zDirectionOffset = GetDirectionalOffset(positiveZSpread, negativeZSpread, appliedZSpread);

        transform.position += transform.right * xDirectionOffset;
        transform.position += transform.forward * zDirectionOffset;
    }

    private float GetDirectionalOffset(float positiveSpread, float negativeSpread, float appliedSpread)
    {
        float requestedSpread = positiveSpread + negativeSpread;

        if (requestedSpread <= 0f || appliedSpread <= 0f)
        {
            return 0f;
        }

        float appliedRatio = appliedSpread / requestedSpread;
        float appliedPositiveSpread = positiveSpread * appliedRatio;
        float appliedNegativeSpread = negativeSpread * appliedRatio;

        return (appliedPositiveSpread - appliedNegativeSpread) * 0.5f;
    }
}
