using UnityEngine;

public class WaterNeighborTrigger : MonoBehaviour
{
    [SerializeField] private RisingWater[] neighboringWaterBlocks;
    [SerializeField] private bool triggerByHeight = true;
    [SerializeField] private float heightNeededToTrigger = 2f;
    [SerializeField] private bool triggerBySpread;
    [SerializeField] private float xScaleNeededToTrigger = 5f;
    [SerializeField] private float zScaleNeededToTrigger = 5f;

    private bool hasTriggeredNeighbors;

    private void Update()
    {
        if (hasTriggeredNeighbors || !ShouldTriggerNeighbors())
        {
            return;
        }

        TriggerNeighbors();
    }

    private bool ShouldTriggerNeighbors()
    {
        Vector3 scale = transform.localScale;
        bool reachedHeight = triggerByHeight && scale.y >= heightNeededToTrigger;
        bool reachedSpread = triggerBySpread
            && scale.x >= xScaleNeededToTrigger
            && scale.z >= zScaleNeededToTrigger;

        return reachedHeight || reachedSpread;
    }

    private void TriggerNeighbors()
    {
        hasTriggeredNeighbors = true;

        foreach (RisingWater waterBlock in neighboringWaterBlocks)
        {
            if (waterBlock != null)
            {
                waterBlock.StartRising();
            }
        }
    }
}
