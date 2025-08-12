-- Insertar usuarios de ejemplo
INSERT INTO usuarios (id, email, password_hash, rol)
VALUES
  ('00000000-0000-0000-0000-000000000001', 'admin@demo.com', 'HASHED_ADMIN', 'admin'),
  ('00000000-0000-0000-0000-000000000002', 'docente1@demo.com', 'HASHED_DOC1', 'docente'),
  ('00000000-0000-0000-0000-000000000003', 'estudiante1@demo.com', 'HASHED_EST1', 'estudiante');

-- Insertar docentes
INSERT INTO docentes (id, usuario_id, nombre, apellido, especialidad)
VALUES
  ('10000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000002', 'Ana', 'Pérez', 'Matemáticas');

-- Insertar estudiantes
INSERT INTO estudiantes (id, usuario_id, nombre, apellido, matricula, fecha_nacimiento)
VALUES
  ('20000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000003', 'Luis', 'García', 'MAT2023001', '2005-04-12');

-- Insertar cursos
INSERT INTO cursos (id, nombre, descripcion, docente_id, codigo)
VALUES
  ('30000000-0000-0000-0000-000000000001', 'Álgebra I', 'Curso básico de álgebra', '10000000-0000-0000-0000-000000000001', 'ALG101');

-- Insertar matrículas
INSERT INTO matriculas (id, estudiante_id, curso_id, fecha_inscripcion)
VALUES
  ('40000000-0000-0000-0000-000000000001', '20000000-0000-0000-0000-000000000001', '30000000-0000-0000-0000-000000000001', '2024-06-01');
