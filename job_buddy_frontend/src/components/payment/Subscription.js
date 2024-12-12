import React from 'react';
import { Link } from 'react-router-dom';
import "../../assets/css/Subscription.css";

const Subscription = () => {
  const plans = [
    {
      title: 'Basic Plan',
      price: '$9.99',
      features: [
        'Access to basic features',
        '10 premium jobs per month',
        'Chat Access',
      ],
      amount: 999, 
    },
    {
      title: 'Standard Plan',
      price: '$19.99',
      features: [
        'Access to all basic features',
        '25 premium jobs per month',
        'Chat Access',
        'Priority email support',
      ],
      amount: 1999,
    },
    {
      title: 'Premium Plan',
      price: '$29.99',
      features: [
        'Unlimited features access',
        'Unlimited jobs access',
        '24/7 support with priority response',
      ],
      amount: 2999,
    },
  ];

  return (
    <div className="subscription-container">
      <h1 className="subscription-title">Choose Your Plan</h1>
      <div className="subscription-cards">
        {plans.map((plan, index) => (
          <div key={index} className="subscription-card">
            <h2 className="plan-title">{plan.title}</h2>
            <p className="plan-price">{plan.price}</p>
            <ul className="plan-features">
              {plan.features.map((feature, i) => (
                <li key={i}>{feature}</li>
              ))}
            </ul>
            <Link to={`/payment/${plan.amount}`} className="buy-now-button">
              Buy Now
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Subscription;
