using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    private Path path;

    public float NextWaveSize = 5f;

    public float NextWaveDelaySeconds = 30f;
    public float NextWave;
    public float GameTime = 0;

    private System.Random rand = new System.Random();

    public bool DebugCountdown = false;
    int GameTimeCountdownValue = 0;

	// Use this for initialization
	public void Start ()
    {
        PathBuilder builder = GetComponent<PathBuilder>();
        path = GetComponent<PathBuilder>()._path;
        NextWave = NextWaveDelaySeconds;
	}
	
	// Update is called once per frame
	public void Update ()
    {
        GameTime += Time.deltaTime;
        if (DebugCountdown && Mathf.RoundToInt(GameTime) > GameTimeCountdownValue)
        {
            GameTimeCountdownValue++;
            Debug.Log(String.Format("{0} seconds until next wave.", Mathf.RoundToInt(NextWave) - GameTimeCountdownValue));
        }

        if (GameTime > NextWave)
        {
            NextWave += NextWaveDelaySeconds;
            StartCoroutine(SpawnWave());
        }
	}

    public IEnumerator SpawnWave()
    {
        int waveSize = Mathf.RoundToInt(NextWaveSize);
        for (int i = 0; i < waveSize; i++)
        {
            Minion minion = MinionPool.Instance.GetMinion();
            minion.Activate(path.Branches[path.OuterBranchIDs[rand.Next(path.OuterBranchIDs.Count)]].InNode);

            // Don't spawn all minions at the same time. They need to come in waves. Not piles.
            yield return new WaitForSeconds(0.5f + UnityEngine.Random.value);
        }
    }
}
