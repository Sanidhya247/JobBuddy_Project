import React, { useState } from 'react';
import "../assets/css/job_search_page.css";
import "../assets/css/Pagination.css";
import JobDetailsCard from './JobDetailsCard';
import Pagination from './commons/Pagination';

const jobListings = [
  {
    title: "Marketing Coordinator | Greenfield Co.",
    location: "Calgary, AB",
    type: "Full Time",
    position: "Employee",
    salary: "$55,000 / year",
    description: "Assist in the development and execution of marketing campaigns, content creation, and event coordination."
  },
  {
    title: "Software Engineer | BlueTech",
    location: "Toronto, ON",
    type: "Contract",
    position: "Contract",
    salary: "$45 / hour",
    description: "Design, develop, and implement software solutions for clients in various industries, ensuring timely delivery and maintenance."
  },
  {
    title: "Graphic Designer | Creative Minds",
    location: "Vancouver, BC",
    type: "Part Time",
    position: "Freelancer",
    salary: "$30 / hour",
    description: "Create engaging visual designs for both print and digital media, including branding materials and social media content."
  },
  {
    title: "HR Manager | People Solutions",
    location: "Edmonton, AB",
    type: "Full Time",
    position: "Employee",
    salary: "$75,000 / year",
    description: "Oversee recruitment, employee relations, and organizational development, ensuring compliance with labor laws and company policies."
  },
  {
    title: "Data Analyst | Insight Analytics",
    location: "Ottawa, ON",
    type: "Contract",
    position: "Contract",
    salary: "$50 / hour",
    description: "Analyze data to generate insights for business decision-making, create reports, and support data-driven strategies."
  },
  {
    title: "Accountant | FinTax",
    location: "Montreal, QC",
    type: "Full Time",
    position: "Employee",
    salary: "$60,000 / year",
    description: "Manage financial records, prepare tax returns, and provide financial advice to clients, ensuring compliance with regulations."
  },
  {
    title: "IT Support Specialist | TechSupport Co.",
    location: "Halifax, NS",
    type: "Full Time",
    position: "Employee",
    salary: "$25 / hour",
    description: "Provide technical support to users, troubleshoot hardware and software issues, and maintain IT systems."
  },
  {
    title: "Customer Success Manager | Startup Growth",
    location: "Waterloo, ON",
    type: "Full Time",
    position: "Employee",
    salary: "$70,000 / year",
    description: "Manage client relationships, ensure successful product implementation, and maintain high levels of customer satisfaction."
  },
  {
    title: "UX/UI Designer | Design Studio",
    location: "Victoria, BC",
    type: "Freelancer",
    position: "Freelancer",
    salary: "$40 / hour",
    description: "Design user interfaces and experiences for web and mobile applications, focusing on usability and aesthetics."
  },
  {
    title: "Warehouse Associate | Global Supply",
    location: "Winnipeg, MB",
    type: "Part Time",
    position: "Employee",
    salary: "$18 / hour",
    description: "Assist in the day-to-day operations of the warehouse, including inventory management, shipping, and receiving."
  },
  {
    title: "Social Media Manager | Buzz Media",
    location: "Toronto, ON",
    type: "Full Time",
    position: "Employee",
    salary: "$50,000 / year",
    description: "Develop and execute social media strategies to increase brand visibility and engagement across multiple platforms."
  },
  {
    title: "Junior Web Developer | Code Ninjas",
    location: "Calgary, AB",
    type: "Internship",
    position: "Intern",
    salary: "$20 / hour",
    description: "Assist in the development of websites and web applications, collaborate with senior developers, and gain hands-on coding experience."
  },
  {
    title: "Project Manager | BuildCorp",
    location: "Edmonton, AB",
    type: "Full Time",
    position: "Employee",
    salary: "$80,000 / year",
    description: "Lead construction projects, manage timelines, budgets, and ensure compliance with safety standards and regulations."
  },
  {
    title: "Operations Manager | Swift Logistics",
    location: "Mississauga, ON",
    type: "Full Time",
    position: "Employee",
    salary: "$90,000 / year",
    description: "Oversee daily logistics operations, manage supply chain activities, and ensure timely delivery of goods and services."
  },
  {
    title: "Content Writer | WordSmiths",
    location: "Quebec City, QC",
    type: "Freelancer",
    position: "Freelancer",
    salary: "$25 / hour",
    description: "Create engaging content for blogs, articles, and social media, focusing on SEO optimization and brand storytelling."
  },
  {
    title: "Mechanical Engineer | InnovateTech",
    location: "Windsor, ON",
    type: "Full Time",
    position: "Employee",
    salary: "$85,000 / year",
    description: "Design and develop mechanical systems, oversee manufacturing processes, and collaborate with cross-functional teams."
  }
];


const Job_Search_Page = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 2;

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = jobListings.slice(indexOfFirstItem, indexOfLastItem);

  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };
  return (

    <div className="main-container">

      <div className="search-bar">
        <div className="search-input">
          <span className="search-icon">Icon</span>
          <input type="text" placeholder="Job Title" />
        </div>
        <button className="search-button">Search</button>
      </div>

      <div className="content-container">

        <div className="filter-sidebar">
          <h2>Filters</h2>
          <div className="filter-group">
            <select>
              <option value="">Title</option>
              <option value="pharmacy-assistant">Pharmacy Assistant</option>
              <option value="pharmacist">Pharmacist</option>
            </select>
            <select>
              <option value="">Company</option>
              <option value="company1">Company 1</option>
              <option value="company2">Company 2</option>
            </select>
            <select>
              <option value="">Job Type</option>
              <option value="full-time">Full Time</option>
              <option value="part-time">Part Time</option>
            </select>
            <select>
              <option value="">Position</option>
              <option value="employee">Employee</option>
              <option value="manager">Manager</option>
            </select>
            <button className="apply-btn">Apply</button>
            <button className="clear-btn">Clear All</button>
          </div>
        </div>
        <div className="job-list">
          {currentItems.map((jobList) => {
            return (<JobDetailsCard title={jobList.title}
              location={jobList.location}
              type={jobList.type}
              position={jobList.position}
              salary={jobList.salary}
              description={jobList.description} />)
          })}
        </div>

      </div>

      <Pagination
        currentPage={currentPage}
        totalItems={jobListings.length}
        itemsPerPage={itemsPerPage}
        onPageChange={handlePageChange}
      />

    </div>

  )
}

export default Job_Search_Page;
