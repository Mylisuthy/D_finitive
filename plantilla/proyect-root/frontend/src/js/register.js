import axios from 'axios'

const form = document.getElementById('registerForm')

form.addEventListener('submit', async (e) => {
  e.preventDefault()
  const email = document.getElementById('regEmail').value
  const password = document.getElementById('regPassword').value
  const rol = document.getElementById('regRol').value

  try {
    const res = await axios.post('http://localhost:3000/api/register', { email, password, rol })
    if (res.data.success) {
      alert('Registro exitoso. Ahora puedes iniciar sesión.')
      window.location.href = './login.html'
    } else {
      alert(res.data.error || 'No se pudo registrar')
    }
  } catch (err) {
    alert(err.response?.data?.error || 'Error al conectar con el servidor')
  }
});
