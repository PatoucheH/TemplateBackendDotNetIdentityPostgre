# .NET Backend Template - Installation et Utilisation

Ce document explique comment installer et utiliser le template `.NET Backend with Identity + PostgreSQL`.

## Installation du Template

### Depuis un Dossier Local

```bash
# Naviguer vers le dossier contenant le template
cd /chemin/vers/DotNetBackendTemplate

# Installer le template
dotnet new install .

# Vérifier que le template est installé
dotnet new list | grep backend-identity-pg
```

### Depuis un Package NuGet (Publication)

Si vous souhaitez publier le template sur NuGet :

1. **Créer le fichier .nuspec** (à la racine du template) :

```xml
<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd">
  <metadata>
    <id>MyCompany.DotNetBackendTemplate</id>
    <version>1.0.0</version>
    <title>.NET Backend with Identity + PostgreSQL</title>
    <authors>VotreNom</authors>
    <description>Clean Architecture backend template with ASP.NET Core Identity, JWT, and PostgreSQL</description>
    <packageTypes>
      <packageType name="Template" />
    </packageTypes>
    <tags>dotnet-new template backend api identity postgresql clean-architecture</tags>
    <license type="expression">MIT</license>
    <repository type="git" url="https://github.com/votre-repo/template" />
  </metadata>
  <files>
    <file src="**/*" exclude="**\bin\**;**\obj\**;**\.git\**;**\.vs\**;*.nuspec;*.nupkg" />
  </files>
</package>
```

2. **Créer le package** :

```bash
nuget pack MyCompany.DotNetBackendTemplate.nuspec -NoDefaultExcludes
```

3. **Publier sur NuGet** :

```bash
dotnet nuget push MyCompany.DotNetBackendTemplate.1.0.0.nupkg --api-key VOTRE_CLE --source https://api.nuget.org/v3/index.json
```

4. **Installer depuis NuGet** :

```bash
dotnet new install MyCompany.DotNetBackendTemplate
```

## Utilisation du Template

### Création Simple

```bash
# Créer un nouveau projet avec le nom "MonProjet"
dotnet new backend-identity-pg -n MonProjet

# Ou dans un dossier spécifique
dotnet new backend-identity-pg -n MonProjet -o ./src/MonProjet
```

### Création avec Options Personnalisées

```bash
# Voir toutes les options disponibles
dotnet new backend-identity-pg --help

# Créer avec des options personnalisées
dotnet new backend-identity-pg -n MonApi \
  --DatabaseName "monapi_db" \
  --DatabaseNameDev "monapi_dev" \
  --PostgresPassword "MonMotDePasse123!" \
  --JwtIssuer "MonApi.Production" \
  --JwtAudience "MonApi.Clients" \
  --AuthorName "Jean Dupont" \
  --AuthorEmail "jean.dupont@example.com"
```

### Options Disponibles

| Option | Description | Valeur par défaut |
|--------|-------------|-------------------|
| `-n`, `--name` | Nom du projet (namespace) | `MyApp` |
| `--Framework` | Framework cible (net10.0, net9.0, net8.0) | `net10.0` |
| `--DatabaseName` | Nom de la base de données PostgreSQL | `myapp_db` |
| `--DatabaseNameDev` | Nom de la base de données de développement | `myapp_dev` |
| `--PostgresUser` | Utilisateur PostgreSQL | `postgres` |
| `--PostgresPassword` | Mot de passe PostgreSQL | `your_password_here` |
| `--PostgresPasswordDev` | Mot de passe PostgreSQL (dev) | `your_dev_password` |
| `--JwtSecretKey` | Clé secrète JWT (min 32 caractères) | Clé par défaut |
| `--JwtIssuer` | Émetteur du token JWT | `MyApp.Api` |
| `--JwtAudience` | Audience du token JWT | `MyApp.Client` |
| `--AuthorName` | Nom de l'auteur (Swagger) | `Your Name` |
| `--AuthorEmail` | Email de l'auteur (Swagger) | `contact@example.com` |

## Structure Générée

Après création, vous obtiendrez :

```
MonProjet/
├── MonProjet.sln
├── MonProjet.Api/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   └── UsersController.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
├── MonProjet.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Services/
│   └── DependencyInjection.cs
├── MonProjet.Domain/
│   ├── Entities/
│   └── Interfaces/
├── MonProjet.Infrastructure/
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   ├── Configurations/
│   │   └── Migrations/
│   ├── Repositories/
│   └── DependencyInjection.cs
├── .gitignore
└── README.md
```

## Étapes Post-Création

### 1. Configurer PostgreSQL

Modifiez `appsettings.Development.json` avec vos credentials PostgreSQL :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=votre_db;Username=postgres;Password=votre_mot_de_passe"
  }
}
```

### 2. Configurer JWT (Important!)

Changez la clé secrète dans `appsettings.json` :

```json
{
  "JwtSettings": {
    "SecretKey": "VOTRE_CLE_TRES_SECURISEE_AU_MOINS_32_CARACTERES!"
  }
}
```

### 3. Appliquer les Migrations

```bash
cd MonProjet
dotnet ef database update --project MonProjet.Infrastructure --startup-project MonProjet.Api
```

### 4. Lancer l'Application

```bash
dotnet run --project MonProjet.Api
```

L'API sera disponible sur :
- **Swagger UI** : https://localhost:5001
- **API** : https://localhost:5001/api

## Désinstallation du Template

```bash
# Désinstaller le template local
dotnet new uninstall /chemin/vers/DotNetBackendTemplate

# Ou désinstaller le package NuGet
dotnet new uninstall MyCompany.DotNetBackendTemplate
```

## Lister les Templates Installés

```bash
# Voir tous les templates installés
dotnet new list

# Filtrer par mot-clé
dotnet new list backend
```

## Mise à Jour du Template

```bash
# Désinstaller l'ancienne version
dotnet new uninstall MyCompany.DotNetBackendTemplate

# Installer la nouvelle version
dotnet new install MyCompany.DotNetBackendTemplate::2.0.0
```

## Dépannage

### Le template n'apparaît pas après installation

```bash
# Vider le cache des templates
dotnet new --debug:reinit

# Réinstaller
dotnet new install /chemin/vers/template
```

### Erreur "Template not found"

Vérifiez que le dossier contient bien `.template.config/template.json` :

```bash
ls -la /chemin/vers/template/.template.config/
```

### Les symboles ne sont pas remplacés

Vérifiez que `sourceName` dans `template.json` correspond exactement au nom utilisé dans les fichiers (sensible à la casse).

## Personnalisation du Template

Pour modifier le template :

1. Modifiez les fichiers sources selon vos besoins
2. Mettez à jour `.template.config/template.json` si nécessaire
3. Réinstallez le template :

```bash
dotnet new uninstall /chemin/vers/template
dotnet new install /chemin/vers/template
```

## Contribution

Pour contribuer au template :

1. Fork le repository
2. Créez une branche pour votre fonctionnalité
3. Faites vos modifications
4. Testez en installant localement
5. Créez une Pull Request

## Support

- **Issues** : https://github.com/votre-repo/issues
- **Documentation** : https://github.com/votre-repo/wiki
