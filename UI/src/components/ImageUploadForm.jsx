/* eslint-disable react/prop-types */
import { useState } from 'react';

const API_BASE = 'https://localhost:7130';

export default function ImageUploadForm({ productID, onUploaded }) {
  const [file, setFile] = useState(null);
  const [fileName, setFileName] = useState('');
  const [title, setTitle] = useState('');
  const [preview, setPreview] = useState(null);
  const [uploading, setUploading] = useState(false);

  const handleFileChange = (e) => {
    const selected = e.target.files[0];
    setFile(selected);
    setPreview(selected ? URL.createObjectURL(selected) : null);
    if (selected) {
      const baseName = selected.name
        .replace(/\.[^/.]+$/, '')       // strip extension
        .replace(/[^a-zA-Z0-9]+/g, '-') // replace spaces/special chars with hyphens
        .toLowerCase();
      setFileName(baseName);
      setTitle(baseName);
    }
  };

  const handleUpload = async () => {
    if (!file) return alert('Please choose a file.');
    if (!productID) return alert('Product ID is required to link this image.');

    setUploading(true);

    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', fileName);
    formData.append('title', title);
    formData.append('productID', productID);

    const res = await fetch(`${API_BASE}/api/image`, {
      method: 'POST',
      body: formData,
    });

    setUploading(false);

    if (!res.ok) {
      const err = await res.json();
      console.log('Upload error:', err);
      alert('Upload failed');
      return;
    }

    const newImage = await res.json();
    onUploaded(newImage);
    setFile(null);
    setFileName('');
    setTitle('');
    setPreview(null);
  };

  return (
    <div className="space-y-3 rounded-2xl border border-[hsl(var(--border))] p-4">
      <div>
        <label className="mb-1.5 block text-sm font-medium">Image File</label>
        <input type="file" accept=".jpg,.jpeg,.png" onChange={handleFileChange}
          className="w-full text-sm" />
      </div>

      {preview && (
        <img src={preview} alt="Preview" className="h-32 w-32 rounded-lg border border-[hsl(var(--border))] object-cover" />
      )}

      <button type="button" onClick={handleUpload} disabled={uploading}
        className="rounded-lg bg-[hsl(var(--primary))] px-4 py-2 text-sm font-medium text-[hsl(var(--primary-foreground))] disabled:opacity-50">
        {uploading ? 'Uploading...' : 'Upload Image'}
      </button>
    </div>
  );
}