import React, { useState } from "react";
import "../../assets/css/input.css";
import "../../../src/style.css";

const Input = ({
  type,
  value,
  onChange,
  name,
  placeholder,
  labelClass,
  id,
  inputClass,
  required,
  labelValue,
  inputStyle,
  labelStyle,
  accept
}) => {
  const [isFocused, setIsFocused] = useState(false);

  const handleFocus = () => {
    setIsFocused(true);
  };

  const handleBlur = () => {
    if (!value) {
      setIsFocused(false);
    }
  };

  return (
    <div className="form-container">
      <label
        style={labelStyle}
        htmlFor={name}
        className={`${labelClass} floating-label txt-green ${isFocused || value ? 'label-up' : ''}`}
      >
        {labelValue}
      </label>
      <input
        type={type}
        required={required || false}
        onChange={onChange}
        id={id}
        value={value}
        style={inputStyle}
        className={`${inputClass} form-input`}
        placeholder={placeholder}
        accept={accept}
        onFocus={handleFocus}
        onBlur={handleBlur}
      />
    </div>
  );
};

export default Input;
