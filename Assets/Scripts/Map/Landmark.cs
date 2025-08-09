using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Landmark : MonoBehaviour
{
    public LandmarkData data;
    public TextMeshProUGUI nameTextObj;
    public TextMeshProUGUI explanationTextObj;
    public Button enterLevelButton;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            nameTextObj.text = data.name;
            explanationTextObj.text = data.explanation;
            enterLevelButton.interactable = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            nameTextObj.text = "";
            explanationTextObj.text = "";       
            enterLevelButton.interactable = false;
        }
    }
}
