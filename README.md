[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/EBv50WFu)

--- 

# 259 Simpra Net Bootcamp Final Projesi

Proje kapsamında sadece dijital ürünler satan bir platform geliştirilmesi beklenmektedir. Hedefimiz Mobil ve web
uygulaması olmak üzere 3 farklı kanal üzerinden satış yapan bir platform için uygulama geliştirmektir. (
Android,IOS,Web).

Proje kapsamında dijital ürün veya ürün lisansları satışı yapılmaktadır. Kullanıcılar dijital ürün veya ürün lisansları
satan sisteme kayıt yaptırarak alışveriş yapabilirler.

Sadakat sistemi ile çalışan bu sistemde kullanıcılar alışveriş yaptıkça extra puan kazanmaktadır.

Kullanıcılar kazandıkları puanları bir sonraki alışverişte kullanarak yeni ürünleri indirimli bir şekilde
alabilmektedirler. Bununla birlike kupon sistemi sayesinde hediye kuponlar ile sepet tutarı üzerinden daha uygun fiyatlı
alışveriş yapabilmektedir.

## Kullanılan Teknolojiler

- [.Net 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [AutoMapper](https://automapper.org/)
- [Fluent Validation](https://fluentvalidation.net/)
- [Swagger](https://swagger.io/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Redis](https://redis.io/)
- [InMemoryCache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-5.0)
- [MsSQL](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Docker Compose](https://docs.docker.com/compose/)
- [Serilog](https://serilog.net/)
- [Postman](https://www.postman.com/)
- [XUnit](https://xunit.net/)
- [Moq]()

### Proje Yapısı

Proje aşağıdaki bileşenlere ayrılmıştır:

- **API**: Web API katmanı, HTTP isteklerini karşılar ve yanıtlar.
    - **Controllers**: API isteklerini karşılayan controller sınıflarını içeren klasördür.
    - **Extensions**: API katmanı için extension metodları içeren klasördür.
    - **logs**: Proje içerisinde oluşan logların tutulduğu klasördür.
    - **serilog.json**: Serilog ayarlarının tutulduğu yapılandırma dosyasıdır.
- **Application**: Uygulama katmanı, API ile UI/CLI arasındaki iletişimi sağlar ve iş mantığını yönetir.
    - **Contracts**: Uygulama katmanı için contract sınıflarını içeren klasördür.
        - **Constants**: Uygulama katmanı için sabitleri içeren klasördür.
        - **Mapper**:  AutoMapper konfigürasyonlarını içeren klasördür.
        - **Repositories**: Repository arayüzlerini içeren klasördür.
        - **Requests**: İstek modellerini içeren klasördür.
        - **Responses**: Yanıt modellerini içeren klasördür.
        - **Services**: İş sınıfları için arayüzleri içeren klasördür.
        - **Validations**: Fluent Validation doğrulama kurallarını içeren klasördür.
    - **Services**: İş sınıflarını içeren klasördür.
    - **ApplicationExtensions.cs**: Uygulama katmanı için extension metodları içeren sınıftır.
- **Core**: Çekirdek katmanı, tüm katmanlar tarafından kullanılan temel bileşenleri içerir.
    - **Application**: Uygulama katmanı için temel bileşenleri içeren klasördür.
    - **CrossCuttingConcerns**: Harici bileşenlerle ilgili temel bileşenleri içeren klasördür.
    - **Domain**: Veritabanı nesneleri için temel bileşenleri içeren klasördür.
    - **Persistence**: Veritabanı katmanı için temel bileşenleri içeren klasördür.
    - **Utilities**: Yardımcı bileşenleri içeren klasördür.
    - **CoreExtensions.cs**: Çekirdek katmanı için extension metodları içeren sınıftır.
- **Domain**: Veritabanı nesnelerini içeren klasördür.
    - **Entities**: Veritabanı nesnelerini içeren klasördür.
    - **Enums**: Veritabanı nesneleri için enumları içeren klasördür.
- **Infrastructure**: Altyapı katmanı, harici bileşenlerle ilgili bileşenleri içerir.
    - **Notification**: Harici bildirim bileşenlerini içeren klasördür.
    - **Payment**: Harici ödeme bileşenlerini içeren klasördür.
    - **InfrastructureExtensions**: Altyapı katmanı için extension metodları içeren sınıftır.
- **Notification.Consumer**: RabbitMQ üzerinden gelen bildirimleri dinleyen konsol uygulamasıdır.
- **Persistence**: Veritabanı katmanı, veritabanı işlemlerini yönetir.
    - **Contexts**: Veritabanı nesnelerinin DbContext sınıflarını içeren klasördür.
    - **EntityConfigurations**: Veritabanı nesnelerinin yapılandırma sınıflarını içeren klasördür.
    - **Migrations**: Veritabanı nesneleri için migration dosyalarını içeren klasördür.
    - **Repositories**: Veritabanı nesneleri için repository sınıflarını içeren klasördür.
    - **PersistenceExtensions**: Veritabanı katmanı için extension metodları içeren sınıftır.

Projenin testleri aşağıdaki bileşenlere ayrılmıştır:

- **Application.Tests**: Uygulama katmanı testleri.
    - **Extensions**: Uygulama katmanı testleri için extension metodları içeren klasördür.
    - **Mocks**: Uygulama katmanı testleri için mock bileşenleri içeren klasördür.
    - **Services**: Uygulama katmanı testleri için iş sınıflarını içeren klasördür.
- **Core.Tests**: Çekirdek katmanı testleri.
    - **Utilities**: Çekirdek katmanındaki yardımcı sınıfların testlerini içeren klasördür.

**🔒Varsayılan Admin Giriş Bilgileri :**

```json
{
  "email": "admin@system.com",
  "password": "1234"
}
```

## Postman Koleksiyonu

[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/23538386-2de3fb65-4479-4663-873a-fc9e291c9d1b?action=collection%2Ffork&source=rip_markdown&collection-url=entityId%3D23538386-2de3fb65-4479-4663-873a-fc9e291c9d1b%26entityType%3Dcollection%26workspaceId%3D81da7b17-d919-484f-81a7-a0ea4c8bd87a)

## Kurulum

### Repository'yi klonlayın

```
git clone https://github.com/P259-Simpra-NET-Bootcamp/final-BerkayMehmetSert.git
```

### Bağımlılıkları yükleyin

```
dotnet restore
```

### Veritabanını oluşturun (MsSQL)

```
dotnet ef database update
```

### Docker Compose ile RabbitMQ ve Redis servislerini çalıştırın

**⚠️Not:** Projede bildirimler varsayılan olarak RabbitMQ ile çalışmaktadır. Eğer RabbitMQ yerine Konsol uygulaması kullanmak isterseniz `PaymentService.cs` içerisindeki **NotificationType** değişkenini **NotificationType.Console** olarak değiştirmeniz yeterlidir.

**⚠️Not:** Proje varsayılan olarak Redis cache ile çalışmaktadır. Eğer Redis yerine InMemoryCache kullanmak isterseniz
`CartService.cs` ve `CouponService.cs` içerisindeki **CacheType** değişkenini **CacheType.Memory** olarak değiştirmeniz yeterlidir.

```
docker-compose up -d
```

### Projeyi çalıştırın

**⚠️Not:** Projeyi çalıştırırken **Notification.Consumer** projesininde çalıştığından emin olun.

```
dotnet run
```
