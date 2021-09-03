using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float changeCubePlaceSpeed;
    [SerializeField] private Transform cubeToPlace;
    [SerializeField] private GameObject allCubes;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject vfx;
    [SerializeField] private Animator[] uIAnimator;
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private Color[] bgColors;
    [SerializeField] private Text score;
    [SerializeField] private GameObject[] cubesToCreate;

    private List<GameObject> cubesToCreatePossible = new List<GameObject>();
    private CubePosition nowCube = new CubePosition(0, 1, 0);
    private List<Vector3> allCubesPositions = new List<Vector3>()
    {
        new Vector3(0,0,0),
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),
        new Vector3(1,0,1),
        new Vector3(-1,0,-1),
        new Vector3(-1,0,1),
        new Vector3(1,0,-1),
    };
    private Rigidbody allCubesRb;
    
    private bool isLoose, startGame;

    private Coroutine showCubePlace;

    private Transform mainCam;
    private float moveCameraPositionY;
    private int prevCountMaxHorizontal;

    private Color toCameraColor;

    private void Start()
    {
        if (PlayerPrefs.GetInt("score") < 5)
            cubesToCreatePossible.Add(cubesToCreate[0]);
        else if (PlayerPrefs.GetInt("score") < 10)
            AddPossibleCubes(2);
        else if (PlayerPrefs.GetInt("score") < 15)
            AddPossibleCubes(3);
        else if (PlayerPrefs.GetInt("score") < 20)
            AddPossibleCubes(4);
        else if (PlayerPrefs.GetInt("score") < 25)
            AddPossibleCubes(5);
        else if (PlayerPrefs.GetInt("score") < 30)
            AddPossibleCubes(6);
        else if (PlayerPrefs.GetInt("score") < 35)
            AddPossibleCubes(7);
        else if (PlayerPrefs.GetInt("score") < 40)
            AddPossibleCubes(8);
        else if (PlayerPrefs.GetInt("score") < 45)
            AddPossibleCubes(9);

        score.text = "<size=50><color=#FF00E6>Best:</color></size> " + PlayerPrefs.GetInt("score") + "<size=40>\nNow:</size> 0";
        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        moveCameraPositionY = 6.57f + nowCube.y - 1f;
        showCubePlace = StartCoroutine(ShowCubePlace());
        allCubesRb = allCubes.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && 
            cubeToPlace != null && 
            !EventSystem.current.IsPointerOverGameObject() &&
            allCubes != null)
        {
#if !UNITY_EDITOR
        if (Input.GetTouch(0).phase != TouchPhase.Began) return;
#endif
            if (!startGame)
            {
                foreach (var item in uIAnimator)
                {
                    item.SetBool("StartGame", true);
                }
                startGame = true;
            }

            GameObject createCube = null;
            if (cubesToCreatePossible.Count == 1)
                createCube = cubesToCreatePossible[0];
            else
                createCube = cubesToCreatePossible[UnityEngine.Random.Range(0, cubesToCreatePossible.Count)];

            GameObject newCube = Instantiate(
                createCube, 
                cubeToPlace.position, 
                Quaternion.identity) as GameObject;

            newCube.transform.SetParent(allCubes.transform);
            nowCube.SetVector(newCube.transform.position);
            allCubesPositions.Add(nowCube.GetVector());

            GameObject newObject = Instantiate(vfx, newCube.transform.position, Quaternion.identity) as GameObject;
            Destroy(newObject, 1f);

            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().Play();
            }


            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;

            SpawnPositions();
            MoveCameraChangeBg();
        }

        if (!isLoose && allCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(cubeToPlace.gameObject);
            isLoose = true;
            StopCoroutine(showCubePlace);
            restartButton.SetActive(true);
        }

        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
            new Vector3(mainCam.localPosition.x, moveCameraPositionY, mainCam.localPosition.z),
            cameraMoveSpeed * Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor,
                toCameraColor, Time.deltaTime / 1.05f);
        }
    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPositions();

            yield return new WaitForSeconds(changeCubePlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x)
        {
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        }

        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)
        {
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        }

        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        }

        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.x - 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        }

        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        }

        int count = positions.Count;
        if (count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, count)];
        else if (count == 0) 
            isLoose = true;
        else
            cubeToPlace.position = positions[0];
    }

    private bool IsPositionEmpty(Vector3 position)
    {
        if (position.y == 0) return false;

        foreach (var item in allCubesPositions)
        {
            if (item.x == position.x && item.y == position.y && item.z == position.z)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveCameraChangeBg()
    {
        int maxX = 0, maxY = 0, maxZ = 0, maxHorizontal;

        foreach (var item in allCubesPositions)
        {
            if (Mathf.Abs(Convert.ToInt32(item.x)) > maxX) 
                maxX = Convert.ToInt32(item.x);

            if ((Convert.ToInt32(item.y)) > maxY)
                maxY = Convert.ToInt32(item.y);

            if (Mathf.Abs(Convert.ToInt32(item.z)) > maxZ)
                maxZ = Convert.ToInt32(item.z);
        }

        maxY--;
        if (PlayerPrefs.GetInt("score") < maxY)
            PlayerPrefs.SetInt("score", maxY);

        score.text = "<size=50><color=#FF00E6>Best:</color></size> "+PlayerPrefs.GetInt("score")+"<size=40>\nNow:</size> "+maxY;

        moveCameraPositionY = 6.57f + nowCube.y - 1f;
        mainCam.localPosition = new Vector3(mainCam.localPosition.x, mainCam.localPosition.y, mainCam.localPosition.z - 0.2f);

        maxHorizontal = maxX > maxZ ? maxX : maxZ;

        if (maxHorizontal % 3 == 0 && prevCountMaxHorizontal != maxHorizontal)
        {
            mainCam.localPosition -= new Vector3(0f, 0f, 2.5f);
            prevCountMaxHorizontal = maxHorizontal;
        }

        if (maxY >= 30)
        {
            toCameraColor = bgColors[2];
        }

        else if (maxY >= 15)
        {
            toCameraColor = bgColors[1];

        }

        else if (maxY >= 5)
        {
            toCameraColor = bgColors[0];
        }

    }

    private void AddPossibleCubes(int till)
    {
        for (int i = 0; i < till; i++)
        {
            cubesToCreatePossible.Add(cubesToCreate[i]);
        }
    }
}

struct CubePosition
{
    public int x, y, z;

    public CubePosition(int x, int y, int z) 
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }

    public void SetVector(Vector3 position)
    {
        x = Convert.ToInt32(position.x);
        y = Convert.ToInt32(position.y);
        z = Convert.ToInt32(position.z);
    }
}