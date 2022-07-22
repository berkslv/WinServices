# Win Services

Bu proje windows üzerinde koşan servisleri kontrol etmek için geliştirilmiştir. 

### BUG

Windows servisleri üzerinde kontrol sağlamak için administirator yetkileri gerekiyor. Run as admin olarak çalıştırmak gerekir. Bu işlemi linux üzerinde sudo ile benzetebiliriz, fakat windows üzerinde sudo gibi bir komut olmadığı için proje seviyesinde configurasyon yapmak gerekiyor, araştırmaların sonucu bu configurasyonu başaramadığım için bu kısımda yarıda bıraktım.


```powershell
cd .\WSConsole\

dotnet run -- --help  # Kullanılabilecek komutları gösterir.
dotnet run -- list  # Aktif ve pasif tüm servisleri döner.
dotnet run -- detail --id MSSQLSERVER  # id ile verilen servis üzerinde mümkün olan operasyonları gösterir.
dotnet run -- start --id MSSQLSERVER # id ile verilen servisi başlatır.
dotnet run -- stop --id MSSQLSERVER # id ile verilen servisi durdurur. 
```

```powershell
# Uygulamayı exe formatına derlemek için
dotnet publish -o ../publish
# exe dosyasını çalıştırmak için
cd ..\publish
.\WSConsole.exe list

````