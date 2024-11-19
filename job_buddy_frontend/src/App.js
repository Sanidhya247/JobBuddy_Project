import './App.css';
import Footer from './components/Footer';
import "../src/style.css";
import { Routes, Route } from "react-router-dom"; 
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


function App() {
  const { user } = useContext(AuthContext);
  return (
    <>
      <Navbar />
      
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/Job" element={<JobSearchPage />} />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
        <Route path="/post" element={<PostJob />} />
        <Route path="/profile" element={<UserProfile />} />
        <Route path="/register" element={<Register />} />
        <Route path="/verify-email" element={<VerifyEmail />} />
        <Route path="/" element={<JobSearchPage />} />
        <Route path="/job/:jobId" element={<JobDetailsPage />} /> {/* Job Details route */}
        <Route path="/job-apply" element={<JobApplication />} />
        <Route path="/chat" element={<ChatPage/>} />
        <Route path="/chat/:chatID?" element={<ChatPage />} />
        <Route path="/connections" element={<FriendRequestsPage/>} />
      </Routes>
      {user && <FloatingChatButton />}
      <Footer />
    </>
  );
}

export default App;
