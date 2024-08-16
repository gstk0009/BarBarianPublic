using UnityEngine;

public class Condition
{
    public float curValue;
    public float maxValue;


    public Condition(float value)
    {
        this.maxValue = value;
        this.curValue = maxValue;
    }

    public void SetCurValueToZero()
    {
        this.curValue = 0;
    }


    public int AddCurValue(float amount)
    {
        if (curValue != maxValue)
        {
            curValue = Mathf.Min(curValue + amount, maxValue);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public void SubtractCurValue(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }
    public void AddMaxValue(float amount)
    {
        maxValue += amount;
    }

    public void SubtractMaxValue(float amount)
    {
        maxValue = Mathf.Max(maxValue - amount, 0.0f);
    }

    public void AddMaxCurValue(float amount)
    {
        maxValue += amount;
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void SubtractMaxCurValue(float amount)
    {
        maxValue = Mathf.Max(maxValue - amount, 0.0f);
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }


    public float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void BuffCurValue(float amount)
    {
        curValue += amount;
    }

}
