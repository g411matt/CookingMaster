using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.StartGame();
    }
}
