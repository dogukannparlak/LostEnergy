# LOST ENERGY

## Game Design Document

### Üçüncü Şahıs Macera / Kaynak Yönetimi Oyunu

| Başlık        | Bilgi                       |
| --------------- | --------------------------- |
| Geliştiren     | Doğukan Parlak - 221805040 |
| Motor           | Unity 6 (URP)               |
| Hedef Platform  | Windows (PC)                |
| Belge Sürümü | 1.0                         |
| Proje Sürümü | 1.0                         |
| Tarih           | Nisan 2026                  |

![Figure 1. Lost Energy kapak görseli](image/ChatGPT%20Image%2013%20Nis%202026%2020_39_57.png)

*Figure 1. Lost Energy için hazırlanan ana kapak görseli.*

![Figure 2. Alternatif kapak tasarımı](image/Gemini_Generated_Image_sncx22sncx22sncx.png)

*Figure 2. Oyunun atmosferini ve anlatı tonunu destekleyen alternatif kapak tasarımı.*

---

## İçindekiler

1. [Tasarım Geçmişi](#1-tasarım-geçmişi)
2. [Oyuna Genel Bakış](#2-oyuna-genel-bakış)
3. [Özellik Seti](#3-özellik-seti)
4. [Oyun Dünyası](#4-oyun-dünyası)
5. [Kamera ve Kontroller](#5-kamera-ve-kontroller)
6. [Oynanış Sistemleri](#6-oynanış-sistemleri)
7. [Dünya Yerleşimi ve İlerleme Akışı](#7-dünya-yerleşimi-ve-ilerleme-akışı)
8. [Karakterler](#8-karakterler)
9. [Kullanıcı Arayüzü](#9-kullanıcı-arayüzü)
10. [Ses ve Müzik](#10-ses-ve-müzik)
11. [Tek Oyunculu Deneyim](#11-tek-oyunculu-deneyim)
12. [Hikaye ve Anlatı](#12-hikaye-ve-anlatı)
13. [Teknik Uygulama](#13-teknik-uygulama)
14. [Asset ve Lisans Bilgileri](#14-asset-ve-lisans-bilgileri)
15. [Kurulum ve Çalıştırma](#15-kurulum-ve-çalıştırma)

---

## 1. Tasarım Geçmişi

### Versiyon 1.0

Bu sürüm, Lost Energy projesinin teslime hazır güncel tasarım belgesini temsil eder.

- Üç oynanabilir sahne tamamlandı: `SampleScene`, `SampleScene2`, `SampleScene3`.
- Üçüncü şahıs karakter kontrolü, orbit kamera ve etkileşim sistemi entegre edildi.
- Oksijen temelli kaynak yönetimi, kristal toplama döngüsü ve tehlikeli bölge sistemi tamamlandı.
- Ana menü, duraklatma menüsü, ayarlar paneli ve yükleme ekranı uygulandı.
- NPC diyalog sistemi, sahneler arası geçiş akışı ve temel ses yönetimi tamamlandı.
- Asset, kaynak ve lisans bilgileri belge içine entegre edildi.

---

## 2. Oyuna Genel Bakış

### Oyun Tanımı

Lost Energy, üçüncü şahıs bakış açısından oynanan tek oyunculu bir macera oyunudur. Oyuncu, hiçliğin ortasında süzülen parçalanmış bir adada hayatta kalmak için oksijenini yönetir, çevreye dağılmış kristalleri toplar, tehlikeli bölgelerden kaçınır ve sahneler arası ilerlemeyi sağlayan kapıları açar.

![Figure 3. Genel oynanış görünümü](image/SampleScene.png)

*Figure 3. Oyuncu karakter, HUD ve sahne yerleşimini birlikte gösteren genel oynanış görünümü.*

### Tasarım Hedefleri

- Basit kurallarla hızlı anlaşılan ancak baskı yaratan bir oynanış kurmak.
- Oksijen mekaniğini hem yaşam göstergesi hem süre baskısı hem de stratejik kaynak olarak kullanmak.
- Çevresel tehditleri, keşfi ve hikaye anlatımını kısa ama tutarlı bir deneyimde birleştirmek.
- Akademik teslim için yönetilebilir kapsamda, bitmiş hissi veren dikey kesit üretmek.

### Temel Tasarım İlkeleri

#### 1. Sürekli Baskı

Oyuncu güvenli bölgede dahi oksijen kaybeder. Bu nedenle hareketsizlik cezalandırılır, karar alma süreci hızlanır ve her kristal anlamlı hale gelir.

#### 2. Net Hedefler

İlk iki sahnede kristal hedefi açıktır. Oyuncu ne toplaması gerektiğini, kapının ne zaman açılacağını ve ilerleme koşulunu sürekli olarak görebilir.

#### 3. Çevreyle Anlatım

Kırmızı tehlikeli zeminler, sahne geçişleri, ada sınırları ve muhafız diyalogları birlikte çalışarak dünyanın bozulmuş yapısını destekler.

### Sık Sorulan Temel Sorular

#### Oyun nedir?

Kaynak yönetimi, keşif ve sahne tabanlı ilerleme üzerine kurulu kısa süreli bir üçüncü şahıs macera deneyimidir.

#### Oyun nerede geçer?

Oyun, tamamlanamamış bir yaratımın parçası olan ve hiçliğin ortasında süzülen bir ada sisteminde geçer.

#### Oyuncu neyi kontrol eder?

Tek bir karakter kontrol edilir. Karakter hareket eder, zıplar, etkileşime girer, kristal toplar ve sahneler arasında ilerler.

#### Oyunun ana odağı nedir?

Ana odak, oksijen baskısı altında doğru rotayı seçmek, kristal toplamak ve çıkışa ulaşmaktır.

#### Bu oyunu farklı kılan unsur nedir?

Oksijen sistemi yalnızca sağlık göstergesi değildir; zaman baskısı, rota optimizasyonu ve ödül yapısını aynı anda belirleyen temel mekaniktir.

---

## 3. Özellik Seti

### Temel Oynanış Özellikleri

- Üçüncü şahıs karakter kontrolü
- Serbest dönüşlü orbit kamera sistemi
- Gerçek zamanlı oksijen tüketim ve yenileme sistemi
- Kristal toplama ve hedef tamamlama yapısı
- `IInteractable` tabanlı etkileşim mimarisi
- Tehlikeli bölge kaynaklı ek oksijen kaybı
- Düşme ve oksijen tükenmesine bağlı ölüm/respawn akışı
- NPC diyalog sistemi ve sahneye bağlı anlatı aktarımı

### Destekleyici Sistemler

- Ana menü ve duraklatma menüsü
- Ayarlar paneli ve ses seviyesi kontrolü
- Yükleme ekranı üzerinden sahne geçişi
- Sahne bazlı müzik değişimi
- Kristal toplama için VFX ve SFX desteği
- Final sahnesinde dallanan bitiş seçimi

### Kullanıcı Deneyimi Özellikleri

- Sürekli görünür oksijen ve kristal göstergeleri
- Yakın etkileşim hedefi için ekranda istem metni
- Açık kontrol şeması
- Kısa oturum süresine uygun kompakt seviye yapısı

---

## 4. Oyun Dünyası

### Genel Yapı

Lost Energy dünyası, tamamlanamamış bir varoluş alanı olarak tasarlanmıştır. Ada fiziksel olarak sınırlıdır; sınırların dışına çıkmak ölümle sonuçlanır. Her yeni sahne, bozulmanın daha görünür hale geldiği bir bölgeyi temsil eder.

### Dünya Özellikleri

#### Parçalanmış Ada Yapısı

Oyun alanı, güvenli yüzeyler ile riskli boşlukların bir arada bulunduğu bir düzen üzerine kuruludur. Böylece yön bulma, platform üzerinde kalma ve güvenli rota seçimi önem kazanır.

#### Bozulma Alanları

Kırmızı tehlikeli yüzeyler yalnızca görsel çeşitlilik sağlamaz; aynı zamanda oyuncuyu rotasını değiştirmeye zorlayan mekanik tehdit bölgeleridir.

### Fiziksel Dünya Özeti

| Özellik              | Değer                  |
| --------------------- | ----------------------- |
| Perspektif            | Üçüncü şahıs (3D) |
| Yürüme Hızı       | 10 birim/sn             |
| Sprint Hızı         | 25 birim/sn             |
| Zıplama Yüksekliği | 1.5 birim               |
| Kamera Mesafesi       | 5 birim                 |

![Figure 4. Sahne 2 genel görünümü](image/SampleScene2.png)

*Figure 4. Bozulmanın arttığı ikinci sahnenin genel görünümü.*

![Figure 5. Sahne 3 genel görünümü](image/SampleScene3.png)

*Figure 5. Final bölümüne ait genel çevre görünümü.*

---

## 5. Kamera ve Kontroller

### Kamera Sistemi

Kamera sistemi `PlayerController3P` içinde yönetilir. Kamera, oyuncu etrafında orbit mantığıyla döner; yaw ve pitch girdileri birbirinden bağımsız ele alınır. Hareket yönü, kamera bakışına göre hesaplandığı için kontrol hissi üçüncü şahıs aksiyon oyunlarına benzer şekilde tasarlanmıştır.

| Parametre        | Değer  |
| ---------------- | ------- |
| Fare Hassasiyeti | 0.2     |
| Pitch Minimum    | -30°   |
| Pitch Maksimum   | +60°   |
| Kamera Mesafesi  | 5 birim |

### Kontrol Şeması

| Eylem                                 | Girdi                 |
| ------------------------------------- | --------------------- |
| Hareket                               | WASD / Yön Tuşları |
| Zıplama                              | Space                 |
| Sprint                                | Left Shift            |
| Etkileşim                            | E                     |
| Kamera                                | Fare                  |
| Duraklatma / İmleç Serbest Bırakma | ESC                   |

---

## 6. Oynanış Sistemleri

### Oksijen Sistemi

Oksijen, oyunun ana kaynak yapısıdır. Oyuncu sahnede bulunduğu sürece oksijen tüketir. Değer sıfıra ulaştığında karakter ölür ve son güvenli başlangıç noktasına geri döner.

| Bölge  | Başlangıç Oksijen | Normal Tüketim | Hazard Ek Tüketim | Kristal Bonusu |
| ------- | -------------------- | --------------- | ------------------ | -------------- |
| Sahne 1 | 100                  | 3 birim/sn      | +5 birim/sn        | +5             |
| Sahne 2 | 100                  | 3 birim/sn      | +6 birim/sn        | +6             |

### Kristal Sistemi

- Kristaller sahne içinde tanımlı alanlarda oluşturulur.
- Oyuncu `E` tuşu ile etkileşime girerek kristali toplar.
- Toplama işlemi sırasında görsel ve işitsel geri bildirim üretilir.
- İlk iki sahnede hedef kristal sayısı tamamlanınca çıkış kapısı açılır.
- Son sahnede kristal hedefi bulunmaz; final seçimi doğrudan oyuncuya sunulur.

### Tehlikeli Bölge Sistemi

- `HazardZone` alanları oksijen tüketimini artırır.
- Bu bölgeler kırmızı yüzeylerle görsel olarak işaretlenmiştir.
- Tehlike alanına girişte müzik ve gerilim hissi güçlendirilir.

![Figure 6. Hazard bölgesi örneği](image/hazardzone.png)

*Figure 6. Tehlikeli bölgenin oyun alanındaki etkisini gösteren örnek ekran görüntüsü.*

![Figure 7. Hazard bölgesi yakın görünüm](image/hazardzone2.png)

*Figure 7. Hazard alanının oyuncu üzerindeki baskısını gösteren ikinci görünüm.*

### Etkileşim Sistemi

Etkileşim yapısı `IInteractable` arayüzü üzerine kuruludur. Kristaller, kapılar, NPC'ler ve iksirler aynı yaklaşımı kullanır. `PlayerInteraction`, oyuncuya en uygun hedefi belirler ve ekranda uygun istem metnini gösterir.

![Figure 8. Kristal etkileşim ekranı](image/crystalınteractable.png)

*Figure 8. Kristal toplama anında görülen etkileşim istemi ve HUD bileşenleri.*

### Ödül ve İlerleme Döngüsü

```text
Kristal toplama -> Oksijen yenileme -> Daha uzun süre hayatta kalma
                -> Daha fazla kristal toplama -> Hedefi tamamlama
                -> Çıkış kapısını açma -> Yeni sahneye geçiş
```

---

## 7. Dünya Yerleşimi ve İlerleme Akışı

### Genel Akış

Oyun üç ana sahneden oluşur. İlk iki sahnede oyuncu belirli sayıda kristal toplar; üçüncü sahnede ise final seçimiyle deneyim sonlandırılır veya döngüye alınır.

### Sahne 1 - Adanın Girişi

| Başlık       | Açıklama                                                       |
| -------------- | ---------------------------------------------------------------- |
| Atmosfer       | Görece sakin başlangıç bölgesi                              |
| Kristal Hedefi | 25                                                               |
| Tehlikeler     | Ada sınırından düşme, düşük yoğunluklu hazard alanları |
| NPC            | Muhafız                                                         |
| Çıkış      | `ExitDoorSceneLoader` ile Sahne 2                              |

### Sahne 2 - Bozulmuş Bölge

| Başlık       | Açıklama                                                                    |
| -------------- | ----------------------------------------------------------------------------- |
| Atmosfer       | Bozulmanın arttığı, daha agresif risk alanları                           |
| Kristal Hedefi | 30                                                                            |
| Tehlikeler     | Ada sınırından düşme, yoğun hazard bölgeleri, daha sert rota baskısı |
| NPC            | Muhafız                                                                      |
| Çıkış      | `ExitDoorSceneLoader` ile Sahne 3                                           |

### Sahne 3 - Yaratımın Sonu

| Başlık       | Açıklama                                                      |
| -------------- | --------------------------------------------------------------- |
| Atmosfer       | Yarım kalmış varoluşun son eşiği                          |
| Kristal Hedefi | 0                                                               |
| Tehlikeler     | Final kararı, dönüş veya çıkış seçimi                  |
| NPC            | Muhafız                                                        |
| Çıkış      | `FinalDoorController` ile döngü sonu veya kazanış ekranı |

### Oyun Akış Şeması

```text
Ana Menü
  -> LoadingScreen
  -> Sahne 1
  -> Sahne 2
  -> Sahne 3
     -> Döngü Kapısı: Sahne 1'e dönüş
     -> Çıkış Kapısı: Tebrikler ekranı
```

![Figure 9. Yükleme ekranı](image/loadingpanel.png)

*Figure 9. Sahneler arası geçiş için kullanılan yükleme ekranı.*

![Figure 10. Döngü kapısı etkileşimi](image/loopdoorınteractable.png)

*Figure 10. Final sahnesinde döngüye dönüş seçeneğini sunan kapı etkileşimi.*

---

## 8. Karakterler

### Oyuncu Karakteri

| Özellik           | Açıklama                                                         |
| ------------------ | ------------------------------------------------------------------ |
| Model              | Kenney Animated Characters Protagonists                            |
| Kontrol Yapısı   | `PlayerController3P` + `CharacterController`                   |
| Temel Amaç        | Oksijen yönetimi altında kristal toplayıp çıkışa ulaşmak   |
| Ölüm Koşulları | Oksijenin sıfırlanması veya `Y < -1` olacak şekilde düşmek |

### NPC - Muhafız

Muhafız, oyunun tek anlatı odaklı NPC'sidir. Her sahnede oyuncuyu karşılar, bulunduğu bölgeye ilişkin bilgiyi verir ve dünyanın arka planını kademeli biçimde açar. Karakter, ada ile bütünleşmiş olduğu için bu dünyayı terk edemez; bu durum onun anlatıdaki işlevini güçlendirir.

### Diyalog Sistemi Davranışı

- `NPCDialogue` ve `DialogueData` yapıları ile yönetilir.
- Diyalog aktifken oksijen tüketimi duraklatılır.
- Diyalog, oyuncuya sistem öğretimi ve hikaye aktarımını aynı anda sağlar.

![Figure 11. Muhafız ile diyalog](image/dialoguepanel.png)

*Figure 11. Muhafız karakteriyle etkileşim sırasında açılan diyalog paneli.*

---

## 9. Kullanıcı Arayüzü

### HUD Bileşenleri

| Bileşen           | İşlev                                                              |
| ------------------ | -------------------------------------------------------------------- |
| Oksijen Slider     | Anlık oksijen düzeyini görsel olarak gösterir                    |
| Oksijen Değeri    | Oksijenin sayısal karşılığını gösterir                       |
| Kristal Sayacı    | Toplanan kristal sayısını `mevcut / hedef` biçiminde gösterir |
| Etkileşim İstemi | Yakındaki etkileşimli nesne için bağlamsal bilgi sunar           |

### Panel Yapısı

| Panel          | Tetiklenme Koşulu                                    |
| -------------- | ----------------------------------------------------- |
| Game Over      | Oksijenin tükenmesi veya düşme ölümü            |
| Win            | Final sahnesinde kazanış çıkışının seçilmesi |
| Pause          | ESC tuşu                                             |
| Settings       | Pause menüsü içinden erişim                       |
| Controls       | Ana menü veya pause menüsü içinden erişim        |
| Loading Screen | Sahne geçişleri sırasında otomatik açılır      |

### Menü Yapısı

Ana menüde oyuna başlama, ayarlar, kontroller ve çıkış seçenekleri yer alır. Oyun başlangıcında `Time.timeScale = 1f` durumundadır ve imleç serbesttir.

![Figure 12. Ana menü ekranı](image/mainmenupanel.png)

*Figure 12. Oyunun ana menü arayüzü.*

![Figure 13. Kontroller ekranı](image/controlspanel.png)

*Figure 13. Oyunun kontrol şemasını gösteren kullanıcı arayüzü ekranı.*

![Figure 14. Ayarlar ekranı](image/settingspanel.png)

*Figure 14. Ses düzeyi ve sessize alma seçeneklerini içeren ayarlar paneli.*

---

## 10. Ses ve Müzik

### Ses Mimarisi

Projede `GameAudioMixer` üzerinden üç temel kanal kullanılmaktadır.

| Kanal     | Amaç                                                     |
| --------- | --------------------------------------------------------- |
| MasterVol | Tüm seslerin genel seviyesi                              |
| MusicVol  | Arka plan müzikleri                                      |
| SFXVol    | Oyun içi efektler, diyalog sesleri ve hazard uyarıları |

### Müzik Kullanımı

- Ana menü ve oyun sahneleri için farklı müzik akışları kullanılır.
- Hazard alanları gerilim hissini artıran ek ses katmanlarıyla desteklenir.
- `MusicManager`, sahne geçişlerinde müzik sürekliliğini korur.

### Kaynak Özeti

| Kaynak                          | İçerik                                                |
| ------------------------------- | ------------------------------------------------------- |
| Flowerhead - SomeWhatGood: Lofi | `Observatory and chill 2`                             |
| Not Jam Music Pack              | `ChillMenu`, `CriticalTheme`, `SwitchWithMeTheme` |
| Freesound                       | Kristal toplama ve NPC ses efektleri                    |

---

## 11. Tek Oyunculu Deneyim

### Oyun Döngüsü

Tek oyunculu deneyim, kısa süreli ancak yoğun karar baskısı yaratan sahne tabanlı bir yapı üzerine kuruludur. Oyuncu her sahnede çevreyi okur, riskli rotaları değerlendirir, kristal toplar ve çıkış kapısına ulaşır.

### Tahmini Oynanış Süresi

| Oynanış Tarzı                    | Süre        |
| ----------------------------------- | ------------ |
| Hızlı Tamamlama                   | 5-6 dakika   |
| Keşif ve Diyalog Odaklı Oynayış | 10-12 dakika |

### Kazanma ve Kaybetme Koşulları

| Durum    | Koşul                                                                 |
| -------- | ---------------------------------------------------------------------- |
| Kazanma  | Sahne 3'te `FinalDoorController` üzerinden çıkış sonunu seçmek |
| Kaybetme | Oksijenin sıfırlanması veya sahneden düşme                        |

![Figure 15. Çıkış kapısı etkileşimi](image/windoorınteractable.png)

*Figure 15. Oyunu tamamlama seçeneğini sunan çıkış kapısı etkileşimi.*

![Figure 16. Tebrikler ekranı](image/windoorpanel.png)

*Figure 16. Oyunun tamamlanmasının ardından gösterilen sonuç ekranı.*

---

## 12. Hikaye ve Anlatı

### Hikaye Özeti

Lost Energy, tamamlanmamış bir yaratımın içinde sıkışmış bir karakterin çıkış arayışını konu alır. Oyun dünyası, işlevini tam olarak yerine getiremeyen bir varoluş alanıdır; bu nedenle oksijen bile kararlı değildir. Kırmızı bozulma bölgeleri, dünyanın çözülen yapısını temsil eder. Kristaller ise bu bozulmuş düzen içinde hâlâ sağlam kalabilmiş parçalar olarak işlev görür.

### Tematik Yapı

| Unsur      | Anlamı                                                  |
| ---------- | -------------------------------------------------------- |
| Ada        | Tamamlanamamış yaratım                                |
| Oksijen    | Dünyanın kararsız ve eksik doğası                   |
| Kristaller | Gerçekliğin korunmuş parçaları                      |
| HazardZone | Bozulmanın fiziksel yansıması                         |
| Muhafız   | Sistemin farkında olan ama dışına çıkamayan tanık |

### Sahne Bazlı Anlatı Gelişimi

| Sahne   | Anlatı İşlevi                                        |
| ------- | ------------------------------------------------------- |
| Sahne 1 | Dünya kurallarının ve temel hedefin tanıtılması   |
| Sahne 2 | Bozulmanın ve tehlikenin yoğunlaştırılması        |
| Sahne 3 | Gerçeğin açıklanması ve oyuncuya seçim sunulması |

### Karakter Motivasyonları

- Oyuncu karakter, içinde bulunduğu dünyayı anlamadan önce hayatta kalmaya ve çıkış yolunu bulmaya çalışır.
- Muhafız, rehberlik işlevi görür; umuttan çok görev duygusuyla hareket eder.

---

## 13. Teknik Uygulama

### Mimari Özeti

```text
LostEnergy Namespace
├── GameManager
├── MusicManager
├── SettingsManager
├── SceneLoader
└── UIManager
```

### Temel Scriptler

| Script                   | Sorumluluk                                           |
| ------------------------ | ---------------------------------------------------- |
| `PlayerController3P`   | Karakter hareketi, sprint, zıplama ve orbit kamera  |
| `OxygenSystem`         | Oksijen tüketimi ve yenilenmesi                     |
| `GameManager`          | Kristal sayımı, hedef kontrolü ve ölüm akışı |
| `CrystalCollectible`   | Kristal etkileşimi, SFX ve VFX tetikleme            |
| `HazardZone`           | Ek oksijen tüketimi uygulama                        |
| `DialogueManager`      | Diyalog akışı ve oksijen duraklatma               |
| `RespawnManager`       | Ölüm sonrası konum sıfırlama                    |
| `PauseManager`         | Duraklatma akışı ve panel yönetimi               |
| `LoadingScreenManager` | Asenkron sahne yükleme ve yükleme ekranı          |

### Teknik Kararlar

- Oyun mantığı modüler script yapıları üzerinden yönetilir.
- `MusicManager` sahneler arası kalıcılık için `DontDestroyOnLoad` yaklaşımını kullanır.
- `SettingsManager`, ses ayarlarını `PlayerPrefs` ile saklar.
- Referans erişimleri `Start()` aşamasında önbelleğe alınarak `Update()` içindeki maliyet azaltılır.

### Performans Notları

- Kristal VFX yapısı düşük nesne yoğunluğu için `Instantiate + Destroy` yaklaşımıyla yeterlidir.
- Sahne ölçekleri kısa oynanış oturumlarına göre sınırlandırıldığı için genel çalışma maliyeti kontrollüdür.

---

## 14. Asset ve Lisans Bilgileri

### Lisans Özeti

| Kaynak Grubu                    | Lisans / Kullanım Şartı                       |
| ------------------------------- | ------------------------------------------------ |
| Kenney assetleri                | CC0 1.0 Universal                                |
| Unity Asset Store assetleri     | Standard Unity Asset Store EULA                  |
| Not Jam Music Pack              | CC0 1.0                                          |
| Flowerhead - SomeWhatGood: Lofi | Royalty free (yazar beyanı)                     |
| Freesound efektleri             | Dosya bazlı CC lisansı                         |
| Unity yerleşik paketleri       | Unity Asset Store EULA / Unity Companion License |

### Kenney Assetleri

Tüm Kenney assetleri [CC0 1.0 Universal](https://creativecommons.org/publicdomain/zero/1.0/) lisansı ile yayınlanmıştır.

| Asset Paketi                     | Sürüm | Kaynak                                       |
| -------------------------------- | ------- | -------------------------------------------- |
| Animated Characters Protagonists | 1.1     | https://kenney.nl/assets/animated-characters |
| Fantasy Town Kit                 | 2.0     | https://kenney.nl/assets/fantasy-town-kit    |
| Graveyard Kit                    | 5.0     | https://kenney.nl/assets/graveyard-kit       |
| Nature Kit                       | 2.1     | https://kenney.nl/assets/nature-kit          |
| Platformer Kit                   | 4.1     | https://kenney.nl/assets/platformer-kit      |

### Unity Asset Store Assetleri

| Asset Paketi     | Yayıncı | Sürüm |
| ---------------- | --------- | ------- |
| Stylized Crystal | LowlyPoly | 1.0     |
| Stylized door    | lowpoly89 | 1.0     |

Lisans: [Unity Asset Store End User License Agreement](https://unity.com/legal/as-terms)

### Ses ve Müzik Kaynakları

| Dosya / Paket                       | Kaynak                                              | Lisans                       |
| ----------------------------------- | --------------------------------------------------- | ---------------------------- |
| 794489__gobbe57__coin-pickup.wav    | https://freesound.org/people/gobbe57/sounds/794489/ | Freesound dosya lisansı     |
| 822698__metris__retro-npc-voice.wav | https://freesound.org/people/Metris/sounds/822698/  | Freesound dosya lisansı     |
| ChillMenu.wav                       | https://not-jam.itch.io/not-jam-music-pack          | CC0 1.0                      |
| CriticalTheme.wav                   | https://not-jam.itch.io/not-jam-music-pack          | CC0 1.0                      |
| SwitchWithMeTheme.wav               | https://not-jam.itch.io/not-jam-music-pack          | CC0 1.0                      |
| Observatory and chill 2.wav         | https://flowerheadmusic.itch.io/somewhat-good-lofi  | Royalty free (yazar beyanı) |

### Unity Yerleşik Paketleri

| Paket                 | Yayıncı          | Lisans                  |
| --------------------- | ------------------ | ----------------------- |
| TextMesh Pro          | Unity Technologies | Unity Asset Store EULA  |
| Input System          | Unity Technologies | Unity Companion License |
| Post Processing Stack | Unity Technologies | Unity Companion License |

### Kullanım Notları

- Unity Asset Store varlıklarının kullanım koşulları standart Unity EULA kapsamındadır.
- Freesound kaynağından alınan dosyalarda lisans tipi dosya bazında ayrıca kontrol edilmelidir.
- CC BY lisanslı bir Freesound dosyası kullanılması halinde yazılı atıf zorunludur.

---

## 15. Kurulum ve Çalıştırma

### Geliştirme Ortamı Gereksinimleri

- Unity 6 (önerilen: 6000.0.x LTS)
- Universal Render Pipeline (URP)
- Input System paketi

### Minimum Sistem Gereksinimleri

- İşletim Sistemi: Windows 10 64-bit
- İşlemci: Intel Core i3 sınıfı veya dengi
- Bellek: 4 GB RAM
- Ekran Kartı: DirectX 11 destekli ekran kartı
- Depolama: En az 1 GB boş alan
- Çözünürlük: 1280 x 720

### Build Alma Adımları

1. Unity Editor içinde `File -> Build Settings` menüsünü açın.
2. Sahnelerin aşağıdaki sırayla listelendiğini doğrulayın:
   - `0` MainMenu
   - `1` LoadingScreen
   - `2` SampleScene
   - `3` SampleScene2
   - `4` SampleScene3
3. Platform olarak `Windows, x86_64` seçimini yapın.
4. `Build` komutu ile çıktı klasörü olarak `Builds/Windows/` dizinini seçin.

### Çalıştırma

Derlenen sürüm, `Builds/Windows/Lost Energy.exe` dosyası üzerinden çalıştırılır.

### Hızlı Kontrol Özeti

```text
WASD        -> Hareket
Left Shift  -> Sprint
Space       -> Zıplama
E           -> Etkileşim / Kristal Toplama
Fare        -> Kamera
ESC         -> Duraklatma Menüsü
```
