# LOST ENERGY

## GDD Lite

### Künye

**Proje Adı:** Lost Energy
**Doküman Sahibi:** Doğukan Parlak
**Öğrenci No:** 221805040
**Motor:** Unity 6 (URP)
**Hedef Platform:** Windows (PC)

## Proje Özeti

Lost Energy, tek oyunculu ve kısa süreli bir üçüncü şahıs toplama ve hayatta kalma oyunudur. Oyuncu, boşlukta süzülen parçalanmış adalar arasında ilerlerken oksijenini yönetir, kristaller toplar, tehlikeli bölgelerden kaçınır ve çıkış kapısına ulaşmaya çalışır.

Proje, kısa sürede anlaşılabilen ancak oyuncu üzerinde sürekli baskı kuran bir oynanış döngüsü hedefler. Temel odak; keşif, kaynak yönetimi, riskli rota seçimi ve kısa ama bütünlüklü bir ilerleyiş sunmaktır.

## Yüksek Konsept

Oyuncu, parçalanmış ve tamamlanmamış bir dünyanın içinde hayatta kalmaya çalışır. Oksijen sürekli azalırken kristaller hem ilerlemek için gerekli bir hedef hem de kısa süreli bir destek kaynağıdır. Ana amaç, yeterli kristali toplayıp çıkışa ulaşmaktır.

## Oyuncu Deneyimi Hedefi

Oyuncunun oyun boyunca şu duyguları yaşaması hedeflenir:

- sürekli zaman ve kaynak baskısı hissetmek
- kristal topladıkça ilerleme ve başarı hissi kazanmak
- kısa ama akılda kalıcı bir macera yaşamak

## Temel Tasarım Sütunları

**1. Sürekli Baskı:** Oksijen güvenli alanlarda bile azaldığı için oyuncu duraksamadan hareket etmeye teşvik edilir.

**2. Net Hedefler:** Oyuncuya kaç kristal toplaması gerektiği ve çıkışın ne zaman açılacağı açık biçimde gösterilir.

**3. Basit ama Etkili Döngü:** Keşif, kristal toplama, oksijen yönetimi ve çıkışa ulaşma döngüsü oyunun çekirdeğini oluşturur.

## Temel Oynanış Döngüsü

**Keşfet, kristal topla, oksijenini koru ve çıkışa ulaş.**

Oyuncu her sahnede çevreyi inceler, kristal toplar, tehlikeli alanlardan kaçınır ve yeterli ilerlemeyi sağladıktan sonra bir sonraki hedefe yönelir. Böylece oyun, baştan sona net ve anlaşılır bir akış sunar.

## Ana Mekanikler

- üçüncü şahıs karakter kontrolü
- serbest kamera hareketi
- oksijenin zamanla azalması
- etkileşimle kristal toplama
- tehlikeli bölgelerde ek oksijen kaybı
- kristal hedefi tamamlanınca çıkışın açılması
- kazanma, kaybetme ve yeniden başlatma akışı

## Kontroller

- **WASD / Ok Tuşları:** Hareket
- **Mouse:** Kamera kontrolü
- **Space:** Zıplama
- **Left Shift:** Koşma
- **E:** Etkileşim
- **ESC:** Duraklatma

## Sahne Yapısı

**1. Main Menu**

Oyunu başlatma, ayarlar ve kontroller ekranına erişim sağlar.

**2. Oyun Sahneleri**

**2.a - Giriş Adası:** Oyuncu burada temel hareket, kamera kullanımı, etkileşim ve kristal toplama mantığını öğrenir. Gerekirse kısa bir NPC karşılaşmasıyla yönlendirilir.

**2.b - Kırık Ada:** Çevre daha karmaşık hale gelir. Tehlikeli yüzeyler, daha zor rota seçimi ve daha dikkatli oksijen yönetimi gerektirir.

**2.c - Son Ada:** Son bölümde oyuncu artık öğrendiği tüm sistemleri birlikte kullanır. Burada oyunun finaline ulaşır ve döngüde kalmak ya da döngüyü kırmak gibi anlatısal bir seçimle karşılaşabilir.

**3. Sonuç Ekranı**

Oyuncu çıkışa ulaşırsa başarı ekranı görür. Oksijeni biterse veya boşluğa düşerse kaybetme ekranı gösterilir ve yeniden deneme seçeneği sunulur. Eğer döngüde kalma seçeneği kullanılırsa oyun, bunu anlatısal bir geri dönüş olarak sunar.

