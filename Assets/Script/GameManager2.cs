using UnityEngine;
using TMPro;

public class GameManager2 : MonoBehaviour
{
    public GameObject[] tetrominoPreFab;
    private GameObject currentTetromino;
    private GameObject nextTetromino;
    public float fallTime = 0.8f;
    private float passedTime = 0f;
    public float PlayerTwoPoints = 0f;
    private GameObject previewTetromino;
    public string playerTwoName;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI PointsText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerTwoName"))
        {
            playerTwoName = PlayerPrefs.GetString("PlayerTwoName");

            if (string.IsNullOrEmpty(playerTwoName))
            {
                playerTwoName = "PlayerTwo";
            }

        }
        else
        {
            playerTwoName = "PlayerOne";
        }
        playerNameText.text = playerTwoName;

        SpawnTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= fallTime)
        {
            passedTime -= fallTime;
            MoveTetromino(Vector3.down);
        }
        UserInput();

        PointsText.text = PlayerTwoPoints.ToString();

    }

    void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetromino(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetromino(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentTetromino.transform.Rotate(0, 0, 90);
            if (!IsValidPossition())
            {
                currentTetromino.transform.Rotate(0, 0, -90);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            fallTime = 0.2f;
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            fallTime = 0.8f;
        }
    }

    void SpawnTetromino()
    {
        if (nextTetromino == null)
        {
            //for first block, choose two tetrominos
            int firstIndex = Random.Range(0, tetrominoPreFab.Length);
            currentTetromino = Instantiate(tetrominoPreFab[firstIndex], new Vector3(22, 18, 0), Quaternion.identity);

            int nextIndex = Random.Range(0, tetrominoPreFab.Length);
            nextTetromino = tetrominoPreFab[nextIndex];
        }

        else
        {
            // create new block
            currentTetromino = Instantiate(nextTetromino, new Vector3(22, 18, 0), Quaternion.identity);
            //prepere next tetromino
            int nextIndex = Random.Range(0, tetrominoPreFab.Length);
            nextTetromino = tetrominoPreFab[nextIndex];

        }

        UpdatePreview();


    }

    void MoveTetromino(Vector3 direction)
    {
        currentTetromino.transform.position += direction;

        if (!IsValidPossition())
        {
            currentTetromino.transform.position -= direction;

            if (direction == Vector3.down)
            {
                GetComponent<Grid2>().UpdateGrid(currentTetromino.transform);
                CheckForLines();
                SpawnTetromino();
            }
        }
    }

    bool IsValidPossition()
    {
        return GetComponent<Grid2>().IsValidPossition(currentTetromino.transform);
    }

    void CheckForLines()
    {
        GetComponent<Grid2>().CheckForLines();

    }

    void UpdatePreview()
    {
        if (previewTetromino != null)
        {
            Destroy(previewTetromino); //destroying previous preview
        }

        // creating new preview and scale it
        previewTetromino = Instantiate(nextTetromino, Vector3.zero, Quaternion.identity);
        previewTetromino.transform.SetParent(GameObject.Find("NextPiecePanel2").transform, false);
        previewTetromino.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }
}
