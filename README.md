# CRM Sistemi

Bu proje, müşteri ilişkileri yönetimi (CRM) için geliştirilmiş, .NET Core tabanlı bir web uygulamasıdır. Clean Architecture prensiplerine göre tasarlanmış backend ve React ile oluşturulmuş frontend içerir.

## Özellikler

### Backend (.NET Core)
- **Framework**: .NET Core 8.0
- **Mimari**: Clean Architecture (Domain, Application, Persistence, API katmanları)
- **Veritabanı**: PostgreSQL
- **Kimlik Doğrulama**: JWT (JSON Web Tokens)
- **Güvenlik**: BCrypt şifre hash'leme

### API Özellikleri
- RESTful API
- Kullanıcı kimlik doğrulama ve yetkilendirme
- Müşteri verileri için CRUD işlemleri
- Bölgeye göre müşteri filtreleme
- Hata yönetimi ve loglama

### Frontend (React.js)
- Modern UI (Material UI)
- Responsive tasarım
- JWT tabanlı kimlik doğrulama
- Müşteri verileri için CRUD işlemleri
- Filtreleme özellikleri

## Kurulum

### Önkoşullar
- .NET Core 8.0 SDK
- Node.js ve npm
- PostgreSQL
- Docker (isteğe bağlı)

### Backend Kurulumu

1. Veritabanını oluşturun:
```bash
# PostgreSQL Docker Container
docker run --name postgres-crm -e POSTGRES_PASSWORD=postgres -e POSTGRES_USER=postgres -e POSTGRES_DB=CRMDB -p 5433:5432 -d postgres:latest
```

2. Projeyi klonlayın ve API'yi çalıştırın:
```bash
# API Projesini çalıştır
cd CRM.API
dotnet run
```

3. Veritabanını başlatın:
```
POST https://localhost:5170/api/seed
```

### Frontend Kurulumu
```bash
# Client uygulamasını çalıştır
cd ClientApp
npm install
npm start
```

## Proje Yapısı

```
CRM/
├── CRM.API/               # API Katmanı
│   ├── Controllers/       # API Endpoint'leri
│   ├── Middleware/        # Hata yakalama middleware'i
│   └── Program.cs         # Uygulama yapılandırması
│
├── CRM.Application/       # Uygulama Katmanı
│   ├── DTOs/              # Veri Transfer Nesneleri
│   ├── Features/          # Özellik ve Komutlar
│   └── Mapping/           # Nesne Eşleştirme
│
├── CRM.Domain/            # Domain Katmanı
│   ├── Common/            # Ortak Sınıflar
│   ├── Entities/          # Domain Varlıkları
│   └── Interfaces/        # Repository Arayüzleri
│
├── CRM.Persistence/       # Persistence Katmanı
│   ├── Repositories/      # Repository Uygulamaları
│   └── ApplicationDbContext.cs # Veritabanı Bağlamı
│
└── ClientApp/             # Frontend React Uygulaması
    ├── public/            # Statik dosyalar
    └── src/               # Kaynak kod
        ├── components/    # Bileşenler
        ├── contexts/      # Context API'leri
        └── services/      # API Servisleri
```

## Kullanım

### Admin Kullanıcısı
- **Kullanıcı adı**: admin
- **Şifre**: admin123

### Normal Kullanıcı
- **Kullanıcı adı**: user
- **Şifre**: user123

## API Endpoint'leri

### Kimlik Doğrulama
- `POST /api/auth/login` - Kullanıcı girişi

### Veritabanı
- `POST /api/seed` - Örnek verileri yükle

### Müşteriler
- `GET /api/customers` - Tüm müşterileri getir
- `GET /api/customers/{id}` - ID'ye göre müşteri getir
- `GET /api/customers/region/{region}` - Bölgeye göre müşterileri filtrele
- `POST /api/customers` - Yeni müşteri ekle
- `PUT /api/customers/{id}` - Müşteriyi güncelle
- `DELETE /api/customers/{id}` - Müşteriyi sil

## Teknolojiler ve Kütüphaneler

### Backend
- ASP.NET Core 8.0
- Entity Framework Core
- MediatR (CQRS)
- Npgsql (PostgreSQL provider)
- JWT Bearer Authentication
- BCrypt.Net-Next

### Frontend
- React.js
- Material UI
- Axios
- React Router
- JWT-Decode

## Güvenlik Özellikleri

- JWT tabanlı kimlik doğrulama
- Rol tabanlı yetkilendirme
- BCrypt şifre hash'leme
- CORS yapılandırması
- HTTPS yönlendirmesi

## Mimari Yaklaşım

Bu proje, Clean Architecture prensiplerini takip eder:

1. **Domain Katmanı**: İş kuralları ve varlıkları içerir
2. **Application Katmanı**: Kullanım senaryoları ve uygulama mantığı
3. **Persistence Katmanı**: Veritabanı erişimi ve veri kalıcılığı
4. **API Katmanı**: Dış dünya ile iletişim

Bu yaklaşım, bağımlılıkların içeriden dışarıya doğru akmasını sağlar, böylece çekirdek iş mantığının dış katmanlara olan bağımlılığı azaltılır. 