using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelBar : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Bar")]
    [SerializeField]
    private Color Background_Color;
    [SerializeField]
    private Color Fill_Color;
    [SerializeField]
    private string Left_Text = "1";
    [SerializeField]
    private string Right_Text = "2";
    [SerializeField]
    private float max = 100;
    [SerializeField]
    private float min = 0;
    private float current;

    [Header("Counter")]

    [SerializeField] private bool counter_active;
    [SerializeField] private string counter_text;
    [SerializeField] private Color counter_color;


    static LevelBar _instance;
    static Scene ActiveScene;

    public static LevelBar Instance{
        get{
            if(ActiveScene == null){
                ActiveScene = SceneManager.GetActiveScene();
                _instance = null;
            }

            //SAME SCENE
            if(SceneManager.GetActiveScene().Equals(ActiveScene)){
                //IF INSTANCE NOT SET
                if(_instance == null){
                    _instance = (LevelBar) GameObject.FindObjectOfType(typeof(LevelBar));
                    
                    if(_instance == null){
                        Debug.LogError("LevelBar: A LevelBar object does not exist in the scene");
                        
                    }
                }
                return _instance;
            }else{
                ActiveScene = SceneManager.GetActiveScene();
                _instance = null;
                return Instance;
            }
        }
    }


    private Image bar_bg;
    private Image bar;
    private Image counter_bg;
    private TextMeshProUGUI lefttext;
    private TextMeshProUGUI righttext;
    private TextMeshProUGUI countertext;

    void Start(){
        bar_bg = transform.Find("BG").GetComponent<Image>();
        bar = transform.Find("BG").Find("Fill").GetComponent<Image>();
        counter_bg = transform.Find("BG").Find("CounterBG").GetComponent<Image>();
        lefttext = transform.Find("BG").Find("TextL").GetComponent<TextMeshProUGUI>();
        righttext = transform.Find("BG").Find("TextR").GetComponent<TextMeshProUGUI>();
        countertext = counter_bg.transform.Find("text").GetComponent<TextMeshProUGUI>();

        lefttext.text = Left_Text;
        lefttext.color = ContrastColor(Background_Color);
        righttext.text = Right_Text;
        righttext.color = ContrastColor(Background_Color);
        countertext.text = counter_text;
        countertext.color = counter_color;
        current = min;

        bar_bg.color = Background_Color;
        bar.color = Fill_Color;

        UpdateBar();
    }

    private Color ContrastColor(Color color)
    {
        float d = 0;

        // Counting the perceptive luminance - human eye favors green color... 
        double luminance = ( 0.299 * color.r + 0.587 * color.g + 0.114 * color.b);

        if (luminance > 0.5)
        d = 0.2f; // bright colors - black font
        else
        d = 1f; // dark colors - white font

        return new Color(d,d,d,1);
    }

    private void UpdateBar(){
        float newfill = Mathf.Clamp((current-min)/(max - min),0,1);
        bar.fillAmount = newfill;

        if(newfill>0.108f){
            lefttext.color = ContrastColor(Fill_Color);
        }else{
            lefttext.color = ContrastColor(Background_Color);
        }

        if(newfill>0.926f){
            righttext.color = ContrastColor(Fill_Color);
        }else{
            righttext.color = ContrastColor(Background_Color);
        }

        if(counter_active){
            counter_bg.gameObject.SetActive(true);
        }else{
            counter_bg.gameObject.SetActive(false);
        }
        countertext.text = counter_text;
        countertext.color = counter_color;
    }
    public void SetValue(float val){
        current = Mathf.Clamp(val,min,max);
        UpdateBar();
    }

    public void AddValue(float val){
        current = Mathf.Clamp(current + val,min,max);
        UpdateBar();
    }

    public bool Completed(){
        if(current == max){
            return true;
        }
        return false;
    }

    public float GetValue(){
        return current;
    }

    public float GetProgress(){
        return (current-min)/(max-min);
    }

    public void UpdateBG_Color(Color bgcol){
        Background_Color = bgcol;
        bar_bg.color = Background_Color;
        UpdateBar();
    }

    public void UpdateFill_Color(Color fc){
        Fill_Color = fc;
        bar.color = Fill_Color;
        UpdateBar();
    }

    public void UpdateCounter_Color(Color fc){
        counter_color = fc;
        countertext.color = counter_color;
        UpdateBar();
    }

    public void UpdateLeftText(string tx){
        Left_Text = tx;
        if(lefttext!=null)
            lefttext.text = Left_Text;
        UpdateBar();
    }

    public void UpdateRightText(string tx){
        Right_Text = tx;
        if(righttext!=null)
            righttext.text = Right_Text;
        UpdateBar();
    }

    public void UpdateCounterText(string tx){
        counter_text = tx;
        if(countertext!=null)
            countertext.text = counter_text;
        UpdateBar();
    }

    public void SetCounterActive(bool active){
        counter_bg.gameObject.SetActive(active);
    }

    public void UpdateMin(float m){
        min = m;
        UpdateBar();
    }

    public void UpdateMax(float m){
        max = m;
        UpdateBar();
    }

    public void ResetBar(){
        Start();
    }
}
