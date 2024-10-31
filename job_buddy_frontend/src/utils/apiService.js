import axios from 'axios';

const apiService = axios.create({
  baseURL: process.env.REACT_APP_API_URL || 'https://localhost:7113', //Backend url goes here
});

// Request Interceptor for attaching JWT Token to each request
apiService.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken'); // Retrieve token from local storage
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`; // Set Authorization header
    }
    return config;
  },
  (error) => {
    return Promise.reject(error); // Handle any errors
  }
);

// Response Interceptor for handling errors 
apiService.interceptors.response.use(
  (response) => response, // Pass through successful responses
  (error) => {
    // Extract the error response
    const { response } = error;

    if (response) {
      if (response.status === 401) {
        // Handle Unauthorized responses 
        localStorage.removeItem('token'); // Clear the JWT token
        window.location.href = '/login'; // Redirect to login page
      } else if (response.status === 403) {
        // Handle Forbidden responses 
        alert("You don't have permission to access this resource.");
      }
    }

    // Return a standard error
    return Promise.reject({
      message: response?.data?.message || "An error occurred.",
      errors: response?.data?.errors || [],
      status: response?.status,
    });
  }
);

export default apiService;
