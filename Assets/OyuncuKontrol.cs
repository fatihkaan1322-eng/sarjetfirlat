using UnityEngine;

public class OyuncuKontrol : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _firlatmaGucu = 10f;
    [SerializeField] private ParticleSystem _tozEfekti;

    private Vector3 _baslangicNoktasi;
    private Vector3 _bitisNoktasi;
    private bool _atisYapiliyor = false;
    private float _beklemeSuresi = 0f;

    // YENÝ: Sonsuz zemin (Yüksekliði 0 olan hayali bir zemin)
    private Plane _sonsuzZemin;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Y ekseninde yukarý bakan (Vector3.up), orijinden geçen (Vector3.zero) bir zemin oluþturuyoruz.
        _sonsuzZemin = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        if (_beklemeSuresi > 0)
            _beklemeSuresi -= Time.deltaTime;

        // 1. Týklama Aný
        if (Input.GetMouseButtonDown(0) && _beklemeSuresi <= 0)
        {
            _baslangicNoktasi = MouseKonumuBul();
        }

        // 2. Býrakma Aný
        if (Input.GetMouseButtonUp(0) && _beklemeSuresi <= 0)
        {
            _bitisNoktasi = MouseKonumuBul();
            _atisYapiliyor = true;
            _beklemeSuresi = 0.5f; // Yarým saniye bekleme
        }
    }

    private void FixedUpdate()
    {
        if (_atisYapiliyor)
        {
            // Yön Hesabý: Baþlangýç - Bitiþ (Sapan mantýðý)
            Vector3 firlatmaYonu = _baslangicNoktasi - _bitisNoktasi;

            // Yükseklik farkýný sýfýrla ki top havalanmasýn veya yere çakýlmasýn
            firlatmaYonu.y = 0;

            // Kuvveti uygula
            _rb.AddForce(firlatmaYonu * _firlatmaGucu, ForceMode.Impulse);

            _atisYapiliyor = false;
        }
    }

    // GÜNCELLENMÝÞ FONKSÝYON: Artýk "Ground" etiketine ihtiyaç duymaz.
    private Vector3 MouseKonumuBul()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float mesafe;

        // Iþýn bizim hayali sonsuz zeminimize çarpýyor mu?
        if (_sonsuzZemin.Raycast(ray, out mesafe))
        {
            // Çarpýyorsa oranýn dünya üzerindeki koordinatýný ver
            return ray.GetPoint(mesafe);
        }

        // Eðer sonsuz zemini bile bulamazsa (imkansýz gibi ama) topun olduðu yeri ver
        return transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // "Ground" yerine herhangi bir þeye çarpýnca efekt çýksýn
        if (_tozEfekti != null) _tozEfekti.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Eðer çarptýðýmýz þeyin etiketi "Coin" ise
        if (other.gameObject.CompareTag("Coin"))
        {
            // 1. Yöneticiye haber ver (Puaný artýr, yeni altýn yarat)
            GameManager.instance.AltinVuruldu();

            // 2. Vurulan altýný yok et
            Destroy(other.gameObject);
        }
    }
}