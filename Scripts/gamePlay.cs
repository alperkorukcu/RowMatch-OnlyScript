using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
//Yunus Alper KÖRÜKCÜ  
public class gamePlay : MonoBehaviour
{
    public Camera mainCamera;
    public Sprite levelBack;
    private Color backColor;
    public Font font;
    private Color panelColor;
    public Sprite red;
    public Sprite green;
    public Sprite blue;
    public Sprite yellow;
    public Sprite check;
    private string objectName;
    public static gamePlay instance;
    float lerpDuration = 1f;
    int curX = 0, curY = 0;
    int sX = 0, sY = 0;
    public Sprite checkImage;
    public Sprite congratsImage;

    string[,] newArray;


    Vector2 mouseDown = new Vector2(), mouseUp = new Vector2(), dir = new Vector2();

    private void Awake()
    {
        instance = this;
    }

    string level = "", gWidth = "", gHeight = "", moves = "", grid = "";

    void Start()
    {
        if (ColorUtility.TryParseHtmlString("#202D41", out backColor))
            mainCamera.backgroundColor = backColor;

        createInfo();

        GameObject.Find("levelText").GetComponent<Text>().text += SceneManager.GetActiveScene().name.Substring(5);

        getLevel();

        newArray = getGrid();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, 120))
            {
                objectName = hit.transform.gameObject.name;


                if (gWidth != "" && gHeight != "" && grid != "")
                {
                    //getGrid()[0, 0]

                    mouseDown = Input.mousePosition;

                    GameObject.Find(objectName).GetComponent<RectTransform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);

                    if (objectName.Split('_')[2].Length == 2)
                    {
                        curX = Convert.ToInt32(objectName.Split('_')[2].Substring(0, 1));
                        curY = Convert.ToInt32(objectName.Split('_')[2].Substring(1, 1));
                    }
                    else
                    {
                        curX = Convert.ToInt32(objectName.Split('_')[2].Substring(0, 2));
                        curY = Convert.ToInt32(objectName.Split('_')[2].Substring(2, 2));
                    }

                }
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseUp = Input.mousePosition;

            dir = mouseUp - mouseDown;

            string cName = "", nName = "";

            if (dir.magnitude > 32)
            {
                var currentObject = "jewel_" + newArray[curX, curY] + "_" + curX.ToString() + curY.ToString();
                string nextObject = null;

                GameObject.Find(currentObject).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    if (dir.x > 0)
                    {
                        if (curX < Convert.ToInt32(gWidth) - 1)
                        {
                            nextObject = "jewel_" + newArray[curX + 1, curY] + "_" + (curX + 1).ToString() + curY.ToString();

                            if (nextObject.Split('_')[2].Length == 2)
                            {
                                sX = Convert.ToInt32(nextObject.Split('_')[2].Substring(0, 1));
                                sY = Convert.ToInt32(nextObject.Split('_')[2].Substring(1, 1));
                            }
                            else
                            {
                                sX = Convert.ToInt32(nextObject.Split('_')[2].Substring(0, 2));
                                sY = Convert.ToInt32(nextObject.Split('_')[2].Substring(2, 2));
                            }

                            cName = "jewel_" + newArray[curX, curY] + "_" + (curX + 1).ToString() + curY.ToString();
                            nName = "jewel_" + newArray[curX + 1, curY] + "_" + (sX - 1).ToString() + sY.ToString();

                            if (nextObject != null)
                                swapTwo(currentObject, nextObject, cName, nName, newArray, curX, curY, (curX + 1), curY, "right", "left");

                        }

                    }
                    else
                    {
                        if (curX > 0)
                        {
                            nextObject = "jewel_" + newArray[curX - 1, curY] + "_" + (curX - 1).ToString() + curY.ToString();

                            if (nextObject.Split('_')[2].Length == 2)
                            {
                                sX = Convert.ToInt32(nextObject.Split('_')[2].Substring(0, 1));
                                sY = Convert.ToInt32(nextObject.Split('_')[2].Substring(1, 1));
                            }
                            else
                            {
                                sX = Convert.ToInt32(nextObject.Split('_')[2].Substring(0, 2));
                                sY = Convert.ToInt32(nextObject.Split('_')[2].Substring(2, 2));
                            }

                            cName = "jewel_" + newArray[curX, curY] + "_" + (curX - 1).ToString() + curY.ToString();
                            nName = "jewel_" + newArray[curX - 1, curY] + "_" + (sX + 1).ToString() + sY.ToString();

                            if (nextObject != null)
                                swapTwo(currentObject, nextObject, cName, nName, newArray, curX, curY, (curX - 1), curY, "left", "right");
                        }
                    }



                }
                if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
                {
                    if (dir.y > 0)
                    {
                        if (curY > 0)
                        {
                            nextObject = "jewel_" + newArray[curX, curY - 1] + "_" + curX.ToString() + (curY - 1).ToString();

                            if (nextObject.Split('_')[2].Length == 2)
                            {
                                sX = Convert.ToInt32(nextObject.Split('_')[2].Substring(0, 1));
                                sY = Convert.ToInt32(nextObject.Split('_')[2].Substring(1, 1));
                            }
                            else
                            {
                                sX = Convert.ToInt32(nextObject.Split('_')[2].Substring(0, 2));
                                sY = Convert.ToInt32(nextObject.Split('_')[2].Substring(2, 2));
                            }

                            cName = "jewel_" + newArray[curX, curY] + "_" + curX.ToString() + (curY - 1).ToString();
                            nName = "jewel_" + newArray[curX, curY - 1] + "_" + sX.ToString() + (sY + 1).ToString();

                            if (nextObject != null)
                                swapTwo(currentObject, nextObject, cName, nName, newArray, curX, curY, curX, (curY - 1), "up", "down");
                        }
                    }
                    else
                    {
                        if (curY < Convert.ToInt32(gHeight) - 1)
                        {
                            nextObject = "jewel_" + newArray[curX, curY + 1] + "_" + curX.ToString() + (curY + 1).ToString();

                            if (nextObject.Split('_')[2].Length == 2)
                            {
                                sX = Convert.ToInt32(nextObject.Split('_')[2].Substring(0, 1));
                                sY = Convert.ToInt32(nextObject.Split('_')[2].Substring(1, 1));
                            }
                            else
                            {
                                sX = Convert.ToInt32(nextObject.Split('_')[2].Substring(0, 2));
                                sY = Convert.ToInt32(nextObject.Split('_')[2].Substring(2, 2));
                            }

                            cName = "jewel_" + newArray[curX, curY] + "_" + curX.ToString() + (curY + 1).ToString();
                            nName = "jewel_" + newArray[curX, curY + 1] + "_" + sX.ToString() + (sY - 1).ToString();

                            if (nextObject != null)
                                swapTwo(currentObject, nextObject, cName, nName, newArray, curX, curY, curX, (curY + 1), "down", "up");

                        }
                    }



                }
            }
        }

    }

    void swapTwo(string currentObject, string nextObject, string curName, string nextName, string[,] nArr, int ncX, int ncY, int scX, int scY, string direction, string ndirection)
    {
        if (nArr[scX, scY] != "c")
        {
            Vector3 currentPos = GameObject.Find(currentObject).transform.localPosition;

            Vector3 nextPos = GameObject.Find(nextObject).transform.localPosition;

            var tempPos = currentPos;

            GameObject.Find(currentObject).GetComponent<LerpHelper>().endPosition = nextPos;
            GameObject.Find(currentObject).GetComponent<LerpHelper>().getDir = direction;
            GameObject.Find(currentObject).GetComponent<LerpHelper>().shouldLerp = true;
            GameObject.Find(nextObject).GetComponent<LerpHelper>().endPosition = tempPos;
            GameObject.Find(nextObject).GetComponent<LerpHelper>().getDir = ndirection;
            GameObject.Find(nextObject).GetComponent<LerpHelper>().shouldLerp = true;

            //Vector3 interpolatedPositionFirst = new Vector3();
            //Vector3 interpolatedPositionSecond = new Vector3();

            //float timeElapsed = 0;

            //while (timeElapsed < lerpDuration)
            //{
            //    timeElapsed += Time.deltaTime;

            //    interpolatedPositionFirst = Vector3.Lerp(currentPos, nextPos, timeElapsed / lerpDuration);

            //    interpolatedPositionSecond = Vector3.Lerp(nextPos, currentPos, timeElapsed / lerpDuration);

            //}
            //GameObject.Find(currentObject).GetComponent<RectTransform>().position = interpolatedPositionFirst;
            //GameObject.Find(nextObject).GetComponent<RectTransform>().position = interpolatedPositionSecond;

            var tempColor = nArr[scX, scY];
            nArr[scX, scY] = nArr[ncX, ncY];
            nArr[ncX, ncY] = tempColor;
            newArray = nArr;

            GameObject.Find(currentObject).name = curName;
            GameObject.Find(nextObject).name = nextName;

            newArray = checkRow(newArray, GameObject.Find(currentObject));

            moveCount();

        }


    }

    string[,] checkRow(string[,] chkArr, GameObject check)
    {
        var countRow = 0;

        for (int y = 0; y < Convert.ToInt32(gHeight); y++)
        {
            for (int x = 0; x < Convert.ToInt32(gWidth); x++)
            {
                if (chkArr[0, y] == chkArr[x, y])
                {
                    countRow++;
                }
            }
            if (countRow == Convert.ToInt32(gWidth) - 1)
            {
                for (int j = 0; j < Convert.ToInt32(gWidth); j++)
                {
                    if(chkArr[j, y] == "r")
                    {
                        int score = Convert.ToInt32(GameObject.Find("scoreText").GetComponent<Text>().text);
                        score += 100;
                        GameObject.Find("scoreText").GetComponent<Text>().text = score.ToString();
                    }
                    if (chkArr[j, y] == "g")
                    {
                        int score = Convert.ToInt32(GameObject.Find("scoreText").GetComponent<Text>().text);
                        score += 150;
                        GameObject.Find("scoreText").GetComponent<Text>().text = score.ToString();
                    }
                    if (chkArr[j, y] == "b")
                    {
                        int score = Convert.ToInt32(GameObject.Find("scoreText").GetComponent<Text>().text);
                        score += 200;
                        GameObject.Find("scoreText").GetComponent<Text>().text = score.ToString();
                    }
                    if (chkArr[j, y] == "y")
                    {
                        int score = Convert.ToInt32(GameObject.Find("scoreText").GetComponent<Text>().text);
                        score += 250;
                        GameObject.Find("scoreText").GetComponent<Text>().text = score.ToString();
                    }


                    chkArr[j, y] = "c";

                    check.GetComponent<Image>().sprite = checkImage;
                    check.name = "jewel_c_" + j.ToString() + y.ToString();

                }

                countRow = 0;
            }
        }

        return chkArr;
    }

    void moveCount()
    {
        int moveCounts = Convert.ToInt32(GameObject.Find("movesText").GetComponent<Text>().text);
        GameObject.Find("movesText").GetComponent<Text>().text = Convert.ToString(--moveCounts);

        string scoreToSave = GameObject.Find("scoreText").GetComponent<Text>().text;
        int levelIndex = Convert.ToInt32(SceneManager.GetActiveScene().name.Substring(5));
        string saveFile = "Assets/Save/save";

        StreamReader tempRead = new StreamReader(saveFile);

        string oldScore = "";

        string[] line = new string[11];

        for (int i = 0; i < 11; i++)
        {
            line[i] = tempRead.ReadLine();

            if (i == levelIndex)
                oldScore = line[i];
        }

        tempRead.Close();

        StreamWriter tempWrite = new StreamWriter(saveFile);

        for (int i = 0; i < 11; i++)
        {
            if (i == levelIndex && (Convert.ToInt32(scoreToSave) > Convert.ToInt32(oldScore)))
                tempWrite.WriteLine(scoreToSave);
            else
                tempWrite.WriteLine(line[i]);
        }

        tempWrite.Close();


        if (moveCounts == 0 && (Convert.ToInt32(scoreToSave) > Convert.ToInt32(oldScore)))
        {
            SceneManager.LoadScene("MainScene");

            StartCoroutine("loadMyScene", scoreToSave);


        }
    }

    IEnumerator loadMyScene(int score)
    {

        while (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainScene"))
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));

        GameObject congrats = new GameObject();
        congrats.name = "congrats";
        congrats.AddComponent<RectTransform>();
        congrats.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").transform);
        congrats.GetComponent<RectTransform>().localPosition = Vector3.zero;
        congrats.GetComponent<RectTransform>().localScale = Vector3.one;
        congrats.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 350);
        congrats.AddComponent<Image>();
        congrats.GetComponent<Image>().sprite = congratsImage;

        GameObject congratsText = new GameObject();
        congratsText.name = "congratsText";
        congratsText.AddComponent<RectTransform>();
        congratsText.GetComponent<RectTransform>().SetParent(congrats.transform);
        congratsText.GetComponent<RectTransform>().localPosition = Vector3.zero;
        congratsText.GetComponent<RectTransform>().localScale = Vector3.one;
        congratsText.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 50);
        congratsText.AddComponent<Text>();
        congratsText.GetComponent<Text>().text = "High Score! " + score.ToString();
        congratsText.GetComponent<Text>().fontSize = 16;
        congratsText.GetComponent<Text>().color = Color.white;
        congratsText.GetComponent<Text>().fontStyle = FontStyle.Bold;
        congratsText.GetComponent<Text>().font = font;
        congratsText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;


    }

    void createInfo()
    {
        GameObject canvas = new GameObject();
        canvas.name = "Canvas";
        canvas.AddComponent<RectTransform>();
        canvas.GetComponent<RectTransform>().SetParent(mainCamera.transform);
        canvas.AddComponent<Canvas>();
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        canvas.GetComponent<Canvas>().worldCamera = mainCamera;
        canvas.AddComponent<CanvasRenderer>();
        canvas.AddComponent<CanvasScaler>();

        GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        GameObject banner = new GameObject();
        banner.name = "Banner";
        banner.AddComponent<RectTransform>();
        banner.GetComponent<RectTransform>().SetParent(canvas.transform);
        banner.GetComponent<RectTransform>().localPosition = new Vector3(0, 260, 0);
        banner.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 135);
        banner.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        banner.AddComponent<CanvasRenderer>();
        banner.AddComponent<Image>();
        banner.GetComponent<Image>().sprite = levelBack;
        banner.GetComponent<Image>().type = Image.Type.Sliced;

        GameObject levelText = new GameObject();
        levelText.name = "levelText";
        levelText.AddComponent<RectTransform>();
        levelText.GetComponent<RectTransform>().SetParent(banner.transform);
        levelText.GetComponent<RectTransform>().localPosition = new Vector3(0, 15, 0);
        levelText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 50);
        levelText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        levelText.AddComponent<CanvasRenderer>();
        levelText.AddComponent<Text>();
        levelText.GetComponent<Text>().text = "Level ";
        levelText.GetComponent<Text>().font = font;
        levelText.GetComponent<Text>().fontSize = 30;
        levelText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        GameObject movesText = new GameObject();
        movesText.name = "movesText";
        movesText.AddComponent<RectTransform>();
        movesText.GetComponent<RectTransform>().SetParent(banner.transform);
        movesText.GetComponent<RectTransform>().localPosition = new Vector3(80, -15, 0);
        movesText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 50);
        movesText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        movesText.AddComponent<CanvasRenderer>();
        movesText.AddComponent<Text>();
        movesText.GetComponent<Text>().text = "Moves: ";
        movesText.GetComponent<Text>().font = font;
        movesText.GetComponent<Text>().fontStyle = FontStyle.Italic;
        movesText.GetComponent<Text>().fontSize = 20;
        movesText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        GameObject scoreText = new GameObject();
        scoreText.name = "scoreText";
        scoreText.AddComponent<RectTransform>();
        scoreText.GetComponent<RectTransform>().SetParent(banner.transform);
        scoreText.GetComponent<RectTransform>().localPosition = new Vector3(-80, -15, 0);
        scoreText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 50);
        scoreText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        scoreText.AddComponent<CanvasRenderer>();
        scoreText.AddComponent<Text>();
        scoreText.GetComponent<Text>().text = "Score: 0";
        scoreText.GetComponent<Text>().font = font;
        scoreText.GetComponent<Text>().fontStyle = FontStyle.Italic;
        scoreText.GetComponent<Text>().fontSize = 20;
        scoreText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

    }

    public string[] getFileList(string path)
    {
        string[] fileList = Directory.GetFiles(path);
        string[] cleanlist = new string[fileList.Length];
        string[] dotlessList = new string[fileList.Length / 2];

        for (int i = 0; i < fileList.Length; i++)
        {
            if (!fileList[i].Contains("."))
            {
                cleanlist[i] = fileList[i];
            }
        }

        for (int i = 0; i < cleanlist.Length - 1; i++)
        {
            if (cleanlist[i] != null)
            {
                var j = (int)i / 2;
                dotlessList[j] = cleanlist[i];
            }
        }

        for (int i = 1; i < dotlessList.Length - 1; i++)
        {
            var tempPath = dotlessList[i + 1];
            dotlessList[i + 1] = dotlessList[i];
            dotlessList[i] = tempPath;
        }

        return dotlessList;
    }
    string[,] getGrid()
    {
        string levelPath = "Assets/Levels";

        string level = "", gWidth = "", gHeight = "", moves = "", grid = "";

        int lvl = Convert.ToInt32(SceneManager.GetActiveScene().name.Substring(5)), counter = 0;
        foreach (string levelFilePath in getFileList(levelPath))
        {
            if (counter == lvl)
                break;

            StreamReader levelInfo = new StreamReader(levelFilePath);

            level = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();
            gWidth = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();
            gHeight = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();
            moves = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();
            grid = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();

            levelInfo.Close();
            counter++;
        }

        grid = grid.Replace(",", "");

        string[,] gridArr = new string[Convert.ToInt32(gWidth), Convert.ToInt32(gHeight)];
        int count = 0;
        for (int y = 0; y < Convert.ToInt32(gHeight); y++)
        {
            for (int x = 0; x < Convert.ToInt32(gWidth); x++)
            {
                gridArr[x, y] = grid.Substring(count, 1);
                count++;
            }
        }

        return gridArr;
    }

    void getLevel()
    {
        string levelPath = "Assets/Levels";

        //string level = "", gWidth = "", gHeight = "", moves = "", grid = "";

        int lvl = Convert.ToInt32(SceneManager.GetActiveScene().name.Substring(5)), counter = 0;
        foreach (string levelFilePath in getFileList(levelPath))
        {
            if (counter == lvl)
                break;

            StreamReader levelInfo = new StreamReader(levelFilePath);

            level = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();
            gWidth = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();
            gHeight = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();
            moves = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();
            grid = levelInfo.ReadLine().Split(':')[1].TrimStart().ToString();

            levelInfo.Close();
            counter++;
        }

        GameObject.Find("movesText").GetComponent<Text>().text += moves;

        grid = grid.Replace(",", "");

        CreateGame(Convert.ToInt32(gWidth), Convert.ToInt32(gHeight), grid);

    }

    float[] getSize(float width, float height, int gWidth, int gHeight)
    {
        float[] size = new float[2];

        size[0] = (width / gWidth);
        size[1] = (height / gHeight);

        return size;
    }

    void createImage(string color, float width, float height, float x, float y, float z, GameObject gamePanel, int nameX, int nameY)
    {

        GameObject jewel = new GameObject();
        jewel.name = "jewel_" + color + "_" + nameX.ToString() + nameY.ToString();
        jewel.AddComponent<RectTransform>();
        jewel.GetComponent<RectTransform>().SetParent(gamePanel.transform);
        jewel.GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
        jewel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        jewel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        jewel.AddComponent<CanvasRenderer>();
        jewel.AddComponent<Image>();
        if (color == "r")
            jewel.GetComponent<Image>().sprite = red;
        if (color == "g")
            jewel.GetComponent<Image>().sprite = green;
        if (color == "b")
            jewel.GetComponent<Image>().sprite = blue;
        if (color == "y")
            jewel.GetComponent<Image>().sprite = yellow;
        jewel.AddComponent<BoxCollider>();
        jewel.GetComponent<BoxCollider>().size = new Vector3(width, height, 1);
        jewel.AddComponent<LerpHelper>();

    }


    void CreateGame(int gWidth, int gHeight, string agrid)
    {
        GameObject gamePanel = new GameObject();
        gamePanel.name = "gamePanel";
        gamePanel.AddComponent<RectTransform>();
        gamePanel.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").transform);
        gamePanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        gamePanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        gamePanel.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        gamePanel.GetComponent<RectTransform>().localPosition = new Vector3(0, -55, 0);
        gamePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(370, 530);
        gamePanel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        gamePanel.AddComponent<CanvasRenderer>();
        gamePanel.AddComponent<Image>();
#if UNITY_EDITOR
        gamePanel.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
#endif
        gamePanel.GetComponent<Image>().color = new Color32(0, 0, 0, 171);
        gamePanel.GetComponent<Image>().type = Image.Type.Sliced;

        int count = 0;
        string colorName;
        float px = -140, py = 225, pz = 0;

        string[,] gridArr = new string[gWidth, gHeight];

        for (int y = 0; y < gHeight; y++)
        {
            for (int x = 0; x < gWidth; x++)
            {
                colorName = agrid.Substring(count, 1);

                gridArr[x, y] = colorName;

                createImage(colorName, getSize(330, 500, gWidth, gHeight)[0], getSize(330, 500, gWidth, gHeight)[1], px, py, pz, gamePanel, x, y);
                px += getSize(330, 500, gWidth, gHeight)[0] + 2.5f;
                count++;
            }
            py -= getSize(330, 500, gWidth, gHeight)[1] + 2.5f;
            px = -140;
        }

    }



}
