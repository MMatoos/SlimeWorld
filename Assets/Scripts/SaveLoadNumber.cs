using UnityEngine;
using UnityEngine.UI;

public class SaveLoadNumber : MonoBehaviour
{
    public int number;
    public Text numberContainer;
    void Start()
    {
        number = 0;
        numberContainer.text = number.ToString();
    }

    public void IncreaseNumber()
    {
        number++;
        numberContainer.text = number.ToString();
    }

    public void DescreaseNumber()
    {
        if (number != 0)
        {
            number--;
            numberContainer.text = number.ToString();
        }
    }
}
