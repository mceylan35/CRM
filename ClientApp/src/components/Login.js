import React, { useState, useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import { 
  Box, Container, Typography, TextField, Button, 
  Paper, Alert, CircularProgress 
} from '@mui/material';
import { authService } from '../services/api';

const Login = () => {
  const { login, error, loading } = useContext(AuthContext);
  const navigate = useNavigate();
  
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [seedLoading, setSeedLoading] = useState(false);
  const [seedMessage, setSeedMessage] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const success = await login(username, password);
    if (success) {
      navigate('/customers');
    }
  };

  const handleSeedDatabase = async () => {
    try {
      setSeedLoading(true);
      setSeedMessage(null);
      
      const result = await authService.seedDatabase();
      setSeedMessage({ 
        type: 'success', 
        message: 'Veritabanı başarıyla oluşturuldu. Şimdi "admin" kullanıcısı ve "admin123" şifresi ile giriş yapabilirsiniz.'
      });
    } catch (err) {
      setSeedMessage({ 
        type: 'error', 
        message: 'Veritabanı oluşturulurken bir hata oluştu: ' + (err.response?.data?.message || err.message)
      });
    } finally {
      setSeedLoading(false);
    }
  };

  return (
    <Container component="main" maxWidth="xs">
      <Paper elevation={3} sx={{ mt: 8, p: 4, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
        <Typography component="h1" variant="h5">
          CRM Sistemine Giriş
        </Typography>
        
        {error && (
          <Alert severity="error" sx={{ mt: 2, width: '100%' }}>
            {error}
          </Alert>
        )}
        
        {seedMessage && (
          <Alert severity={seedMessage.type} sx={{ mt: 2, width: '100%' }}>
            {seedMessage.message}
          </Alert>
        )}
        
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 3, width: '100%' }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="username"
            label="Kullanıcı Adı"
            name="username"
            autoComplete="username"
            autoFocus
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="Şifre"
            type="password"
            id="password"
            autoComplete="current-password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
            disabled={loading}
          >
            {loading ? <CircularProgress size={24} /> : 'Giriş Yap'}
          </Button>
          
          <Button
            fullWidth
            variant="outlined"
            color="secondary"
            onClick={handleSeedDatabase}
            disabled={seedLoading}
            sx={{ mt: 1 }}
          >
            {seedLoading ? <CircularProgress size={24} /> : 'Veritabanını Başlat'}
          </Button>
          
          <Typography variant="body2" color="text.secondary" align="center" sx={{ mt: 2 }}>
            İlk çalıştırmada veritabanını başlatmak için yukarıdaki butona tıklayın.
          </Typography>
        </Box>
      </Paper>
    </Container>
  );
};

export default Login; 