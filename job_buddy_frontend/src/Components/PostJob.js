import React, { useState } from "react";
import "../assets/css/post_job.css";
import "../../src/style.css";
import Input from "./commons/Input";
import Button from "./commons/Button";

const PostJob = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    address: "",
    city: "",
    province: "",
    interviewTime: "",
    resume: null,
    termsAccepted: false,
  });

  const handleChange = (e) => {
    const { name, value, type, checked, files } = e.target;
    if (type === "file") {
      setFormData({ ...formData, resume: files[0] });
    } else if (type === "checkbox") {
      setFormData({ ...formData, [name]: checked });
    } else {
      setFormData({ ...formData, [name]: value });
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Form data submitted:", formData);
    // Add your form submission logic here
  };

  return (
    <div className="job-application-container container">
    <div className="job-application-header">
      <h2>Job Application</h2>
      <p>Please fill out the form below to complete your application.</p>
    </div>
      <form onSubmit={handleSubmit}>
      <div className="form-group">

        <div className="row content-space-between">
          <div className="column">
            <Input
              type="text"
              value={formData.firstName}
              onChange={handleChange}
              name="firstName"
              placeholder="First Name"
              required={true}
              labelValue={"First Name"}
            />
          </div>
          <div className="column">
            <Input
              type="text"
              value={formData.lastName}
              onChange={handleChange}
              name="lastName"
              placeholder="Last Name"
              required={true}
              labelValue={"Last Name"}
            />
          </div>
        </div>
      </div>

        <div className="form-group">
          <Input
            type="email"
            value={formData.email}
            onChange={handleChange}
            name="email"
            placeholder="E-mail"
            required={true}
            labelValue={"Email"}
          />
        </div>
        <div className="form-group">
          <Input
            type="text"
            value={formData.address}
            onChange={handleChange}
            name="address"
            placeholder="Address"
            required={true}
            labelValue={"Address"}
          />
        </div>
        <div className="form-group">
          <div className="row content-space-between">
            <div className="column">
              <Input
                type="text"
                value={formData.city}
                onChange={handleChange}
                name="city"
                placeholder="City"
                required={true}
                labelValue={"City"}
              />
            </div>
            <div className="column">
              <Input
                type="text"
                value={formData.province}
                onChange={handleChange}
                name="province"
                placeholder="Province"
                required={true}
                labelValue={"Province"}
              />
            </div>
          </div>
        </div>
        <div className="form-group">
          <Input
            type="textarea"
            value={formData.interviewTime}
            onChange={handleChange}
            name="interviewTime"
            placeholder="Available date and Time for Interview"
            required={true}
            labelValue={"Available Date and Time for Interview"}
          />
        </div>
        <div className="form-group">
          <Input
            type="file"
            name="resume"
            onChange={handleChange}
            accept=".pdf, .doc, .docx"
            required={true}
            labelValue={"Upload Your Resume"}
          />
        </div>
        {/* <div className="form-group">
          <Input
            type="checkbox"
            name="termsAccepted"
            checked={formData.termsAccepted}
            onChange={handleChange}
            required={true}
            labelValue={
              <>
                Accept <a href="#">Terms of Use</a> and{" "}
                <a href="#">Privacy Policy</a> and to receiving marketing
                emails.
              </>
            }
          />
        </div> */}
        <div className="form-group">
          <Button label={"submit"} />
        </div>
      </form>
    </div>
  );
};

export default PostJob;
