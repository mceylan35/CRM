import axios from 'axios';

const API_URL = 'http://localhost:5170/api';

// Token'ı localStorage'dan alma
const getAuthToken = () => {
  return localStorage.getItem('token');
};

// API istekleri için temel ayarlar
const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Her istekte token'ı header'a ekleme
api.interceptors.request.use(
  (config) => {
    const token = getAuthToken();
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Auth servisleri
export const authService = {
  login: async (credentials) => {
    const response = await api.post('/auth/login', credentials);
    return response.data;
  },
  seedDatabase: async () => {
    const response = await api.post('/seed');
    return response.data;
  }
};

// Customer servisleri
export const customerService = {
  getAll: async () => {
    const response = await api.get('/customers');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/customers/${id}`);
    return response.data;
  },
  getByRegion: async (region) => {
    const response = await api.get(`/customers/region/${region}`);
    return response.data;
  },
  create: async (customer) => {
    const response = await api.post('/customers', customer);
    return response.data;
  },
  update: async (id, customer) => {
    const response = await api.put(`/customers/${id}`, customer);
    return response.data;
  },
  delete: async (id) => {
    const response = await api.delete(`/customers/${id}`);
    return response.data;
  }
}; 