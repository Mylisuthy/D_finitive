-- Índice para búsqueda rápida de usuarios por email
CREATE INDEX idx_usuarios_email ON usuarios(email);

-- Índice para búsqueda de estudiantes por matrícula
CREATE INDEX idx_estudiantes_matricula ON estudiantes(matricula);

-- Índice para búsqueda de cursos por código
CREATE INDEX idx_cursos_codigo ON cursos(codigo);

-- Índice para matrículas por estudiante
CREATE INDEX idx_matriculas_estudiante ON matriculas(estudiante_id);

-- Índice para matrículas por curso
CREATE INDEX idx_matriculas_curso ON matriculas(curso_id);
