using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] tetrominoPreFab;
    private GameObject currentTetromino;
    private GameObject nextTetromino;
    public float fallTime = 0.8f;
    private float passedTime = 0f;
    public float PlayerOnePoints = 0f;
    private GameObject previewTetromino;
    public string playerOneName;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI PointsText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerOneName"))
        {
            playerOneName = PlayerPrefs.GetString("PlayerOneName");

            if (string.IsNullOrEmpty(playerOneName))
            {
                playerOneName = "PlayerOne";
            }

        }
        else 
        {
            playerOneName = "PlayerOne";
        }
        playerNameText.text = playerOneName;
    
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

        PointsText.text = PlayerOnePoints.ToString();

    }

    void UserInput() 
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveTetromino(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveTetromino(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.W)) 
        {
            currentTetromino.transform.Rotate(0, 0, 90);
            if (!IsValidPossition()) 
            {
                currentTetromino.transform.Rotate(0, 0, -90);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            fallTime = 0.2f;
        }
        else if(Input.GetKeyUp(KeyCode.S))
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
            currentTetromino = Instantiate(tetrominoPreFab[firstIndex], new Vector3(5, 18, 0), Quaternion.identity);

            int nextIndex = Random.Range(0, tetrominoPreFab.Length);
            nextTetromino = tetrominoPreFab[nextIndex];
        }

        else
        {
            // create new block
            currentTetromino = Instantiate(nextTetromino, new Vector3(5, 18, 0), Quaternion.identity);
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
                GetComponent<Grid>().UpdateGrid(currentTetromino.transform);
                CheckForLines();
                SpawnTetromino();
            }
        }
    }

    bool IsValidPossition() 
    {
        return GetComponent<Grid>().IsValidPossition(currentTetromino.transform);
    }

    void CheckForLines() 
    {
        GetComponent<Grid>().CheckForLines();
        
    }

    void UpdatePreview() 
    {
        if (previewTetromino != null) 
        {
            Destroy(previewTetromino); //destroying previous preview
        }

        // creating new preview and scale it
        previewTetromino = Instantiate(nextTetromino, Vector3.zero, Quaternion.identity);
        previewTetromino.transform.SetParent(GameObject.Find("NextPiecePanel").transform,false);
        previewTetromino.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }
}
