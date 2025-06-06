import React from "react";
import { Layout, Menu } from 'antd';
import cl from './HeaderRuW.module.css';
import { BellFilled } from '@ant-design/icons';
import { Avatar } from "antd";
import { UserOutlined } from '@ant-design/icons';

const { Header, Content, Footer, Sider } = Layout;


const HeaderRuW = ({activeSidebar}) => {
    return (
        <Header className={cl.header}>
            <div className={cl.burger} onClick={activeSidebar}>
                <span className={cl.burger_rect} />
                <span className={cl.burger_rect} />
                <span className={cl.burger_rect} />
            </div>
            <img src="./src/images/ruwitterlogo.svg" alt="" className={cl.logo} />
            <div className={cl.header_icons}>
                <Avatar size={50} icon={<UserOutlined />} className={cl.avatar} />
            </div>
            
        </Header>
    );
}

export default HeaderRuW;