## Nuget-Packages
	Serilog
	Serilog.Sinks.Files z.B.
	Serilog.Sinks.Console bla bla

---
## Standard Serilog-Logger
Normalerweise kannst du mit Serilog z.B. so einen Logger erstellen
```csharp
Log.Logger = new LoggerConfiguration()
            .WriteTo.Console() // Log to the console
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day) // Log to a file with daily log rotation
            .CreateLogger();


Log.Information("Hello, Serilog!");
```

Der Typ ` Log.Logger ` ist dann ja eigentlich `ILogger` von Serilog, möge man meinen.
Jedoch:
	- Logger wird mit nem Builder-Pattern gebaut wie du sehen kannst.
	- Wichtig ist hier, dass du auf `Log.**Logger**` zugreifst, also `Log` ist die statische Klasse und `Logger` ist das Objekt. `Logger` ist aber vom Typen `LoggerConfiguration`. (Sehen wir ja an der Erstellung des Loggers)

---
## Was ist jetzt eigentlich das Ziel?
Das Ziel ist es, dass man in einer willkürlichen Klasse in einer Zeile, mit schöner Syntax, einen Logger erstellen kann, welcher sich auf die Klasse bezieht im Output, damit wir halt nice Logs haben.
Ziel könnte z.B. so aussehen
```csharp
public class SomeClass
{
	private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<SomeClassOderHaltIrgendeineKlasseAlsKontext>(
	"AusgabePfadDerOptionalIst.log", 
	bool: ConsoleUndFileLog?, 
	LogEventLevel.IrgendeinLogLovel);
}
```
```
```

---
### Was kann man hier erkennen?
Im Vergleich zu Beispiel 1 greifen wir in beiden Fällen auf `Log.Logger` zu, um einen Logger zu erstellen!
In Beispiel 1 assignen wir eine Config zu der Serilog Singleton (oder was das auch ist) `Log`-Klasse.
In Beispiel 2 Erstellen wir einen `ILogger`, indem wir das Serilog Singleton (oder so) Ding kopieren, mit einem veränderten `Logger` Object, welches ja unsere Konfiguration ist!

---
## Logger customizen
Dementsprechend kannst du Methoden machen zur Erweiterung, welche im Endeffekt eine solche `LoggerConfiguration` zurück geben müssen.

---
### Beispiel "ToConsoleAndFile"
Ziel ist es also eigentlich den identischen Logger zu dem wie im obigen Beispiel zu erstellen, jedoch erstmal nur die LoggerConfiguration.
Also:

```csharp
public static **LoggerConfiguration** ToConsoleAndFile(this LoggerConfiguration loggerConfig, string logFilePath)
        {
            return loggerConfig.**WriteTo.File**(
                    new RenderedCompactJsonFormatter(),
                    logFilePath,
                    rollingInterval: RollingInterval.Day)
                .**WriteTo.Console**();
        }
```

---
Man kann auch in dieser Config das Log-Level einstellen, machen also auch eine Methode dafür:

```csharp

public static LoggerConfiguration WithLogLevel(this LoggerConfiguration loggerConfig, LogEventLevel logLevel)
{
            return loggerConfig.MinimumLevel.Is(logLevel);
}
```
---
## Zusammenführen
Ziel ist also die Erstellung und Instanziierung eines `ILogger`. 
Wir wollen die Möglichkeit haben bei Bedarf die Logs als Datei zu speichern, in der Konsole zu zeigen, oder beides gleichzeitig.
Und ein Log-Level soll angegeben werden können.

Führen wir die obigen Methoden zusammen, kann dies in der Art so aussehen:
```csharp
public static ILogger ForContextWithConfig<T>(
            this ILogger logger,
            string? logFilePath = null,
            bool fileAndConsole = false,
            LogEventLevel logLevel = LogEventLevel.Information)
            
{
	// Erstelle neue LoggerConfiguration
    var loggerConfig = new LoggerConfiguration();

	// Je nach Input nutze entsprechende Methoden
    if (fileAndConsole && !string.IsNullOrEmpty(logFilePath))
	    loggerConfig = loggerConfig.ToConsoleAndFile(logFilePath);
    else
        loggerConfig = string.IsNullOrEmpty(logFilePath) 
        ? loggerConfig.ToConsole() 
        : loggerConfig.ToFile(logFilePath);

	// Setze Log-Level
    loggerConfig = loggerConfig.WithLogLevel(logLevel);

	// Returne einen aus der LoggerConfiguration erstellen Logger, für den Kontext T, wobei T in den meisten Fällen der Caller der Methode ist
    return loggerConfig.CreateLogger().ForContext<T>();
}
```

Somit können wir uns also easy Logger erstellen, wie z.B. so:
```csharp
public class MessageCorpService
{
	//...
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpService>("./Logs/MessageCorpService.log", true, LogEventLevel.Debug);
    
    //...
}
```
