import { useState } from 'react';
import PropTypes from 'prop-types';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import axios from 'axios';

const AvailableTables = ({ onSelectTable }) => {
    const [date, setDate] = useState(new Date());
    const [guests, setGuests] = useState(1);
    const [availableTables, setAvailableTables] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [showTables, setShowTables] = useState(false);

    const openingTime = new Date();
    openingTime.setHours(10, 0, 0, 0);

    const closingTime = new Date();
    closingTime.setHours(22, 0, 0, 0);

    const validateTime = (selectedDate) => {
        const selectedTime = selectedDate.getHours();
        const selectedMinutes = selectedDate.getMinutes();

        const isWithinBusinessHours =
            selectedTime >= 10 && (selectedTime < 22 || (selectedTime === 22 && selectedMinutes === 0));

        if (!isWithinBusinessHours) {
            setError('Please select a time between 10:00 AM and 10:00 PM.');
            return false;
        }

        setError('');
        return true;
    };

    const handleDateChange = (newDate) => {
        if (validateTime(newDate)) {
            setDate(newDate);
        }
    };

    const fetchAvailableTables = async () => {
        try {
            setLoading(true);
            setError('');
            setShowTables(false);

            const utcDate = new Date(date.getTime() - date.getTimezoneOffset() * 60000).toISOString();

            const response = await axios.get('https://localhost:7157/api/table/available-tables', {
                params: {
                    reservationDateTime: utcDate,
                    numberOfGuests: guests,
                },
            });

            setAvailableTables(response.data);
            setShowTables(true);
        } catch (err) {
            setError(err.response?.data || 'Failed to load available tables.');
        } finally {
            setLoading(false);
        }
    };

    const handleTableClick = (tableId) => {
        onSelectTable(tableId, date, guests);
    };

    return (
        <div className="min-h-screen bg-gray-900 flex flex-col items-center">
            <div className="w-full max-w-3xl p-6">
                <h5 className="text-2xl font-semibold text-center mb-4">When would you like to visit us?</h5>

                <div className="mb-6 flex justify-center">
                    <DatePicker
                        selected={date}
                        onChange={handleDateChange}
                        showTimeSelect
                        dateFormat="Pp"
                        className="bg-gray-800 text-white py-2 px-4 rounded-md border border-gray-600 focus:border-indigo-500 focus:ring-indigo-500 w-full"
                        minTime={openingTime}
                        maxTime={closingTime}
                    />
                </div>

                <h3 className="text-2xl font-semibold text-center mb-4">How many will attend this booking?</h3>

                <div className="flex justify-center mb-6">
                    <input
                        type="number"
                        value={guests}
                        onChange={(e) => setGuests(e.target.value)}
                        min="1"
                        max="20"
                        className="bg-gray-800 text-white py-2 px-4 rounded-md border border-gray-600 focus:border-indigo-500 focus:ring-indigo-500 w-32 text-center"
                    />
                </div>

                <div className="flex justify-center">
                    <button
                        onClick={fetchAvailableTables}
                        className="bg-indigo-600 hover:bg-indigo-800 text-white py-2 px-6 rounded-lg transition"
                        disabled={!!error}
                    >
                        Check Available Tables
                    </button>
                </div>

                {loading && <p className="text-center text-indigo-400 mt-4">Loading tables...</p>}
                {error && <p className="text-center text-red-500 mt-4">{error}</p>}
            </div>

            {showTables && (
                <div className="w-full max-w-3xl p-6">
                    <h3 className="text-2xl font-semibold text-center mt-8 mb-4">Available Tables</h3>
                    <ul className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                        {availableTables.length > 0 ? (
                            availableTables.map((table) => (
                                <li key={table.tableId} className="text-center">
                                    <button
                                        className="bg-gray-800 hover:bg-gray-700 text-white py-4 px-6 rounded-lg transition shadow-lg w-full h-full"
                                        onClick={() => handleTableClick(table.tableId)}
                                    >
                                        <div className="font-bold">Table {table.tableNumber}</div>
                                        <div className="mt-2">{table.description}</div>
                                    </button>
                                </li>
                            ))
                        ) : (
                            <li className="text-center text-gray-400 col-span-full">No tables available.</li>
                        )}
                    </ul>
                </div>
            )}

            <div className="py-4 w-full max-w-3xl">
                <div className="text-center">
                    <a
                        href="https://localhost:7250/"
                        className="text-gray-400 hover:text-gray-200 transition text-sm"
                    >
                        Return to Homepage
                    </a>
                </div>
            </div>
        </div>
    );
};

AvailableTables.propTypes = {
    onSelectTable: PropTypes.func.isRequired,
};

export default AvailableTables;