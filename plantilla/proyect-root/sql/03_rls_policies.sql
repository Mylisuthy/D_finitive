-- Activar RLS en todas las tablas principales
ALTER TABLE usuarios ENABLE ROW LEVEL SECURITY;
ALTER TABLE docentes ENABLE ROW LEVEL SECURITY;
ALTER TABLE estudiantes ENABLE ROW LEVEL SECURITY;
ALTER TABLE cursos ENABLE ROW LEVEL SECURITY;
ALTER TABLE matriculas ENABLE ROW LEVEL SECURITY;

-- Política: los usuarios solo pueden ver/modificar su propio registro
CREATE POLICY "Usuarios: solo propio"
    ON usuarios
    FOR SELECT USING (id::text = auth.uid())
    WITH CHECK (id::text = auth.uid());

-- Política: los estudiantes solo pueden ver/modificar su propio registro
CREATE POLICY "Estudiantes: solo propio"
    ON estudiantes
    FOR SELECT USING (usuario_id::text = auth.uid())
    WITH CHECK (usuario_id::text = auth.uid());

-- Política: los docentes solo pueden ver/modificar su propio registro
CREATE POLICY "Docentes: solo propio"
    ON docentes
    FOR SELECT USING (usuario_id::text = auth.uid())
    WITH CHECK (usuario_id::text = auth.uid());

-- Política: los estudiantes solo pueden ver sus matrículas
CREATE POLICY "Matriculas: estudiante propio"
    ON matriculas
    FOR SELECT USING (
        estudiante_id IN (SELECT id FROM estudiantes WHERE usuario_id::text = auth.uid())
    );

-- Política: los docentes pueden ver cursos que dictan
CREATE POLICY "Cursos: docente propio"
    ON cursos
    FOR SELECT USING (
        docente_id IN (SELECT id FROM docentes WHERE usuario_id::text = auth.uid())
        OR auth.role() = 'service_role'
    );

-- Nota: Agregar políticas adicionales según necesidades de negocio y roles.
