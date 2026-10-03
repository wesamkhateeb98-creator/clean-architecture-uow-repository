# 4. Putting It Together

## One request through every layer
`POST /api/products` with `{ "name": "Keyboard", "price": 49.99, "stock": 100, "categoryId": 1 }`

```mermaid
sequenceDiagram
    autonumber
    participant Client
    participant MW as ErrorHandlingMiddleware<br/>(Api)
    participant Ctrl as ProductsController<br/>(Api)
    participant Svc as ProductService<br/>(Application)
    participant UoW as UnitOfWork<br/>(Infrastructure)
    participant Repo as Category/ProductRepository<br/>(Infrastructure)
    participant DB as PostgreSQL

    Client->>MW: POST /api/products
    MW->>Ctrl: next()
    Ctrl->>Svc: CreateAsync(request)
    Svc->>UoW: Categories.ExistsAsync(1)
    UoW->>Repo: ExistsAsync(1)
    Repo->>DB: SELECT EXISTS(...)
    DB-->>Svc: true
    Svc->>Svc: new Product { ... }
    Svc->>UoW: Products.AddAsync(product)
    Note over UoW: tracked, not saved
    Svc->>UoW: SaveChangesAsync()
    UoW->>DB: INSERT INTO "Products" ... RETURNING "Id"
    DB-->>Svc: Id = 4
    Svc-->>Ctrl: 4
    Ctrl-->>Client: 201 Created  { "id": 4 }
```

## Who does what
| Piece | Pattern | Responsibility |
|---|---|---|
| `Product`, `Category` | Clean Architecture: **Domain** | Pure data + rules, no dependencies |
| `ProductService` | Clean Architecture: **Application** | Business flow: validate, create, decide |
| `IProductRepository` → `ProductRepository` | **Repository** | *How* to read/stage one entity type |
| `IUnitOfWork` → `UnitOfWork` | **Unit of Work** | Group repositories + commit once |
| `AppDbContext` | Infrastructure | EF Core ↔ PostgreSQL mapping |
| `ProductsController` | Clean Architecture: **Presentation** | HTTP ↔ service, nothing else |
| `Program.cs` | Composition root | Wire interfaces to implementations |

## Errors
```mermaid
flowchart LR
    Svc["ProductService<br/>throw new NotFoundException()"] --> MW[ErrorHandlingMiddleware]
    MW -->|NotFoundException| R404["404 { error }"]
    MW -->|BadRequestException| R400["400 { error }"]
```
Application throws **its own** exceptions, with no knowledge of HTTP. The Api maps them to status codes.

## Folder map
```
Shop.Demo/
├── Shop.Domain/Entities/               Product.cs, Category.cs
├── Shop.Application/
│   ├── Interfaces/                     IRepository, IProductRepository, ICategoryRepository, IUnitOfWork
│   ├── Services/                       ProductService, CategoryService
│   ├── DTOs/                           ProductDtos, CategoryDtos
│   ├── Exceptions/                     NotFoundException, BadRequestException
│   └── DependencyInjection.cs
├── Shop.Infrastructure/
│   ├── Persistence/                    AppDbContext, UnitOfWork, DbInitializer
│   ├── Repositories/                   Repository<T>, ProductRepository, CategoryRepository
│   └── DependencyInjection.cs
└── Shop.Api/
    ├── Controllers/                    ProductsController, CategoriesController
    ├── Middleware/                     ErrorHandlingMiddleware
    ├── Program.cs
    └── Shop.Api.http                   ready-to-run requests
```
