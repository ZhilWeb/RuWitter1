import React, { useContext } from "react";
import { Navigate, Route, Routes } from "react-router";
// import { privateRoutes, publicRoutes } from "../router";
import { AuthContext } from "../context";
import { LoadingOutlined } from '@ant-design/icons';
import Login from '../pages/Login/Login';


const AppRouter = () => {
    const { isAuth, isLoading } = useContext(AuthContext);
    console.log(isAuth);

    if (isLoading) {
        return <LoadingOutlined />
    }
    return (
        isAuth
            ?
            <Routes>
                <Route
                    path={'/posts'}
                    element={<Posts />}
                    exact={true}
                />
                <Route
                    path={'/posts/:id'}
                    element={<PostIdPage />}
                    exact={true}
                />
                <Route path="*" element={<Navigate to="/posts" replace />} />
            </Routes>
            :
            <Routes>
                <Route path="/login" element={<Login />} exact={true} />
                <Route path="*" element={<Navigate to="/login" replace />} />
            </Routes>
        /*
        <Routes>
            {publicRoutes.map(route =>
                <Route
                    path={route.path}
                    element={route.component}
                    exact={route.exact}
                    key={route.path}
                />
            )}
            <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
        */
       /*
        <Routes>
            <Route path="/login" element={<Login />} />
       </Routes>
        */
    );
};

export default AppRouter;