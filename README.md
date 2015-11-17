# Projekt treningowy do warsztatu "Wykorzystanie usług Azure do budowania skalowalnych systemów" 

Warsztat dla: [Future Processing](https://www.future-processing.pl/)

Warsztat w wersji 7h

## Instrukcja

### Zadanie 1 - prosta aplikacja na Azure

- Stwórz dwie usługi: Azure Web App oraz Azure SQL Database.
- Stwórz fork tego repozytorium na własne konto GitHub.
- Uruchom aplikację z tego repozytorium, wykorzystując funkcję Continuous Deployment, usługi Web App.
- Skonfiguruj Web App, aby korzystała ze stworzonej SQL Database (nadpisz connection string w ustawieniach Web App).

Kryteria akceptacji:
- Potrafisz uruchomić aplikację i dodać nowy projekt do systemu.

### Zadanie 2 - storage


``` git 
git checkout ex-2-start-storage 

```

- Załóż usługę Storage Account.
- Przy pomocy [Azure Storage Explorer](https://azurestorageexplorer.codeplex.com/) dodaj dowolny plik i pobrać go z portalu Azure.
- W repozytorium przełączyć się na branch: 
- Uzupełnij implantację klasy FilesStorageService, tak aby zapisywała ona pliki w Azure Storage i zwracała adres nowego bloba (skorzystaj z podpowiedzi w kodzie)

Kryteria akceptacji:
- Potrafisz dodać nowy wpis ze zdjęciem do dziennika projektu oraz wyświetlić to zdjęcie na liście wpisów.

### Zadanie 3 - przetwarzanie w tle

``` git 
git checkout ex-3-start-background-processing

```
<<<<<<< HEAD
Część pierwsza
=======
>>>>>>> ex-3-start-background-processing

- Stwórz usługę Service Bus Namespace
- Zaimplementuj komunikację pomiędzy Web App a Web Job przy pomocy Service Bus. 
	- Uzupełnij implementację w miejscach wskazanych komentarzem "TODO ex3:"
    - Uzupełnij brakujące wartości w konfiguracji
- Przy pracy nad tym zadaniem przydatne może się okazać kożystanie z Web Job Dashboard. Poniższy artykuł pokazuje jak go uruchomić:
    - http://blogs.msdn.com/b/jmstall/archive/2014/01/27/getting-a-dashboard-for-local-development-with-the-webjobs-sdk.aspx
    - Aktualizacja do artykułu: zamiast klucza "AzureJobsRuntime", należy odpowiednio ustawić "AzureWebJobsDashboard" i "AzureWebJobsStorage".
    
Kryteria akceptacji:
- Zdjęcia dodawane przez aplikację, są zmniejszane przez Web Job i na liście wpisów wyświetlane są ich miniatury.

Część druga:
- Skonfiguruj aplikację konsolową PictureOptimizer aby została automatycznie wgrana jako Web Job

Pełne rozwiązanie:
``` git 
git checkout ex-3-extra-background-processing-as-web-job

```


### Zadanie 4 - application insights

- Skonfiguruj aplikację, aby wysyłała dane telemetryczne do usługi Application Insights

Pełne rozwiązanie:

``` git 
git checkout ex-4-finish-application-insights

```

### Zadanie 5 - skalowanie

``` git 
git checkout ex-5-scaling

```

- Uruchom powyższa wersję aplikacji w Azure
- Skonfiguruj Web App ręcznie, aby obsługiwała ruch na więcej niż jednej instancji. Upewnij się, że requesty obsługiwane są przez różne instancje.
- Extra: Skonfiguruj w Web App reguły autoskalowania i doprowadź do obciążenia systemu, tak, aby doprowadzić do skalowania
    - Stwórz funkcę obsługi wiadomości dla nowej kolejki
    - Zaimplementuj w niej kod mocno obciążający procesor np.: http://stackoverflow.com/questions/13001578/i-need-a-slow-c-sharp-function)
    - Wykorzystaj Service Bus Explorer (https://code.msdn.microsoft.com/windowsapps/Service-Bus-Explorer-f2abca5a) aby dodać ko kolejki dużo wiadomości i mocno obciążyć WebApp.


### Zadanie 6 - Azure Resource Manager

``` git 
git checkout ex-6-start-arm

```

- Zrób deployment Resource Grupy wykorzystując template file oraz komendy Powershell w katalogu AzureResourceManager.
- Rozwiń template file tak aby deployowane środowisko uruchamiało aplikację działającą analogicznie do stworzonej wcześniej przez portal.
    - Skonfiguruj ustawienia firewall, aby zezwalał na połączenia od innych usług Azure.
    - Skonfiguruj automatyczny deployment projektu z kodu na GitHub'ie do usługi Web App.
    - Skonfiguruj connection string w Web App, aby ta korzystała ze stworzonej bazy danych.
    - Zadanie z gwiazdką: skonfiguruj connection string do stworzonego storage accout.
- Przydatne linki 
    - https://resources.azure.com/
    - https://github.com/Azure/azure-quickstart-templates/blob/master/201-web-app-github-deploy/azuredeploy.json
    - https://azure.microsoft.com/pl-pl/documentation/articles/resource-group-authoring-templates/
    - https://azure.microsoft.com/pl-pl/documentation/articles/resource-group-template-functions/ 

