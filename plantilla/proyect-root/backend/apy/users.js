const express = require('express')
const router = express.Router()
const supabase = require('../supabase_client')
const validator = require('validator')

// Obtener todos los usuarios (solo admin)
router.get('/', async (req, res) => {
  // ...validar rol admin...
  try {
    const { data, error } = await supabase.from('usuarios').select('*')
    if (error) throw error
    res.json(data)
  } catch (err) {
    res.status(500).json({ error: 'Error al obtener usuarios' })
  }
})

// Crear usuario con validación avanzada
router.post('/', async (req, res) => {
  const { email, password_hash, rol } = req.body
  // Validación de email y rol
  if (!email || !validator.isEmail(email)) {
    return res.status(400).json({ error: 'Email inválido' })
  }
  if (!password_hash || password_hash.length < 6) {
    return res.status(400).json({ error: 'Contraseña demasiado corta' })
  }
  if (!['admin', 'docente', 'estudiante'].includes(rol)) {
    return res.status(400).json({ error: 'Rol inválido' })
  }
  try {
    const { data, error } = await supabase.from('usuarios').insert([{ email, password_hash, rol }])
    if (error) throw error
    res.status(201).json(data)
  } catch (err) {
    res.status(500).json({ error: 'Error al crear usuario' })
  }
})

// ...agregar update y delete según necesidades...

module.exports = router
