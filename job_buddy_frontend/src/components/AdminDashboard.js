import React, { useState, useEffect } from 'react';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    BarElement,
    ArcElement,
    Tooltip,
    Legend,
} from 'chart.js';
import { Bar, Pie } from 'react-chartjs-2';
import apiService from '../utils/apiService';
import Loader from './commons/Loader';
import '../assets/css/AdminDashboard.css';
import AdminActions from './AdminActions';

ChartJS.register(
    CategoryScale,
    LinearScale,
    BarElement,
    ArcElement,
    Tooltip,
    Legend
);

const AdminDashboard = () => {
    const [statistics, setStatistics] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchStatistics();
    }, []);

    const fetchStatistics = async () => {
        try {
            const response = await apiService.get('/api/admin/dashboard-statistics');
            console.log("Statistics Fetched:", response.data); 
            setStatistics(response.data); 
        } catch (error) {
            console.error("Error fetching statistics:", error);
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return <Loader />;
    }

    if (!statistics) {
        return <p>No data available. Please try again later.</p>;
    }

    const jobData = {
        labels: ['Total Jobs', 'Active Jobs'],
        datasets: [
            {
                label: 'Job Statistics',
                data: [statistics.totalJobs, statistics.activeJobs],
                backgroundColor: ['#36A2EB', '#FF6384'],
            },
        ],
    };

    const userData = {
        labels: ['Total Users', 'Active Users'],
        datasets: [
            {
                label: 'User Statistics',
                data: [statistics.totalUsers, statistics.activeUsers],
                backgroundColor: ['#FFCE56', '#4BC0C0'],
            },
        ],
    };

    return (
        <div className="admin-dashboard">
            <h1>Admin Dashboard</h1>
            <div className="stats-container">
                <div className="stat-card">
                    <h3>Total Applications</h3>
                    <p>{statistics.totalApplications}</p>
                </div>
                <div className="chart">
                    <h3>Job Statistics</h3>
                    <Bar data={jobData} />
                </div>
                <div className="chart">
                    <h3>User Statistics</h3>
                    <Pie data={userData} />
                </div>
            </div>
            <AdminActions />
        </div>
    );
};

export default AdminDashboard;
