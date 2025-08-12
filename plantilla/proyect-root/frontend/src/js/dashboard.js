import axios from 'axios'
// Importa Chart.js desde CDN si no está en node_modules
import Chart from 'chart.js/auto'

const token = localStorage.getItem('token')
if (!token) {
  window.location.href = './login.html'
}

async function loadMetrics() {
  try {
    const res = await axios.get('/api/metrics', {
      headers: { Authorization: `Bearer ${token}` }
    })
    // Renderiza métricas en texto
    document.getElementById('metrics').innerHTML = `
      <h3>Estadísticas</h3>
      <div style="display: flex; gap: 2rem;">
        <div>
          <span style="font-size:2rem;font-weight:bold;color:#4f46e5">${res.data.totalEstudiantes ?? 0}</span>
          <div>Total de estudiantes</div>
        </div>
        <div>
          <span style="font-size:2rem;font-weight:bold;color:#f59e0b">${res.data.totalCursos ?? 0}</span>
          <div>Total de cursos</div>
        </div>
      </div>
      <canvas id="metricsChart" width="400" height="180" style="margin-top:1rem;"></canvas>
    `
    // Renderiza gráfico de barras
    const ctx = document.getElementById('metricsChart').getContext('2d')
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['Estudiantes', 'Cursos'],
        datasets: [{
          label: 'Cantidad',
          data: [res.data.totalEstudiantes ?? 0, res.data.totalCursos ?? 0],
          backgroundColor: ['#4f46e5', '#f59e0b']
        }]
      },
      options: {
        plugins: {
          legend: { display: false }
        },
        responsive: true,
        scales: {
          y: { beginAtZero: true, ticks: { precision:0 } }
        }
      }
    })
  } catch (err) {
    document.getElementById('metrics').innerHTML = `<p style="color:red;">Error al cargar métricas</p>`
  }
}

let estudiantesData = []

async function loadEstudiantes() {
  try {
    const res = await axios.get('/api/estudiantes', {
      headers: { Authorization: `Bearer ${token}` }
    })
    estudiantesData = res.data
    renderEstudiantes(estudiantesData)
  } catch (err) {
    const tbody = document.querySelector('#tablaEstudiantes tbody')
    tbody.innerHTML = `<tr><td colspan="5" style="color:red;">Error al cargar estudiantes</td></tr>`
  }
}

function renderEstudiantes(data) {
  const tbody = document.querySelector('#tablaEstudiantes tbody')
  tbody.innerHTML = ''
  data.forEach(est => {
    const tr = document.createElement('tr')
    tr.innerHTML = `
      <td>${est.nombre ?? ''}</td>
      <td>${est.apellido ?? ''}</td>
      <td>${est.matricula ?? ''}</td>
      <td>${est.fecha_nacimiento ?? ''}</td>
      <td>${est.email ?? ''}</td>
    `
    tbody.appendChild(tr)
  })
}

// Filtro en tiempo real
document.addEventListener('DOMContentLoaded', () => {
  const filtro = document.getElementById('filtroEstudiantes')
  if (filtro) {
    filtro.addEventListener('input', e => {
      const val = e.target.value.toLowerCase()
      const filtrados = estudiantesData.filter(est =>
        (est.nombre ?? '').toLowerCase().includes(val) ||
        (est.apellido ?? '').toLowerCase().includes(val) ||
        (est.matricula ?? '').toLowerCase().includes(val) ||
        (est.email ?? '').toLowerCase().includes(val)
      )
      renderEstudiantes(filtrados)
    })
  }

  // Exportar a CSV
  const exportBtn = document.getElementById('exportarCSV')
  if (exportBtn) {
    exportBtn.addEventListener('click', () => {
      if (!estudiantesData.length) {
        alert('No hay datos para exportar')
        return
      }
      const encabezados = ['nombre','apellido','matricula','fecha_nacimiento','email']
      const filas = estudiantesData.map(est =>
        encabezados.map(campo => `"${(est[campo] ?? '').replace(/"/g, '""')}"`).join(',')
      )
      const csv = [encabezados.join(','), ...filas].join('\r\n')
      const blob = new Blob([csv], { type: 'text/csv' })
      const url = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = 'estudiantes_export.csv'
      document.body.appendChild(a)
      a.click()
      document.body.removeChild(a)
      URL.revokeObjectURL(url)
    })
  }
})

loadMetrics()
loadEstudiantes()
