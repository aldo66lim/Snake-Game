using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private Vector2 dir = Vector2.right;
    private int score = 0;
    private int bestScore = 0;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;

    [SerializeField] private KeyCode upButton = KeyCode.W;
    [SerializeField] private KeyCode downButton = KeyCode.S;
    [SerializeField] private KeyCode rightButton = KeyCode.D;
    [SerializeField] private KeyCode leftButton = KeyCode.A;

    private List<Transform> _segments = new List<Transform>();

    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private int initialSize = 3;

    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource scoreSound;


    private void Start()
    {
        bestScoreText.text = "Hi-Score : " + bestScore;
        ResetState();
    }


    private void Update()
    {
        if(Input.GetKeyDown(upButton))
        {
            dir = Vector2.up;
        }
        else if(Input.GetKeyDown(downButton))
        {
            dir = Vector2.down;
        }
        else if (Input.GetKeyDown(rightButton))
        {
            dir = Vector2.right;
        }
        else if (Input.GetKeyDown(leftButton))
        {
            dir = Vector2.left;
        }
    }

    private void FixedUpdate()
    {
        for (int i =_segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector2(
            Mathf.Round(this.transform.position.x) + dir.x,
            Mathf.Round(this.transform.position.y) + dir.y
            );
    }

    private void Grow()
    {
        Transform segment = Instantiate (this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }
    private void ResetState()
    {
        score = 0;
        scoreText.text = "Score : " + score;
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        for(int i = 1; i < this.initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            scoreSound.Play();
            Grow();
            score++;
            scoreText.text = "Score : " + score;
            if (score > bestScore)
            { bestScore = score; }
            bestScoreText.text = "Hi-Score : " + bestScore;
        }
        else if(collision.tag == "Obstacle")
        {
            deathSound.Play();
            ResetState();
        }
    }
}