## Kaynak ve Başarı Sistemi

Oyunun temel kaynağı oksijendir.

- oyuncu zamanla oksijen kaybeder
- tehlikeli bölgelerde oksijen kaybı artar
- kristaller oksijeni kısmen yenileyebilir
- oksijen sıfırlanırsa oyuncu bölümü kaybeder

Başarı koşulu, gerekli kristal sayısına ulaşıp çıkış kapısına gitmektir. Başarısızlık ise oksijenin tükenmesi ya da oyuncunun adalardan düşmesiyle oluşur.

## Zorluk İlerleyişi

Zorluk, oyuncuya sistemleri öğretirken kademeli olarak artar.


Bu yapı, öğretme ve zorluk artışını aynı oyun akışı içinde sade bir şekilde kurar.

## Başarısızlık Geri Bildirimi

Oyuncu başarısız olduğunda sebebi açık biçimde anlamalıdır.

- ekranda kısa bir kaybetme mesajı görünür
- oksijenin bittiği ya da boşluğa düşüldüğü belirtilir
- kısa bir ses efekti ile başarısızlık anı desteklenir
- oyuncuya hızlıca yeniden deneme seçeneği sunulur

Bu geri bildirim, oyuncuya net bilgi verirken yeniden denemeyi teşvik eder.

## Arayüz ve Ses

Arayüzde oyuncuya sürekli gerekli bilgiler gösterilir:

- oksijen seviyesi
- kristal sayacı
- etkileşim yazısı
- duraklatma menüsü
- sonuç panelleri

Ses tarafında menü ve oynanış müzikleri ayrıdır. Kristal toplama, tehlike, başarı ve başarısızlık anlarında geri bildirim veren temel ses efektleri kullanılır.

## Teknik Plan

Proje Unity içinde küçük ve yönetilebilir sistemler halinde kurulacaktır:

- `PlayerController3P`: hareket, zıplama ve kamera kontrolü
- `OxygenSystem`: oksijen tüketimi ve yenileme
- `GameManager`: kristal takibi, sahne akışı ve temel oyun yönetimi
- `HazardZone`: tehlikeli alanlarda ek oksijen kaybı
- `PauseManager`: duraklatma menüsü yönetimi
- `UIManager`: HUD, uyarılar ve sonuç panelleri

Bu yapı, sistemi modüler tutarak proje kapsamının öğrenci düzeyinde yönetilebilir kalmasını sağlar.

## Minimum Teslim Hedefi

Bu proje için temel gösterilebilir çıktı şunlardır:

- çalışan bir ana menü
- oynanabilir oyun sahneleri
- üçüncü şahıs karakter kontrolü
- oksijen ve kristal sistemi
- tehlike bölgeleri
- çıkış hedefi ve kapı açılma mantığı
- basit kazanma ve kaybetme ekranları

## Riskler ve Kapsam Kontrolü

Projede en büyük risk, gereksiz özellik ekleyerek kapsamı büyütmektir. Bu nedenle odak şu sistemlerde tutulmalıdır:

- hareket
- oksijen yönetimi
- kristal toplama
- sahne içi ilerleyiş
- temel arayüz ve geri bildirim

NPC diyalogları, dallanan sonlar, daha yoğun hikaye katmanları ve ileri görsel efektler ana oynanış tamamlandıktan sonra değerlendirilmelidir.

## Bu Proje Neden Derse Uygun?

Lost Energy bu ders için uygun bir projedir çünkü üçüncü şahıs oynanış gerekliliğini doğrudan karşılar, birden fazla sahneden oluşan düzenli bir oyun akışını destekler ve oyuncunun ilerlemesini oksijen, kristal ve çıkış hedefi gibi net sistemlerle ölçülebilir hale getirir. Proje kapsamı da öğrenci düzeyinde gerçekçidir; önce temel bir prototip olarak hareket, toplama ve hayatta kalma döngüsü kurulabilir, ardından arayüz, ses, sahne geçişleri ve genel sunum kalitesi geliştirilerek daha güçlü bir final teslimine dönüştürülebilir. Bu yapı sayesinde proje hem teknik olarak yönetilebilir hem de ders çıktıları açısından yeterli derinliğe sahiptir.
