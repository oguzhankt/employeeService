# Employee API ve AuthGuard .NET 8 

Proje 3 controller ve 2 farklı veritabanından oluşmakta.
- Employee controllerı üzerinde authorizasyon gerektiren basit CRUD yapısı bulunmakta.
- User controllerı kayıt oluşturma ve authorize olunması halinde giriş yapılan user bilgisini göstermekte.
- AuthGuard controllerı ise login ve token işlemlerinden sorumlu

Kullanılan framework ve teknolojiler:
- MVC yapısı var
- Persistence için SQLite ve EF Core Code-First olarak kurgulandı.
- User yapısı için Microsoft.AspNetCore.Identity kullanıldı
- Authorizasyon için jwt bearer token kullanıldı
- SwaggerUI üzerinden bütün işlemler yapılabilir

Sistem sırayla şu şekilde test edilebilir:
1. /user/create endpointi ile kullanıcı oluşturur. Burda şifrenizin en az 1 rakam **ve** büyük harf içerdiğinden emin olun
- Ya da halihazırda veritabanında var olan:
> email: test@test.com
> password: Test123.
- test kullanıcısını kullanabilirsiniz.

2. Login olmamız ve access token yaratmamız gerekiyor.
- Bunun için /authguard/create endpointine yukardaki test userı veya kendi yarattığınız kullanıcı bilgileriyle istek atılması gerekiyor. Örnek curl:
> curl -X 'POST' \
'http://localhost:5119/AuthGuard/createToken' \
-H 'accept: */*' \
-H 'Content-Type: application/json' \
-d '{
"email": "test@test.com",
"password": "Test123"
}'
- Örnek response:
> {
"accessToken": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjZhZWY5ZjYzLTA1ZTItNDEwNi04NTdhLTI5ZGQxODA5YTgwMCIsImVtYWlsIjoidGVzdEB0ZXN0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ0ZXN0IiwianRpIjoiNDQ4NGI5MTEtN2ZhMC00MjlhLTk0OTMtYjFmNWQ3NzUyZGUwIiwiYXVkIjoid3d3LmF1dGhzZXJ2ZXIuY29tIiwibmJmIjoxNzMxMzgyOTAxLCJleHAiOjE3MzEzODMyMDEsImlzcyI6Ind3dy5hdXRoc2VydmVyLmNvbSJ9.HwvZT3YpAMaDyzLFwcKx15X9SHWKYsbDEdURfpNSeMU",
"accessTokenExpiration": "2024-11-12T06:46:41.276179+03:00",
"refreshToken": "qHS9hXljGlZOKblt8LhlVEi1UBEhPuDqO9QTLIanem8=",
"refreshTokenExpiration": "2024-11-12T16:41:41.29018+03:00"
}
3. Burda bize dönen accessTokenı authorizasyon gereken bütün endpointlerde kullanarak istekleri atabiliriz. Örnek curl:
> curl -X 'GET' \
'http://localhost:5119/User' \
-H 'accept: */*' \
-H 'Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjZhZWY5ZjYzLTA1ZTItNDEwNi04NTdhLTI5ZGQxODA5YTgwMCIsImVtYWlsIjoidGVzdEB0ZXN0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ0ZXN0IiwianRpIjoiNDQ4NGI5MTEtN2ZhMC00MjlhLTk0OTMtYjFmNWQ3NzUyZGUwIiwiYXVkIjoid3d3LmF1dGhzZXJ2ZXIuY29tIiwibmJmIjoxNzMxMzgyOTAxLCJleHAiOjE3MzEzODMyMDEsImlzcyI6Ind3dy5hdXRoc2VydmVyLmNvbSJ9.HwvZT3YpAMaDyzLFwcKx15X9SHWKYsbDEdURfpNSeMU'
- accessToken 5 dakika, refresh token ise 10 saat içerisinde deaktive olur.
- /authguard/createbyrefreshtoken endpointiyle refreshtoken kullanılarak login olmaya gerek kalmadan yeni bir access token yaratılabilir.
- Logout işlemi için /authguard/revoketoken kullanılması yeterli olucaktır.