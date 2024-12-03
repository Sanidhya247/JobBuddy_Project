import React, { useState } from "react";
import "../../assets/css/input.css";
import "../../../src/style.css";

const Input = (props) => {
  return (
    <>
      <div class="text-field-container">
        <label htmlFor={props?.name || props?.id} className="text-field-label">
          {props.placeholder}
        </label>
        {props.type === "textarea" ? (
          <textarea {...props} className="text-field-input" />
        ) : (
          <input {...props} className="text-field-input" />
        )}
      </div>
    </>
  );
};

export default Input;
