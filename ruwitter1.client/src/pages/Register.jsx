import { useState } from "react";
import { useNavigate } from "react-router-dom";
import cl from "./Login/Login.module.css";

function Register() {
    // state variables for email and passwords
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const navigate = useNavigate();

    // state variable for error messages
    const [error, setError] = useState("");

    const handleLoginClick = () => {
        navigate("/login");
    }


    // handle change events for input fields
    const handleChange = (e) => {
        const { name, value } = e.target;
        if (name === "email") setEmail(value);
        if (name === "password") setPassword(value);
        if (name === "confirmPassword") setConfirmPassword(value);
    };

    // handle submit event for the form
    const handleSubmit = (e) => {
        e.preventDefault();
        // validate email and passwords
        if (!email || !password || !confirmPassword) {
            setError("Пожалуйста, заполните все поля.");
        } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            setError("Пожалуйста, введите корректный адрес электронной почты.");
        } else if (password !== confirmPassword) {
            setError("Пароли не совпадают.");
        } else {
            // clear error message
            setError("");
            // post data to the /register api
            fetch("/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email: email,
                    password: password,
                }),
            })
                //.then((response) => response.json())
                .then((data) => {
                    // handle success or error from the server
                    console.log(data);
                    if (data.ok)
                        setError("Регистрация прошла успешно.");
                    else
                        setError("Ошибка регистрации.");

                })
                .catch((error) => {
                    // handle network error
                    console.error(error);
                    setError("Ошибка регистрации.");
                });
        }
    };

    return (
        <div className={cl.formTitle}>
            <h2 style={{ color: '#1DA1F2' }} >RuWitter</h2>

            <form onSubmit={handleSubmit} className={cl.loginForm}>
                <div className={cl.formGroup}>
                    <label className={cl.formLabel} htmlFor="email">Email:</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        value={email}
                        onChange={handleChange}
                        className={cl.formInput}
                    />
                </div>
                <div className={cl.formGroup}>
                    <label className={cl.formLabel} htmlFor="password">Пароль:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        value={password}
                        onChange={handleChange}
                        className={cl.formInput}
                    />
                </div>
                <div className={cl.formGroup}>
                    <label className={cl.formLabel} htmlFor="confirmPassword">Подтвердите пароль:</label>
                    <input
                        type="password"
                        id="confirmPassword"
                        name="confirmPassword"
                        value={confirmPassword}
                        onChange={handleChange}
                        className={cl.formInput}
                    />
                </div>
                <p style={{ fontSize: '16px' }}>Пароль должен содержать 6 символов, заглавные и строчные английские буквы, цифры и специальные символы</p>
                <div className={cl.actionButtons}>
                    <button type="submit" className={cl.submitBtn}>Зарегистрироваться</button>
                    <button onClick={handleLoginClick} className={cl.registerBtn}>Вернуться к авторизации</button>
                </div>
                <p style={{ fontSize: '16px' }}>Нажимая кнопку "Зарегистрироваться", вы соглашаетесь с <a href="/useragreement" target="_blank">политикой конфиденциальности</a> и <a href="/privacy" target="_blank">пользовательским соглашением</a></p>
            </form>

            {error && <p className="error">{error}</p>}
        </div>
    );
}

export default Register;