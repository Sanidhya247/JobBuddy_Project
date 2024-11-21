import "./App.css";
import Footer from "./components/Footer";
import "../src/style.css";
import { Routes, Route, useLocation, Navigate } from "react-router-dom"; 
import Home from './components/Home';
import Contact from './components/Contact';
import About from './components/About';
import Login from "./components/authentication/Login";
import Register from "./components/authentication/Register";
import PostJob from "./components/PostJob"; 
import JobSearchPage from "./components/Job_Search_Page";
import VerifyEmail from './components/authentication/VerifyEmail';
import UserProfile from './components/userProfile/UserProfile';
import Navbar from './components/Navbar';
import JobDetailsPage from './components/JobDetailsPage';
import JobApplication from './components/JobApplication';
import { useContext } from 'react';
import AuthContext from './context/AuthContext';
import FloatingChatButton from './components/chat/FloatingChatButton';
import ChatPage from './components/chat/ChatPage';
import FriendRequestsPage from './components/chat/FriendRequestsPage';
import PaymentPage from './components/payment/PaymentForm';
import Subscription from './components/payment/Subscription';
import PaymentSuccess from './components/payment/PaymentSuccess';
import AdminDashboard from './components/AdminDashboard';
import AdminActions from './components/AdminActions';
import AccessibilityPanel from './components/AccessibilityPanel';



function App() {
  const { user } = useContext(AuthContext);
  const location = useLocation();
  return (
    <>
      <Navbar />

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route
          path="/job"
          element={
            user?.role === "Employer" ? (
              <Navigate to="/" replace />
            ) : (
              <JobSearchPage />
            )
          }
        />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
        <Route
          path="/post"
          element={
            user?.role === "Job Seeker" ? (
              <Navigate to="/" replace />
            ) : (
              <PostJob />
            )
          }
        />
        <Route path="/profile" element={<UserProfile />} />
        <Route path="/register" element={<Register />} />
        <Route path="/verify-email" element={<VerifyEmail />} />
        <Route path="/" element={<JobSearchPage />} />
        <Route path="/job/:jobId" element={<JobDetailsPage />} /> {/* Job Details route */}
        <Route path="/apply/:jobId" element={<JobApplication />} />
        <Route path="/chat" element={<ChatPage/>} />
        <Route path="/chat/:chatID?" element={<ChatPage />} />
        <Route path="/connections" element={<FriendRequestsPage/>} />
        <Route path="/subscription" element={<Subscription/>} />
        <Route path="/payment/:amount" element={<PaymentPage />} />
        <Route path="/Payment-success/:userID?" element = {<PaymentSuccess/>} />
        <Route path="/admin-dashboard" element={<AdminDashboard />} />
        <Route path="/admin-actions" element={<AdminActions />} />
      </Routes>
      <AccessibilityPanel />
      <Footer />
    </>
  );
}

export default App;
