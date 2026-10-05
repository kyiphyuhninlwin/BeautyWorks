import { BrowserRouter, Routes, Route, useLocation } from "react-router-dom"
import Navbar from "./components/Navbar"
import Dashboard from './pages/Dashboard';
import Products from "./pages/Products"
import Brands from "./pages/Brands"
import Categories from "./pages/Categories"
import Subcategories from "./pages/Subcategories"
import ProductTypes from "./pages/ProductTypes"
import Status from "./pages/Status"
import Variants from "./pages/Variants"
import Login from "./pages/Login"
import ProtectedRoute from './components/ProtectedRoute';

function AppContent() {
  const location = useLocation();
  const hideNavbar = location.pathname === '/login';

  return (
    <div className={hideNavbar ? '' : 'min-h-screen bg-[hsl(var(--muted))]'}>
      {!hideNavbar && <Navbar />}

      {hideNavbar ? (
        <Routes>
          <Route path="/login" element={<Login />} />
        </Routes>
      ) : (
        <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
          <Routes>
            <Route path="/" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
            <Route path="/products" element={<ProtectedRoute><Products /></ProtectedRoute>} />
            <Route path="/brands" element={<ProtectedRoute><Brands /></ProtectedRoute>} />
            <Route path="/categories" element={<ProtectedRoute><Categories /></ProtectedRoute>} />
            <Route path="/subcategories" element={<ProtectedRoute><Subcategories /></ProtectedRoute>} />
            <Route path="/product-types" element={<ProtectedRoute><ProductTypes /></ProtectedRoute>} />
            <Route path="/status" element={<ProtectedRoute><Status /></ProtectedRoute>} />
            <Route path="/variants" element={<ProtectedRoute><Variants /></ProtectedRoute>} />
          </Routes>
        </main>
      )}
    </div>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <AppContent />
    </BrowserRouter>
  );
}