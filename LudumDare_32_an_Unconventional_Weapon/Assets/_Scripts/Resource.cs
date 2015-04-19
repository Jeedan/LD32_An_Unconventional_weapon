using UnityEngine;
using System.Collections;

[System.Serializable]
public class Resource
{
    public float currentResource;
    private float maxResource;
    public float MaxResource { get { return maxResource; } set { maxResource = value; } }

    // Use this for initialization
    public Resource()
    {
    }

    public float SubtractResource(float value, float amount)
    {
        float result = value - amount;

        if (result < 0.0f)
        {
            isDead();
            result = 0.0f;
        }

        return (result);
    }

    // no more resource available
    public bool isDead()
    {
        if (currentResource > 0.0f)
        {
            return false;
        }
        else
        {
            currentResource = 0.0f;
            return true;
        }
    }

    public void ResetResource()
    {
        currentResource = maxResource;
    }

}
