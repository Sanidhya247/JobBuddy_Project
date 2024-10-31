import React, { useState } from 'react';
import axios from 'axios';

const ResumeUpload = () => {
  const [selectedFile, setSelectedFile] = useState(null);

  const handleFileChange = (event) => {
    setSelectedFile(event.target.files[0]);
  };

  const handleUpload = async () => {
    if (!selectedFile) {
      alert("Please select a file before uploading.");
      return;
    }

    const formData = new FormData();
    formData.append("resumeFile", selectedFile);
    formData.append("UserID", selectedFile);
    formData.append("Title", selectedFile);

    try {
      const response = await axios.post("/api/upload/resume", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      alert("Resume uploaded successfully!");
      console.log("Upload response:", response.data);
    } catch (error) {
      console.error("Error uploading file:", error);
      alert("Failed to upload resume.");
    }
  };

  return (
    <div>
      <input type="file" accept=".pdf,.doc,.docx" onChange={handleFileChange} />
      <button onClick={handleUpload}>Upload Resume</button>
    </div>
  );
};

export default ResumeUpload;
