import './App.css';

import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Login from './pages/Login/Login';
import Register from './pages/Register';


function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/" element={<Home />} />
            </Routes>
        </BrowserRouter>
    );

}
export default App;

































/*import { useEffect, useState } from 'react';
import { BrowserRouter, Route, Routes } from "react-router";
import AppRouter from "./components/AppRouter";
import { AuthContext } from "./context";
import { Layout } from 'antd';
import './App.css';
import FileUpload from './components/FileUpload';
import HeaderRuW from './components/HeaderRuW/HeaderRuW';
import SidebarRuW from './components/SidebarRuW/SidebarRuW';
import './App.css';
import ContentRuW from './components/ContentRuW/ContentRuW';
import axios from 'axios';


function App() {
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(localStorage.getItem('token'));
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        if (token) {
            axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
            setIsAuthenticated(true);
            // You might want to fetch user details here
        } else {
            delete axios.defaults.headers.common['Authorization'];
            setIsAuthenticated(false);
        }
    }, [token]);

    const login = async (username, password) => {
        try {
            const response = await axios.post('https://localhost:7091/login', {
                username,
                password
            });

            localStorage.setItem('token', response.data.token);
            setToken(response.data.token);
            return true;
        } catch (error) {
            console.error('Login failed:', error);
            return false;
        }
    };

    const register = async (username, email, password) => {
        try {
            console.log('auth');
            await axios.post('https://localhost:7091/register', {
                username,
                email,
                password
            });
            return true;
        } catch (error) {
            console.error('Registration failed:', error);
            return false;
        }
    };

    const logout = () => {
        localStorage.removeItem('token');
        setToken(null);
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{
            user,
            token,
            isAuthenticated,
            login,
            register,
            logout
        }}>
            <BrowserRouter>
                <AppRouter />
            </BrowserRouter>
        </AuthContext.Provider>

    );

   /*
    const { Header, Content, Footer, Sider } = Layout;

    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const ToggleSidebar = () => {
        setActiveSidebar(!isActiveSidebar);
    };


    return (
        <Layout>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className="main_container">
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW />
            </Layout>
        </Layout>
   );
   */
    

    /*
    async function populateWeatherData() {
        const response = await fetch('weatherforecast');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }
    */

// }


// export default App;