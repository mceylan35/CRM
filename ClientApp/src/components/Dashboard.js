import React, { useState, useEffect, useContext } from 'react';
import { Container, Typography, Grid, Paper, Box, CircularProgress, Alert } from '@mui/material';
import { customerService } from '../services/api';
import { AuthContext } from '../contexts/AuthContext';
import PeopleIcon from '@mui/icons-material/People';
import PublicIcon from '@mui/icons-material/Public';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import { format } from 'date-fns';

const Dashboard = () => {
  const { currentUser } = useContext(AuthContext);
  const [customerStats, setCustomerStats] = useState({
    total: 0,
    byRegion: {},
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        setError(null);

        const customers = await customerService.getAll();
        
        // Bölgeye göre müşteri sayısını hesapla
        const regionMap = {};
        customers.forEach(customer => {
          if (!regionMap[customer.region]) {
            regionMap[customer.region] = 0;
          }
          regionMap[customer.region]++;
        });

        setCustomerStats({
          total: customers.length,
          byRegion: regionMap
        });
      } catch (err) {
        setError('Veri yüklenirken bir hata oluştu: ' + (err.response?.data?.error || err.message));
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="80vh">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Container maxWidth="lg">
      <Typography variant="h4" gutterBottom sx={{ mt: 4, mb: 4 }}>
        Merhaba, {currentUser.username}
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      <Grid container spacing={3}>
        {/* Toplam müşteri sayısı kartı */}
        <Grid item xs={12} md={4}>
          <Paper
            sx={{
              p: 3,
              display: 'flex',
              flexDirection: 'column',
              height: 140,
              backgroundColor: '#e3f2fd',
            }}
          >
            <Box display="flex" alignItems="center">
              <PeopleIcon sx={{ fontSize: 40, color: '#1976d2', mr: 2 }} />
              <Typography variant="h5" component="div">
                Toplam Müşteri
              </Typography>
            </Box>
            <Typography variant="h3" component="div" sx={{ mt: 2, fontWeight: 'bold' }}>
              {customerStats.total}
            </Typography>
          </Paper>
        </Grid>

        {/* Bölge sayısı kartı */}
        <Grid item xs={12} md={4}>
          <Paper
            sx={{
              p: 3,
              display: 'flex',
              flexDirection: 'column',
              height: 140,
              backgroundColor: '#e8f5e9',
            }}
          >
            <Box display="flex" alignItems="center">
              <PublicIcon sx={{ fontSize: 40, color: '#388e3c', mr: 2 }} />
              <Typography variant="h5" component="div">
                Toplam Bölge
              </Typography>
            </Box>
            <Typography variant="h3" component="div" sx={{ mt: 2, fontWeight: 'bold' }}>
              {Object.keys(customerStats.byRegion).length}
            </Typography>
          </Paper>
        </Grid>

        {/* Tarih kartı */}
        <Grid item xs={12} md={4}>
          <Paper
            sx={{
              p: 3,
              display: 'flex',
              flexDirection: 'column',
              height: 140,
              backgroundColor: '#fff8e1',
            }}
          >
            <Box display="flex" alignItems="center">
              <CalendarTodayIcon sx={{ fontSize: 40, color: '#ffa000', mr: 2 }} />
              <Typography variant="h5" component="div">
                Bugün
              </Typography>
            </Box>
            <Typography variant="h5" component="div" sx={{ mt: 2 }}>
              {format(new Date(), 'dd MMMM yyyy')}
            </Typography>
          </Paper>
        </Grid>

        {/* Bölgelere göre müşteri dağılımı */}
        <Grid item xs={12}>
          <Paper sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Bölgelere Göre Müşteri Dağılımı
            </Typography>
            <Grid container spacing={2} sx={{ mt: 2 }}>
              {Object.entries(customerStats.byRegion).map(([region, count]) => (
                <Grid item xs={12} sm={6} md={4} key={region}>
                  <Paper sx={{ p: 2, backgroundColor: '#f5f5f5' }}>
                    <Typography variant="body1">{region}</Typography>
                    <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
                      {count} Müşteri
                    </Typography>
                  </Paper>
                </Grid>
              ))}
            </Grid>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
};

export default Dashboard; 