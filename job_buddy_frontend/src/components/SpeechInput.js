// components/SpeechInput.js
import React, { useEffect, useState } from "react";
import SpeechRecognition, { useSpeechRecognition } from "react-speech-recognition";
import { toast } from "react-toastify";
import { FaMicrophone , FaStop} from 'react-icons/fa';
import "../../src/assets/css/speech.css";



const SpeechInput = ({ label,type, name, value, onChange, error ,ariaLabel }) => {
  const { transcript, resetTranscript } = useSpeechRecognition();
  const [isListening , setIsListening] = useState(false);

  console.log(name)

  const handleListening = () => {
    setIsListening(!isListening);
  }

  const handleSpeechInput = () => {
    resetTranscript();
    SpeechRecognition.startListening();
    toast.success("Press Stop to get the transcript.")
  };

  const stopListening = () => {
    SpeechRecognition.stopListening();
    onChange({ target: { name, value: transcript } });
  };

  useEffect(() => {
    if(isListening){
      handleSpeechInput()
    }else{
      stopListening();
    }
  },[isListening])
  
  if (!SpeechRecognition.browserSupportsSpeechRecognition()) {
    return toast.error("Your browser does not support speech recognition.");
  }

  return (
    <div className="form-group text-field-container ">
      <label htmlFor={name} className="text-field-label">
        {label}
      </label>
      <input
        aria-label={ariaLabel}
        type={type}
        name={name}
        value={value}
        onChange={onChange}
        placeholder={label}
        htmlFor={name}
        className={`text-field-input ${error ? "input-error" : ""}`}
      />
      <button aria-label="Start Listening" type="button" className="voice-input-button" onClick={handleListening}>
        <FaMicrophone />
      </button>
      <button
        type="button"
        className="btn-stop-listening bg-red"
        aria-label="Stop Listening"
        onClick={stopListening}
        disabled={!transcript}
      >
        <FaStop />
      </button>
      {error && <span className="error-text">{error}</span>}
    {/* </div> */}
    </div>

  );
};

export default SpeechInput;
