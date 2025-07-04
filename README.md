# 📦 AuthService Api

## 📄 Descripción

Esta es una API RESTful para gestionar un sistema de inventario de productos.  
Permite realizar operaciones **CRUD** (Crear, Leer, Actualizar, Eliminar) sobre productos, así como gestionar la **autenticación y autorización de usuarios** mediante JWT.
El proyecto implementa la **arquitectura hexagonal** y sigue los principios **SOLID**, garantizando escalabilidad, mantenibilidad y separación clara de responsabilidades.

---

## 🚀 Tecnologías Utilizadas

- **.NET Core 8** – Framework principal de desarrollo
- **Entity Framework Core** – ORM para acceso a la base de datos
- **SQL Server** – Motor de base de datos relacional
- **JWT** – JSON Web Tokens para autenticación y autorización

---

## 🛠 Requisitos

- [.NET SDK 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server instalado localmente o accesible remotamente

---

## ⚙️ Configuración

### 1. Crear la base de datos

Crea una base de datos llamada `TaskManagerDB` en SQL Server.  
Ejecuta el script **`ScriptCreacion de tablas.sql`** ubicado en la raíz del respositorio.  
Este script crea las tablas necesarias y un **usuario administrador por defecto**.

Email:admin@sistema.com
Password:1234

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Configurar cadena de conexión

Edita el archivo `appsettings.json` y configura tu cadena de conexión a SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=TaskManagerDB;Trusted_Connection=True;"
}
```

### 4. Aplicar migraciones (opcional)

```bash
dotnet ef database update
```

### 5. Ejecutar la API

```bash
dotnet run
```

La API estará disponible por defecto en:  
📍 `https://localhost:7130`

---

## 🔐 Autenticación y Seguridad

- La API utiliza **JWT Bearer Tokens** para proteger los endpoints.
- Para acceder a rutas protegidas, incluye el token en la cabecera `Authorization`:

```http
Authorization: Bearer tu_token_aqui
```

- Se implementa autorización por **roles**: `Administrador`, `Supervisor`, `Empleado`.
- La seguridad está reforzada mediante un **middleware personalizado** que:
  - Captura errores no controlados
  - Responde con mensajes uniformes y trazabilidad
  - Evita fugas de información interna

---

## ⚠️ Manejo de Errores y Respuestas

La API devuelve respuestas estandarizadas en el siguiente formato JSON:

```json
{
  "code": 400,
  "message": "El campo Email es obligatorio.",
  "traceid": "f97a8c1c-34e6-4e5b-a23a-fd39f95d1f19",
  "data": null
}
```

Esto garantiza una comunicación clara y consistente entre el backend y los consumidores.

---

## 🧪 Pruebas Unitarias

Este proyecto cuenta con pruebas unitarias implementadas con:

- `xUnit` – Framework de pruebas
- `Moq` – Simulación de servicios y dependencias

Se han probado casos como:

- Creación y edición de usuarios y tareas
- Controladores (`UserController`, `TaskController`) con dependencias mockeadas

Para ejecutar las pruebas:

```bash
dotnet test
```

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT.  
Consulta el archivo [LICENSE](./LICENSE) para más información.

---

## 📬 Contacto

¿Tienes preguntas, sugerencias o encontraste un bug?

**Email:** [nirn18345@gmail.com](mailto:nirn18345@gmail.com)
