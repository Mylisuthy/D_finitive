// Servidor principal Express para el backend académico
require('dotenv').config()
const express = require('express')
const cors = require('cors')
const morgan = require('morgan')
const jwt = require('jsonwebtoken')
const validator = require('validator')
const helmet = require('helmet')

// Rutas de la API
const usersRouter = require('./apy/users')
const estudiantesRouter = require('./apy/estudiantes')
const cursosRouter = require('./apy/cursos')
const genericRouter = require('./apy/generic')
// ...agregar más routers según dominio...

const app = express();

// Seguridad HTTP headers
app.use(helmet());

// CORS restrictivo (ajusta el origen en producción)
app.use(cors({
  origin: process.env.FRONTEND_URL || 'http://localhost:5173',
  credentials: true
}));
app.use(express.json())
app.use(morgan('dev'))

// Middleware de autenticación JWT
function authMiddleware(req, res, next) {
  const authHeader = req.headers.authorization
  if (!authHeader) return res.status(401).json({ error: 'Token requerido' })
  const token = authHeader.split(' ')[1]
  jwt.verify(token, process.env.JWT_SECRET, (err, user) => {
    if (err) return res.status(403).json({ error: 'Token inválido' })
    req.user = user
    next()
  })
};

// Protección básica XSS y sanitización
app.use(express.json({
  verify: (req, res, buf) => {
    // Rechaza payloads sospechosos (muy básicos)
    const str = buf.toString('utf8')
    if (/<script>|<\/script>/i.test(str)) {
      throw new Error('Payload sospechoso de XSS')
    }
  }
}));

// Sugerencia: Para CSRF, usar librerías como csurf si se usan cookies/sesiones
// const csurf = require('csurf')
// app.use(csurf())

// Endpoint de login (devuelve JWT)
app.post('/api/login', async (req, res) => {
  const { email, password } = req.body
  if (!email || !validator.isEmail(email) || !password) {
    return res.status(400).json({ success: false, error: 'Datos inválidos' })
  }
  // ...validar email y password contra la base de datos...
  // Aquí deberías comparar el hash, esto es solo ejemplo:
  const supabase = require('./supabase_client')
  const { data, error } = await supabase
    .from('usuarios')
    .select('*')
    .eq('email', email)
    .single()
  if (error || !data) return res.status(401).json({ success: false, error: 'Credenciales inválidas' })
  // Aquí deberías comparar el hash real de la contraseña
  if (password !== data.password_hash) return res.status(401).json({ success: false, error: 'Credenciales inválidas' })
  // Generar JWT
  const token = jwt.sign({ id: data.id, rol: data.rol, email: data.email }, process.env.JWT_SECRET, { expiresIn: '8h' })
  res.json({ success: true, token })
});

// Endpoint de registro (opcional)
app.post('/api/register', async (req, res) => {
  const { email, password, rol } = req.body
  if (!email || !password || !rol) return res.status(400).json({ error: 'Datos incompletos' })
  // ...validar unicidad y sanitizar...
  const supabase = require('./supabase_client')
  // Verifica si el usuario ya existe
  const { data: existing } = await supabase.from('usuarios').select('id').eq('email', email).single()
  if (existing) return res.status(400).json({ error: 'El correo ya está registrado' })
  // Inserta el usuario (usa hash real en producción)
  const { data, error } = await supabase.from('usuarios').insert([{ email, password_hash: password, rol }])
  if (error) return res.status(400).json({ error: 'No se pudo registrar usuario' })
  res.status(201).json({ success: true })
});

// Rutas protegidas
app.use('/api/usuarios', authMiddleware, usersRouter)
app.use('/api/estudiantes', authMiddleware, estudiantesRouter)
app.use('/api/cursos', authMiddleware, cursosRouter)
app.use('/api/generic', authMiddleware, genericRouter)
// ...agregar más rutas protegidas...

// Endpoint de métricas (ejemplo)
app.get('/api/metrics', authMiddleware, async (req, res) => {
  const supabase = require('./supabase_client')
  // Total de estudiantes
  const { count: totalEstudiantes } = await supabase.from('estudiantes').select('*', { count: 'exact', head: true })
  // Total de cursos
  const { count: totalCursos } = await supabase.from('cursos').select('*', { count: 'exact', head: true })
  res.json({ totalEstudiantes, totalCursos })
});

// Manejo centralizado de errores
app.use((err, req, res, next) => {
  console.error(err)
  res.status(500).json({ error: 'Error interno del servidor' })
});

const PORT = process.env.PORT || 3000
app.listen(PORT, () => {
  console.log(`Servidor backend escuchando en puerto ${PORT}`)
});
