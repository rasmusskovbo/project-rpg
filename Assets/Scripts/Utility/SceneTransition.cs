using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator crossfadeAnimator;
    private float animationSpeed = 1f;
    private Dictionary<SceneIndexType, int> indexToType;

    private void Start()
    {
        crossfadeAnimator = GetComponentInChildren<Animator>();
        indexToType = new Dictionary<SceneIndexType, int>
        {
            {SceneIndexType.MainMenu, 0},
            {SceneIndexType.Exploration, 1},
            {SceneIndexType.Combat, 2},
        };
    }

    public void LoadScene(SceneIndexType scene)
    {
        StartCoroutine(AnimateSceneTransition(indexToType[scene]));
    }

    IEnumerator AnimateSceneTransition(int buildIndex)
    {
        crossfadeAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(animationSpeed);

        SceneManager.LoadScene(buildIndex);
    }
}
