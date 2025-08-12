// routes/generic.js
const express = require('express');
const supabase = require('../supabase_client'); // tu cliente ya configurado

const router = express.Router();

// READ (GET)
router.get('/:tabla', async (req, res) => {
  const { tabla } = req.params;
  const query = req.query; // filtros, select, limit, etc.

  try {
    const { data, error } = await supabase
      .from(tabla)
      .select(query.select || '*')
      .limit(query.limit ? parseInt(query.limit) : 100);

    if (error) throw error;

    res.json({ ok: true, data });
  } catch (err) {
    res.status(400).json({ ok: false, error: err.message });
  }
});

// CREATE (POST)
router.post('/:tabla', async (req, res) => {
  const { tabla } = req.params;
  const body = req.body;

  try {
    const { data, error } = await supabase
      .from(tabla)
      .insert(body)
      .select();

    if (error) throw error;

    res.json({ ok: true, data });
  } catch (err) {
    res.status(400).json({ ok: false, error: err.message });
  }
});

// UPDATE (PUT)
router.put('/:tabla/:id', async (req, res) => {
  const { tabla, id } = req.params;
  const body = req.body;

  try {
    const { data, error } = await supabase
      .from(tabla)
      .update(body)
      .eq('id', id)
      .select();

    if (error) throw error;

    res.json({ ok: true, data });
  } catch (err) {
    res.status(400).json({ ok: false, error: err.message });
  }
});

// DELETE
router.delete('/:tabla/:id', async (req, res) => {
  const { tabla, id } = req.params;

  try {
    const { data, error } = await supabase
      .from(tabla)
      .delete()
      .eq('id', id);

    if (error) throw error;

    res.json({ ok: true, data });
  } catch (err) {
    res.status(400).json({ ok: false, error: err.message });
  }
});

module.exports = router;
