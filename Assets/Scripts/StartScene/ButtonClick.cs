using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ButtonClick : MonoBehaviour
{
    public void OnButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundType.MENU_CLICK);
        SceneNavigator.Instance.GoToTutorialScene();
    }
}