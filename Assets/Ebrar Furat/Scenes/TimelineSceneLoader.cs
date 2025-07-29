using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineSceneLoader : MonoBehaviour
{
    public PlayableDirector timeline;
    public string nextSceneName;

    void Start()
    {
        timeline.stopped += OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector obj)
    {
        SceneManager.LoadScene("TutorialScene");
    }
}