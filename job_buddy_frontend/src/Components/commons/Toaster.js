import React, { useEffect, useState } from 'react';
import './styles/Toaster.css';

const Toaster = ({ message, type = "info", duration = 3000, onClose }) => {
  const [visible, setVisible] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => {
      setVisible(false);
      if (onClose) onClose();
    }, duration);

    return () => clearTimeout(timer);
  }, [duration, onClose]);

  if (!visible) return null;

  return (
    <div className={`toaster ${type}`}>
      <span>{message}</span>
      <button className="toaster-close-btn" onClick={() => setVisible(false)}>
        X
      </button>
    </div>
  );
};

export default Toaster;
