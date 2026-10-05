import { useEffect, useState } from 'react';
import { Sparkles, Plus, Search, Trash2, Pencil, Loader2 } from 'lucide-react';

const API_BASE = 'https://localhost:7130';

export default function Brands() {
    const [brands, setBrands] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');
    const [formOpen, setFormOpen] = useState(false);
    const [editingBrand, setEditingBrand] = useState(null);
    const [form, setForm] = useState({ name: '' });

    useEffect(() => {
      fetch(`${API_BASE}/api/brand`)
        .then(res => res.json())
        .then(data => { setBrands(data); setLoading(false); });
    }, []);

    const filtered = brands
      .filter(b => b.name.toLowerCase().includes(search.toLowerCase()))
      .sort((a, b) => a.name.localeCompare(b.name));

    const handleChange = (e) => {
      setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleAdd = () => {
      setEditingBrand(null);
      setForm({ name: '' });
      setFormOpen(true);
    };

    const handleEdit = (brand) => {
      setEditingBrand(brand);
      setForm({ name: brand.name });
      setFormOpen(true);
    };

    const handleSubmit = async (e) => {
      e.preventDefault();

      if (editingBrand) {
        const res = await fetch(`${API_BASE}/api/brand/${editingBrand.id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(form)
        });
        if (!res.ok) { alert('Failed to update'); return; }
        const updated = await res.json();
        setBrands(brands.map(b => b.id === editingBrand.id ? updated : b));
      } 
      else {
        const res = await fetch(`${API_BASE}/api/brand`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(form)
        });
        if (!res.ok) { alert('Failed to save'); return; }
        const newBrand = await res.json();
        setBrands([...brands, newBrand]);
      }
      setFormOpen(false);
    };

    const handleDelete = async (id) => {
      if (!window.confirm('Delete this brand?')) return;
      await fetch(`${API_BASE}/api/brand/${id}`, { method: 'DELETE' });
      setBrands(brands.filter(b => b.id !== id));
    };

    return (
      <div>
        <h1 className="text-3xl font-bold text-[hsl(var(--primary))]">Brands</h1>
        <p className="mt-1 text-[hsl(var(--muted-foreground))]">Manage your cosmetics brands.</p>

        <div className="mt-6 flex items-center justify-between">
          <div className="relative w-full max-w-xs">
              <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-[hsl(var(--muted-foreground))]" />
              <input
                  placeholder="Search brands..."
                  value={search}
                  onChange={e => setSearch(e.target.value)}
                  className="w-full rounded-lg border border-[hsl(var(--border))] py-2 pl-9 pr-3 text-sm outline-none focus:ring-2 focus:ring-[hsl(var(--primary))]"
              />
          </div>

          <button
              onClick={handleAdd}
              className="flex items-center gap-2 rounded-lg bg-[hsl(var(--primary))] px-4 py-2 text-sm font-medium text-[hsl(var(--primary-foreground))] hover:opacity-90"
          >
              <Plus className="h-4 w-4" /> Add Brand
          </button>
        </div>

        {formOpen && (
          <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
            <div className="w-full max-w-sm rounded-2xl bg-[hsl(var(--background))] p-6 shadow-lg">
              <div className="mb-5 flex items-center justify-between">
                <h2 className="text-lg font-semibold">{editingBrand ? 'Edit Brand' : 'Add New Brand'}</h2>
                <button onClick={() => setFormOpen(false)} className="text-[hsl(var(--muted-foreground))]">✕</button>
              </div>

              <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                  <label className="mb-1.5 block text-sm font-medium">
                      Name <span className="text-red-500">*</span>
                  </label>
                  <input name="name" value={form.name} onChange={handleChange} required
                      className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm" />
                </div>

                <div className="flex justify-end gap-3 pt-2">
                  <button type="button" onClick={() => setFormOpen(false)}
                      className="rounded-lg border border-[hsl(var(--border))] px-5 py-2 text-sm">
                      Cancel
                  </button>
                  <button type="submit"
                      className="rounded-lg bg-[hsl(var(--primary))] px-5 py-2 text-sm font-medium text-[hsl(var(--primary-foreground))]">
                      {editingBrand ? 'Update' : 'Add Brand'}
                  </button>
                </div>
              </form>
            </div>
          </div>
        )}

        {loading ? (
          <div className="flex justify-center py-12">
            <Loader2 className="h-8 w-8 animate-spin text-[hsl(var(--primary))]" />
          </div>
      ) : filtered.length === 0 ? (
          <div className="mt-10 flex flex-col items-center py-12 text-[hsl(var(--muted-foreground))]">
            <Sparkles className="mb-2 h-10 w-10" />
            <p>No brands yet. Add your first brand!</p>
          </div>
      ) : (
          <div className="mt-6 grid grid-cols-2 gap-3 sm:grid-cols-3 lg:grid-cols-4">
            {filtered.map(brand => (
              <div key={brand.id} className="flex items-center justify-between rounded-xl border border-[hsl(var(--border))] bg-[hsl(var(--primary))]/5 p-4">
                <span className="font-medium">{brand.name}</span>
                <div className="flex gap-2">
                  <button onClick={() => handleEdit(brand)} className="text-[hsl(var(--muted-foreground))]">
                    <Pencil className="h-4 w-4" />
                  </button>
                  <button onClick={() => handleDelete(brand.id)} className="text-red-500">
                    <Trash2 className="h-4 w-4" />
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    );
}