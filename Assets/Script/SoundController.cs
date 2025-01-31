using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    //// シングルトン
    //public static SoundController Instance;

    //void Awake()
    //{
    //    // もし無ければセットする
    //    if (null == Instance)
    //    {
    //        // サウンドの設定
    //        audioSource = GetComponent<AudioSource>();
    //        audioSource.loop = true;
    //        // 最初に作られたオブジェクトをセットする
    //        Instance = this;
    //        // シーンをまたいでもオブジェクトを削除しない
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //    // 2回目以降に生成されたオブジェクトは削除する
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

    //// 再生装置
    //AudioSource audioSource;
    //// BGM音源
    //[SerializeField] List<AudioClip> audioClipsBGM;
    //// SE音源
    //[SerializeField] List<AudioClip> audioClipsSE;

    //// BGM再生
    //public void PlayBGM(int index)
    //{
    //    audioSource.clip = audioClipsBGM[index];
    //    audioSource.Play();
    //}

    //// SE再生
    //public void PlaySE(int index)
    //{
    //    audioSource.PlayOneShot(audioClipsSE[index]);
    //}



    public static SoundController Instance { get; private set; }

    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> audioClipsBGM;
    [SerializeField] private List<AudioClip> audioClipsSE;

    private Coroutine fadeCoroutine;
    private string currentSceneName = "";

    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("SoundController: 初回インスタンス作成");
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.Log("SoundController: 重複したインスタンスを削除");
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        Debug.Log("SoundController: シーンロード - " + currentSceneName);

        if (currentSceneName == "GameScene") // **ゲームシーンならBGMを流す**
        {
            Debug.Log("SoundController: GameScene で BGM を再生");
            PlayBGM(0, 1.0f);
        }
        else // **それ以外のシーン (TitleScene など) ではBGMを止める**
        {
            Debug.Log("SoundController: 非GameScene で BGM を停止");
            StopBGM();
        }
    }

    public void PlayBGM(int index, float fadeDuration = 1.0f)
    {
        if (index < 0 || index >= audioClipsBGM.Count) return;

        Debug.Log($"SoundController: PlayBGM 呼び出し (index: {index})");

        if (audioSource.clip != audioClipsBGM[index] || !audioSource.isPlaying)
        {
            Debug.Log("SoundController: BGM をセットして再生");
            audioSource.clip = audioClipsBGM[index];
            audioSource.loop = true;
            audioSource.volume = 1.0f;
            audioSource.Play();
        }
        else
        {
            Debug.Log("SoundController: BGM はすでに再生中");
        }
    }

    public void PlaySE(int index)
    {
        if (index < 0 || index >= audioClipsSE.Count) return;
        audioSource.PlayOneShot(audioClipsSE[index]);
    }

    public void StopBGM()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        Debug.Log("SoundController: BGM 停止");
        audioSource.Stop();
    }
}
