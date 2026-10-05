import { useState, useEffect } from "react";
import { Package, Sparkles, ChartColumnStacked, Layers, PackageSearch, Image  } from "lucide-react";
import { cn } from "@/lib/utils";

const API_BASE = 'https://localhost:7130';

export default function Dashboard() {
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [producttypes, setProductTypes] = useState([]);
  const [subcategories, setSubcategories] = useState([]);
  const [brands, setBrands] = useState([]);
  const [images, setImages] = useState([]);

  useEffect(() => {
    fetch(`${API_BASE}/api/product`)
      .then(res => res.json()).then(data => setProducts(data));
    fetch(`${API_BASE}/api/categories`)
      .then(res => res.json()).then(data => setCategories(data));
    fetch(`${API_BASE}/api/producttype`)
      .then(res => res.json()).then(data => setProductTypes(data));
    fetch(`${API_BASE}/api/subcategory`)
      .then(res => res.json()).then(data => setSubcategories(data));
    fetch(`${API_BASE}/api/brand`)
      .then(res => res.json()).then(data => setBrands(data));
    fetch(`${API_BASE}/api/image`)
      .then(res => res.json()).then(data => setImages(data));
  }, []);

  const stats = [
    { label: "Products", value: products.length, icon: Package, color: "text-rose-500 bg-rose-50" },
    { label: "Brands", value: brands.length, icon: Sparkles, color: "text-violet-500 bg-violet-50" },
    { label: "Categories", value: categories.length, icon: ChartColumnStacked , color: "text-amber-500 bg-amber-50" },
    { label: "Subcategories", value: subcategories.length, icon: Layers, color: "text-sky-500 bg-sky-50" },
    { label: "Product Types", value: producttypes.length, icon: PackageSearch, color: "text-emerald-500 bg-emerald-50" },
    { label: "Images", value: images.length, icon: Image , color: "text-pink-500 bg-pink-50" },
  ];

  return (
    <div>
      <h1 className="text-3xl font-bold text-[hsl(var(--primary))]">Dashboard</h1>
      <p className="mt-1 text-[hsl(var(--muted-foreground))]">
        Overview of your cosmetics catalog at a glance.
      </p>

      <div className="mt-8 grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {stats.map((stat) => (
          <div
            key={stat.label}
            className="rounded-2xl border border-[hsl(var(--border))] bg-[hsl(var(--background))] p-6 shadow-sm"
          >
            <span className={cn("flex h-11 w-11 items-center justify-center rounded-xl", stat.color)}>
              <stat.icon className="h-5 w-5" />
            </span>
            <p className="mt-4 text-3xl font-bold">{stat.value}</p>
            <p className="text-sm text-[hsl(var(--muted-foreground))]">{stat.label}</p>
          </div>
        ))}
      </div>

      <div className="mt-8 rounded-2xl border border-[hsl(var(--border))] bg-[hsl(var(--background))] p-6 shadow-sm">
        <div className="flex items-center justify-between">
          <h2 className="text-lg font-semibold text-[hsl(var(--primary))]">Recently added products</h2>
          <a href="/products" className="text-sm font-medium text-[hsl(var(--primary))]">
            View all
          </a>
        </div>

        <div className="mt-4 divide-y divide-[hsl(var(--border))]">
          {products.length === 0 && (
            <p className="py-4 text-sm text-[hsl(var(--muted-foreground))]">No products yet.</p>
          )}
          {products.slice(-5).reverse().map((p) => (
            <div key={p.id} className="flex items-center justify-between py-3">
              <div>
                <p>{p.name}</p>
                <p className="text-sm text-[hsl(var(--muted-foreground))]">{p.description}</p>
              </div>
              <p>${p.price}</p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}