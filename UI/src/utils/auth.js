export function saveToken(token) {
  localStorage.setItem('token', token);
}

export function getToken() {
  return localStorage.getItem('token');
}

export function removeToken() {
  localStorage.removeItem('token');
}

export function isLoggedIn() {
  return !!getToken();
}

// Helper to build fetch headers with the token attached
export function authHeaders() {
  const token = getToken();
  return token ? { 'Authorization': `Bearer ${token}` } : {};
}

export function getUserFromToken() {
  const token = getToken();
  if (!token) return null;

  try {
    const payload = token.split('.')[1]; // JWT = header.payload.signature
    const decoded = JSON.parse(atob(payload));
    return {
      email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || decoded.email,
      role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decoded.role
    };
  } catch (err) {
    return null;
  }
}