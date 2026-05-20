using UnityEngine;

public class RisingWater : MonoBehaviour
{
    [SerializeField] private float riseAmount = 0.25f;
    [SerializeField] private float timeBetweenRises = 1f;
    [SerializeField] private bool startRisingOnPlay = true;
    [SerializeField] private bool useMaximumHeight;
    [SerializeField] private float maximumHeight = 10f;

    private float timer;
    private bool isRising;

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
            RaiseWater();
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

    private void RaiseWater()
    {
        Vector3 position = transform.position;
        position.y += riseAmount;

        if (useMaximumHeight && position.y > maximumHeight)
        {
            position.y = maximumHeight;
            isRising = false;
        }

        transform.position = position;
    }
}
