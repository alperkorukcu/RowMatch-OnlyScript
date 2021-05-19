using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;
using UnityEngine.SceneManagement;
//Yunus Alper KÖRÜKCÜ  

public class MainMenu : MonoBehaviour
{
    public Camera mainCamera;
    public Font levelsButtonFont;
    public Sprite levelsImage;
    private Canvas canvas;
    private Color color;
    private Color colorRib;
    public Sprite closeImage;
    public Sprite popupBackground;
    public Sprite levelBar;
    public Sprite playImg;
    public Sprite lockImg;

    void Start()
    {
        CreateCanvas();

        CreateButton(canvas, new Vector3(0, 0, 0), new Vector2(0.0155f, 0.0155f), "menuButton", levelsImage);
    }

    void Update()
    {
        if (GameObject.Find("backPanel") != null)
        {
            if (GameObject.Find("backPanel").activeSelf)
            {
                var buttonName = EventSystem.current.currentSelectedGameObject.name;

                buttonName = buttonName.ToString().Substring(10);

                if(buttonName != "")
                    GameObject.Find("playButton" + buttonName).GetComponent<Button>().onClick.AddListener(delegate { LoadSceneLvl(Convert.ToInt32(buttonName)); });
            }
            
        }        
    }

    void createPlay(int level, string moves, string hScore, int lockLevel, int y)
    {
        var lockFlag = false;
        if (level <= lockLevel)
        {
            lockFlag = true;
        }

        GameObject levelPanel = new GameObject();
        levelPanel.name = "levelPanel" + level.ToString();
        levelPanel.AddComponent<RectTransform>();
        levelPanel.GetComponent<RectTransform>().SetParent(GameObject.Find("Content").transform);
        levelPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, y, 0);
        levelPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(395, 70);
        levelPanel.AddComponent<CanvasRenderer>();
        levelPanel.AddComponent<Image>();
        levelPanel.GetComponent<Image>().sprite = levelBar;
        levelPanel.GetComponent<Image>().type = Image.Type.Sliced;
        levelPanel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        GameObject playButton = new GameObject();
        playButton.name = "playButton" + level.ToString();
        playButton.AddComponent<RectTransform>();
        playButton.GetComponent<RectTransform>().SetParent(levelPanel.transform);
        playButton.GetComponent<RectTransform>().localPosition = new Vector3(80, 0, 0);
        playButton.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        playButton.AddComponent<CanvasRenderer>();
        playButton.AddComponent<GraphicRaycaster>();
        playButton.AddComponent<Button>();
        playButton.AddComponent<Image>();
        playButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        if (lockFlag)
        {
            playButton.GetComponent<Image>().sprite = playImg;
            playButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            playButton.GetComponent<Image>().sprite = lockImg;
            levelPanel.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            playButton.GetComponent<Button>().interactable = false;
        }

        GameObject levelTitle = new GameObject();
        levelTitle.name = "levelTitle" + level.ToString();
        levelTitle.AddComponent<RectTransform>();
        levelTitle.GetComponent<RectTransform>().SetParent(levelPanel.transform);
        levelTitle.GetComponent<RectTransform>().localPosition = new Vector3(-20, 7.5f, 0);
        levelTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        levelTitle.AddComponent<CanvasRenderer>();
        levelTitle.AddComponent<Text>();
        levelTitle.GetComponent<Text>().text = "Level " + level.ToString() + " - " + moves.ToString() + " moves";
        levelTitle.GetComponent<Text>().fontStyle = FontStyle.Bold;
        levelTitle.GetComponent<Text>().fontSize = 16;
        levelTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        levelTitle.GetComponent<Text>().font = levelsButtonFont;
        levelTitle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        GameObject levelDesc = new GameObject();
        levelDesc.name = "levelDesc" + level.ToString();
        levelDesc.AddComponent<RectTransform>();
        levelDesc.GetComponent<RectTransform>().SetParent(levelPanel.transform);
        levelDesc.GetComponent<RectTransform>().localPosition = new Vector3(-20, -8, 0);
        levelDesc.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        levelDesc.AddComponent<CanvasRenderer>();
        levelDesc.AddComponent<Text>();

        if (Convert.ToInt32(hScore) > 0)
        {
            levelDesc.GetComponent<Text>().text = "High Score: " + hScore.ToString();
        }
        else if (lockFlag)
        {
            levelDesc.GetComponent<Text>().text = "No Score";
        }
        else
        {
            levelDesc.GetComponent<Text>().text = "Locked Level";
        }

