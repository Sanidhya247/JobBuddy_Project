import React from 'react';

const Button = ({ id, label, onClick, className, type = "button", disabled = false }) => {
    return (
        <button 
            type={type} 
            className={`${className} btn-submit`} 
            onClick={onClick} 
            disabled={disabled}
            id={id}
        >
            {label}
        </button>
    );
}

export default Button;
