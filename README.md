# CaseStudyKAF

## Kullanılan Oyun Motoru
- Unity **6000.1.10f1**

## Kullanılan Ekstra Teknolojiler ve Paketler
- **Addressables** → Asset yönetimi ve yükleme optimizasyonu için kullanıldı.  
- **UniTask** → Asenkron işlemleri daha performanslı ve okunabilir hale getirmek için kullanıldı.  
- **Zenject** → Bağımlılık enjeksiyonu ve esnek mimari kurgusu için tercih edildi.

- ## Açıklamalar ve Notlar

### Açıklamalar
- Menüye **manuel stres testi** için özel bir seçenek eklenmiştir. Bu sayede çok sayıda düşman ve nesne aynı anda sahneye alınarak performans gözlemlenebilir.  
- Ekstralarda verilen **2. silah seçeneği** yerine, karaktere **skill sistemi** eklenmiştir. Bu sistem, oynanışa çeşitlilik kazandırmak için geliştirilmiştir.  
- Kod yapısı **Zenject** ile modüler hale getirilmiş, böylece ekleme/çıkarma işlemleri daha kolay yapılabilir.
- Oyunda her düşman dalgası 1 seviye olarak kabul edilmiş ve her dalga sonunda oyun kaydedilmektedir.
- Temel amacımız kuleyi korumaktır.

### Notlar
- Oyunda kullanılan düşman dalgaları **Addressables** üzerinden yüklenmektedir.  
- Performans için **async/await yerine UniTask** kullanılmıştır.
- Kendi bilgisayarımdan yaptığım stres testinde sahneye 2500 düşman objesi çağırdığım zaman fps 33-39 arası değerlerde dolaştı. 3000'in üstünde çıktığım zaman 27 civarına düştüğünü gözlemlendi.
