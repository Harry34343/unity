using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instancia;
    public CanvasGroup fade;
    public float duracion = 0.5f;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        instancia = this;
        DontDestroyOnLoad(gameObject);

        if (fade != null)
        {
            fade.alpha = 0;
            fade.blocksRaycasts = false;
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        StopAllCoroutines();
        StartCoroutine(Fade(1, 0)); 
    }

    public void CambiarEscena(string nombre)
    {
        StopAllCoroutines();
        StartCoroutine(FadeYLoad(nombre));
    }

    IEnumerator FadeYLoad(string escena)
    {
        fade.blocksRaycasts = true; // Prevent clicking buttons during transition
        yield return StartCoroutine(Fade(0, 1)); // Fade to Black
        SceneManager.LoadScene(escena);
    }

    IEnumerator Fade(float inicio, float fin)
    {
        Debug.Log("Enter");
        if (fade == null)
        {
          yield break;  
        }
        Debug.Log("Fading: " + fade.name + " alpha=" + fade.alpha);

        float t = 0;
        fade.alpha = inicio;

        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            fade.alpha = Mathf.Lerp(inicio, fin, t / duracion);
            yield return null;
        }

        fade.alpha = fin;
        
        // IMPORTANT: If we faded in (to 0), stop blocking raycasts so we can play!
        if (fin <= 0) 
        {
            fade.blocksRaycasts = false;
        }
    }
}