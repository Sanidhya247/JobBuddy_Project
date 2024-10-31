import React, { useRef, useState } from "react";
import "../assets/css/job_search_page.css";
const ResumeUpload = () => {
  const [selectedFile, setSelectedFile] = useState(null);
  const fileInputRef = useRef(null);

  const handleFileChange = async (event) => {
    setSelectedFile(event.target.files[0]);
  };

  const handleUpload = async () => {
    fileInputRef.current.click();
  };

  return (
    <div className="resume-upload-container" onClick={handleUpload}>
      <input
        hidden
        ref={fileInputRef}
        type="file"
        accept=".pdf,.doc,.docx"
        onChange={handleFileChange}
      />
      <span className="upload-resume-text">Upload Resume</span>
    </div>
  );
};

export default ResumeUpload;
