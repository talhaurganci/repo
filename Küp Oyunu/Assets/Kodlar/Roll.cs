using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Roll : MonoBehaviour
{
    
    public float rotationPeriod = 0.2f;
    Vector3 scale;
    Rigidbody  rb;
    public string level2;
    bool kapi = true;
    int a = 0;
    int b = 0;
    



    bool isRotate = false;                  // Küpün dönüp dönmediğini tespit etmek için işaretleyin
    float directionX = 0;                   // Dönüş yönü bayrağı
    float directionZ = 0;                   // Dönüş yönü bayrağı

    float startAngleRad = 0;                // Yatay düzlemden dönmeden önce ağırlık merkezinin açısı
    Vector3 startPos;                       // Döndürmeden önce küp konumu
    float rotationTime = 0;                 // Rotasyon sırasında geçen süre
    float radius = 1;                       // Ağırlık merkezinin yörünge yarıçapı (şimdilik geçici olarak 1)
    Quaternion fromRotation;                // Döndürmeden önce küp kuarterniyonu
    Quaternion toRotation;                  // Döndürmeden sonra küp kuarterniyonu

    bool active = true;
    public GameObject topPrefab;
    // Use this for initialization
    void Start()
    {

        // Bir karenin boyutunu alın
        scale = transform.lossyScale;
        rb = GetComponent<Rigidbody>();
      //  rb.constraints = RigidbodyConstraints.FreezePositionZ;
      //  rb.constraints = RigidbodyConstraints.FreezePositionX;

        Debug.Log ("[x, y, z] = [" + scale.x + ", " + scale.y + ", " + scale.z + "]");

    }



    // Update is called once per frame
    void Update()
    {



        float x = 0;
        float y = 0;

        // Tuş vuruşlarını alın。
        x = Input.GetAxisRaw("Horizontal");
        if (x == 0)
        {
            y = Input.GetAxisRaw("Vertical");
        }


        // Bir tuş girişi varsa ve Küp dönmüyorsa, Küpü döndürün.
        if ((x != 0 || y != 0) && !isRotate)
        {
            directionX = y;                                                             // Dönme yönü ayarı (x veya y, 0 olmalıdır)
            directionZ = x;                                                             // Dönme yönü ayarı (x veya y, 0 olmalıdır)
            startPos = transform.position;                                              // Koordinatları döndürmeden önce tutar
            fromRotation = transform.rotation;                                          // Çeyrekliği rotasyondan önce tutar
            transform.Rotate(directionZ * 90, 0, directionX * 90, Space.World);         // 90 derece dönme yönünde döndürün
            toRotation = transform.rotation;                                            // Dönüşten sonra çeyreği tutar
            transform.rotation = fromRotation;                                          // Küp'ün dönüşünü önceki dönüşe geri döndür. (Dönüşümün sığ bir kopyası olup olmadığını merak ediyorum ...)
            setRadius();                                                                // Dönüş yarıçapını hesaplayın
            rotationTime = 0;                                                           // Dönüş yarıçapını hesaplayın
            isRotate = true;                                                            // Dönüş yarıçapını hesaplayın
        }       
    }

    

    void FixedUpdate()
    {

        if (isRotate&&active)
        {

            rotationTime += Time.fixedDeltaTime;                                    // Geçen süreyi artırın
            float ratio = Mathf.Lerp(0, 1, rotationTime / rotationPeriod);          // Geçerli geçen sürenin dönüş süresine oranı

            // Hareket
            float thetaRad = Mathf.Lerp(0, Mathf.PI / 2f, ratio);                   // Radyan cinsinden dönme açısı。
            float distanceX = -directionX * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + thetaRad));      // X ekseni hareket mesafesi。-İşaret, hareket yönünü anahtarla eşleştirmektir。
            float distanceY = radius * (Mathf.Sin(startAngleRad + thetaRad) - Mathf.Sin(startAngleRad));                        // Y ekseni hareket mesafesi
            float distanceZ = directionZ * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + thetaRad));           // Z ekseni hareket mesafesi
            transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, startPos.z + distanceZ);           // Mevcut konumu ayarla

            // rotasyon
            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, ratio);      // Quaternion.Lerp Geçerli dönüş açısını ile ayarlayın (ne kullanışlı bir işlev)

            // Hareketin / dönüşün sonunda her parametreyi başlatın. İsRotate bayrağını indirin.
            if (ratio == 1)
            {
                isRotate = false;
                directionX = 0;
                directionZ = 0;
                rotationTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "outside")
        {
            active = false;//Küpün hareketi için gerekli olduğu şart false olduğu anda küp hareket etmeyi durdurur ve oyuncu yanar.
            Destroy(this.gameObject, 3); // 3 sanıye sonra destroy 
            rb.constraints = RigidbodyConstraints.None;
            GameObject.Find("GameManager").transform.GetComponent<createPlayer>().Create();//GameManager objesinin createplayer adlı componentine ulaşıp create fonksiyonunu çalıştırır.
            a = 0;
            b = 0;
        }
        if(other.transform.tag == "outside2")
            {
            active = false;
            Destroy(this.gameObject, 3); // 3 sanıye sonra destroy 
            rb.constraints = RigidbodyConstraints.None;
            GameObject.Find("GameManager").transform.GetComponent<createPlayer>().Create();
            a = 0;
            b = 0;
            GameObject.Find("gecis").transform.GetComponent<MeshRenderer>().enabled = false;//Geçiş adlı objenin MeshRenderer componentini kapatır.
            GameObject.Find("gecis2").transform.GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("gecis").transform.GetComponent<BoxCollider>().enabled = false;//Geçiş adlı objenin BoxCollider componentini kapatır.
            GameObject.Find("gecis2").transform.GetComponent<BoxCollider>().enabled = false;
            GameObject.Find("gecis3").transform.GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("gecis4").transform.GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("gecis3").transform.GetComponent<BoxCollider>().enabled = false;
            GameObject.Find("gecis4").transform.GetComponent<BoxCollider>().enabled = false;
            }
                    
        if (other.transform.tag == "bolum")
        {
            Invoke("bolumgec", 3f);//4 saniye sonra bolumgec metodu çalışır.
        }
        if (other.transform.tag == "portal" && a%2==0)
        {
             GameObject.Find("gecis").transform.GetComponent<MeshRenderer>().enabled = true;
             GameObject.Find("gecis2").transform.GetComponent<MeshRenderer>().enabled = true;
             GameObject.Find("gecis").transform.GetComponent<BoxCollider>().enabled = true;
             GameObject.Find("gecis2").transform.GetComponent<BoxCollider>().enabled = true;          
            //Debug.Log("a 0 iken değeri  " + a);                                        
        }
        if (other.transform.tag == "portal" && a%2==1)
        {
            GameObject.Find("gecis").transform.GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("gecis2").transform.GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("gecis").transform.GetComponent<BoxCollider>().enabled = false;
            GameObject.Find("gecis2").transform.GetComponent<BoxCollider>().enabled = false;
            //Debug.Log("a 1 iken değeri  " + a);
        }
        if (other.transform.tag == "portal2" && b % 2 == 0)
        {
            GameObject.Find("gecis3").transform.GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("gecis4").transform.GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("gecis3").transform.GetComponent<BoxCollider>().enabled = false;
            GameObject.Find("gecis4").transform.GetComponent<BoxCollider>().enabled = false;
            //Debug.Log("b 0 iken değeri  " + b);
            a--;
            
        }
        if (other.transform.tag == "portal2" && b % 2 == 1)
        {
            GameObject.Find("gecis3").transform.GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("gecis4").transform.GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("gecis3").transform.GetComponent<BoxCollider>().enabled = true;
            GameObject.Find("gecis4").transform.GetComponent<BoxCollider>().enabled = true;
            //Debug.Log("b 1 iken değeri  " + b);
            a--;
        }
        a++;
        b++;
        /*a nın ve b nin mod 2 si alınarak portal kısmına geldiğinde
        ontrigger metodu ile köprünün açılıp kapatılması sağlanmıştır.*/
    }
           
    public void bolumgec()
    {
        active = false;
        SceneManager.LoadScene("level2");//Level 2 ye geçer.
        Destroy(this.gameObject, 3); // 3 sanıye sonra destroy 
        rb.constraints = RigidbodyConstraints.None;
        GameObject.Find("GameManager").transform.GetComponent<createPlayer>().Create();
    }


    void setRadius()
    {

        Vector3 dirVec = new Vector3(0, 0, 0);          // Hareket yönü vektörü
        Vector3 nomVec = Vector3.up;                    // (0,1,0)

        // Hareket yönünü vektöre dönüştürme
        if (directionX != 0)
        {                           // X yönünde hareket edin
            dirVec = Vector3.right;                     // (1,0,0)
        }
        else if (directionZ != 0)
        {                   // Z yönünde hareket et
            dirVec = Vector3.forward;                   // (0,0,1)
        }

        // Hareket yönü vektörünün iç çarpımından ve Nesnenin yönünden hareket yönündeki yarıçapı hesaplayın ve açıyı başlatın
        if (Mathf.Abs(Vector3.Dot(transform.right, dirVec)) > 0.99)
        {                       // Hareket yönü nesnenin x yönüdür
            if (Mathf.Abs(Vector3.Dot(transform.up, nomVec)) > 0.99)
            {                   // Global'in y ekseni, nesnenin y yönüdür
                radius = Mathf.Sqrt(Mathf.Pow(scale.x / 2f, 2f) + Mathf.Pow(scale.y / 2f, 2f)); // Dönüş yarıçapı
                startAngleRad = Mathf.Atan2(scale.y, scale.x);                              // Yatay düzlemden dönmeden önce ağırlık merkezinin açısı
            }
            else if (Mathf.Abs(Vector3.Dot(transform.forward, nomVec)) > 0.99)
            {       // Global'in y ekseni, nesnenin z yönüdür
                radius = Mathf.Sqrt(Mathf.Pow(scale.x / 2f, 2f) + Mathf.Pow(scale.z / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.z, scale.x);
            }

        }
        else if (Mathf.Abs(Vector3.Dot(transform.up, dirVec)) > 0.99)
        {                   // Hareket yönü, nesnenin y yönüdür
            if (Mathf.Abs(Vector3.Dot(transform.right, nomVec)) > 0.99)
            {                   // Global'in y ekseni, nesnenin x yönüdür
                radius = Mathf.Sqrt(Mathf.Pow(scale.y / 2f, 2f) + Mathf.Pow(scale.x / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.x, scale.y);
            }
            else if (Mathf.Abs(Vector3.Dot(transform.forward, nomVec)) > 0.99)
            {       // Global'in y ekseni, nesnenin z yönüdür
                radius = Mathf.Sqrt(Mathf.Pow(scale.y / 2f, 2f) + Mathf.Pow(scale.z / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.z, scale.y);
            }
        }
        else if (Mathf.Abs(Vector3.Dot(transform.forward, dirVec)) > 0.99)
        {           // Hareket yönü, nesnenin z yönüdür
            if (Mathf.Abs(Vector3.Dot(transform.right, nomVec)) > 0.99)
            {                   // Global'in y ekseni, nesnenin x yönüdür
                radius = Mathf.Sqrt(Mathf.Pow(scale.z / 2f, 2f) + Mathf.Pow(scale.x / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.x, scale.z);
            }
            else if (Mathf.Abs(Vector3.Dot(transform.up, nomVec)) > 0.99)
            {               // Global'in y ekseni, nesnenin y yönüdür
                radius = Mathf.Sqrt(Mathf.Pow(scale.z / 2f, 2f) + Mathf.Pow(scale.y / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.y, scale.z);
            }
        }        
    }
}