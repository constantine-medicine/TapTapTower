using UnityEngine;

public class IsEnabled : MonoBehaviour
{
    [SerializeField] private int needToUnlock;
    [SerializeField] private Material blackMaterial;

    private void Start()
    {
        if (PlayerPrefs.GetInt("score") < needToUnlock)
            GetComponent<MeshRenderer>().material = blackMaterial;
    }
}
