using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasButtons : MonoBehaviour
{
    [SerializeField] Sprite musicOn, musicOff;

    private void Start()
    {
        if (PlayerPrefs.GetString("music") == "No" && gameObject.name == "Sound")
            GetComponent<Image>().sprite = musicOff;
    }

    public void RestartGame()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Shop");
    }

    public void CloseShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("SampleScene");
    }

    public void InstagramOpen()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        Application.OpenURL("https://www.instagram.com/polkovnik1806/");
    }

    public void MusicWork()
    {
        if (PlayerPrefs.GetString("music") == "No")
        {
            PlayerPrefs.SetString("music", "Yes");
            if (PlayerPrefs.GetString("music") != "No")
                GetComponent<AudioSource>().Play();
            GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetString("music", "No");

            GetComponent<Image>().sprite = musicOff;
        }
    }
}
