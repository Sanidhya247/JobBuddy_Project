import logo from './logo.svg';
import './App.css';
import Navbar from './Components/Navbar';
import Footer from './Components/Footer';
import "../src/style.css";
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './Components/Home';
import Login from './Components/Login';
import Signup from './Components/Signup';
import Contact from './Components/Contact';
import About from './Components/About';


function App() {
  return (
    <>

    <Router>
      <Navbar />
      
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/sign-up" element={<Signup />} />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
      </Routes>
      <Footer />
    </Router>
    </>
  );
}

export default App;
