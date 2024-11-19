import "./App.css";
import Footer from "./Components/Footer";
import "../src/style.css";
import { Routes, Route, useNavigate, Navigate } from "react-router-dom";
import Home from "./Components/Home";
import Contact from "./Components/Contact";
import About from "./Components/About";
import Login from "./Components/authentication/Login";
import Register from "./Components/authentication/Register";
import PostJob from "./Components/PostJob";
import JobSearchPage from "./Components/Job_Search_Page";
import VerifyEmail from "./Components/authentication/VerifyEmail";
import UserProfile from "./Components/userProfile/UserProfile";
import Navbar from "./Components/Navbar";
import JobDetailsPage from "./Components/JobDetailsPage";
import JobApplication from "./Components/JobApplication";

function App() {
  const user = JSON.parse(localStorage.getItem("user")) || {};
  console.log(user);
  return (
    <>
      <Navbar />

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route
          path="/job"
          element={
            user.role === "Employer" ? (
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
            user.role === "Job Seeker" ? (
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
        {/* Job Details route */}
        <Route path="/job/:jobId" element={<JobDetailsPage />} />{" "}
        <Route
          path="/job-apply"
          element={
            user.role === "Employer" ? (
              <Navigate to="/" replace />
            ) : (
              <JobApplication />
            )
          }
        />
      </Routes>
      <Footer />
    </>
  );
}

export default App;
