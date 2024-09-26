import React from 'react';

const Button = ({ label, onClick, className, type = "button", disabled = false }) => {
    return (
        <button 
            type={type} 
            className={className} 
            onClick={onClick} 
            disabled={disabled}
        >
            {label}
        </button>
    );
}

export default Button;
