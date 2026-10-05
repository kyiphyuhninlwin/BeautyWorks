import {useEffect, useState} from 'react';
import { Sparkles, Plus, Search, Trash2, Pencil, Loader2  } from 'lucide-react';
//import {cn} from "@/lib/utils";

const API_BASE = 'https://localhost:7130';

export default function Categories() {
    const [categories, setCategories] = useState([]);
    const [editingCategory, setEditingCategory] = useState(null);

    const [search, setSearch] = useState('');
    const [formOpen, setFormOpen] = useState(false);

    const [form, setForm] = useState({
        name: '', productTypeId: ''
    });

    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetch(`${API_BASE}/api/categories`)
            .then(res => res.json()).then(data => { setCategories(data); setLoading(false); });
    }, []);

    // Search filter
    const filtered = categories.filter(c =>
        c.name.toLowerCase().includes(search.toLowerCase())
    );

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleAdd = () => {
        setEditingCategory(null);
        setForm({ name: '', productTypeId: '' });
        setFormOpen(true);
    };

    const handleEdit = (category) => {
        setEditingCategory(category);
        setForm({
          ...category
        });
        setFormOpen(true);
    };

    const handleSubmit = async (e) => {
      e.preventDefault();

      if (editingCategory) {
          // Edit existing category
          const res = await fetch(`${API_BASE}/api/categories/${editingCategory.id}`, {
              method: 'PUT',
              headers: { 'Content-Type': 'application/json' },
              body: JSON.stringify(form)
          });
          if (!res.ok) { alert('Failed to update'); return; }
          const updated = await res.json();
          setCategories(categories.map(c => c.id === editingCategory.id ? updated : c));
      } 
      else {
          // Add new category
          const res = await fetch(`${API_BASE}/api/categories`, {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
              body: JSON.stringify(form)
          });
          if (!res.ok) { alert('Failed to save'); return; }
          const newCategory = await res.json();
          setCategories([...categories, newCategory]);
      }
      setFormOpen(false);
    };

    const handleDelete = async (id) => {
        if (!window.confirm('Delete this category?')) return;
        await fetch(`${API_BASE}/api/categories/${id}`, {
        method: 'DELETE'
        });
        setCategories(categories.filter(c => c.id !== id));
    };

    
    return (
          <div>
            <h1 className="text-3xl font-bold text-[hsl(var(--primary))]">Categories</h1>
            <p className="mt-1 text-[hsl(var(--muted-foreground))]">Manage your cosmetics categories.</p>
    
            <div className="mt-6 flex items-center justify-between">
              <div className="relative w-full max-w-xs">
                  <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-[hsl(var(--muted-foreground))]" />
                  <input
                      placeholder="Search categories..."
                      value={search}
                      onChange={e => setSearch(e.target.value)}
                      className="w-full rounded-lg border border-[hsl(var(--border))] py-2 pl-9 pr-3 text-sm outline-none focus:ring-2 focus:ring-[hsl(var(--primary))]"
                  />
              </div>
    
              <button
                  onClick={handleAdd}
                  className="flex items-center gap-2 rounded-lg bg-[hsl(var(--primary))] px-4 py-2 text-sm font-medium text-[hsl(var(--primary-foreground))] hover:opacity-90"
              >
                  <Plus className="h-4 w-4" /> Add Category
              </button>
            </div>
    
            {formOpen && (
              <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
                <div className="w-full max-w-sm rounded-2xl bg-[hsl(var(--background))] p-6 shadow-lg">
                  <div className="mb-5 flex items-center justify-between">
                    <h2 className="text-lg font-semibold">{editingCategory ? 'Edit Category' : 'Add New Category'}</h2>
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
                          {editingCategory ? 'Update' : 'Add Category'}
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
                <p>No categories yet. Add your first category!</p>
              </div>
          ) : (
              <div className="mt-6 grid grid-cols-2 gap-3 sm:grid-cols-3 lg:grid-cols-4">
                {filtered.map(category => (
                  <div key={category.id} className="flex items-center justify-between rounded-xl border border-[hsl(var(--border))] bg-[hsl(var(--primary))]/5 p-4">
                    <span className="font-medium">{category.name}</span>
                    <div className="flex gap-2">
                      <button onClick={() => handleEdit(category)} className="text-[hsl(var(--muted-foreground))]">
                        <Pencil className="h-4 w-4" />
                      </button>
                      <button onClick={() => handleDelete(category.id)} className="text-red-500">
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
