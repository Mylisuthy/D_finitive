require('dotenv').config()
// Cliente centralizado de Supabase para Node.js
const { createClient } = require('@supabase/supabase-js')

// Variables de entorno para seguridad
const SUPABASE_URL = process.env.SUPABASE_URL
const SUPABASE_KEY = process.env.SUPABASE_SERVICE_KEY

const supabase = createClient(SUPABASE_URL, SUPABASE_KEY)

module.exports = supabase
