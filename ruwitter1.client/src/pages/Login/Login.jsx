import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { EyeInvisibleOutlined, EyeOutlined } from "@ant-design/icons";
import cl from "./Login.module.css";

function Login() {
    // state variables for email and passwords
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [rememberme, setRememberme] = useState(false);
    const [passwordVisible, setPasswordVisible] = useState(false);
    // state variable for error messages
    const [error, setError] = useState("");
    const navigate = useNavigate();

    // handle change events for input fields
    const handleChange = (e) => {
        const { name, value } = e.target;
        if (name === "email") setEmail(value);
        if (name === "password") setPassword(value);
        if (name === "rememberme") setRememberme(e.target.checked);
    };

    const handleRegisterClick = () => {
        navigate("/register");
    }

    const handlePasswordVisibleClick = (e) => {
        e.preventDefault();
        setPasswordVisible(prev => !prev);
    }

    // handle submit event for the form
    const handleSubmit = (e) => {
        e.preventDefault();
        // validate email and passwords
        if (!email || !password) {
            setError("Пожалуйста, заполните все поля.");
        } else {
            // clear error message
            setError("");
            // post data to the /register api

            var loginurl = "";
            if (rememberme == true)
                loginurl = "/login?useCookies=true";
            else
                loginurl = "/login?useSessionCookies=true";

            fetch(loginurl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email: email,
                    password: password,
                }),
            })

                .then((data) => {
                    // handle success or error from the server
                    console.log(data);
                    if (data.ok) {
                        setError("Авторизация прошла успешно.");
                        window.location.href = '/';
                    }
                    else
                        setError("Ошибка авторизации.");

                })
                .catch((error) => {
                    // handle network error
                    console.error(error);
                    setError("Ошибка авторизации.");
                });
        }
    };

    return (
        <div className={cl.formTitle}>
            <h2 style={{ color: '#1DA1F2' }} >RuWitter</h2>
            <h3>Авторизация</h3>
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
                        type={passwordVisible ? "text" : "password"}
                        id="password"
                        name="password"
                        value={password}
                        onChange={handleChange}
                        className={cl.formInput}
                    />
                    <button onClick={handlePasswordVisibleClick} style={{ height: '30px' }}>{passwordVisible ? <EyeOutlined /> : <EyeInvisibleOutlined />}</button>
                </div>
                <div className={cl.checkboxGroup}>
                    <input
                        type="checkbox"
                        id="rememberme"
                        name="rememberme"
                        checked={rememberme}
                        onChange={handleChange}
                        className={cl.nativeCheckbox}
                    />
                    <span className={cl.checkboxLabel}>Запомнить меня</span>
                </div>
                <div className={cl.actionButtons}>
                    <button type="submit" className={cl.submitBtn}>Войти</button>
                    <button onClick={handleRegisterClick} className={cl.registerBtn}>Зарегистрироваться</button>
                </div>
            </form>
            {error && <p className="error">{error}</p>}
        </div>
    );
}

export default Login;