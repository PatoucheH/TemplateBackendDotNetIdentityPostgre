# MyTemplate - Backend .NET Template

Template backend réutilisable avec Clean Architecture, ASP.NET Core Identity et PostgreSQL.

## Structure du Projet

```
MyTemplate/
├── MyTemplate.Api/                 # Couche présentation (Web API)
│   ├── Controllers/               # Contrôleurs API
│   ├── Program.cs                 # Point d'entrée et configuration
│   └── appsettings.json          # Configuration de l'application
│
├── MyTemplate.Application/         # Couche application (logique métier)
│   ├── DTOs/                      # Data Transfer Objects
│   │   ├── Auth/                  # DTOs d'authentification
│   │   └── Common/                # DTOs communs (ApiResponse)
│   ├── Interfaces/                # Interfaces des services
│   ├── Services/                  # Implémentations des services
│   └── DependencyInjection.cs    # Configuration DI
│
├── MyTemplate.Domain/              # Couche domaine (cœur métier)
│   ├── Entities/                  # Entités du domaine
│   │   ├── ApplicationUser.cs    # Utilisateur personnalisé
│   │   └── BaseEntity.cs         # Classe de base + exemples
│   └── Interfaces/               # Interfaces repository
│       ├── IRepository.cs        # Repository générique
│       ├── IUserRepository.cs    # Repository utilisateur
│       └── IUnitOfWork.cs        # Unit of Work
│
├── MyTemplate.Infrastructure/      # Couche infrastructure
│   ├── Data/
│   │   ├── ApplicationDbContext.cs    # DbContext avec Identity
│   │   ├── Configurations/            # Configurations Fluent API
│   │   └── Migrations/                # Migrations EF Core
│   ├── Repositories/              # Implémentations repositories
│   └── DependencyInjection.cs    # Configuration DI
│
└── MyTemplate.sln                  # Solution Visual Studio
```

## Technologies Utilisées

- **.NET 10** - Framework
- **ASP.NET Core Identity** - Authentification et autorisation
- **Entity Framework Core 10** - ORM
- **PostgreSQL (Npgsql)** - Base de données
- **JWT (JSON Web Tokens)** - Authentification API
- **Swashbuckle** - Documentation OpenAPI/Swagger

## Prérequis

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) (version 13+)
- Un IDE (Visual Studio 2022, VS Code, Rider)

## Configuration Rapide

### 1. Cloner et Renommer

```bash
# Cloner le template
git clone <repository-url> MonProjet
cd MonProjet

# Renommer le namespace (optionnel)
# Utilisez la fonction "Renommer" de votre IDE
# ou faites un rechercher/remplacer de "MyTemplate" par "MonProjet"
```

### 2. Configurer PostgreSQL

Modifiez `appsettings.Development.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=votre_db;Username=postgres;Password=votre_password"
  }
}
```

### 3. Configurer JWT (Important!)

Dans `appsettings.json`, changez la clé secrète :

```json
{
  "JwtSettings": {
    "SecretKey": "VOTRE_CLE_SECRETE_TRES_LONGUE_ET_SECURISEE_AU_MOINS_32_CARACTERES!"
  }
}
```

**En production**, utilisez des variables d'environnement ou Azure Key Vault.

### 4. Appliquer les Migrations

```bash
# Créer la base de données et appliquer les migrations
dotnet ef database update --project MyTemplate.Infrastructure --startup-project MyTemplate.Api
```

### 5. Lancer l'Application

```bash
cd MyTemplate.Api
dotnet run
```

L'API sera disponible sur :
- **Swagger UI** : https://localhost:5001 (ou http://localhost:5000)
- **API** : https://localhost:5001/api

## Commandes Utiles

### Migrations Entity Framework

```bash
# Ajouter une migration
dotnet ef migrations add NomMigration --project MyTemplate.Infrastructure --startup-project MyTemplate.Api --output-dir Data/Migrations

# Appliquer les migrations
dotnet ef database update --project MyTemplate.Infrastructure --startup-project MyTemplate.Api

# Annuler la dernière migration (non appliquée)
dotnet ef migrations remove --project MyTemplate.Infrastructure --startup-project MyTemplate.Api

# Générer un script SQL
dotnet ef migrations script --project MyTemplate.Infrastructure --startup-project MyTemplate.Api -o migration.sql
```

### Build et Test

```bash
# Build la solution
dotnet build

# Restaurer les packages
dotnet restore

# Lancer en mode watch (hot reload)
cd MyTemplate.Api
dotnet watch run
```

## Architecture Clean Architecture

### Flux de Dépendances

```
Api → Application → Domain
 ↓         ↓
Infrastructure ←────┘
```

