import React, { useState, useEffect } from 'react';
import { 
  Container, Typography, Box, Button, TextField, 
  CircularProgress, Alert, Dialog, DialogTitle, 
  DialogContent, DialogActions, MenuItem, Select, InputLabel, 
  FormControl, Paper
} from '@mui/material';
import { DataGrid } from '@mui/x-data-grid';
import { customerService } from '../services/api';
import { format } from 'date-fns';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

const CustomerList = () => {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchRegion, setSearchRegion] = useState('');
  const [isAddDialogOpen, setIsAddDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    region: ''
  });

  const fetchCustomers = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await customerService.getAll();
      setCustomers(data);
    } catch (err) {
      setError('Müşteri verileri yüklenirken bir hata oluştu: ' + (err.response?.data?.error || err.message));
    } finally {
      setLoading(false);
    }
  };

  const searchCustomersByRegion = async () => {
    if (!searchRegion.trim()) {
      fetchCustomers();
      return;
    }

    try {
      setLoading(true);
      setError(null);
      const data = await customerService.getByRegion(searchRegion);
      setCustomers(data);
    } catch (err) {
      setError('Müşteri arama sırasında bir hata oluştu: ' + (err.response?.data?.error || err.message));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCustomers();
  }, []);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const resetFormData = () => {
    setFormData({
      firstName: '',
      lastName: '',
      email: '',
      region: ''
    });
  };

  const handleAddCustomer = () => {
    resetFormData();
    setIsAddDialogOpen(true);
  };

  const handleEditCustomer = (customer) => {
    setSelectedCustomer(customer);
    setFormData({
      firstName: customer.firstName,
      lastName: customer.lastName,
      email: customer.email,
      region: customer.region
    });
    setIsEditDialogOpen(true);
  };

  const handleDeleteCustomer = (customer) => {
    setSelectedCustomer(customer);
    setIsDeleteDialogOpen(true);
  };

  const submitAddCustomer = async () => {
    try {
      setLoading(true);
      await customerService.create(formData);
      setIsAddDialogOpen(false);
      resetFormData();
      await fetchCustomers();
    } catch (err) {
      setError('Müşteri eklenirken bir hata oluştu: ' + (err.response?.data?.error || err.message));
    } finally {
      setLoading(false);
    }
  };

  const submitEditCustomer = async () => {
    try {
      setLoading(true);
      await customerService.update(selectedCustomer.id, {
        id: selectedCustomer.id,
        ...formData
      });
      setIsEditDialogOpen(false);
      resetFormData();
      await fetchCustomers();
    } catch (err) {
      setError('Müşteri güncellenirken bir hata oluştu: ' + (err.response?.data?.error || err.message));
    } finally {
      setLoading(false);
    }
  };

  const submitDeleteCustomer = async () => {
    try {
      setLoading(true);
      await customerService.delete(selectedCustomer.id);
      setIsDeleteDialogOpen(false);
      await fetchCustomers();
    } catch (err) {
      setError('Müşteri silinirken bir hata oluştu: ' + (err.response?.data?.error || err.message));
    } finally {
      setLoading(false);
    }
  };

  const columns = [
    { field: 'id', headerName: 'ID', width: 70 },
    { field: 'firstName', headerName: 'Ad', width: 130 },
    { field: 'lastName', headerName: 'Soyad', width: 130 },
    { field: 'fullName', headerName: 'Tam Ad', width: 180 },
    { field: 'email', headerName: 'E-posta', width: 220 },
    { field: 'region', headerName: 'Bölge', width: 150 },
    { 
      field: 'registrationDate', 
      headerName: 'Kayıt Tarihi', 
      width: 180,
      valueFormatter: (params) => {
        return format(new Date(params.value), 'dd/MM/yyyy HH:mm');
      }
    },
    {
      field: 'actions',
      headerName: 'İşlemler',
      width: 150,
      renderCell: (params) => (
        <Box>
          <Button
            size="small"
            onClick={() => handleEditCustomer(params.row)}
            startIcon={<EditIcon />}
          >
            Düzenle
          </Button>
          <Button
            size="small"
            color="error"
            onClick={() => handleDeleteCustomer(params.row)}
            startIcon={<DeleteIcon />}
          >
            Sil
          </Button>
        </Box>
      )
    }
  ];

  return (
    <Container maxWidth="lg">
      <Typography variant="h4" component="h1" gutterBottom sx={{ mt: 4, mb: 4 }}>
        Müşteri Yönetimi
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <Paper elevation={3} sx={{ p: 3, mb: 3 }}>
        <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
          <Typography variant="h6">Müşteri Listesi</Typography>
          <Button
            variant="contained"
            color="primary"
            startIcon={<AddIcon />}
            onClick={handleAddCustomer}
          >
            Yeni Müşteri Ekle
          </Button>
        </Box>

        <Box display="flex" mb={3}>
          <TextField
            label="Bölgeye Göre Ara"
            variant="outlined"
            size="small"
            value={searchRegion}
            onChange={(e) => setSearchRegion(e.target.value)}
            sx={{ mr: 1, flexGrow: 1 }}
          />
          <Button
            variant="outlined"
            onClick={searchCustomersByRegion}
          >
            Ara
          </Button>
          <Button
            variant="text"
            onClick={fetchCustomers}
            sx={{ ml: 1 }}
          >
            Tümünü Göster
          </Button>
        </Box>

        <div style={{ height: 400, width: '100%' }}>
          {loading ? (
            <Box display="flex" justifyContent="center" alignItems="center" height="100%">
              <CircularProgress />
            </Box>
          ) : (
            <DataGrid
              rows={customers}
              columns={columns}
              pageSize={5}
              rowsPerPageOptions={[5, 10, 20]}
              checkboxSelection
              disableSelectionOnClick
            />
          )}
        </div>
      </Paper>

      {/* Müşteri Ekleme Dialog */}
      <Dialog open={isAddDialogOpen} onClose={() => setIsAddDialogOpen(false)}>
        <DialogTitle>Yeni Müşteri Ekle</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            id="firstName"
            name="firstName"
            label="Ad"
            type="text"
            fullWidth
            variant="outlined"
            value={formData.firstName}
            onChange={handleInputChange}
          />
          <TextField
            margin="dense"
            id="lastName"
            name="lastName"
            label="Soyad"
            type="text"
            fullWidth
            variant="outlined"
            value={formData.lastName}
            onChange={handleInputChange}
          />
          <TextField
            margin="dense"
            id="email"
            name="email"
            label="E-posta"
            type="email"
            fullWidth
            variant="outlined"
            value={formData.email}
            onChange={handleInputChange}
          />
          <FormControl fullWidth margin="dense">
            <InputLabel id="region-label">Bölge</InputLabel>
            <Select
              labelId="region-label"
              id="region"
              name="region"
              value={formData.region}
              label="Bölge"
              onChange={handleInputChange}
            >
              <MenuItem value="North America">Kuzey Amerika</MenuItem>
              <MenuItem value="South America">Güney Amerika</MenuItem>
              <MenuItem value="Europe">Avrupa</MenuItem>
              <MenuItem value="Asia">Asya</MenuItem>
              <MenuItem value="Africa">Afrika</MenuItem>
              <MenuItem value="Australia">Avustralya</MenuItem>
            </Select>
          </FormControl>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setIsAddDialogOpen(false)}>İptal</Button>
          <Button onClick={submitAddCustomer} color="primary">Ekle</Button>
        </DialogActions>
      </Dialog>

      {/* Müşteri Düzenleme Dialog */}
      <Dialog open={isEditDialogOpen} onClose={() => setIsEditDialogOpen(false)}>
        <DialogTitle>Müşteri Düzenle</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            id="firstName"
            name="firstName"
            label="Ad"
            type="text"
            fullWidth
            variant="outlined"
            value={formData.firstName}
            onChange={handleInputChange}
          />
          <TextField
            margin="dense"
            id="lastName"
            name="lastName"
            label="Soyad"
            type="text"
            fullWidth
            variant="outlined"
            value={formData.lastName}
            onChange={handleInputChange}
          />
          <TextField
            margin="dense"
            id="email"
            name="email"
            label="E-posta"
            type="email"
            fullWidth
            variant="outlined"
            value={formData.email}
            onChange={handleInputChange}
          />
          <FormControl fullWidth margin="dense">
            <InputLabel id="region-label">Bölge</InputLabel>
            <Select
              labelId="region-label"
              id="region"
              name="region"
              value={formData.region}
              label="Bölge"
              onChange={handleInputChange}
            >
              <MenuItem value="North America">Kuzey Amerika</MenuItem>
              <MenuItem value="South America">Güney Amerika</MenuItem>
              <MenuItem value="Europe">Avrupa</MenuItem>
              <MenuItem value="Asia">Asya</MenuItem>
              <MenuItem value="Africa">Afrika</MenuItem>
              <MenuItem value="Australia">Avustralya</MenuItem>
            </Select>
          </FormControl>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setIsEditDialogOpen(false)}>İptal</Button>
          <Button onClick={submitEditCustomer} color="primary">Güncelle</Button>
        </DialogActions>
      </Dialog>

      {/* Müşteri Silme Dialog */}
      <Dialog open={isDeleteDialogOpen} onClose={() => setIsDeleteDialogOpen(false)}>
        <DialogTitle>Müşteriyi Sil</DialogTitle>
        <DialogContent>
          <Typography>
            {selectedCustomer?.fullName} isimli müşteriyi silmek istediğinizden emin misiniz?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setIsDeleteDialogOpen(false)}>İptal</Button>
          <Button onClick={submitDeleteCustomer} color="error">Sil</Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default CustomerList; 