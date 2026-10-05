import { useEffect, useState } from 'react';
import { Sparkles, Plus, Search, Trash2, Pencil, Loader2 } from 'lucide-react';
import {cn} from "@/lib/utils";

const API_BASE = 'https://localhost:7130';

export default function Subcategories() {
    const [subcategories, setSubcategories] = useState([]);
    const [productTypes, setProductTypes] = useState([]);

    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');
    const [formOpen, setFormOpen] = useState(false);
    
    const [editingSubcategory, setEditingSubcategory] = useState(null);
    const [form, setForm] = useState({ name: '' , productTypeId: '', hasVariant: false});

    useEffect(() => {
      fetch(`${API_BASE}/api/subcategory?pageSize=100`)
        .then(res => res.json())
        .then(data => { setSubcategories(data); setLoading(false); });
      fetch(`${API_BASE}/api/producttype?pageSize=100`)
        .then(res => res.json())
        .then(data => { setProductTypes(data); });
    }, []);

    const lookupName = (list, id) => list.find(x => x.id === id)?.name ?? '—';

    const filtered = subcategories
      .filter(s => s.name.toLowerCase().includes(search.toLowerCase()))
      .sort((a, b) => a.name.localeCompare(b.name));

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setForm({ ...form, [name]: type === 'checkbox' ? checked : value });
    };

    const handleAdd = () => {
      setEditingSubcategory(null);
      setForm({ name: '' , productTypeId: '', hasVariant: false });
      setFormOpen(true);
    };

    const handleEdit = (subcategory) => {
      setEditingSubcategory(subcategory);
      setForm({ name: subcategory.name, productTypeId: subcategory.productTypeId, hasVariant: subcategory.hasVariant });
      setFormOpen(true);
    };

    const handleSubmit = async (e) => {
      e.preventDefault();

      if (editingSubcategory) {
        const res = await fetch(`${API_BASE}/api/subcategory/${editingSubcategory.id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(form)
        });
        if (!res.ok) { alert('Failed to update'); return; }
        const updated = await res.json();
        setSubcategories(subcategories.map(s => s.id === editingSubcategory.id ? updated : s));
      } 
      else {
        const res = await fetch(`${API_BASE}/api/subcategory`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(form)
        });
        if (!res.ok) { alert('Failed to save'); return; }
        const newSubcategory = await res.json();
        setSubcategories([...subcategories, newSubcategory]);
      }
      setFormOpen(false);
    };

    const handleDelete = async (id) => {
      if (!window.confirm('Delete this subcategory?')) return;
      await fetch(`${API_BASE}/api/subcategory/${id}`, { method: 'DELETE' });
      setSubcategories(subcategories.filter(s => s.id !== id));
    };

    return (
      <div>
        <h1 className="text-3xl font-bold text-[hsl(var(--primary))]">Subcategories</h1>
        <p className="mt-1 text-[hsl(var(--muted-foreground))]">Manage your cosmetics subcategories.</p>

        <div className="mt-6 flex items-center justify-between">
          <div className="relative w-full max-w-xs">
              <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-[hsl(var(--muted-foreground))]" />
              <input
                  placeholder="Search subcategories..."
                  value={search}
                  onChange={e => setSearch(e.target.value)}
                  className="w-full rounded-lg border border-[hsl(var(--border))] py-2 pl-9 pr-3 text-sm outline-none focus:ring-2 focus:ring-[hsl(var(--primary))]"
              />
          </div>

          <button
              onClick={handleAdd}
              className="flex items-center gap-2 rounded-lg bg-[hsl(var(--primary))] px-4 py-2 text-sm font-medium text-[hsl(var(--primary-foreground))] hover:opacity-90"
          >
              <Plus className="h-4 w-4" /> Add Subcategory
          </button>
        </div>

        {formOpen && (
          <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
            <div className="w-full max-w-sm rounded-2xl bg-[hsl(var(--background))] p-6 shadow-lg">
              <div className="mb-5 flex items-center justify-between">
                <h2 className="text-lg font-semibold">{editingSubcategory ? 'Edit Subcategory' : 'Add New Subcategory'}</h2>
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
                <div>
                  <label className="mb-1.5 block text-sm font-medium">Product Type</label>
                  <select name="productTypeID" value={form.productTypeID} onChange={handleChange}
                    className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm">
                    <option value="" className="text-[hsl(var(--muted-foreground))]">Select product type</option>
                    {productTypes.map(pt => <option key={pt.id} value={pt.id}>{pt.name}</option>)}
                  </select>
                </div>
                <div className="flex items-center gap-2">
                    <input
                        type="checkbox"
                        id="hasVariant"
                        name="hasVariant"
                        checked={form.hasVariant}
                        onChange={handleChange}
                        className="h-4 w-4 rounded border-[hsl(var(--border))]"
                    />
                    <label htmlFor="hasVariant" className="text-sm font-medium">
                        Has Variants
                    </label>
                </div>


                <div className="flex justify-end gap-3 pt-2">
                  <button type="button" onClick={() => setFormOpen(false)}
                      className="rounded-lg border border-[hsl(var(--border))] px-5 py-2 text-sm">
                      Cancel
                  </button>
                  <button type="submit"
                      className="rounded-lg bg-[hsl(var(--primary))] px-5 py-2 text-sm font-medium text-[hsl(var(--primary-foreground))]">
                      {editingSubcategory ? 'Update' : 'Add Subcategory'}
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
            <p>No subcategories yet. Add your first subcategory!</p>
          </div>
        ) : (
          <div className="mt-6 overflow-hidden rounded-2xl border border-[hsl(var(--border))]">
            <table className="w-full border-collapse text-sm">
                <thead>
                <tr className="bg-[hsl(var(--primary))] text-white">
                    <th className="p-3 font-medium">Name</th>
                    <th className="p-3 font-medium">Product Type</th>
                    <th className="p-3 font-medium">Has Variants</th>
                    <th className="p-3"></th>
                </tr>
                </thead>
                <tbody>
                {filtered.map((subcategory, index) => {
                    return(
                    <tr
                        key={subcategory.id}
                        className={cn(
                        "border-t border-[hsl(var(--border))] hover:bg-[hsl(var(--muted))] transition-colors",
                        index % 2 === 1 && "bg-[hsl(var(--muted))]/30"
                        )}
                    >
                        <td className="p-3 text-center font-medium">{subcategory.name}</td>
                        <td className="p-3 text-center">{lookupName(productTypes, subcategory.productTypeID)}</td>
                        <td className="p-3 text-center">
                            <span className={cn(
                                "inline-flex items-center gap-1.5 rounded-full px-2.5 py-1 text-xs font-medium",
                                subcategory.hasVariant
                                    ? "bg-emerald-100 text-emerald-700"
                                    : "bg-red-100 text-red-700"
                            )}>
                                {subcategory.hasVariant ? 'Yes' : 'No'}
                            </span>
                        </td>
                        <td className="p-3 text-center">
                            <div className="flex gap-2">
                                <button onClick={() => handleEdit(subcategory)} className="text-[hsl(var(--muted-foreground))]">
                                <Pencil className="h-4 w-4" />
                                </button>
                                <button onClick={() => handleDelete(subcategory.id)} className="text-red-500">
                                <Trash2 className="h-4 w-4" />
                                </button>
                            </div>
                        </td>
                    </tr>
                    );
                })}
                </tbody>
            </table>
            </div>
        )}
      </div>
    );
}