import './App.css';
import Navbar from './Components/Navbar';
import Footer from './Components/Footer';
import "../src/style.css";
import { Routes, Route } from "react-router-dom"; 
import Home from './Components/Home';
import Contact from './Components/Contact';
import About from './Components/About';
import Login from "./Components/authentication/Login";
import Register from "./Components/authentication/Register";
import PostJob from "./Components/PostJob"; 
import Profile from "./Components/Profile"; 
import JobSearchPage from "./Components/Job_Search_Page";
import JobDetailsPage from './Components/JobDetailsPage';
import VerifyEmail from './Components/authentication/VerifyEmail';


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
        <Route path="/profile" element={<Profile />} />
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
