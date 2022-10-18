using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform spawnPosition;
    public Rigidbody ballObject;
    [ReadOnly] public int score1;
    [ReadOnly] public int score2;

    public static GameManager instance;
    void Start()
    {
        instance = this;

        score1 = 0;
        score2 = 0;
    }

    private void OnDestroy()
    {
        instance=null;
    }

   public void ScoredGoal(bool side1)
    {
        if (side1)
            score1++;
        else
            score2++;
        UIManager.instance.UpdateScore(score1, score2);

        RespawnBall();
    }

    private async void RespawnBall()
    {

        var ballTrail= ballObject.transform.Find("Ball Trail");

        ballObject.gameObject.SetActive(false);
        ballTrail?.gameObject.SetActive(false);

        await Task.Delay(200);
        ballObject.velocity = Vector3.zero;
        ballObject.angularVelocity = Vector3.zero;
        ballObject.transform.position = spawnPosition.position;
        ballObject.isKinematic = true;
        ballObject.gameObject.SetActive(true);

        await Task.Delay(1500);
        ballObject.isKinematic = false;
        ballTrail?.gameObject.SetActive(true);

        

    }
}
