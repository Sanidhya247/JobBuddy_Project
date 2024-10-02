import React, { createContext, useState, useEffect, useCallback } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const AuthContext = createContext();

//Declare the states
export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [token, setToken] = useState(localStorage.getItem("authToken") || "");

  const navigate = useNavigate();

  // Define backend URL using environment variables
  const BASE_URL = process.env.REACT_APP_API_URL || "https://localhost:7113";

  // Configure Axios instance for authentication-related API calls
  const axiosInstance = axios.create({
    baseURL: BASE_URL,
  });

  // Logout function to clear state and navigate to login
  const logout = useCallback(() => {
    setUser(null);
    setToken("");
    localStorage.removeItem("authToken");
    navigate("/login");
  }, [navigate]);

  // Effect to check user authentication status on initial load
  useEffect(() => {
    const checkAuthenticatedUser = async () => {
      try {
        if (token) {
          const { data } = await axiosInstance.get("/api/auth/verify-token", {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
          setUser(data.user);
        }
      } catch (err) {
        setError("Invalid token. Please log in again.");
        logout();
      } finally {
        setLoading(false);
      }
    };
    checkAuthenticatedUser();
  }, [token, logout, axiosInstance]);

  // Register function 
  const register = async (registerData) => {
    try {
      const { data } = await axiosInstance.post("/api/auth/register", registerData);
      if (data.success) {
        setUser(data.data);
        setError(null);
      } else {
        throw new Error(data.message);
      }
    } catch (err) {
      throw new Error(err.response?.data?.message || "Registration failed. Please try again.");
    }
  };

  // Login function 
  const login = async (loginData) => {
    try {
      const { data } = await axiosInstance.post("/api/auth/login", loginData);
      if (data.success) {
        setToken(data.data); //set token 
        localStorage.setItem("authToken", data.data); //add token to local storage
        setUser(null);
        setError(null);
      } else {
        throw new Error(data.message);
      }
    } catch (err) {
      throw new Error(err.response?.data?.message || "Login failed. Please try again.");
    }
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        loading,
        error,
        register,
        login,
        logout,
        setError,
        setUser,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
