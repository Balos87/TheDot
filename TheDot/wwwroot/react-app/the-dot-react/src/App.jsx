import { useState, useEffect } from 'react';
import AvailableTables from './components/AvailableTables';
import BookingForm from './components/BookingForm';
import Cookies from 'js-cookie';
import {jwtDecode} from "jwt-decode";

const App = () => {
    const [selectedTable, setSelectedTable] = useState(null);
    const [reservationDateTime, setReservationDateTime] = useState(null);
    const [guests, setGuests] = useState(1);
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [userName, setUserName] = useState('');
    const [userId, setUserId] = useState(null);

    useEffect(() => {
        const token = Cookies.get('JwtToken');
        console.log("Token from cookie:", token);

        if (token) {
            try {
                const decodedToken = jwtDecode(token);
                console.log("Decoded JWT:", decodedToken);

                setUserName(decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]);
                setUserId(decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
                setIsLoggedIn(true);
            } catch (error) {
                console.error("Error decoding token:", error);
                setIsLoggedIn(false);
            }
        } else {
            console.log("No token found, user is not logged in.");
            setIsLoggedIn(false);
        }
    }, []);

    const goBack = () => {
        setSelectedTable(null);
    };

    if (!isLoggedIn) {
        return (
            <div className="flex flex-col items-center justify-center h-screen bg-gray-900 text-white">
                <h2 className="text-2xl font-bold mb-4">Please login to make a booking.</h2>
                <a
                    className="bg-indigo-600 hover:bg-indigo-800 text-white py-2 px-4 rounded transition"
                    href="https://localhost:7250/login"
                >
                    Login
                </a>
            </div>
        );
    }

    return (
        <div className="flex flex-col flex-grow items-center justify-center min-h-screen bg-gray-900 text-white">
            <h1 className="text-4xl text-center mb-8">THE DOT</h1>

            {!selectedTable ? (
                <AvailableTables
                    onSelectTable={(tableId, date, guests) => {
                        setSelectedTable(tableId);
                        setReservationDateTime(date);
                        setGuests(guests);
                    }} />
            )
            : 
            (
                <BookingForm
                    tableId={selectedTable}
                    reservationDateTime={reservationDateTime}
                    guests={guests}
                    userId={userId}
                    goBack={goBack}
                    userName={userName}
                />
            )}
        </div>
    );

};

export default App;