const express = require('express')
const router = express.Router()
const supabase = require('../supabase_client')
const validator = require('validator')

// Obtener todos los estudiantes
router.get('/', async (req, res) => {
  try {
    const { data, error } = await supabase.from('estudiantes').select('*')
    if (error) throw error
    res.json(data)
  } catch (err) {
    res.status(500).json({ error: 'Error al obtener estudiantes' })
  }
})

// Crear estudiante con validación avanzada
router.post('/', async (req, res) => {
  const { usuario_id, nombre, apellido, matricula, fecha_nacimiento } = req.body
  if (!usuario_id || !validator.isUUID(usuario_id + '')) {
    return res.status(400).json({ error: 'usuario_id inválido' })
  }
  if (!nombre || !apellido || !matricula) {
    return res.status(400).json({ error: 'Datos incompletos' })
  }
  if (fecha_nacimiento && !validator.isDate(fecha_nacimiento + '')) {
    return res.status(400).json({ error: 'Fecha inválida' })
  }
  try {
    const { data, error } = await supabase.from('estudiantes').insert([{ usuario_id, nombre, apellido, matricula, fecha_nacimiento }])
    if (error) throw error
    res.status(201).json(data)
  } catch (err) {
    res.status(500).json({ error: 'Error al crear estudiante' })
  }
})

// Carga masiva de estudiantes (desde CSV)
router.post('/bulk', async (req, res) => {
  const estudiantes = req.body
  if (!Array.isArray(estudiantes) || estudiantes.length === 0) {
    return res.status(400).json({ error: 'Datos de estudiantes inválidos' })
  }
  // Validación básica de cada registro
  for (const est of estudiantes) {
    if (!est.usuario_id || !validator.isUUID(est.usuario_id + '') ||
        !est.nombre || !est.apellido || !est.matricula) {
      return res.status(400).json({ error: 'Registro de estudiante inválido', registro: est })
    }
    if (est.fecha_nacimiento && !validator.isDate(est.fecha_nacimiento + '')) {
      return res.status(400).json({ error: 'Fecha inválida', registro: est })
    }
  }
  try {
    const { data, error } = await supabase.from('estudiantes').insert(estudiantes)
    if (error) throw error
    res.status(201).json({ inserted: data.length })
  } catch (err) {
    res.status(500).json({ error: 'Error en carga masiva' })
  }
})

// ...agregar update y delete...

module.exports = router
