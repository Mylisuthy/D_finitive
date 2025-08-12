-- Tabla de usuarios del sistema
CREATE TABLE usuarios (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(), -- Identificador único
    email VARCHAR(120) UNIQUE NOT NULL,            -- Correo electrónico único
    password_hash VARCHAR(255) NOT NULL,           -- Hash de la contraseña
    rol VARCHAR(30) NOT NULL CHECK (rol IN ('admin', 'docente', 'estudiante')), -- Rol del usuario
    creado_en TIMESTAMP DEFAULT now()              -- Fecha de creación
);

-- Tabla de docentes
CREATE TABLE docentes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id UUID NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE, -- Relación 1:1 con usuarios
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    especialidad VARCHAR(100),
    UNIQUE(usuario_id)
);

-- Tabla de estudiantes
CREATE TABLE estudiantes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id UUID NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    matricula VARCHAR(20) UNIQUE NOT NULL, -- Número de matrícula único
    fecha_nacimiento DATE,
    UNIQUE(usuario_id)
);

-- Tabla de cursos
CREATE TABLE cursos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    docente_id UUID REFERENCES docentes(id) ON DELETE SET NULL, -- Un curso puede tener un docente asignado
    codigo VARCHAR(20) UNIQUE NOT NULL
);

-- Tabla de matrículas (relación muchos a muchos entre estudiantes y cursos)
CREATE TABLE matriculas (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    estudiante_id UUID NOT NULL REFERENCES estudiantes(id) ON DELETE CASCADE,
    curso_id UUID NOT NULL REFERENCES cursos(id) ON DELETE CASCADE,
    fecha_inscripcion DATE DEFAULT now(),
    UNIQUE(estudiante_id, curso_id) -- Un estudiante no puede inscribirse dos veces al mismo curso
);

-- ...pueden agregarse más tablas según el dominio...
