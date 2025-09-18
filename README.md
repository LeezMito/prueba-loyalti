# 🛒 Prueba Loyalti — Inventory App

## 📌 Resumen
Aplicación **Full Stack**:  
- **Backend**: .NET 8 + EF Core + SQL Server, JWT Auth, arquitectura por capas (`Inventory.Business`, `Inventory.Data`, `Inventory.Entities`, `Inventory.WebApi`).  
- **Frontend**: Angular (standalone components), carrito de compras, login/register, checkout.

La base de datos se **crea con valores por defecto** (tiendas, artículos, relaciones) al aplicar la migración inicial.

Repo: https://github.com/LeezMito/prueba-loyalti

---

## ⚙️ Requisitos

- **.NET 8 SDK**
- **SQL Server**
- **Node.js 18+** y **Angular CLI** (`npm i -g @angular/cli`)
- **Git**

---

## 🚀 Puesta en marcha

Clona el repo:

```bash
git clone https://github.com/LeezMito/prueba-loyalti.git
cd prueba-loyalti/src
```

### 1) Backend (.NET)

1. **Configura la conexión de la base de datos** en `Inventory.WebApi/appsettings.json` con tus propieas credenciales:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=InventarioDb;User Id=sa;Password=TuPassword123;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "8pFQUeBSSFA3dRwdJujGklhFWy6Seh9TphDvvuJILCl3PaRJZYyQQcQktmiEBZ8uWlZoXoq63RfDD+rK52h6RA==",
    "Issuer": "Inventory.Api",
    "Audience": "Inventory.Web"
  }
}
```

2. **Crear / actualizar DB con migración inicial**  
   > La base se poblará con **datos por defecto** durante la migración/seeding.

```bash
dotnet ef migrations add InitialCreate -p Inventory.Data -s Inventory.WebApi
dotnet ef database update -p Inventory.Data -s Inventory.WebApi
```

3. **Levantar la API**

```bash
dotnet run --project .\Inventory.WebApi\
```

- Swagger: `https://localhost:5001/swagger`

> Si usas HTTPS, Postman/Angular deben apuntar a `https://localhost:5001`.

---

### 2) Frontend (Angular)

1. **Instala dependencias**

```bash
cd Frontend
npm install
```

2. **Configura el endpoint** en `Frontend/src/environments/environment.ts`:

```ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001'
};
```

3. **Correr**

```bash
ng serve -o
```

Frontend: `http://localhost:4200`

---

## 🛒 Flujo principal
1. **Registro / Login** (genera JWT).  
2. **Listado de productos** (muestra stock y sucursales).  
3. **Carrito** (controla stock; si supera stock muestra mensaje).  
4. **Checkout** (confirma compra → **descuenta stock** y **limpia carrito** en servidor).  

---

## 🧰 Comandos útiles

**Resetear DB** (drop + recreate):
```bash
dotnet ef database drop -f -p Inventory.Data -s Inventory.WebApi
dotnet ef database update -p Inventory.Data -s Inventory.WebApi
```

**Nueva migración**:
```bash
dotnet ef migrations add NombreMigracion -p Inventory.Data -s Inventory.WebApi
dotnet ef database update -p Inventory.Data -s Inventory.WebApi
```

**Ejecutar API directamente**:
```bash
dotnet run --project .\Inventory.WebApi\
```

---

## 🧩 Estructura

```
/Frontend             # Angular app
/Inventory.Business   # Lógica de negocio (servicios)
/Inventory.Data       # EF Core (DbContext, migraciones, seeding)
/Inventory.Entities   # Entidades + DTOs
/Inventory.WebApi     # ASP.NET Core Web API
/src.sln              # Solution .NET
```

---

## 👤 Autor
**Guillermo Altair Santos Aguilar** — Fullstack (.NET + Angular + SQL Server)
