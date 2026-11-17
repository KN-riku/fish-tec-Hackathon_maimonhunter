using UnityEngine;

public class SideMenuController : MonoBehaviour
{
    public GameObject sideMenuPanel; // インスペクターでSideMenuPanelを登録

    void Start()
    {
        // 起動時はメニューを隠しておく
        sideMenuPanel.SetActive(false);
    }

    // メニューを開く（三本線ボタンに割り当てる）
    public void OpenMenu()
    {
        sideMenuPanel.SetActive(true);
    }

    // メニューを閉じる（×ボタンに割り当てる）
    public void CloseMenu()
    {
        sideMenuPanel.SetActive(false);
    }
}