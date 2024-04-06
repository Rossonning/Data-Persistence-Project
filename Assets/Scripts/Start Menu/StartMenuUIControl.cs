using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
public class StartMenuUIControl : MonoBehaviour
{
    [SerializeField] private TMP_InputField _userNameInput;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartNew()
    {
        MenuManager.Instance.playerName = _userNameInput.text;
        SceneManager.LoadScene(1);
    }
}