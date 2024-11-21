import React, { useState, useEffect, useContext } from 'react';
import { loadStripe } from '@stripe/stripe-js';
import { Elements, PaymentElement, useStripe, useElements } from '@stripe/react-stripe-js';
import axios from '../../utils/apiService';
import { useParams, useNavigate } from 'react-router-dom';
import "../../assets/css/Payment.css";
import AuthContext from '../../context/AuthContext';
import Loader from '../commons/Loader';

// Load Stripe
const stripePromise = loadStripe(process.env.REACT_APP_STRIPE_PUBLISHABLE_KEY);

const PaymentForm = ({ clientSecret, amount, userId }) => {
  const stripe = useStripe();
  const elements = useElements();
  const [paymentStatus, setPaymentStatus] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    if (!stripe || !elements) return;

    const { error, paymentIntent } = await stripe.confirmPayment({
      elements,
      confirmParams: {
        return_url: `${window.location.origin}/payment-success?userId=${userId}&amount=${amount}`, 
      },
    });

    setLoading(false);

    if (error) {
      setPaymentStatus(`Payment failed: ${error.message}`);
    } else if (paymentIntent?.status === 'succeeded') {
      setPaymentStatus('Payment successful!');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="payment-form">
      <h2>Complete Your Payment</h2>
      <p className="amount-info">Payment Amount: ${amount / 100}</p>
      <div className="payment-element-container">
        <PaymentElement />
      </div>
      <button type="submit" className="pay-button" disabled={!stripe || loading}>
        {loading ? 'Processing...' : 'Pay'}
      </button>
      {paymentStatus && (
        <p className={`payment-status ${paymentStatus.includes('successful') ? 'success' : 'error'}`}>
          {paymentStatus}
        </p>
      )}
    </form>
  );
};

const PaymentPage = () => {
  const [clientSecret, setClientSecret] = useState('');
  const { user } = useContext(AuthContext);
  const { amount } = useParams();
  const userId = user.userID;

  useEffect(() => {
    const fetchClientSecret = async () => {
      try {
        const { data } = await axios.post('/api/payment/create-payment-intent', { amount: parseInt(amount, 10) });
        setClientSecret(data.data.clientSecret);
      } catch (error) {
        console.error('Error fetching client secret:', error);
      }
    };

    fetchClientSecret();
  }, [amount]);

  const options = {
    clientSecret,
    appearance: {
      theme: 'stripe',
    },
  };

  return (
    <div className="payment-page">
      {clientSecret ? (
        <Elements stripe={stripePromise} options={options}>
          <PaymentForm clientSecret={clientSecret} amount={amount} userId={userId} />
        </Elements>
      ) : (
        <Loader />
      )}
    </div>
  );
};

export default PaymentPage;
