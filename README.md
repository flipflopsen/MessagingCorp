# MessagingCorp - A Transparent Messenger

**What is this about:**
  
This project focuses on implementing a Messenger Service with fully user-controllable, switchable, and cascading encryption options, encompassing both asymmetric and symmetric encryption methods. The primary goal is to create a platform for educational purposes, providing an environment to explore new technologies, discover novel concepts, and experience different facets of programming that may not be encountered in everyday work.

## Setup

1. **Install SurrealDB:**

   - **Install on Windows:**
     ```bash
     iwr https://windows.surrealdb.com -useb | iex
     ```
     or
     ```bash
     choco install surreal --pre
     ```
     or
     ```bash
     scoop install surreal
     ```

   - **Install on Linux:**
     ```bash
     curl -sSf https://install.surrealdb.com | sh
     ```

2. **Run SurrealDB:**
   ```bash
   surreal start -b 127.0.0.1:35167 -u corpadmin -p lulz123 --auth -l debug memory
   ```


## Open To-Do's

- **Lobby Management:**
  - Create lobbies, allow users to join them, and manage database objects.

- **Crypto and Key Management/Provider:**
  - The key provider should securely save and access user keys for crypto.
  - The crypto provider executes the encryption.

- **Actions:**
  - Start a new conversation (with another user ID).
  - Create new symmetric/asymmetric keys, IV, salt, etc., and make it configurable from the client side.

- **More Crypto:**
  - Implement a variety of asymmetric and symmetric algorithms, including:
    - **Symmetric:**
      - Serpent
      - Chacha_twenty
      - Others that could be fast and effective
    - **Asymmetric:**
      - RSA (Important!)
      - ECC
      - DSA
      - ECDSA
      - NTRUEncrypt (could get very hard)
      - McEliece
      - More post-quantum algorithms


## How-To's
### DI with Ninject
For in-depth information, check out their [Wiki](https://github.com/ninject/Ninject/wiki).

At the core, we utilize a Kernel, specifically the Kernel of `KernelLevel.Driver`. "IKernel" itself is a Ninject concept. A Kernel comprises "Modules," which can be defined like this:

```csharp
public class DatabaseModule : NinjectModule
{
    public override void Load()
    {
        this.Bind<IDatabaseAccess>().To<SurrealDatabaseAccess>();

        // use different implementations
        //this.Bind<IDatabaseAccess>().To<DatabaseAccessMock>();

        // maybe singletons?
        //this.Bind<IDatabaseAccess>().To<SurrealDatabaseAccess>().InSingletonScope();
    }
}
```

In this example, the commented-out line provides an alternate implementation of "IDatabaseAccess," enabling easy component switching. It's essential to update both the interface and other implementations when making changes.

To integrate this module into a Kernel, typically instantiated in MessagingCorp.Services.MessageCorpService.InitializeDiKernels(), we use the KernelLevel.Driver. Add the DatabaseModule like this:

```csharp
// Inside MessagingCorp.Services.MessageCorpService.InitializeDiKernels()
kernels[KernelLevel.Driver] = new StandardKernel(
    commonServiceModule,
    new CryptoModule(),
    new CachingModule(),
    new DatabaseModule(), // <- here is our newly created module.
    new AuthenticationModule(),
    new UserManagementModule()
);
//...
```

Finally, inject the module into a class, such as the `KeyProvider`, where we can use the `SurrealDatabaseAccess` implementation of `IDatabaseAccess` when needed.

#### Constructor Injection
When providing the `[Inject]` Attribute above the construtor, Ninject 'automagically' injects our binding, allowing us to use it freely

```csharp
public class KeyProvider : IKeyProvider
{
    private readonly IDatabaseAccess dbAccess;

    [Inject]
    public KeyProvider(IDatabaseAccess dbAccess) 
    { 
    	this.dbAccess = dbAccess
    }
}

```

#### Property Injection

**Important Note:** Property injection is typically considered bad practice. While there are cases where it might be reasonable, most of the time, it isn't recommended.

Alternatively, property injection can be achieved by marking the property with the `[Inject]` attribute. Here's an example using the `KeyProvider` class:

```csharp
public class KeyProvider : IKeyProvider
{
    [Inject]
    public IDatabaseAccess DbAccess { get; set; }

    // Other properties and methods...
}
```

## Packages
### In Use
- **Ninject** for DI
- **Bouncycastle** for Crypto
- **Serilog** for Logging
- **SurrealDB.NET** for Database Access to SurrealDB
- **NUnit** for Testing
- **FluentValidation** for building nice Validators

### Interesting
- [AutoMapper](https://automapper.org/) for object convertion
- [CacheManager](https://github.com/MichaCo/CacheManager) for advanced caching scenarios
- [Ocelot](https://github.com/ThreeMammals/Ocelot) If we want to change the Architecture to Microservice, then this could be wild
- [Dapper](https://github.com/DapperLib/Dapper) In case of other DB than Surreal
- [FluentValidation](https://github.com/FluentValidation/FluentValidation) In case of need of validators
- [Lamar](https://github.com/jasperfx/lamar) for fast IoC (like StructureMap), in case of ninject being too lame or idk.