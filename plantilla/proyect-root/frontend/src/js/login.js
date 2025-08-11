import axios from 'axios'

const form = document.getElementById('loginForm')

form.addEventListener('submit', async (e) => {
  e.preventDefault()
  const email = document.getElementById('email').value
  const password = document.getElementById('password').value

  try {
    const res = await axios.post('/api/login', { email, password })
    if (res.data.success) {
      localStorage.setItem('token', res.data.token)
      window.location.href = './dashboard.html'
    } else {
      alert('Credenciales inválidas')
    }
  } catch (err) {
    console.error(err)
    alert('Error al conectar con el servidor')
  }
});

// El código ya es compatible con la integración backend actual
