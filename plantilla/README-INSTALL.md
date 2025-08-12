# Guía de instalación y ejecución del proyecto

## 1. Instalar dependencias del backend

```powershell
cd plantilla\proyect-root\backend
npm install
```

---

## 2. Configurar variables de entorno del backend

Crea un archivo `.env` en la carpeta `backend` con el siguiente contenido (ajusta los valores según tu proyecto Supabase):

```
SUPABASE_URL=tu_url_supabase
SUPABASE_SERVICE_ROLE=tu_clave_supabase
JWT_SECRET=un_secreto_seguro
```

---

## 3. Iniciar el backend

```powershell
npm start
```

El backend debe iniciar en el puerto 3000.

---

## 4. Instalar dependencias del frontend

```powershell
cd ..\frontend
npm install
```

---

## 5. Iniciar el frontend

```powershell
npm run dev
```

El frontend estará disponible normalmente en `http://localhost:5173`.

---

## 6. (Opcional) Configurar la base de datos

Ejecuta los scripts SQL en Supabase para crear las tablas y datos iniciales si es necesario (`plantilla/proyect-root/sql/`).

---

## 7. ¡Listo!

Ya puedes usar la aplicación desde el navegador.
