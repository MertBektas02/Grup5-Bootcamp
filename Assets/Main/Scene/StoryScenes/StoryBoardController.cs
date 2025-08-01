using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class StoryboardController : MonoBehaviour
{
    public PlayableDirector director;
    public string nextSceneName;

    void Start()
    {
        director.stopped += OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector pd)
    {
        if (pd == director)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
