import React, { useEffect } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import axios from '../../utils/apiService';
import "../../assets/css/PaymentSuccess.css";

const PaymentSuccess = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  useEffect(() => {
    const userId = searchParams.get('userId');

    if (userId) {
      const updatePremiumStatus = async () => {
        try {
          await axios.put(`/api/payment/update-premium-status/${userId}`);
        } catch (error) {
          console.error('Error updating premium status:', error);
        }
      };

      updatePremiumStatus();
    }
  }, [searchParams]);

  return (
    <div className="payment-success">
      <h1>Thank You for Subscribing!</h1>
      <p>Your payment was successful. You are now a premium user.</p>
      <button onClick={() => navigate('/profile')} className="go-to-profile-btn">
        Go to Profile
      </button>
    </div>
  );
};

export default PaymentSuccess;
