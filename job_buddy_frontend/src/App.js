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


function App() {
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
  
      </Routes>
      <Footer />
    </>
  );
}

export default App;
