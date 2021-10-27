using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

//RatherGood MultiplayerARPG play intro video mod

namespace MultiplayerARPG
{
    [RequireComponent(typeof(VideoPlayer))]
    public class UISceneLoadingWithIntroVideo : UISceneLoading
    {
        VideoPlayer loadScreenVideoPlayer;

        [SerializeField] bool videoIsPlaying = false;

        [SerializeField] GameObject introVideoRoot;

        //CORE MOD: add "protected virtual" to LoadSceneRoutine
        protected override IEnumerator LoadSceneRoutine(string sceneName)
        {

#if CLIENT_BUILD

            if (sceneName == GameInstance.Singleton.HomeSceneName)
            {
                rootObject.gameObject.SetActive(false);
                introVideoRoot.gameObject.SetActive(true);

                loadScreenVideoPlayer.loopPointReached += EndVideo;

                videoIsPlaying = true;

                loadScreenVideoPlayer.Play();

                yield return new WaitForEndOfFrame();

                while (videoIsPlaying)
                {
                    yield return new WaitForEndOfFrame();
                }

                loadScreenVideoPlayer.Stop();
                loadScreenVideoPlayer.loopPointReached -= EndVideo;

                introVideoRoot.gameObject.SetActive(false);

                //Loading will commence when video has finished or canceled.
                yield return base.LoadSceneRoutine(sceneName);

            }
            else
            {
                yield return base.LoadSceneRoutine(sceneName);
            }
#else
            yield return base.LoadSceneRoutine(sceneName);
#endif

        }

        /// <summary>
        /// Will skip remaining video. Linked to skipIntro button if provided.
        /// </summary>
        public void SkipIntroButton()
        {
            videoIsPlaying = false;
        }

        //Can call to end early from a "SkipIntro" button or whatnot
        public void EndVideo(VideoPlayer vp)
        {
            if (loadScreenVideoPlayer == null)
                return;

            videoIsPlaying = false;
        }

        private void Start()
        {
            loadScreenVideoPlayer = GetComponent<VideoPlayer>();
        }


    }
}