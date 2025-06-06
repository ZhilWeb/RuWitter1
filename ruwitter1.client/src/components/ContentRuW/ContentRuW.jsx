import React from 'react';
import { Layout } from 'antd';
import cl from './ContentRuW.module.css';

const { Header, Content, Footer, Sider } = Layout;

const ContentRuW = ({ children }) => {
    return (
        <Content className={cl.content}>{ children }</Content>
    );
};


export default ContentRuW;
