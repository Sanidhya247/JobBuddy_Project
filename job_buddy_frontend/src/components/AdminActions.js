import React, { useState, useEffect } from 'react';
import apiService from '../utils/apiService';
import Loader from './commons/Loader';
import '../assets/css/AdminActions.css';

const AdminActions = () => {
    const [users, setUsers] = useState([]);
    const [jobs, setJobs] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchAdminData();
    }, []);

    const fetchAdminData = async () => {
        try {
            const [userResponse, jobResponse] = await Promise.all([
                apiService.get('/api/UserProfile/all'),
                apiService.get('/api/JobListing'),
            ]);
            setUsers(userResponse.data.data);
            setJobs(jobResponse.data.data);
        } catch (error) {
            console.error("Error fetching admin data:", error);
        } finally {
            setLoading(false);
        }
    };

    const toggleUserStatus = async (userId, isActive) => {
        try {
            await apiService.post(`/api/admin/user/${userId}/set-status?isActive=${isActive}`);
            fetchAdminData();
        } catch (error) {
            console.error("Error toggling user status:", error);
        }
    };

    const toggleJobApproval = async (jobId, isApproved) => {
        try {
            await apiService.post(`/api/admin/job/${jobId}/set-approval?isApproved=${isApproved}`);
            fetchAdminData();
        } catch (error) {
            console.error("Error toggling job approval:", error);
        }
    };

    const deleteEmployer = async (employerId) => {
        try {
            await apiService.delete(`/api/admin/employer/${employerId}`);
            fetchAdminData();
        } catch (error) {
            console.error("Error deleting employer profile:", error);
        }
    };

    if (loading) {
        return <Loader />;
    }

    return (
        <div className="admin-actions">
            <h2>User Management</h2>
            <table>
                <thead>
                    <tr>
                        <th>User</th>
                        <th>Status</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {users.length > 0 ? (
                        users.map((user) => (
                            <tr key={user.userID}>
                                <td>{user.fullName}</td>
                                <td>{user.isActive ? "Active" : "Inactive"}</td>
                                <td>
                                    <button
                                        onClick={() =>
                                            toggleUserStatus(user.userID, !user.isActive)
                                        }
                                    >
                                        {user.isActive ? "Deactivate" : "Activate"}
                                    </button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="3" style={{ textAlign: "center" }}>
                                No data available
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

            <h2>Job Management</h2>
            <table>
                <thead>
                    <tr>
                        <th>Job Title</th>
                        <th>Status</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {jobs.length > 0 ? (
                        jobs.map((job) => (
                            <tr key={job.jobID}>
                                <td>{job.jobTitle}</td>
                                <td>{job.isApproved ? "Approved" : "Pending"}</td>
                                <td>
                                    <button
                                        onClick={() =>
                                            toggleJobApproval(job.jobID, !job.isApproved)
                                        }
                                    >
                                        {job.isApproved ? "Disapprove" : "Approve"}
                                    </button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="3" style={{ textAlign: "center" }}>
                                No data available
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

            <h2>Employer Management</h2>
            <table>
                <thead>
                    <tr>
                        <th>Employer Name</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {users.filter((user) => user.role === "Employer").length > 0 ? (
                        users
                            .filter((user) => user.role === "Employer")
                            .map((employer) => (
                                <tr key={employer.userID}>
                                    <td>{employer.fullName}</td>
                                    <td>
                                        <button onClick={() => deleteEmployer(employer.userID)}>
                                            Delete Employer
                                        </button>
                                    </td>
                                </tr>
                            ))
                    ) : (
                        <tr>
                            <td colSpan="2" style={{ textAlign: "center" }}>
                                No data available
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default AdminActions;