- **Domain** : Pas de dépendances externes, contient les entités et interfaces
- **Application** : Dépend du Domain, contient la logique métier
- **Infrastructure** : Dépend du Domain, implémente les interfaces (DB, services externes)
- **Api** : Dépend d'Application et Infrastructure, configure l'application

### Principes

1. **Séparation des préoccupations** : Chaque couche a une responsabilité unique
2. **Inversion des dépendances** : Les couches internes définissent les interfaces
3. **Testabilité** : Les services sont facilement mockables via les interfaces

## Personnalisation

### Ajouter une Nouvelle Entité

1. **Créer l'entité** dans `MyTemplate.Domain/Entities/` :

```csharp
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

2. **Ajouter le DbSet** dans `ApplicationDbContext.cs` :

```csharp
public DbSet<Product> Products => Set<Product>();
```

3. **Créer la configuration** dans `Data/Configurations/` :

```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2);
    }
}
```

4. **Ajouter une migration** :

```bash
dotnet ef migrations add AddProduct --project MyTemplate.Infrastructure --startup-project MyTemplate.Api --output-dir Data/Migrations
```

### Ajouter un Nouveau Service

1. **Créer l'interface** dans `MyTemplate.Application/Interfaces/` :

```csharp
public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<ProductDto> CreateAsync(CreateProductDto dto);
}
```

2. **Créer l'implémentation** dans `MyTemplate.Application/Services/` :

```csharp
public class ProductService : IProductService
{
    private readonly IRepository<Product> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IRepository<Product> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    // Implémenter les méthodes...
}
```

3. **Enregistrer dans le DI** (`Application/DependencyInjection.cs`) :

```csharp
services.AddScoped<IProductService, ProductService>();
```

## Endpoints API Disponibles

### Authentification (`/api/auth`)

| Méthode | Endpoint | Description | Auth |
|---------|----------|-------------|------|
| POST | `/register` | Inscription | Non |
| POST | `/login` | Connexion | Non |
| POST | `/refresh` | Rafraîchir le token | Non |
| POST | `/logout` | Déconnexion | Oui |
| GET | `/me` | Utilisateur courant | Oui |

### Utilisateurs (`/api/users`)

| Méthode | Endpoint | Description | Auth |
|---------|----------|-------------|------|
| GET | `/` | Liste des utilisateurs | Admin |
| GET | `/{id}` | Détail utilisateur | Oui |
| PUT | `/me` | Modifier son profil | Oui |
| PUT | `/me/password` | Changer son mot de passe | Oui |
| DELETE | `/{id}` | Désactiver un utilisateur | Admin |
| POST | `/{id}/roles/{role}` | Ajouter un rôle | Admin |
| DELETE | `/{id}/roles/{role}` | Retirer un rôle | Admin |

## Configuration des Options

### Identity (mot de passe, lockout)

Dans `Infrastructure/DependencyInjection.cs` :

```csharp
services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Règles de mot de passe
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;

    // Lockout
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

    // Utilisateur
    options.User.RequireUniqueEmail = true;
});
```

### CORS

Dans `Program.cs`, modifiez les origines autorisées :

```csharp
options.AddPolicy("Production", policy =>
{
    policy.WithOrigins("https://votre-frontend.com")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
});
```

### JWT

Dans `appsettings.json` :

```json
{
  "JwtSettings": {
    "ExpirationInMinutes": "60",
    "RememberMeExpirationInDays": "7"
  }
}
```

## Bonnes Pratiques

1. **Ne jamais commiter de secrets** - Utilisez `appsettings.Development.json` (gitignore) ou des variables d'environnement
2. **Toujours valider les entrées** - Les DTOs utilisent les Data Annotations
3. **Utiliser le soft delete** - `BaseEntity.IsDeleted` pour la traçabilité
4. **Logger les actions importantes** - `ILogger` est injecté dans les services
5. **Transactions** - Utilisez `IUnitOfWork` pour les opérations multiples

## Dépannage

### Erreur de connexion PostgreSQL

Vérifiez que :
- PostgreSQL est démarré
- Le port 5432 est accessible
- Les credentials sont corrects
- La base de données existe

### Erreur JWT "SecretKey not configured"

La clé JWT doit avoir au moins 32 caractères. Vérifiez `appsettings.json`.

### Migrations échouent

```bash
# Vérifier l'état des migrations
dotnet ef migrations list --project MyTemplate.Infrastructure --startup-project MyTemplate.Api

# Réinitialiser (attention: perte de données!)
dotnet ef database drop --project MyTemplate.Infrastructure --startup-project MyTemplate.Api
dotnet ef database update --project MyTemplate.Infrastructure --startup-project MyTemplate.Api
```

## Licence

Ce template est libre d'utilisation pour vos projets.
