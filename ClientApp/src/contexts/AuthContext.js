import React, { createContext, useState, useEffect } from 'react';
import { authService } from '../services/api';
import jwt_decode from 'jwt-decode';

export const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [currentUser, setCurrentUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    // Sayfa yüklendiğinde localStorage'dan token kontrolü
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decodedToken = jwt_decode(token);
        const currentTime = Date.now() / 1000;
        
        if (decodedToken.exp < currentTime) {
          // Token süresi dolmuşsa
          logout();
        } else {
          // Token geçerliyse kullanıcı bilgilerini ayarla
          setCurrentUser({
            username: decodedToken.name,
            role: decodedToken.role,
            token: token
          });
        }
      } catch (err) {
        console.error('Token çözümlenemedi:', err);
        logout();
      }
    }
    setLoading(false);
  }, []);

  const login = async (username, password) => {
    try {
      setError(null);
      setLoading(true);
      const response = await authService.login({ username, password });
      
      // Token'ı localStorage'a kaydet
      localStorage.setItem('token', response.token);
      
      // Kullanıcı bilgilerini ayarla
      setCurrentUser({
        username: response.username,
        role: response.role,
        token: response.token
      });
      
      return true;
    } catch (err) {
      setError(err.response?.data?.error || 'Giriş sırasında bir hata oluştu');
      return false;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    setCurrentUser(null);
  };

  // Auth context değerleri
  const value = {
    currentUser,
    loading,
    error,
    login,
    logout
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}; 