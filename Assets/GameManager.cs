using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Elemanlarý")]
    public Text puanYazisi;
    public Text sureYazisi;
    public Text sonucYazisi;

    [Header("Oyun Ayarlarý")]
    public GameObject altinPrefabi;
    public float oyunSuresi = 35f;
    public int hedefPuan = 40;

    private int _suankiPuan = 0;
    private bool _oyunBittiMi = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        puanYazisi.text = "Puan: 0";
        sonucYazisi.text = "";
        YeniAltinYarat();
    }

    void Update()
    {
        if (!_oyunBittiMi)
        {
            oyunSuresi -= Time.deltaTime;

            sureYazisi.text = "Süre: " + Mathf.Ceil(oyunSuresi).ToString();

            if (oyunSuresi <= 0)
            {
                OyunBitti(false);
            }
        }
    }

    public void AltinVuruldu()
    {
        if (_oyunBittiMi) return;

        _suankiPuan += 2;
        puanYazisi.text = "Puan: " + _suankiPuan;

        if (_suankiPuan >= hedefPuan)
        {
            OyunBitti(true);
        }
        else
        {
            YeniAltinYarat();
        }
    }

    void OyunBitti(bool kazandiMi)
    {
        _oyunBittiMi = true;
        Time.timeScale = 0;

        if (kazandiMi)
        {
            sonucYazisi.text = "TEBRÝKLER!\nKAZANDINIZ";
            sonucYazisi.color = Color.green;
        }
        else
        {
            sonucYazisi.text = "SÜRE BÝTTÝ!\nKAYBETTÝNÝZ";
            sonucYazisi.color = Color.red;
            sureYazisi.text = "Süre: 0";
        }
    }

    void YeniAltinYarat()
    {
        float rastgeleX = Random.Range(-4f, 4f);
        float rastgeleZ = Random.Range(-4f, 4f);

        // ÝÞTE DÜZELTÝLEN YER: Y deðeri 0f yapýldý (Yerde dursun diye)
        Instantiate(altinPrefabi, new Vector3(rastgeleX, 0f, rastgeleZ), Quaternion.identity);
    }
}