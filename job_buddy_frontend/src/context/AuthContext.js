import React, { createContext, useState, useEffect, useCallback, useRef } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [token, setToken] = useState(localStorage.getItem("authToken") || "");
  const [loggedOut, setLoggedOut] = useState(false); // Track if logout was triggered
  const navigate = useNavigate();
  const hasCheckedToken = useRef(false); // Track if the token check has already been performed

  // Define backend URL using environment variables
  const BASE_URL = process.env.REACT_APP_API_URL || "https://localhost:7113";

  // Configure Axios instance for authentication-related API calls
  const axiosInstance = axios.create({
    baseURL: BASE_URL,
  });

  // Logout function
  const logout = useCallback(() => {
    localStorage.removeItem("authToken"); // Clear local storage token
    setUser(null); // Immediately set user state to null
    setToken(""); // Clear the token state
    setLoggedOut(true); // Set the loggedOut flag to true
    navigate("/login", { replace: true });
  }, [navigate]);

  // Check if user is authenticated on initial load
  useEffect(() => {
    const checkAuthenticatedUser = async () => {
      // Prevent repeated calls if already checked or during logout
      if (loggedOut || hasCheckedToken.current) return;

      try {
        if (token) {
          const { data } = await axiosInstance.get("/api/auth/verify-token", {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
          setUser(data?.data || null);
        }
      } catch (err) {
        setError("Invalid token. Please log in again.");
        logout();
      } finally {
        setLoading(false);
        hasCheckedToken.current = true; // Mark as checked to prevent re-checks
      }
    };

    checkAuthenticatedUser();
  }, [token, logout, axiosInstance, loggedOut]);

  // Register function
  const register = async (registerData) => {
    try {
      const { data } = await axiosInstance.post("/api/auth/register", registerData);
      if (data.success) {
        setUser(data.data.user);
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
        setToken(data.data.token);
        localStorage.setItem("authToken", data.data.token);
        localStorage.setItem("role", data.data.user.role);
        const decodedToken = JSON.parse(atob(data.data.token.split('.')[1]));
        console.log("user profile", data.data.user); 
        setUser({user: data.data.user, id: decodedToken.UserID,
          email: decodedToken.email,
          role: data.data.user.role,
          fullName: decodedToken.fullName,});
        setError(null);
        setLoggedOut(false); // Reset loggedOut flag on successful login
        hasCheckedToken.current = false; // Reset token check flag
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