        levelDesc.GetComponent<Text>().fontStyle = FontStyle.Italic;
        levelDesc.GetComponent<Text>().fontSize = 13;
        levelDesc.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        levelDesc.GetComponent<Text>().font = levelsButtonFont;
        levelDesc.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

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


    public void fillList()
    {
        string levelPath = "Assets/Levels";
        string saveFile = "Assets/Save/save";

        string hScore = "", moves = "", lockLevel = "";

        int y = 370;

        int level = 1;
        foreach (string levelFilePath in getFileList(levelPath))
        {
            StreamReader levelInfo = new StreamReader(levelFilePath);
            StreamReader currentSave = new StreamReader(saveFile);

            for (int i = 0; i < 4; i++)
            {
                var temp = levelInfo.ReadLine();
                if (i == 3)
                {
                    moves = temp.Split(':')[1].TrimStart().ToString();
                }
            }

            for (int i = 1; i <= getFileList(levelPath).Length; i++)
            {
                var temp = currentSave.ReadLine();

                if (i == 1)
                {
                    lockLevel = temp.Split(':')[1].TrimStart().ToString();
                }

                if (i == level + 1)
                {
                    hScore = temp.Split(':')[1].TrimStart().ToString();
                }
            }
            
            Debug.Log("level: " + level + ", moves: " + moves + ", hScore: " + hScore + ", lockLevel: " + lockLevel.ToString());
            createPlay(level, moves, hScore, Convert.ToInt32(lockLevel), y);

            y -= 80;

            currentSave.Close();
            levelInfo.Close();
            level++;
        }
    }

    void LoadSceneLvl(int sceneLevel)
    {
        //Debug.Log("Loaded Scene: " + sceneLevel);
        SceneManager.LoadScene("Scenes/Scene" + sceneLevel.ToString());
    }

    void loadLevels()
    {
        if(GameObject.Find("backPanel") == null)
        {
            CreatePopUpMenu();
            GameObject.Find("closeButton").GetComponent<Button>().onClick.AddListener(closeMenu);
            GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>().value = 1f;
            fillList();                  
        }     
    }

    void closeMenu()
    {
        GameObject.Find("backPanel").SetActive(false);
    }

    public void CreateCanvas()
    {
        GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        GameObject obj = new GameObject();
        obj.name = "Canvas";
        canvas = obj.AddComponent<Canvas>();
        canvas.transform.SetParent(mainCamera.transform);
        obj.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
        obj.AddComponent<CanvasScaler>();
        obj.AddComponent<GraphicRaycaster>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCamera;
        canvas.pixelPerfect = true;
    }

    public void CreateButton(Canvas canvas, Vector3 position, Vector2 size, string name, Sprite img)
    {
        GameObject button = new GameObject();
        button.name = name;
        button.AddComponent<RectTransform>();
        button.AddComponent<Button>();
        button.AddComponent<CanvasRenderer>();
        button.AddComponent<GraphicRaycaster>();
        button.AddComponent<Text>();
        button.GetComponent<Text>().text = "LEVELS";
        button.GetComponent<Text>().font = levelsButtonFont;
        button.GetComponent<Text>().fontSize = 44;
        button.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        button.transform.position = position;
        button.gameObject.transform.localScale = new Vector3(size.x, size.y, 1);

        GameObject.Find("menuButton").GetComponent<Button>().onClick.AddListener(loadLevels);

        GameObject backImage = new GameObject("buttonBackground");
        Image newImage = backImage.AddComponent<Image>();
        newImage.sprite = img;
        backImage.GetComponent<RectTransform>().localScale = new Vector3(0.0155f, 0.0155f, 0.0155f);
        backImage.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 50);
        backImage.GetComponent<RectTransform>().SetParent(canvas.transform);

