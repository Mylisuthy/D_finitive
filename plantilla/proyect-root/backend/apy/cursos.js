const express = require('express')
const router = express.Router()
const supabase = require('../supabase_client')
const validator = require('validator')

// Obtener todos los cursos
router.get('/', async (req, res) => {
  try {
    const { data, error } = await supabase.from('cursos').select('*')
    if (error) throw error
    res.json(data)
  } catch (err) {
    res.status(500).json({ error: 'Error al obtener cursos' })
  }
})

// Crear curso con validación avanzada
router.post('/', async (req, res) => {
  const { nombre, descripcion, docente_id, codigo } = req.body
  if (!nombre || !codigo) {
    return res.status(400).json({ error: 'Datos incompletos' })
  }
  if (docente_id && !validator.isUUID(docente_id + '')) {
    return res.status(400).json({ error: 'docente_id inválido' })
  }
  try {
    const { data, error } = await supabase.from('cursos').insert([{ nombre, descripcion, docente_id, codigo }])
    if (error) throw error
    res.status(201).json(data)
  } catch (err) {
    res.status(500).json({ error: 'Error al crear curso' })
  }
})

// ...agregar update y delete...

module.exports = router
