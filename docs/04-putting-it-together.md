# 4. Putting It Together

## One request through every layer
`POST /api/v1.0/products` with `{ "name": "Keyboard", "price": 49.99, "stock": 100, "categoryId": 1 }`

```mermaid
sequenceDiagram
    autonumber
    participant Client
    participant MW as ErrorHandlingMiddleware<br/>(Presentation)
    participant Ctrl as ProductsController<br/>(Presentation)
    participant Svc as IProductService → ProductService<br/>(Application)
    participant UoW as UnitOfWork<br/>(Infrastructure)
    participant Repo as Category/ProductRepository<br/>(Infrastructure)
    participant DB as PostgreSQL

    Client->>MW: POST /api/v1.0/products
    MW->>Ctrl: next()
    Ctrl->>Ctrl: request.ToModel() → AddProductModel
    Ctrl->>Svc: AddAsync(model)
    Svc->>UoW: Categories.EnsureExistsAsync(1)
    UoW->>Repo: EnsureExistsAsync(1)
    Repo->>DB: SELECT EXISTS(...)
    DB-->>Repo: true (false → throw NotFoundException)
    Svc->>Svc: Product.Create(...)
    Svc->>UoW: Products.AddAsync(product)
    Note over UoW: tracked, not saved
    Svc->>UoW: CompleteAsync()
    UoW->>DB: INSERT INTO "Product" ... RETURNING "Id"
    DB-->>Svc: Id = 4
    Svc-->>Ctrl: 4
    Ctrl-->>Client: 201 Created  { "id": 4 }
```

## Who does what
| Piece | Pattern | Responsibility |
|---|---|---|
| `Product`, `Category` (`: IEntity`) | Clean Architecture: **Domain** | State + rules; created via `Create()`, changed via methods |
| `NotFoundException`, … (`IProblemDetailsProvider`) | Clean Architecture: **Domain** | Describe errors without knowing HTTP |
| `IProductService` → `ProductService` | Clean Architecture: **Application** | Business flow: check, create, decide |
| `IRepository<T>` → `Repository<T>` | **Repository** | Shared CRUD for every `IEntity` |
| `IProductRepository` → `ProductRepository` | **Repository** | *How* to read/stage one entity type |
| `IUnitOfWork` → `UnitOfWork` | **Unit of Work** | Group repositories; `CompleteAsync` / `Transaction` |
| `AppDbContext` + `Configurations/` | Infrastructure | EF Core ↔ PostgreSQL mapping |
| `ProductsController : BaseController` | Clean Architecture: **Presentation** | HTTP ↔ service, nothing else |
| `Program.cs` | Composition root | `RegisterPresentation` → `RegisterApplication` → `RegisterInfrastructure` |

## Errors
```mermaid
flowchart LR
    Svc["Service / Repository<br/>throw new NotFoundException()"] --> MW[ErrorHandlingMiddleware]
    MW -->|"Type = Not Found"| R404["404"]
    MW -->|"Type = Already Exists"| R409["409"]
    MW -->|"Type = Failed Precondition"| R400["400"]
    MW -->|"anything else"| R500["500 (logged)"]
```
Domain exceptions describe themselves through `GetProblemDetails()`. Only Presentation turns them into status codes. Full walkthrough: [5. Error Handling](05-error-handling.md).

```json
{ "status": 404, "title": "Category 99 not found.", "type": "Not Found", "detail": null, "extensions": {} }
```

## Folder map
```
Shop.Demo/
├── global.json  Directory.Build.props  docker-compose.yml
├── Shop.Domain/
│   ├── Entities/                       IEntity, Product, Category
│   └── Exceptions/                     NotFound, AlreadyExists, FailedPrecondition
│       └── Abstraction/                IProblemDetailsProvider, ServiceProblemDetails
├── Shop.Application/
│   ├── Abstracts/                      IRepository<T>, IUnitOfWork
│   │   ├── Repositories/               IProductRepository, ICategoryRepository
│   │   └── Services/                   IProductService, ICategoryService
│   ├── Services/                       ProductService, CategoryService
│   ├── Models/                         ProductModels, CategoryModels
│   └── Extensions/                     RegistrationExtensions, MapperExtensions
├── Shop.Infrastructure/
│   ├── Persistence/                    AppDbContext, DbInitializer
│   │   └── Configurations/             ProductConfiguration, CategoryConfiguration
│   ├── Repositories/                   Repository<T>, ProductRepository, CategoryRepository, UnitOfWork
│   └── Extensions/                     RegistrationExtensions
└── Shop.Presentation/
    ├── Controllers/                    BaseController, ProductsController, CategoriesController
    ├── Models/Requests/                ProductRequests, CategoryRequests
    ├── Extensions/                     RegistrationExtensions, Mapper/MapperExtensions
    ├── Middleware/                     ErrorHandlingMiddleware
    ├── Program.cs
    └── Shop.Presentation.http          ready-to-run requests
```

---
[← 3. Clean Architecture](03-clean-architecture.md) · [Next: 5. Error Handling →](05-error-handling.md)
