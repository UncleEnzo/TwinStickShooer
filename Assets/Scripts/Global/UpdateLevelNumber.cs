using UnityEngine;
using UnityEngine.UI;

public class UpdateLevelNumber : MonoBehaviour
{
    public Text levelText;
    // Start is called before the first frame update
    void Start()
    {
        int currentLevel = SceneLoader.getCurrentLevel();
        if (currentLevel == SceneLoader.hubWorldIndex)
        {
            levelText.text = "Hub Level";
        }
        else
        {
            levelText.text = "Level " + (currentLevel - 1);
        }
    }
}
