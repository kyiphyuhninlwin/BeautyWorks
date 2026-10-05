import {useEffect, useState} from 'react';
import { Package, Plus, Search, Trash2, Pencil, Loader2  } from 'lucide-react';
import ImageUploadForm from '@/components/ImageUploadForm.jsx';
import {cn} from "@/lib/utils";

const API_BASE = 'https://localhost:7130';

export default function Products() {
    const formatDate = (dateString) => {
        if (!dateString) return '—';
        return new Date(dateString).toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' });
    };

    const [products, setProducts] = useState([]);
    const [subcategories, setSubcategories] = useState([]);
    const [brands, setBrands] = useState([]);
    const [images, setImages] = useState([]);
    const [status, SetStatus] = useState([]);
    //const [variants, setVariants] = useState([]);

    const [search, setSearch] = useState('');
    const [formOpen, setFormOpen] = useState(false);
    const [editingProduct, setEditingProduct] = useState(null);

    const [form, setForm] = useState({
        name: '', price: '',  quantity: '', description: '', publishedDate: '', expiredDate: '', 
        subcategoryID: '', statusID: '', brandID: '', imageID: '', variantID: '', orderID: ''
    });

    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetch(`${API_BASE}/api/product`)
            .then(res => res.json()).then(data => { setProducts(data); setLoading(false); });
        fetch(`${API_BASE}/api/subcategory`)
            .then(res => res.json()).then(data => setSubcategories(data));
        fetch(`${API_BASE}/api/brand?pageSize=100`)
            .then(res => res.json()).then(data => setBrands(data));
        fetch(`${API_BASE}/api/image`)
            .then(res => res.json()).then(data => setImages(data));
        fetch(`${API_BASE}/api/status`)
            .then(res => res.json()).then(data => SetStatus(data));
        // fetch(`${API_BASE}/api/variant`)
        //     .then(res => res.json()).then(data => setVariants(data));
    }, []);

    const lookupName = (list, id) => list.find(x => x.id === id)?.name ?? '—';

    // Search filter
    const filtered = products
      .filter(p =>
        `${p.name} ${p.price} ${p.quantity} ${p.description} ${p.publishedDate} ${p.expiredDate} 
          ${p.subcategoryName} ${p.statusName} ${p.brandName} ${p.imageName} ${p.variantName}`
        .toLowerCase().includes(search.toLowerCase()))
      .sort((a, b) => a.name.localeCompare(b.name));

      const [variantInput, setVariantInput] = useState('');

      const addVariant = () => {
        const trimmed = variantInput.trim();
        if (trimmed && !form.variants.includes(trimmed)) {
          setForm({ ...form, variants: [...form.variants, trimmed] });
          setVariantInput('');
        }
      };

      const removeVariant = (name) => {
        setForm({ ...form, variants: form.variants.filter(v => v !== name) });
      };

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleAdd = () => {
        setEditingProduct(null);
        setForm({ name: '', price: '', quantity: '', description: '', publishedDate: '', expiredDate: '', 
          subcategoryID: '', statusID: '', brandID: '', imageID: '', variantID: '', orderID: '' });
        setFormOpen(true);
    };

    const handleEdit = (product) => {
      console.log('Full product object:', product);
    console.log('product.variants specifically:', product.variants);
        setEditingProduct(product);
        setForm({
          ...product,
          publishedDate: product.publishedDate ? product.publishedDate.split('T')[0] : '',
          expiredDate: product.expiredDate ? product.expiredDate.split('T')[0] : '',
          variants: product.variants ? product.variants.map(v => v.name) : [],
          imageID: product.imageID ?? '',
          brandID: product.brandID ?? '',
          subcategoryID: product.subcategoryID ?? '',
          statusID: product.statusID ?? '',
        });
        setShowUpload(false);
        setFormOpen(true);
    };

      const handleSubmit = async (e) => {
        e.preventDefault();

        const payload = {
          ...form,
          variants: undefined, // remove old key
          variantNames: subcategoryHasVariant ? form.variants : []
        };

        console.log('Payload being sent:', payload);

        if (editingProduct) {
            // Edit existing product
            const res = await fetch(`${API_BASE}/api/product/${editingProduct.id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (!res.ok) { alert('Failed to update'); return; }
            const updated = await res.json();
            setProducts(products.map(p => p.id === editingProduct.id ? updated : p));
        } 
        else {
            // Add new product
            const res = await fetch(`${API_BASE}/api/product`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (!res.ok) { alert('Failed to save'); return; }
            const newProduct = await res.json();
            setProducts([...products, newProduct]);
        }
        setFormOpen(false);
      };

    const handleDelete = async (id) => {
        if (!window.confirm('Delete this product?')) return;
        await fetch(`${API_BASE}/api/product/${id}`, {
        method: 'DELETE'
        });
        setProducts(products.filter(p => p.id !== id));
    };

    // Find the currently selected subcategory object
    const selectedSubcategory = subcategories.find(s => s.id === Number(form.subcategoryID));
    const subcategoryHasVariant = selectedSubcategory?.hasVariant === true;

    const [showUpload, setShowUpload] = useState(false);

    return (
    <div>
      <h1 className="text-3xl font-bold text-[hsl(var(--primary))]">Products</h1>
      <p className="mt-1 text-[hsl(var(--muted-foreground))]">Full cosmetics catalog.</p>

      {/* Search + Add */}
      <div className="mt-6 flex items-center justify-between">
        <div className="relative w-full max-w-xs">
          <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-[hsl(var(--muted-foreground))]" />
          <input
            placeholder="Search products..."
            value={search}
            onChange={e => setSearch(e.target.value)}
            className="w-full rounded-lg border border-[hsl(var(--border))] py-2 pl-9 pr-3 text-sm outline-none focus:ring-2 focus:ring-[hsl(var(--primary))]"
          />
        </div>

        <button
          onClick={handleAdd}
          className="flex items-center gap-2 rounded-lg bg-[hsl(var(--primary))] px-4 py-2 text-sm font-medium text-[hsl(var(--primary-foreground))] hover:opacity-90"
        >
          <Plus className="h-4 w-4" /> Add Product
        </button>
      </div>

      {/* Modal */}
      {formOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-6">
          <div className="max-h-[100vh] w-full max-w-2xl overflow-y-auto rounded-2xl bg-[hsl(var(--background))] p-8 shadow-lg">
            <div className="mb-5 flex items-center justify-between">
              <h2 className="text-lg font-semibold">{editingProduct ? 'Edit Product' : 'Add New Product'}</h2>
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
              <div className="grid grid-cols-3 gap-4">
                <div>
                  <label className="mb-1.5 block text-sm font-medium">
                    Order ID <span className="text-red-500">*</span>
                  </label>
                  <input name="orderID" type="number" value={form.orderID} onChange={handleChange} required
                    className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm" />
                </div>
                <div>
                  <label className="mb-1.5 block text-sm font-medium">Price</label>
                  <input name="price" type="number" value={form.price} onChange={handleChange}
                  className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm" />
                </div>
                <div>
                  <label className="mb-1.5 block text-sm font-medium">Quantity</label>
                  <input name="quantity" type="number" value={form.quantity} onChange={handleChange}
                  className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm" />
                </div>
              </div>

              <div>
                <label className="mb-1.5 block text-sm font-medium">Description</label>
                <textarea name="description" value={form.description} onChange={handleChange} rows={2}
                  className="w-full resize-y rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm" />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="mb-1.5 block text-sm font-medium">Published Date</label>
                  <input name="publishedDate" type="date" value={form.publishedDate} onChange={handleChange}
                  className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm"/>
                </div>
                <div>
                  <label className="mb-1.5 block text-sm font-medium">Expired Date</label>
                  <input name="expiredDate" type="date" value={form.expiredDate} onChange={handleChange}
                  className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm"/>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="mb-1.5 block text-sm font-medium">Subcategory</label>
                  <select name="subcategoryID" value={form.subcategoryID} onChange={handleChange}
                    className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm">
                    <option value="" className="text-[hsl(var(--muted-foreground))]">Select subcategory</option>
                    {subcategories.map(s => <option key={s.id} value={s.id}>{s.name}</option>)}
                  </select>
                </div>
                <div>
                  <label className="mb-1.5 block text-sm font-medium">Brand</label>
                  <select name="brandID" value={form.brandID} onChange={handleChange}
                    className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm">
                    <option value="">Select brand</option>
                    {brands.map(b => <option key={b.id} value={b.id}>{b.name}</option>)}
                  </select>
                </div>
              </div>

              <div>
                <label className="mb-1.5 block text-sm font-medium">Status</label>
                <select name="statusID" value={form.statusID} onChange={handleChange}
                  className="w-full rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm">
                  <option value="">Select status</option>
                  {status.map(s => <option key={s.id} value={s.id}>{s.name}</option>)}
                </select>
              </div>

              {subcategoryHasVariant && (
                <div>
                  <label className="mb-1.5 block text-sm font-medium">
                    Variants <span className="text-red-500">*</span>
                  </label>
                  <div className="mb-2 flex flex-wrap gap-2">
                    {form.variants.map(name => (
                      <span key={name} className="inline-flex items-center gap-1 rounded-full bg-[hsl(var(--accent))] px-3 py-1 text-sm">
                        {name}
                        <button type="button" onClick={() => removeVariant(name)} className="text-[hsl(var(--muted-foreground))]">✕</button>
                      </span>
                    ))}
                  </div>
                  <div className="flex gap-2">
                    <input
                      value={variantInput}
                      onChange={e => setVariantInput(e.target.value)}
                      onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); addVariant(); } }}
                      placeholder="Type a shade name..."
                      className="flex-1 rounded-lg border border-[hsl(var(--border))] px-3 py-2 text-sm"
                    />
                    <button type="button" onClick={addVariant}
                      className="rounded-lg bg-[hsl(var(--primary))] px-4 py-2 text-sm text-white">
                      Add
                    </button>
                  </div>
                </div>
              )}

              {editingProduct && (
                <div className="mt-4">
                  <label className="mb-1.5 block text-sm font-medium">Image</label>
                  {(() => {
                    const currentImage = images.find(img => img.id === form.imageID);
                    console.log('Image Url:', currentImage?.url);
                    return currentImage ? (
                      <div className="mb-3 flex items-center gap-3">
                        <img
                          src={currentImage.url}
                          alt={currentImage.name}
                          className="h-16 w-16 rounded-lg border border-[hsl(var(--border))] object-cover"
                        />
                        <div className="flex-1">
                          <p className="text-sm">{currentImage.name}</p>
                          <button type="button" onClick={() => setShowUpload(true)}
                            className="text-xs font-medium text-[hsl(var(--primary))] underline">
                            Change Image
                          </button>
                        </div>
                      </div>
                    ) : (
                      <p className="mb-3 text-sm text-[hsl(var(--muted-foreground))]">No image uploaded yet.</p>
                    );
                  })()}

                  {(showUpload || !form.imageID) && (
                    <ImageUploadForm
                      productID={editingProduct.id}
                      onUploaded={(newImage) => {
                        setImages([...images, newImage]);
                        setForm({ ...form, imageID: newImage.id });
                        setShowUpload(false);
                      }}
                    />
                  )}
                </div>
              )}

              <div className="flex justify-end gap-3 pt-2">
                <button type="button" onClick={() => setFormOpen(false)}
                  className="rounded-lg border border-[hsl(var(--border))] px-5 py-2 text-sm">
                  Cancel
                </button>
                <button type="submit"
                  className="rounded-lg bg-[hsl(var(--primary))] px-5 py-2 text-sm font-medium text-[hsl(var(--primary-foreground))]">
                  {editingProduct ? 'Update' : 'Add Product'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Table */}
      {loading ? (
        <div className="flex justify-center py-12">
          <Loader2 className="h-8 w-8 animate-spin text-[hsl(var(--primary))]" />
        </div>
      ) : filtered.length === 0 ? (
        <div className="mt-10 flex flex-col items-center py-12 text-[hsl(var(--muted-foreground))]">
          <Package className="mb-2 h-10 w-10" />
          <p>No products yet. Add your first product!</p>
        </div>
      ) : (
        <div className="mt-6 overflow-hidden rounded-2xl border border-[hsl(var(--border))]">
          <table className="w-full border-collapse text-sm">
            <thead>
              <tr className="bg-[hsl(var(--primary))] text-white">
                <th className="p-3 font-medium">Brand</th>
                <th className="p-3 font-medium">Name</th>
                <th className="p-3 font-medium">Price</th>
                <th className="p-3 font-medium">Published Date</th>
                <th className="p-3 font-medium">Expired Date</th>
                <th className="p-3 font-medium">Subcategory</th>
                <th className="p-3 font-medium">Status</th>
                <th className="p-3"></th>
              </tr>
            </thead>
            <tbody>
              {filtered.map((product, index) => {
                const statusColor = {
                  'In Stock': 'bg-emerald-100 text-emerald-700',
                  'Out Of Stock': 'bg-red-100 text-red-700',
                  'Preorder': 'bg-amber-100 text-amber-700',
                }[lookupName(status, product.statusID)] || 'bg-[hsl(var(--accent))] text-[hsl(var(--accent-foreground))]';

                return (
                  <tr
                    key={product.id}
                    className={cn(
                      "border-t border-[hsl(var(--border))] hover:bg-[hsl(var(--muted))] transition-colors",
                      index % 2 === 1 && "bg-[hsl(var(--muted))]/30"
                    )}
                  >
                    <td className="p-3 text-center font-medium">{lookupName(brands, product.brandID)}</td>
                    <td className="p-3 text-center font-medium">{product.name}</td>
                    <td className="p-3 text-center">{product.price != null ? `$${Number(product.price).toFixed(2)}` : '—'}</td>
                    <td className="p-3 text-center">{formatDate(product.publishedDate)}</td>
                    <td className="p-3 text-center">{formatDate(product.expiredDate)}</td>
                    <td className="p-3 text-center">{lookupName(subcategories, product.subcategoryID)}</td>
                    <td className="p-3 text-center">
                      <span className={cn("inline-flex items-center gap-1.5 rounded-full px-2.5 py-1 text-xs font-medium", statusColor)}>
                        {lookupName(status, product.statusID)}
                      </span>
                    </td>
                    <td className="p-3 text-center">
                      <div className="flex gap-2">
                        <button onClick={() => handleEdit(product)} className="text-[hsl(var(--muted-foreground))]">
                          <Pencil className="h-4 w-4" />
                        </button>
                        <button onClick={() => handleDelete(product.id)} className="text-red-500">
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
