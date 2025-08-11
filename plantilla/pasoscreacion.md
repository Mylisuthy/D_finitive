**creacion carpeta principal**
mkdir proyecto-fullstack
cd proyecto-fullstack

**Inicializar Git**
git init

**Crear backend con Node.js**
mkdir backend
cd backend
npm init -y
npm install express cors dotenv @supabase/supabase-js
npm install --save-dev nodemon

**📄 Crear index.js**
echo import express from 'express'; > index.js
echo import cors from 'cors'; >> index.js
echo import dotenv from 'dotenv'; >> index.js
echo dotenv.config(); >> index.js
echo const app = express(); >> index.js
echo app.use(cors()); >> index.js
echo app.use(express.json()); >> index.js
echo app.get('/', (req, res) => res.send('Backend funcionando 🚀')); >> index.js
echo app.listen(3000, () => console.log('Servidor backend en http://localhost:3000')); >> index.js
---
```
echo "import express from 'express';
import cors from 'cors';
import dotenv from 'dotenv';

dotenv.config();

const app = express();
app.use(cors());
app.use(express.json());

app.get('/', (req, res) => res.send('Backend funcionando 🚀'));
app.listen(3000, () => console.log('Servidor backend en http://localhost:3000'));
" > index.js
```
---

**Luego regresas a la raíz:**
cd ..

**Crear frontend con Vite**
npm create vite@latest frontend

**Después:**
cd frontend
npm install
cd ..

**Carpetas adicionales**
mkdir sql
mkdir data
mkdir postman

**Crear README**
echo # Proyecto Fullstack con Vite + Express + Supabase > README.md

**Para probar:**

Backend →
cd backend
npx nodemon index.js

Frontend →
cd frontend
npm run dev

melos

**1️⃣ Backend**

Instala las dependencias necesarias:

npm init -y

npm install express cors dotenv @supabase/supabase-js
npm install --save-dev nodemon

**Para correrlo:**
npx nodemon index.js

**2️⃣ Frontend**
Entra en la carpeta del frontend:

cd ../frontend

npm install

Para correrlo:

npm run dev

**3️⃣ Si es la primera vez que clonas o limpiaste node_modules**

Si borraste node_modules o es un clon nuevo, solo asegúrate de correr:

cd backend && npm install
cd ../frontend && npm install

