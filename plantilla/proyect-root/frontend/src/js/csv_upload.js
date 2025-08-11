import Papa from 'papaparse'
import axios from 'axios'

const input = document.getElementById('csvInput')
const token = localStorage.getItem('token')

function escapeHTML(str) {
  return str.replace(/[&<>"']/g, function (m) {
    return ({
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      "'": '&#39;'
    })[m]
  })
}

input.addEventListener('change', (e) => {
  const file = e.target.files[0]
  if (!file) return
  Papa.parse(file, {
    header: true,
    skipEmptyLines: true,
    complete: async function(results) {
      // Validación y sanitización en frontend
      const estudiantes = results.data
        .map(row => ({
          ...row,
          nombre: escapeHTML(row.nombre || ''),
          apellido: escapeHTML(row.apellido || ''),
          matricula: escapeHTML(row.matricula || ''),
          fecha_nacimiento: row.fecha_nacimiento,
          email: escapeHTML(row.email || '')
        }))
        .filter(row =>
          row.nombre && row.apellido && row.matricula && row.email
        )
      if (estudiantes.length === 0) {
        alert('El archivo no contiene registros válidos.')
        return
      }
      try {
        const res = await axios.post('/api/estudiantes/bulk', estudiantes, {
          headers: { Authorization: `Bearer ${token}` }
        })
        alert(`Carga exitosa: ${res.data.inserted} registros`)
      } catch (err) {
        if (err.response && err.response.data && err.response.data.error) {
          alert('Error: ' + err.response.data.error)
        } else {
          alert('Error en carga masiva')
        }
      }
    }
  })
})
