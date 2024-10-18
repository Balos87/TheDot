import { useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import PropTypes from 'prop-types';

const BookingForm = ({ tableId, reservationDateTime, guests, userId, goBack, userName }) => {
    const [successMessage, setSuccessMessage] = useState('');
    const [errorMessage, setErrorMessage] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        const localDate = new Date(reservationDateTime);
        const timezoneOffsetInMinutes = localDate.getTimezoneOffset();
        const adjustedDate = new Date(localDate.getTime() - timezoneOffsetInMinutes * 60000);
        const utcReservationDateTime = adjustedDate.toISOString();

        const bookingData = {
            userName,
            userId,
            tableId,
            numberOfGuests: guests,
            reservationDateTime: utcReservationDateTime,
        };

        try {
            const token = Cookies.get('JwtToken');

            const response = await axios.post('https://localhost:7157/api/booking/create', bookingData, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (response.status === 200) {
                setSuccessMessage('Thank you for choosing to book a visit with us!');
                setErrorMessage('');
            } else {
                setErrorMessage('Sorry but the booking failed, please retry or contact us!');
                setSuccessMessage('');
            }
        } catch (err) {
            setErrorMessage(err.response?.data || 'Sorry but the booking failed, please retry or contact us!');
            setSuccessMessage('');
        }
    };

    return (
        <div className="bg-gray-900 text-white p-6 rounded-lg shadow-md max-w-lg mx-auto mt-8">
            <h3 className="text-2xl font-bold text-center mb-6">
                Please confirm the booking! <br />
            </h3>
            <h5 className="text-xl font-normal text-center mb-6">
                {userName} <br />
                Table {tableId} <br />
                {new Date(reservationDateTime.getTime() - (reservationDateTime.getTimezoneOffset() * 60000))
                    .toISOString()
                    .slice(0, 16)
                    .replace('T', ' ')} <br />
                Guests: {guests}
            </h5>

            <form onSubmit={handleSubmit} className="flex justify-center space-x-4">
                <button
                    type="submit"
                    className="bg-green-600 hover:bg-green-700 text-white py-2 px-6 rounded-lg transition relative hover:shadow-green-500 hover:shadow-lg hover:scale-105"
                >
                    Create Booking
                </button>
                <button
                    type="button"
                    onClick={goBack}
                    className="bg-red-600 hover:bg-red-700 text-white py-2 px-6 rounded-lg transition relative hover:shadow-red-500 hover:shadow-lg hover:scale-105"
                >
                    Go Back
                </button>
            </form>

            {successMessage && (
                <p className="text-blue-400 text-center mt-4">{successMessage}</p>
            )}
            {errorMessage && (
                <p className="text-red-500 text-center mt-4">{errorMessage}</p>
            )}

            <div className="flex justify-center mt-8">
                <a
                    href="https://localhost:7250/"
                    className={`${successMessage
                        ? 'rounded-lg text-blue-400 hover:text-blue-500 text-xl transition transform hover:scale-110 shadow-lg shadow-blue-400 hover:shadow-blue-500'
                            : 'text-gray-400 hover:text-gray-200 transition text-sm'
                        }`}
                >
                    Return to Homepage
                </a>
            </div>
        </div>
    );
};

BookingForm.propTypes = {
    tableId: PropTypes.number.isRequired,
    reservationDateTime: PropTypes.instanceOf(Date).isRequired,
    guests: PropTypes.number.isRequired,
    userId: PropTypes.number.isRequired,
    goBack: PropTypes.func.isRequired,
    userName: PropTypes.string.isRequired,
};

export default BookingForm;