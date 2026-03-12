# API REST de Usuarios (ASP.NET Core + EF Core)

## 📋 Tabla de contenidos

* [Características](#-características)
* [Arquitectura del proyecto](#-arquitectura-del-proyecto)
* [Estructura real del proyecto](#-estructura-real-del-proyecto)
* [Principios SOLID aplicados](#-principios-solid-aplicados)
* [Requisitos](#-requisitos)
* [Ejecución](#-ejecución)

## ✨ Características

- API REST para gestión de usuarios.
- Autenticación con JSON Web Tokens (JWT).
- Hashing seguro de contraseñas con **bcrypt** (workFactor: 12).
- Refresh token para renovar sesión sin relogin (cookie `HttpOnly`).
- Endpoints protegidos con `[Authorize]`.
- Operaciones CRUD completas:
	- `GET /api/users`
	- `GET /api/users/{id}`
	- `POST /api/users`
	- `PUT /api/users/{id}`
	- `DELETE /api/users/{id}`
- Endpoints de autenticación:
	- `POST /api/auth/login`
	- `POST /api/auth/refresh` (usa cookie `HttpOnly`)
- Registro de usuarios mediante `POST /api/users`.
- Persistencia con Entity Framework Core + SQLite (Code First).
- Migraciones aplicadas automáticamente al iniciar la aplicación.
- Validación de correo único a nivel de servicio y base de datos (índice único).
- Validación de entrada con DataAnnotations (`Required`, `MinLength`, `MaxLength`, `EmailAddress`).
- Swagger habilitado en entorno de desarrollo.

## 🧱 Arquitectura del proyecto

La solución usa una arquitectura por capas simple:

- **Controllers**: exponen endpoints HTTP y devuelven respuestas REST.
- **Models**: entidad `User` y DTOs de entrada/salida (`UserCreateRequestDto`, `UserUpdateRequestDto`, `UserResponseDto`, `LoginRequestDto`, `AuthResponseDto`, `AuthTokensDto`).
- **Services**: lógica de negocio, autenticación JWT, refresh tokens y gestión de usuarios (`UserService`, `AuthService`, `JwtTokenService`, `PasswordHasher`, `RefreshTokenFactory`).
- **Services/Interfaces**: contratos de servicios (`IUserService`, `IAuthService`, `IPasswordHasher`, `IJwtTokenService`, `IRefreshTokenFactory`).
- **Common**: utilidades transversales (`ServiceResult`, `UserServiceErrorCodes`, `UserMapper`).
- **Data**: `AppDbContext` y configuración EF Core; migraciones en `Data/Migrations/`.
- **Configuration**: opciones tipadas (`JwtOptions`).
- **Program.cs**: configuración de servicios, middleware y pipeline de la aplicación (extensiones inlininadas).

Flujo principal:

`HTTP Request -> Controller -> Service -> DbContext (EF Core) -> SQLite`

Flujo de autenticación:

`Register (POST /api/users) -> Login -> Access Token (body) + Refresh Token (cookie HttpOnly) -> Authorize (Swagger) -> Endpoints protegidos -> Refresh`

## 🗂️ Estructura real del proyecto

```text
user-api/
├── README.md
├── docs/
│   └── images/
├── user-api_csharp/
│   ├── Db/
│   │   └── users.db
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── user-api_csharp.csproj
│   └── src/
│       ├── Configuration/
│       │   └── JwtOptions.cs
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   └── UsersController.cs
│       ├── Common/
│       │   ├── ServiceResult.cs
│       │   ├── UserServiceErrorCodes.cs
│       │   └── UserMapper.cs
│       ├── Data/
│       │   ├── AppDbContext.cs
│       │   └── Migrations/
│       │       ├── 20260227233825_InitialCreate.cs
│       │       ├── 20260227233825_InitialCreate.Designer.cs
│       │       ├── 20260305185548_AddJwtAuthFields.cs
│       │       ├── 20260305185548_AddJwtAuthFields.Designer.cs
│       │       └── AppDbContextModelSnapshot.cs
│       ├── Models/
│       │   ├── User.cs
│       │   └── UserDtos.cs
│       └── Services/
│           ├── AuthService.cs
│           ├── JwtTokenService.cs
│           ├── PasswordHasher.cs
│           ├── RefreshTokenFactory.cs
│           ├── UserService.cs
│           └── Interfaces/
│               ├── IAuthService.cs
│               ├── IJwtTokenService.cs
│               ├── IPasswordHasher.cs
│               ├── IRefreshTokenFactory.cs
│               └── IUserService.cs
└── user-api.slnx
```

## 🧠 Principios SOLID aplicados

- **S — Single Responsibility**
	- `UsersController` y `AuthController` se enfocan en HTTP.
	- `UserService` centraliza la lógica de negocio de usuarios.
	- `AuthService` centraliza lógica de autenticación y refresh tokens.
	- `UserMapper` centraliza el mapeo DTO/entidad.
	- `JwtTokenService`, `PasswordHasher` y `RefreshTokenFactory` separan responsabilidades de seguridad.

- **O — Open/Closed**
	- Puedes extender reglas de negocio en `UserService` y autenticación sin romper controladores ni contratos.

- **L — Liskov Substitution**
	- Se respeta al trabajar mediante contrato `IUserService` e `IAuthService`.

- **I — Interface Segregation**
	- `IUserService`, `IAuthService`, `IPasswordHasher`, `IJwtTokenService` e `IRefreshTokenFactory` exponen contratos específicos.

- **D — Dependency Inversion**
	- Controladores y servicios dependen de abstracciones (`IUserService`, `IAuthService`, `IPasswordHasher`, etc.), no de implementaciones concretas.

## 📝 Cambios recientes

### Arquitectura simplificada
- Reorganización de carpetas para mayor cohesión:
  - DTOs movidos a `Models/` para estar junto a las entidades que representan.
  - Interfaces de servicios movidas a `Services/Interfaces/` (conceptualmente pertenecen a la capa de servicios).
  - Utilidades transversales (`ServiceResult`, `UserServiceErrorCodes`, `UserMapper`) movidas a `Common/`.
  - Migraciones de EF Core movidas a `Data/Migrations/` (son artefactos de la capa de datos).
- Carpetas eliminadas por simplicidad:
  - `Extensions/` (configuración inlininada en `Program.cs`).
  - `Security/` (funciones consolidadas en servicios).
  - `Mappers/`, `Interfaces/`, `DTOs/` (reorganizadas en destinos más lógicos).

### Integración de bcrypt
- Las contraseñas de usuario ahora se hashean con **bcrypt** (configuración de workFactor: 12).
  - Más seguro que SHA-256 gracias al salt aleatorio y coste computacional.
  - Método `IPasswordHasher.Verify()` permite validar sin comparación directa.
- Los refresh tokens continúan usando SHA-256 determinista para:
  - Permitir búsquedas rápidas por igualdad en BD.
  - Mantener eficiencia sin comprometer seguridad del refresh token opaco.
- Paquete agregado: `BCrypt.Net-Next` v4.0.3.

## ✅ Requisitos

- .NET SDK 10.0 (o superior compatible con el `TargetFramework` actual).
- CLI de `dotnet` disponible en terminal.
- Sistema operativo compatible con .NET (macOS, Linux o Windows).

## 🚀 Ejecución

1. Restaurar y compilar:

```bash
dotnet restore user-api_csharp/user-api_csharp.csproj
dotnet build user-api_csharp/user-api_csharp.csproj
```

2. Ejecutar la API:

```bash
dotnet run --project user-api_csharp/user-api_csharp.csproj
```

3. Abrir Swagger:

- `http://localhost:5222/swagger`

4. Endpoint raíz:

- `http://localhost:5222/`

## 🔐 Configuración JWT

La configuración se encuentra en:

- `user-api_csharp/appsettings.json`
- `user-api_csharp/appsettings.Development.json`

Campos usados:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:Key`
- `Jwt:AccessTokenMinutes`
- `Jwt:RefreshTokenMinutes`

Importante:

- Cambia `Jwt:Key` por una clave larga y aleatoria antes de usar en producción.

## 🧪 Pruebas de autenticación

1. Crear usuario (registro):

```bash
curl -X POST http://localhost:5222/api/users \
	-H "Content-Type: application/json" \
	-d '{
		"name":"David Calderon",
		"email":"david@example.com",
		"password":"Secret1234",
		"dateOfBirth":"1995-05-20"
	}'
```

2. Login para obtener tokens:

```bash
curl -c /tmp/auth_cookies.txt -X POST http://localhost:5222/api/auth/login \
	-H "Content-Type: application/json" \
	-d '{
		"email":"david@example.com",
		"password":"Secret1234"
	}'
```

Respuesta esperada:

- `accessToken` en el body.
- `refreshToken` en cookie `HttpOnly` (no en body).

3. Autorizar Swagger con el access token:

- Click en `Authorize` en la parte superior de Swagger.
- Pegar el token JWT.
- Si Swagger ya maneja esquema bearer, pegar solo el token (sin prefijo `Bearer `).

4. Consumir endpoint protegido:

```bash
curl http://localhost:5222/api/users \
	-H "Authorization: Bearer <ACCESS_TOKEN>"
```

5. Refrescar token:

```bash
curl -b /tmp/auth_cookies.txt -c /tmp/auth_cookies.txt \
	-X POST http://localhost:5222/api/auth/refresh
```

Nota:

- Si no envías la cookie de refresh, el endpoint responde `401 Unauthorized`.

6. Validación de seguridad:

- `GET /api/users` sin token devuelve `401 Unauthorized`.
- `GET /api/users` con token válido devuelve `200 OK`.

### Capturas de pruebas (Swagger)

#### GET /
![GET root](docs/images/01-get-root.png)

#### GET /api/Users
![GET users](docs/images/02-get-users.png)

#### POST /api/Users
![POST users](docs/images/03-post-users.png)

#### GET /api/Users/{id}
![GET user by id](docs/images/04-get-user-by-id.png)

#### PUT /api/Users/{id}
![PUT user](docs/images/05-put-user.png)

#### DELETE /api/Users/{id}
![DELETE user](docs/images/06-delete-user.png)