        button.GetComponent<RectTransform>().SetParent(backImage.transform);
    }

    public void CreatePopUpMenu()
    {
        GameObject backPanel = new GameObject();
        backPanel.name = "backPanel";
        backPanel.AddComponent<RectTransform>();
        backPanel.GetComponent<RectTransform>().SetParent(canvas.transform);
        backPanel.transform.position = new Vector3(0, 0, 1);
        backPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(297.08f, 271.39f);

        backPanel.AddComponent<CanvasRenderer>();
        backPanel.AddComponent<Image>();
        #if UNITY_EDITOR
        backPanel.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        #endif
        backPanel.GetComponent<Image>().type = Image.Type.Tiled;

        if (ColorUtility.TryParseHtmlString("#FFBD00", out color))
            backPanel.GetComponent<Image>().color = color;

        backPanel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        GameObject insidePanel = new GameObject();
        insidePanel.name = "insidePanel";
        insidePanel.AddComponent<RectTransform>();
        insidePanel.GetComponent<RectTransform>().SetParent(backPanel.transform);
        insidePanel.transform.position = new Vector3(0, -0.20f, 2);
        insidePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(294f, 224.2f);

        insidePanel.AddComponent<CanvasRenderer>();
        insidePanel.AddComponent<Image>();
        #if UNITY_EDITOR
        insidePanel.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        #endif
        insidePanel.GetComponent<Image>().type = Image.Type.Tiled;

        insidePanel.GetComponent<Image>().color = new Color32(255, 255, 255, 150);

        insidePanel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        GameObject levelScroll = new GameObject();
        levelScroll.name = "levelScroll";
        levelScroll.AddComponent<RectTransform>();
        levelScroll.GetComponent<RectTransform>().SetParent(insidePanel.transform);

        levelScroll.transform.position = new Vector3(0, -0.2f, 3);
        levelScroll.GetComponent<RectTransform>().sizeDelta = new Vector2(294f, 224.2f);
        levelScroll.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        levelScroll.AddComponent<CanvasRenderer>();
        levelScroll.AddComponent<Image>();
        #if UNITY_EDITOR
        levelScroll.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        #endif
        levelScroll.GetComponent<Image>().type = Image.Type.Tiled;
        levelScroll.AddComponent<ScrollRect>();
        levelScroll.GetComponent<ScrollRect>().horizontal = false;
        levelScroll.GetComponent<Image>().color = new Color32(255, 255, 255, 150);

        GameObject viewPort = new GameObject();
        viewPort.name = "viewPort";
        viewPort.AddComponent<RectTransform>();
        viewPort.GetComponent<RectTransform>().SetParent(levelScroll.transform);
        viewPort.AddComponent<CanvasRenderer>();
        viewPort.AddComponent<Image>();
        viewPort.AddComponent<Mask>();
        viewPort.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        viewPort.GetComponent<RectTransform>().offsetMax = new Vector2(-17, viewPort.GetComponent<RectTransform>().offsetMax.y);
        viewPort.GetComponent<RectTransform>().offsetMin = new Vector2(viewPort.GetComponent<RectTransform>().offsetMin.x, 17);
        viewPort.GetComponent<Mask>().showMaskGraphic = false;
        viewPort.GetComponent<RectTransform>().transform.position = new Vector3(0, -0.2f, 60);
        viewPort.GetComponent<RectTransform>().sizeDelta = new Vector2(272, 224.2f);

        GameObject content = new GameObject();
        content.name = "Content";
        content.AddComponent<RectTransform>();
        content.GetComponent<RectTransform>().SetParent(viewPort.transform);
        content.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        content.GetComponent<RectTransform>().offsetMax = new Vector2(0, content.GetComponent<RectTransform>().offsetMax.y);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(270, 832.5f);
        content.GetComponent<RectTransform>().transform.position = new Vector3(0, 0, 50);

        levelScroll.GetComponent<ScrollRect>().content = content.GetComponent<RectTransform>();
        levelScroll.GetComponent<ScrollRect>().viewport = viewPort.GetComponent<RectTransform>();

        GameObject scrollVert = new GameObject();
        scrollVert.name = "Scrollbar Vertical";
        scrollVert.AddComponent<RectTransform>();
        scrollVert.GetComponent<RectTransform>().SetParent(levelScroll.transform);
        scrollVert.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        scrollVert.GetComponent<RectTransform>().offsetMax = new Vector2(-274, scrollVert.GetComponent<RectTransform>().offsetMax.y);
        scrollVert.GetComponent<RectTransform>().offsetMax = new Vector2(scrollVert.GetComponent<RectTransform>().offsetMax.x, 0);
        scrollVert.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 0);
        scrollVert.GetComponent<RectTransform>().offsetMin = new Vector2(scrollVert.GetComponent<RectTransform>().offsetMin.x, 0);
        scrollVert.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, -194);
        scrollVert.GetComponent<RectTransform>().offsetMin = new Vector2(scrollVert.GetComponent<RectTransform>().offsetMin.x, 0);
        scrollVert.GetComponent<RectTransform>().offsetMax = new Vector2(scrollVert.GetComponent<RectTransform>().offsetMax.x, 0);
        scrollVert.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        scrollVert.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        scrollVert.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
        scrollVert.AddComponent<CanvasRenderer>();
        scrollVert.AddComponent<Image>();
        #if UNITY_EDITOR
        scrollVert.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        #endif
        scrollVert.GetComponent<Image>().type = Image.Type.Tiled;
        scrollVert.AddComponent<Scrollbar>();
        scrollVert.GetComponent<Scrollbar>().direction = Scrollbar.Direction.BottomToTop;

        GameObject slidingArea = new GameObject();
        slidingArea.name = "Sliding Area";
        slidingArea.AddComponent<RectTransform>();
        slidingArea.GetComponent<RectTransform>().SetParent(scrollVert.transform);
        slidingArea.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        slidingArea.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        slidingArea.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        slidingArea.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        slidingArea.GetComponent<RectTransform>().offsetMin = new Vector2(10, slidingArea.GetComponent<RectTransform>().offsetMin.y);
        slidingArea.GetComponent<RectTransform>().offsetMax = new Vector2(-10, slidingArea.GetComponent<RectTransform>().offsetMax.y);
        slidingArea.GetComponent<RectTransform>().offsetMax = new Vector2(slidingArea.GetComponent<RectTransform>().offsetMax.x, -10);
        slidingArea.GetComponent<RectTransform>().offsetMin = new Vector2(slidingArea.GetComponent<RectTransform>().offsetMin.x, 10);

        GameObject handle = new GameObject();
        handle.name = "Handle";
        handle.AddComponent<RectTransform>();
        handle.GetComponent<RectTransform>().SetParent(slidingArea.transform);
        handle.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        handle.GetComponent<RectTransform>().anchorMax = new Vector2(0.2f, 1);
        handle.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        handle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        handle.GetComponent<RectTransform>().offsetMin = new Vector2(-10, handle.GetComponent<RectTransform>().offsetMin.y);
        handle.GetComponent<RectTransform>().offsetMax = new Vector2(10, handle.GetComponent<RectTransform>().offsetMax.y);
        handle.GetComponent<RectTransform>().offsetMax = new Vector2(handle.GetComponent<RectTransform>().offsetMax.x, 10);
        handle.GetComponent<RectTransform>().offsetMin = new Vector2(handle.GetComponent<RectTransform>().offsetMin.x, -4.24f);
        handle.AddComponent<CanvasRenderer>();
        handle.AddComponent<Image>();
        #if UNITY_EDITOR
        handle.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        #endif
        handle.GetComponent<Image>().type = Image.Type.Sliced;

        scrollVert.GetComponent<Scrollbar>().targetGraphic = handle.GetComponent<Image>();
        scrollVert.GetComponent<Scrollbar>().handleRect = handle.GetComponent<RectTransform>();

        levelScroll.GetComponent<ScrollRect>().verticalScrollbar = scrollVert.GetComponent<Scrollbar>();
        levelScroll.GetComponent<ScrollRect>().verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        levelScroll.GetComponent<ScrollRect>().verticalScrollbarSpacing = -3;

        GameObject ribbon = new GameObject();
        ribbon.name = "ribbon";
        ribbon.AddComponent<RectTransform>();
        ribbon.GetComponent<RectTransform>().position = new Vector3(0, 1.8f, 0);
        ribbon.GetComponent<RectTransform>().sizeDelta = new Vector2(311.7f, 54f);
        ribbon.AddComponent<CanvasRenderer>();
        ribbon.AddComponent<Image>();
        ribbon.GetComponent<Image>().sprite = popupBackground;
        if (ColorUtility.TryParseHtmlString("#FFE100", out colorRib))
            ribbon.GetComponent<Image>().color = colorRib;
        ribbon.GetComponent<Transform>().SetParent(backPanel.transform);
        ribbon.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        GameObject popupText = new GameObject();
        popupText.name = "popupText";
        popupText.AddComponent<RectTransform>();
        popupText.GetComponent<RectTransform>().SetParent(ribbon.transform);
        popupText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        popupText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        popupText.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        popupText.GetComponent<RectTransform>().localPosition = new Vector3(0, 4.5f, 0);
        popupText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        popupText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        popupText.AddComponent<CanvasRenderer>();
        popupText.AddComponent<Text>();
        popupText.GetComponent<Text>().text = "LEVELS";
        popupText.GetComponent<Text>().font = levelsButtonFont;
        popupText.GetComponent<Text>().fontSize = 30;
        popupText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        GameObject closeButton = new GameObject();
        closeButton.name = "closeButton";
        closeButton.AddComponent<RectTransform>();
        closeButton.GetComponent<RectTransform>().SetParent(ribbon.transform);
        closeButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        closeButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        closeButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        closeButton.GetComponent<RectTransform>().localPosition = new Vector3(130, 4.5f, 0);
        closeButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        closeButton.AddComponent<Image>();
        closeButton.GetComponent<Image>().sprite = closeImage;
        closeButton.GetComponent<Image>().type = Image.Type.Simple;
        closeButton.AddComponent<Button>();
        closeButton.AddComponent<GraphicRaycaster>();
    }
}
