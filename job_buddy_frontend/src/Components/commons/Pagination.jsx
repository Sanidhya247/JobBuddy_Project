import React from 'react';

const Pagination = ({ currentPage, totalItems, itemsPerPage, onPageChange }) => {
  const totalPages = Math.ceil(totalItems / itemsPerPage);

  const handlePageClick = (pageNumber) => {
    onPageChange(pageNumber);
  };

  return (
    <div className="pagination">
      <button 
        onClick={() => handlePageClick(currentPage - 1)} 
        disabled={currentPage === 1}
      >
        <b>&lt;</b>
      </button>
      
      {Array.from({ length: totalPages }, (_, index) => (
        <button
          key={index}
          onClick={() => handlePageClick(index + 1)}
          className={currentPage === index + 1 ? 'active' : ''}
        >
          {index + 1}
        </button>
      ))}

      <button 
        onClick={() => handlePageClick(currentPage + 1)} 
        disabled={currentPage === totalPages}
      >
        <b>&gt;</b>
      </button>
    </div>
  );
};

export default Pagination;
