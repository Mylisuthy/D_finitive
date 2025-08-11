# Proyecto Académico Full Stack

## Carga masiva de estudiantes

1. Ve al dashboard y selecciona el archivo CSV con los encabezados: `nombre,apellido,matricula,fecha_nacimiento,email`.
2. El sistema validará y enviará los datos al backend.
3. Los registros válidos serán insertados en la base de datos.

## Seguridad

- Todas las rutas protegidas requieren JWT.
- Validación y sanitización de entradas en backend y frontend.
- Políticas RLS activas en la base de datos.
- Se recomienda usar HTTPS y restringir CORS en producción.

## Seguridad avanzada

- **CORS**: Solo permite solicitudes desde el frontend configurado.
- **Helmet**: Añade cabeceras HTTP seguras.
- **XSS**: Se rechazan payloads con `<script>` en el backend.
- **CSRF**: Se recomienda usar tokens CSRF si se usan cookies/sesiones.
- **RLS**: Políticas de Row Level Security activas en la base de datos.
- **Validación y sanitización**: Todas las entradas se validan en backend y frontend.

## Estructura

- `/backend/apy/estudiantes.js`: Endpoint `/bulk` para carga masiva.
- `/frontend/src/js/csv_upload.js`: Lógica de carga y validación de CSV.
- `/data/estudiantes.csv`: Ejemplo de archivo de carga.

## Ejemplo de CSV

```csv
nombre,apellido,matricula,fecha_nacimiento,email
Luis,García,MAT2023001,2005-04-12,estudiante1@demo.com
María,López,MAT2023002,2006-02-20,estudiante2@demo.com
```

## Endpoints principales

### Autenticación

- `POST /api/login`
  - **Body:** `{ "email": "usuario@demo.com", "password": "123456" }`
  - **Response:** `{ "success": true, "token": "JWT..." }`

### Usuarios

- `GET /api/usuarios` (requiere token admin)
- `POST /api/usuarios` (requiere token admin)

### Estudiantes

- `GET /api/estudiantes` (requiere token)
- `POST /api/estudiantes` (requiere token)
- `POST /api/estudiantes/bulk` (requiere token, carga masiva)

### Cursos

- `GET /api/cursos` (requiere token)
- `POST /api/cursos` (requiere token)

### Métricas

- `GET /api/metrics` (requiere token)
  - **Response:** `{ "totalEstudiantes": 10, "totalCursos": 5 }`

## Ejemplo de uso de token

Incluye el token JWT en el header:

```
Authorization: Bearer <token>
```

## Recomendaciones de despliegue

- Usar HTTPS en producción.
- Configurar variables de entorno seguras.
- Revisar y ajustar las políticas RLS según el dominio.

## Pruebas extremas recomendadas

- **Login con credenciales incorrectas:**  
  Enviar usuario o contraseña inválidos y verificar que la respuesta sea 401 y no revele información sensible.
- **Carga masiva con datos corruptos:**  
  Subir un CSV con campos vacíos, fechas mal formateadas o duplicados. El backend debe rechazar los registros inválidos y mostrar el error correspondiente.
- **Inyección de scripts en formularios:**  
  Intentar enviar `<script>alert(1)</script>` en cualquier campo de texto. El backend debe rechazar el payload y el frontend debe sanitizar la entrada.
- **Acceso a endpoints protegidos sin token:**  
  Realizar peticiones a `/api/estudiantes` o `/api/cursos` sin token o con token inválido. Debe responder 401/403.
- **Carga masiva con más de 1000 registros:**  
  Verificar que el sistema maneje correctamente grandes volúmenes y reporte errores si se supera el límite permitido.

## Seguridad en el frontend

- **Sanitización de entradas:**  
  Utiliza funciones de escape en los formularios antes de enviar datos al backend.
- **Almacenamiento seguro del token:**  
  El token JWT se guarda en `localStorage` y se envía solo por header Authorization.
- **Protección XSS:**  
  Nunca insertes datos del usuario directamente en el DOM sin sanitizar.
- **CORS:**  
  El backend solo acepta solicitudes del dominio frontend configurado.

## Guía visual y flujo de usuario

1. **Pantalla de inicio:**  
   Botones para iniciar sesión o registrarse.
2. **Login:**  
   Formulario accesible y validado. Si el login es exitoso, redirige al dashboard.
3. **Dashboard:**
   - Muestra métricas atractivas (total de estudiantes, cursos, etc.).
   - Permite cargar estudiantes desde CSV con feedback visual.
   - Muestra mensajes claros de éxito o error.
4. **Carga masiva:**
   - El usuario selecciona un archivo CSV.
   - El sistema valida y muestra cuántos registros fueron insertados o si hubo errores.

## Ejemplo de respuesta de error

```json
{
  "error": "usuario_id inválido",
  "registro": {
    "usuario_id": "123",
    "nombre": "Luis",
    "apellido": "García",
    "matricula": "MAT2023001"
  }
}
```

## Ejemplo de respuesta de éxito en carga masiva

```json
{
  "inserted": 2
}
```

---

## Recomendaciones de diseño

- Usa los estilos de `/frontend/src/assets/css/style.css` para mantener coherencia visual.
- Utiliza animaciones CSS (`.fade-in`) para mejorar la experiencia.
- Los formularios deben ser accesibles (uso de labels, focus, etc.).
- Los mensajes de error y éxito deben ser claros y visibles.

---

## Recursos útiles

- [Documentación oficial de Supabase](https://supabase.com/docs)
- [Guía de seguridad OWASP para aplicaciones web](https://owasp.org/www-project-top-ten/)
