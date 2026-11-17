using UnityEngine;

public class FishingMethodManager : MonoBehaviour
{
    public static FishingMethodManager Instance;

    public FishingMethod currentMethod = FishingMethod.Teichi;

    void Awake()
    {
        Instance = this;
    }

    public void SetMethod(FishingMethod method)
    {
        currentMethod = method;
        Debug.Log("åªç›ÇÃãôñ@: " + currentMethod);
    }

    public float GetMultiplier()
    {
        switch (currentMethod)
        {
            case FishingMethod.Maki:
                return 1f;

            case FishingMethod.Ippon:
                return 3f;

            case FishingMethod.Sashi:
                return 1.5f;

            case FishingMethod.Teichi:
                return 2f;
        }

        return 1f;
    }
}