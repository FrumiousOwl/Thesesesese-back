import React, { useState } from 'react';
import './App.css';
import axios from 'axios';

function App() {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    datePurchased: new Date().toISOString(),
    defective: 0,
    available: 0,
    deployed: 0,
  });

  const handleChange = (event: { target: { name: any; value: any; }; }) => {
    const { name, value } = event.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    try {
      const response = await axios.post('https://localhost:7234/api/Category', formData);
      console.log(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="App">
      <h1>Category Form</h1>
      <form onSubmit={handleSubmit}>
        <label htmlFor="name">Name:</label>
        <input type="text" id="name" name="name" value={formData.name} onChange={handleChange} required />
        <br />
        <label htmlFor="description">Description:</label>
        <textarea id="description" name="description" value={formData.description} onChange={handleChange}/>
        <br />
        <label htmlFor="datePurchased">Date Purchased:</label>
        <input type="datetime-local" id="datePurchased" name="datePurchased" value={formData.datePurchased} onChange={handleChange} />
        <br />
        <label htmlFor="defective">Defective:</label>
        <input type="number" id="defective" name="defective" value={formData.defective} onChange={handleChange} />
        <br />
        <label htmlFor="available">Available:</label>
        <input type="number" id="available" name="available" value={formData.available} onChange={handleChange} />
        <br />
        <label htmlFor="deployed">Deployed:</label>
        <input type="number" id="deployed" name="deployed" value={formData.deployed} onChange={handleChange} />
        <br />
        <button type="submit">Submit</button>
      </form>
    </div>
  );
}

export default App;
